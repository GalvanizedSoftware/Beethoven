using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class Method
  {
    protected readonly IParameter parameter;

    protected Method(string name, IMethodMatcher methodMatcher, IParameter parameter = null)
    {
      Name = name;
      MethodMatcher = methodMatcher ?? new MatchNothing();
      this.parameter = parameter;
    }

    public string Name { get; }
    public IMethodMatcher MethodMatcher { get; }

    public virtual void Invoke(object localInstance, 
      Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo) =>
      throw new MissingMemberException(methodInfo.DeclaringType?.FullName, methodInfo.Name);

    public virtual void InvokeFindInstance(IInstanceMap instanceMap,
      Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo) =>
      Invoke(instanceMap.GetLocal(parameter), returnAction, parameters, genericArguments, methodInfo);
  }
}