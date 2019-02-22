using System;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Events;

namespace GalvanizedSoftware.Beethoven.Generic.Events
{
  internal class EventAction : ITypeBinding<EventInvokers>
  {
    private readonly EventInvoker<Action> eventInvoker;

    public EventAction(string name)
    {
      eventInvoker = new EventInvoker<Action>(name);
    }

    public void TriggerEvent()
    {
      eventInvoker?.Invoke();
    }

    public void Bind(EventInvokers master)
    {
      eventInvoker.Bind(master);
    }
  }
}