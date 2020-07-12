using GalvanizedSoftware.Beethoven.Core;
using Microsoft.CodeAnalysis;
using System;
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
    private Assembly callingAssembly = GetCallingAssembly();
    private object[] generalPartDefinitions;

    public BeethovenFactory(params object[] generalPartDefinitions)
    {
      this.generalPartDefinitions = generalPartDefinitions;
    }


    public object Generate(Type type, params object[] partDefinitions) =>
      generateMethodInfo
        .MakeGenericMethod(type)
        .Invoke(this, new object[] { partDefinitions, Array.Empty<object>() });

    public T Generate<T>(params object[] partDefinitions) where T : class =>
      CompileInternal<T>(partDefinitions).Create();

    public T Generate<T>(object[] partDefinitions, object[] parameters) where T : class =>
      CompileInternal<T>(partDefinitions).Create(parameters);

    public CompiledTypeDefinition<T> Compile<T>(object[] partDefinitions) where T : class =>
      CompileInternal<T>(partDefinitions);

    internal T GenerateInternal<T>(object[] partDefinitions, object[] parameters) where T : class =>
      CompileInternal<T>(partDefinitions).Create(parameters);

    private CompiledTypeDefinition<T> CompileInternal<T>(object[] partDefinitions) where T : class =>
      TypeDefinition.Create<T>(partDefinitions, generalPartDefinitions)
        .CompileInternal(callingAssembly);

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
