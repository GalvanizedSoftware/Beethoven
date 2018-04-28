using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core;

namespace GalvanizedSoftware.Beethoven
{
  public sealed class BeethovenFactory
  {
    private static readonly ProxyGenerator generator = new ProxyGenerator();

    public T Generate<T>(params object[] partDefinitions) where T : class
    {
      InstanceContainer<T> instanceContainer = new InstanceContainer<T>(partDefinitions);
      IInterceptor interceptor = instanceContainer.GetMaster<IInterceptor>();
      T target = typeof(T).IsInterface ?
        generator.CreateInterfaceProxyWithoutTarget<T>(interceptor) :
        generator.CreateClassProxy<T>(interceptor);
      instanceContainer.Bind(target);
      return target;
    }
  }
}