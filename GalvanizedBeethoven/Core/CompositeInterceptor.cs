using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core
{
  public class CompositeInterceptor : IInterceptor
  {
    private readonly IInterceptor[] interceptors;
    private readonly Method[] methods;

    public CompositeInterceptor(IInterceptor previous, params IInterceptor[] newInterceptors)
    {
      interceptors = (previous is CompositeInterceptor compositeInterceptor ?
          compositeInterceptor.interceptors.Concat(newInterceptors) :
          new[] { previous }.Concat(newInterceptors)).
        ToArray();
      methods = interceptors
        .OfType<Method>()
        .ToArray();
    }

    public void Intercept(IInvocation invocation)
    {
      MethodInfo methodInfo = invocation.Method;
      if (methodInfo.IsSpecialName)
      {
        foreach (IInterceptor interceptor in interceptors)
          interceptor.Intercept(invocation);
        return;
      }
      (Type, string)[] parameterTypes = methodInfo.GetParameterTypeAndNames();
      methods.FirstOrDefault(
        method => method.MethodMatcher.IsMatch(parameterTypes, invocation.GenericArguments, methodInfo.ReturnType))?
        .Intercept(invocation);
    }
  }
}
