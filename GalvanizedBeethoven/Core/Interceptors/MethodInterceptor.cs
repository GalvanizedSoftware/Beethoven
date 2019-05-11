using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal abstract class MethodInterceptor : IInterceptor
  {
    internal abstract void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo);

    public void Intercept(IInvocation invocation) =>
      Invoke(value => invocation.ReturnValue = value, invocation.Arguments, invocation.GenericArguments, invocation.Method);
  }
}
