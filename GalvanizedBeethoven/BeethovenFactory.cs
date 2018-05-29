using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Events;
using GalvanizedSoftware.Beethoven.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven
{
  public sealed class BeethovenFactory
  {
    private static readonly ProxyGenerator generator = new ProxyGenerator();
    private readonly Dictionary<WeakReference, EventInvokers> generatedObjects = new Dictionary<WeakReference, EventInvokers>();

    public T Generate<T>(params object[] partDefinitions) where T : class
    {
      InstanceContainer<T> instanceContainer = new InstanceContainer<T>(partDefinitions);
      IInterceptor interceptor = instanceContainer.GetMaster<IInterceptor>();
      T target = typeof(T).IsInterface ?
        generator.CreateInterfaceProxyWithoutTarget<T>(interceptor) :
        generator.CreateClassProxy<T>(interceptor);
      instanceContainer.Bind(target);
      generatedObjects.Add(new WeakReference(target), instanceContainer.EventInvokers);
      return target;
    }

    public IEventTrigger CreateEventTrigger(object mainObject, string name)
    {
      return generatedObjects.Single(pair => pair.Key.Target == mainObject).Value[name];
    }
  }
}