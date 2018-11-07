using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core
{
  public class CompositeInterceptor : IInterceptor
  {
    private readonly IInterceptor[] interceptors;
    private readonly Method[] methods;
    private readonly PropertyInterceptor[] properties;

    public CompositeInterceptor(IInterceptor previous, params IInterceptor[] newInterceptors)
    {
      interceptors = (previous is CompositeInterceptor compositeInterceptor ?
          compositeInterceptor.interceptors.Concat(newInterceptors) :
          new[] { previous }.Concat(newInterceptors)).
        ToArray();
      methods = interceptors
        .OfType<Method>()
        .ToArray();
      properties = interceptors
        .OfType<PropertyInterceptor>()
        .ToArray();
    }

    public void Intercept(IInvocation invocation)
    {
      MethodInfo methodInfo = invocation.Method;
      if (methodInfo.IsSpecialName)
      {
        IEnumerable<IInterceptor> matchingProperties = properties
          .Where(property => property.Property.IsMatch(methodInfo));
        foreach (IInterceptor interceptor in matchingProperties)
          interceptor.Intercept(invocation);
        return;
      }
      (Type, string)[] parameterTypes = methodInfo.GetParameterTypeAndNames();
      methods.FirstOrDefault(
        method => method.IsMatch(parameterTypes, invocation.GenericArguments, methodInfo.ReturnType))?
        .Intercept(invocation);
    }
  }
}
