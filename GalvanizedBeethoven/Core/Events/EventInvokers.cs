using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  internal class EventInvokers
  {
    private readonly Dictionary<string, ActionEventInvoker> dictionary = new Dictionary<string, ActionEventInvoker>();

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
  }
}