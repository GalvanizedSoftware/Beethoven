using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal class CompositeInterceptor : IGeneralInterceptor
  {
    private readonly IGeneralInterceptor[] interceptors;
    private readonly Method[] methods;

    public CompositeInterceptor(IGeneralInterceptor previous,
      params IGeneralInterceptor[] newInterceptors)
    {
      interceptors = (previous is CompositeInterceptor compositeInterceptor ?
          compositeInterceptor.interceptors.Concat(newInterceptors) :
          new[] { previous }.Concat(newInterceptors)).
        ToArray();
      methods = interceptors
        .Select(interceptor => interceptor.MainDefinition)
        .OfType<Method>()
        .ToArray();
    }

    public void Invoke(InstanceMap instanceMap, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      if (methodInfo.IsSpecialName)
      {
        foreach (IGeneralInterceptor interceptor in interceptors)
          interceptor.Invoke(instanceMap, returnAction, parameters, genericArguments, methodInfo);
        return;
      }
      (Type, string)[] parameterTypes = methodInfo.GetParameterTypeAndNames();
      methods.FirstOrDefault(
        method => method.MethodMatcher.IsMatch(parameterTypes, genericArguments, methodInfo.ReturnType))?
        .InvokeFindInstance(instanceMap, returnAction, parameters, genericArguments, methodInfo);
    }

    public object MainDefinition => null;
  }
}
