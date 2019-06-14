using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Binding;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  internal class EventInvokers : IBindingParent
  {
    private readonly Dictionary<string, ActionEventInvoker> dictionary = new Dictionary<string, ActionEventInvoker>();
    private readonly ObjectProviderBinder<ITypeBinding<EventInvokers>> objectProviderBinder;

    public EventInvokers(IObjectProvider objectProvider)
    {
      objectProviderBinder = new ObjectProviderBinder<ITypeBinding<EventInvokers>>(objectProvider);
    }

    public ActionEventInvoker this[string name]
    {
      get
      {
        if (dictionary.TryGetValue(name, out ActionEventInvoker actionEventInvoker))
          return actionEventInvoker;
        actionEventInvoker = new ActionEventInvoker(name);
        dictionary.Add(name, actionEventInvoker);
        return actionEventInvoker;
      }
    }

    public void Bind(object target) => 
      objectProviderBinder.Bind(binding => binding.Bind(this));
  }
}