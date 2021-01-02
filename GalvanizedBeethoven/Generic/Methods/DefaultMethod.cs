using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class DefaultMethod : IDefinitions, IMainTypeUser
  {
    private readonly Func<MethodInfo, object[], object> mainFunc;
    private Type mainType;

    public DefaultMethod(Func<MethodInfo, object[], object> mainFunc)
    {
      this.mainFunc = mainFunc ?? ((_, __) => null);
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

    public void Set(Type setMainType) =>
      mainType = setMainType;

    public IEnumerable<IDefinition> GetDefinitions<TInterface>() where TInterface : class => 
      mainType
        .GetAllMethodsAndInherited()
        .Select(methodInfo => new MappedDefaultMethod(methodInfo, mainFunc));
  }
}
