using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Events;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class InstanceContainer<T> : IObjectProvider where T : class
  {
    private readonly IObjectProvider objectProviderHandler;

    public InstanceContainer(object[] partDefinitions,
      object[] parameters, EventInvokers eventInvokers, IEnumerable<InterceptorMap> wrapperFactories)
    {
      MasterInterceptor = new MasterInterceptor(
        new InstanceMap(partDefinitions, parameters), 
        wrapperFactories);
      EventInvokersBinding eventInvokersBinding = new EventInvokersBinding(this, eventInvokers);
      objectProviderHandler = new ObjectProviderHandler(
        partDefinitions
        .Append(new TargetBindingParent(this))
        .Append(eventInvokersBinding)
        .Append(MasterInterceptor));
    }

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