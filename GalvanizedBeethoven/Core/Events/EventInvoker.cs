using GalvanizedSoftware.Beethoven.Core.Binding;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  internal class EventInvoker : ITypeBinding<EventInvokers>
  {
    private readonly string name;
    private ActionEventInvoker actionEventNotifier;

    public EventInvoker(string name)
    {
      this.name = name;
    }

    public void Bind(EventInvokers eventInvokers)
    {
      actionEventNotifier = eventInvokers[name];
    }

    public object Invoke(params object[] args)
    {
      return actionEventNotifier.Notify(args);
    }
  }
}