﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Methods;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class MasterInterceptor : IInterceptor, IObjectProvider
  {
    private readonly Dictionary<string, IInterceptor> interceptorsMap = new Dictionary<string, IInterceptor>();
    private readonly IObjectProvider objectProviderHandler;

    public MasterInterceptor(params IEnumerable<InterceptorMap>[] interceptors)
    {
      foreach (InterceptorMap interceptorMap in interceptors.SelectMany(maps => maps))
      {
        string name = interceptorMap.Item1;
        IInterceptor newInterceptor = interceptorMap.Item2;
        if (interceptorsMap.TryGetValue(name, out IInterceptor interceptor))
          interceptorsMap[name] = new CompositeInterceptor(interceptor, newInterceptor);
        else
          interceptorsMap.Add(name, newInterceptor);
      }
      objectProviderHandler = new ObjectProviderHandler(interceptorsMap.Values);
    }

    public void Intercept(IInvocation invocation)
    {
      interceptorsMap
        .Where(pair => pair.Key == invocation.Method.Name)
        .Select(pair => pair.Value)
        .Single()
        .Intercept(invocation);
    }

    public IEnumerable<TChild> Get<TChild>()
    {
      return objectProviderHandler.Get<TChild>();
    }
  }
}