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

    public void Invoke(InstanceMap instanceMap, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      if (methodInfo.IsSpecialName)
        foreach (IGeneralInterceptor interceptor in interceptors)
          interceptor.Invoke(instanceMap, ref returnValue, parameters, genericArguments, methodInfo);
      else
        methods.FirstOrDefault(method =>
            method.MethodMatcher
              .IsMatch(methodInfo.GetParameterTypeAndNames(), genericArguments, methodInfo.ReturnType))?
          .InvokeFindInstance(instanceMap, ref returnValue, parameters, genericArguments, methodInfo);
    }

    public object MainDefinition => null;
  }
}
