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
    private readonly InterceptorsMap interceptorsMap = new InterceptorsMap();
    private readonly IObjectProvider objectProviderHandler;

    public MasterInterceptor(InstanceMap instanceMap, params IEnumerable<InterceptorMap>[] interceptors)
    {
      this.instanceMap = instanceMap;
      foreach ((MethodInfo memberInfo, IGeneralInterceptor interceptor) in interceptors.SelectMany(maps => maps))
        interceptorsMap.Add(memberInfo, interceptor);
      objectProviderHandler = new ObjectProviderHandler(interceptorsMap.Values);
    }

    public void Intercept(IInvocation invocation)
    {
      // find local instance from instanceMap
      MethodInfo methodInfo = invocation.Method;
      IGeneralInterceptor generalInterceptor = interceptorsMap.Find(methodInfo);
      if (generalInterceptor == null)
        throw new MissingMethodException($"{methodInfo} was not implemented");
      generalInterceptor
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