using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Reflection.Assembly;

namespace GalvanizedSoftware.Beethoven
{
  public sealed class BeethovenFactory
  {
    private static readonly MethodInfo generateMethodInfo = typeof(BeethovenFactory)
      .GetMethods(Constants.ResolveFlags)
      .Where(info => info.Name == nameof(GenerateInternal))
      .First(info => info.IsGenericMethod);
    private IEnumerable<object> generalPartDefinitions;

    public BeethovenFactory(params object[] generalPartDefinitions)
    {
      this.generalPartDefinitions = generalPartDefinitions;
    }


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
      partDefinitions = partDefinitions.Concat(generalPartDefinitions).ToArray();
      Type type = typeof(T);
      partDefinitions.OfType<IMainTypeUser>().SetAll(type);

      object[] allPartDefinitions = partDefinitions
        .Concat(
          new WrapperGenerator<T>(partDefinitions)
          .GetDefinitions())
        .ToArray();
      IDefinition[] definitions = allPartDefinitions
        .GetAllDefinitions();
      string className = $"{type.GetFormattedName()}Implementation";
      ClassGenerator classGenerator = new ClassGenerator(type, className, definitions);
      string code = classGenerator.Generate().Format(0);

      IEnumerable<Assembly> assemblyCache = new AssemblyCache<T>(callingAssembly);
      Compiler compiler = new Compiler(code, assemblyCache);
      Type compiledType = compiler.GenerateAssembly().GetType($"{type.Namespace}.{className}");
      return new CompiledTypeDefinition<T>(compiledType, new BindingParents(allPartDefinitions));
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
