using GalvanizedSoftware.Beethoven.Core.Binding;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  internal class EventInvokersBinding : IBindingParent
  {
    private readonly ObjectProviderBinder<ITypeBinding<EventInvokers>> objectProviderBinder;
    private readonly EventInvokers eventInvokers;

    public EventInvokersBinding(IObjectProvider objectProvider, EventInvokers eventInvokers)
    {
      this.eventInvokers = eventInvokers;
      objectProviderBinder = new ObjectProviderBinder<ITypeBinding<EventInvokers>>(objectProvider);
    }

    public void Bind(object target) => 
      objectProviderBinder.Bind(binding => binding.Bind(eventInvokers));
  }
}