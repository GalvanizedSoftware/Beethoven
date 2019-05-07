using Castle.DynamicProxy;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class Method : IInterceptor
  {
    protected Method(string name)
    {
      Name = name;
      MethodMatcher = null;
    }

    protected Method(string name, IMethodMatcher methodMatcher)
    {
      Name = name;
      MethodMatcher = methodMatcher;
    }

    public string Name { get; }
    public IMethodMatcher MethodMatcher { get; }

    internal abstract void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo);

    public void Intercept(IInvocation invocation) =>
      Invoke(value => invocation.ReturnValue = value, invocation.Arguments, invocation.GenericArguments, invocation.Method);
  }
}