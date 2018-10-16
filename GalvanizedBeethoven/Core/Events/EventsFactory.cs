using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  internal sealed class EventsFactory<T> : IEnumerable<InterceptorMap>
  {
    private readonly EventInvokers eventInvokers;
    private readonly Type masterType = typeof(T);

    public EventsFactory(EventInvokers eventInvokers)
    {
      this.eventInvokers = eventInvokers;
    }

    public IEnumerator<InterceptorMap> GetEnumerator()
    {
      return masterType.GetAllTypes().SelectMany(type => type.GetEvents()).Select(eventInfo => eventInfo.Name)
        .SelectMany(GetEventHandlers).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private IEnumerable<InterceptorMap> GetEventHandlers(string name)
    {
      ActionEventInvoker actionEventNotifier = eventInvokers[name];
      yield return new InterceptorMap("add_" + name, new EventAddInterceptor(actionEventNotifier));
      yield return new InterceptorMap("remove_" + name, new EventRemoveInterceptor(actionEventNotifier));
    }
  }
}