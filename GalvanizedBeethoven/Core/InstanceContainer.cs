using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Events;
using GalvanizedSoftware.Beethoven.Core.Properties;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class InstanceContainer<T> : IObjectProvider where T : class
  {
    private readonly IObjectProvider objectProviderHandler;
    private readonly PropertiesSignatureChecker<T> propertiesSignatureChecker = new PropertiesSignatureChecker<T>();

    public InstanceContainer(object[] partDefinitions, List<object> wrappers, object[] parameters)
    {
      propertiesSignatureChecker.CheckSignatures(wrappers);
      InstanceMap instanceMap = new InstanceMap(partDefinitions, parameters);
      EventInvokers = new EventInvokers(this);
      MasterInterceptor = new MasterInterceptor(
        instanceMap,
        new WrapperFactories<T>(wrappers, EventInvokers));
      objectProviderHandler = new ObjectProviderHandler(
        partDefinitions
        .Append(new TargetBindingParent(this))
        .Append(EventInvokers)
        .Append(MasterInterceptor));
    }

    public EventInvokers EventInvokers { get; }

    public IEnumerable<TChild> Get<TChild>() =>
      objectProviderHandler.Get<TChild>();

    internal void Bind(T target)
    {
      foreach (IBindingParent bindingParent in Get<IBindingParent>())
        bindingParent.Bind(target);
    }

    internal IInterceptor MasterInterceptor { get; }
  }
}