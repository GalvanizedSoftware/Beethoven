using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Events;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class InstanceContainer<T> : IObjectProvider where T : class
  {
    private readonly Dictionary<Type, object> masters = new Dictionary<Type, object>();
    private readonly IObjectProvider objectProviderHandler;
    private readonly PropertiesSignatureChecker<T> propertiesSignatureChecker = new PropertiesSignatureChecker<T>();

    public InstanceContainer(object[] partDefinitions, List<object> wrappers)
    {
      AddMaster(new TargetBindingParent(this));
      EventInvokers = AddMaster(new EventInvokers(this));
      propertiesSignatureChecker.CheckSignatures(wrappers);
      MasterInterceptor masterInterceptor = new MasterInterceptor(
        new WrapperFactories(wrappers),
        new EventsFactory<T>(EventInvokers));
      AddMaster<IInterceptor>(masterInterceptor);
      objectProviderHandler = new ObjectProviderHandler(
        masters.Values
          .Concat(partDefinitions.OfType<IBindingParent>()));
    }

    public EventInvokers EventInvokers { get; }

    public IEnumerable<TChild> Get<TChild>() => objectProviderHandler.Get<TChild>();

    internal void Bind(T target)
    {
      foreach (IBindingParent bindingParent in Get<IBindingParent>())
        bindingParent.Bind(target);
    }

    internal TMaster GetMaster<TMaster>() =>
      masters.TryGetValue(typeof(TMaster), out object value) ? (TMaster)value : default;

    private TMaster AddMaster<TMaster>(TMaster instance)
    {
      masters.Add(typeof(TMaster), instance);
      return instance;
    }
  }
}