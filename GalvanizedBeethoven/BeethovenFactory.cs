using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven
{
  public sealed class BeethovenFactory
  {
    private static readonly MethodInfo generateMethodInfo = typeof(BeethovenFactory)
      .GetMethods()
      .Where(info => info.Name == nameof(Generate))
      .First(info => info.IsGenericMethod);

    public BeethovenFactory(params object[] generalPartDefinitions)
    {
      GeneralPartDefinitions = generalPartDefinitions;
    }

    public IEnumerable<object> GeneralPartDefinitions { get; set; }

    public object Generate(Type type, params object[] partDefinitions) =>
      generateMethodInfo
        .MakeGenericMethod(type)
        .Invoke(this, new object[] { partDefinitions });

    private static int assemblyNumber = 1;

    public T Generate<T>(params object[] partDefinitions) where T : class =>
      Compile<T>(partDefinitions).Create();

    public T Generate<T>(object[] partDefinitions, object[] parameters) where T : class =>
      Compile<T>(partDefinitions).Create(parameters);

    public CompiledTypeDefinition<T> Compile<T>(object[] partDefinitions) where T : class
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
      Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
      loadedAssemblyNames.Clear();
      assemblyCache.Clear();
      analyzingAssemblies.Clear();
      foreach (Assembly assembly in loadedAssemblies)
        loadedAssemblyNames.Add(assembly.GetName().FullName, assembly);
      analyzingAssemblies.AddRange(assemblies);
      IEnumerable<Assembly> allAssemblies = assemblies
        .SelectMany(GetAssemblies)
        .Where(assembly => loadedAssemblies.Contains(assembly))
        .Distinct()
        .ToArray();
      MetadataReference[] references = allAssemblies
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

    Dictionary<Assembly, Assembly[]> assemblyCache = new Dictionary<Assembly, Assembly[]>();
    Dictionary<string, Assembly> loadedAssemblyNames = new Dictionary<string, Assembly>();

    private IEnumerable<Assembly> GetAssemblies(Assembly assembly)
    {
      if (assemblyCache.TryGetValue(assembly, out Assembly[] result))
        return result;
      result = FindReferencedAssemblies(assembly)
        .Append(assembly)
        .ToArray();
      assemblyCache.Add(assembly, result);
      return result;
    }

    private IEnumerable<Assembly> FindReferencedAssemblies(Assembly assembly)
    {
      IEnumerable<Assembly> enumerable = assembly
                 .GetReferencedAssemblies()
                 .Select(LoadAssembly)
                 .SelectMany(FindNew)
                 .ToArray();
      return enumerable
        .SelectMany(GetAssemblies);
    }

    private IEnumerable<Assembly> FindNew(Assembly assembly)
    {
      if (analyzingAssemblies.Contains(assembly) || assembly == null)
        yield break;
      analyzingAssemblies.Add(assembly);
      yield return assembly;
    }

    private readonly List<Assembly> analyzingAssemblies = new List<Assembly>();

    private Assembly LoadAssembly(AssemblyName assemblyName)
    {
      string fullName = assemblyName.FullName;
      if (loadedAssemblyNames.TryGetValue(fullName, out Assembly assembly))
        return assembly;
      //assembly = Assembly.ReflectionOnlyLoad(fullName);
      //loadedAssemblies.Add(fullName, assembly);
      return assembly;
    }

    internal T Create<T>(object[] partDefinitions, object[] parameters)
      where T : class
    {
      return Generate<T>(partDefinitions, parameters);
    }

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
