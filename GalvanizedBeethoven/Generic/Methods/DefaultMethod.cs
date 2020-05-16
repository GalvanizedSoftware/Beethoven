using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class DefaultMethod : IEnumerable<IDefinition>, IMainTypeUser
  {
    private readonly Func<MethodInfo, object[], object> mainFunc;
    private Type mainType;

    public DefaultMethod(Func<MethodInfo, object[], object> mainFunc)
    {
      this.mainFunc = mainFunc ?? ((unused1, unused2) => null);
    }

    public DefaultMethod(Action<MethodInfo, object[]> mainAction)
    {
      mainFunc = (methodInfo, objects) =>
      {
        mainAction?.Invoke(methodInfo, objects);
        return methodInfo.GetDefaultReturnValue();
      };
    }

    public MethodDefinition CreateMapped(MethodInfo methodInfo) =>
      new MappedDefaultMethod(methodInfo, mainFunc);

    public IMethodMatcher MethodMatcher { get; } = new MatchAnything();

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      MethodInfo methodInfo = generatorContext?.MemberInfo as MethodInfo;
      return methodInfo == null ?
        Enumerable.Empty<string>() :
        (new MappedDefaultMethod(methodInfo, mainFunc))
          .GetGenerator(generatorContext)
          .Generate(generatorContext);
    }

    public void Set(Type mainType) =>
      this.mainType = mainType;

    public IEnumerator<IDefinition> GetEnumerator() => 
      mainType
        .GetAllMethodsAndInherited()
        .Select(methodInfo => new MappedDefaultMethod(methodInfo, mainFunc))
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
