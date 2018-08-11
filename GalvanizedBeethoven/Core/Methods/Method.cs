using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class Method : IInterceptor
  {
    protected Method(string name)
    {
      Name = name;
    }

    public string Name { get; }

    public abstract bool IsMatch(IEnumerable<Type> parameters, Type[] genericArguments, Type returnType);

    internal abstract void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo);

    public void Intercept(IInvocation invocation)
    {
      Invoke(value => invocation.ReturnValue = value, invocation.Arguments, invocation.GenericArguments, invocation.Method);
    }
  }
}