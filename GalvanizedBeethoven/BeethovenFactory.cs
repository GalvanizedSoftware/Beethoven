using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using static System.Reflection.Assembly;

namespace GalvanizedSoftware.Beethoven
{
  public sealed class BeethovenFactory
  {
    private static int assemblyNumber = 1;
    private static readonly MethodInfo generateMethodInfo = typeof(BeethovenFactory)
      .GetMethods(Constants.ResolveFlags)
      .Where(info => info.Name == nameof(GenerateInternal))
      .First(info => info.IsGenericMethod);

    public BeethovenFactory(params object[] generalPartDefinitions)
    {
      GeneralPartDefinitions = generalPartDefinitions;
    }

    public IEnumerable<object> GeneralPartDefinitions { get; set; }

    public object Generate(Type type, params object[] partDefinitions) =>
      generateMethodInfo
        .MakeGenericMethod(type)
        .Invoke(this, new object[] { GetCallingAssembly(), partDefinitions, Array.Empty<object>() });

    public T Generate<T>(params object[] partDefinitions) where T : class =>
      CompileInternal<T>(GetCallingAssembly(), partDefinitions).Create();

    public T Generate<T>(object[] partDefinitions, object[] parameters) where T : class =>
      CompileInternal<T>(GetCallingAssembly(), partDefinitions).Create(parameters);

    public CompiledTypeDefinition<T> Compile<T>(object[] partDefinitions) where T : class =>
      CompileInternal<T>(GetCallingAssembly(), partDefinitions);

    internal T GenerateInternal<T>(Assembly callingAssembly, object[] partDefinitions, object[] parameters) where T : class =>
      CompileInternal<T>(callingAssembly, partDefinitions).Create(parameters);

    internal CompiledTypeDefinition<T> CompileInternal<T>(Assembly callingAssembly, object[] partDefinitions) where T : class
    {
      partDefinitions = partDefinitions.Concat(GeneralPartDefinitions).ToArray();

      Type type = typeof(T);
      partDefinitions.OfType<IMainTypeUser>().SetAll(type);

      IDefinition[] wrapperDefinitions = new WrapperGenerator<T>(partDefinitions).ToArray();
      IEnumerable<object> allPartDefinitions = partDefinitions
        .Concat(wrapperDefinitions);
      IDefinition[] definitions = allPartDefinitions
        .GetAllDefinitions();
      string className = $"{type.GetFormattedName()}Implementation";
      ClassGenerator classGenerator = new ClassGenerator(type, className, definitions);
      GeneratorContext generatorContext = new GeneratorContext(className, type);
      string code = classGenerator.Generate(generatorContext).Format(0);
      Assembly[] assemblies = new[]
      {
        typeof(object).Assembly,
        typeof(T).Assembly,
        Assembly.GetCallingAssembly(),
      };
      AssemblyCache<T> assemblyCache = new AssemblyCache<T>(callingAssembly);
      MetadataReference[] references = assemblyCache
        .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
        .ToArray();
      SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
      CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
      CSharpCompilation compilation = CSharpCompilation.Create(
        $"TmpAssembly{++assemblyNumber}",
        new[] { syntaxTree },
        references,
        options);
      using (MemoryStream assemblyStream = new MemoryStream())
      using (MemoryStream pbdStream = new MemoryStream())
      {
        EmitResult result = compilation.Emit(assemblyStream, pbdStream);
        string[] erros = result
          .Diagnostics
          .Select(error => error.ToString())
          .ToArray();
        if (!result.Success)
          throw new Exception("Internal compilation error");
        Assembly assembly = Assembly.Load(assemblyStream.ToArray(), pbdStream.ToArray());
        Type compiledType = assembly.GetType($"{type.Namespace}.{className}");
        IBindingParent bindingParents = new BindingParents(allPartDefinitions);
        return new CompiledTypeDefinition<T>(compiledType, bindingParents);
      }
    }

    private IEnumerable<ICodeGenerator> GetAllCodeGenerators(object part) =>
      part switch
      {
        ICodeGenerator codeGenerator => new[] { codeGenerator },
        IEnumerable<ICodeGenerator> codeGenerators => codeGenerators,
        _ => Enumerable.Empty<ICodeGenerator>()
      };

    public static bool Implements<TInterface, TClass>() =>
      !new GeneralSignatureChecker(typeof(TInterface), typeof(TClass))
        .FindMissing()
        .Any();

    public static bool Implements<TInterface>(object instance) =>
      !new GeneralSignatureChecker(typeof(TInterface), instance?.GetType())
        .FindMissing()
        .Any();
  }
}
