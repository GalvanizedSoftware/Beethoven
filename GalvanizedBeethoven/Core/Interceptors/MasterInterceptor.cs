using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
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
      string methodName = invocation.Method.Name;
      interceptorsMap
        .Where(pair => pair.Key == methodName)
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