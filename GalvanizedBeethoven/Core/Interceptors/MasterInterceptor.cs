using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal class MasterInterceptor : IInterceptor, IObjectProvider
  {
    private readonly InstanceMap instanceMap;
    private readonly Dictionary<string, IGeneralInterceptor> interceptorsMap = new Dictionary<string, IGeneralInterceptor>();
    private readonly IObjectProvider objectProviderHandler;

    public MasterInterceptor(InstanceMap instanceMap, params IEnumerable<InterceptorMap>[] interceptors)
    {
      this.instanceMap = instanceMap;
      foreach ((string name, IGeneralInterceptor interceptor) in interceptors.SelectMany(maps => maps))
      {
        if (interceptorsMap.TryGetValue(name, out IGeneralInterceptor currentInterceptor))
          interceptorsMap[name] = new CompositeInterceptor(currentInterceptor, interceptor);
        else
          interceptorsMap.Add(name, interceptor);
      }
      objectProviderHandler = new ObjectProviderHandler(interceptorsMap.Values);
    }

    public void Intercept(IInvocation invocation)
    {
      // find local instance from instanceMap
      MethodInfo methodInfo = invocation.Method;
      string methodName = methodInfo.Name;
      interceptorsMap
        .Where(pair => pair.Key == methodName)
        .Select(pair => pair.Value)
        .Single()
        .Invoke(
          instanceMap, 
          value => invocation.ReturnValue = value, 
          invocation.Arguments, 
          invocation.GenericArguments, 
          methodInfo);
    }

    public IEnumerable<TChild> Get<TChild>() => 
      objectProviderHandler.Get<TChild>();
  }
}