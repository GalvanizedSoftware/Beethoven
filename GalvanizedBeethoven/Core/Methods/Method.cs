using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class Method
  {
    private readonly Parameter parameter;

    protected Method(string name, IMethodMatcher methodMatcher, Parameter parameter = null)
    {
      Name = name;
      MethodMatcher = methodMatcher;
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