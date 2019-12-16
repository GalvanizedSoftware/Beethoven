using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Interceptors;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  internal sealed class EventsFactory<T> : IEnumerable<IInterceptorProvider>
  {
    private readonly EventInvokers eventInvokers;
    private readonly Type masterType = typeof(T);

    public EventsFactory(EventInvokers eventInvokers)
    {
      this.eventInvokers = eventInvokers;
    }

    public IEnumerator<IInterceptorProvider> GetEnumerator() =>
      masterType
        .GetAllTypes()
        .SelectMany(type => type.GetEvents())
        .Select(eventInfo => eventInfo.Name)
        .Select(name => eventInvokers[name])
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}