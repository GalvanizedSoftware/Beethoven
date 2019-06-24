using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Events;
using GalvanizedSoftware.Beethoven.Generic.Events;

namespace GalvanizedSoftware.Beethoven
{
  public class CompiledTypeDefinition<T> where T : class
  {
    private readonly object[] partDefinitions;
    private readonly EventInvokers eventInvokers = new EventInvokers();
    private readonly InterceptorMap[] interceptorMaps;
    private readonly BeethovenFactory beethovenFactory;
    private readonly List<(string, Action<IEventTrigger>)> eventList;

    internal CompiledTypeDefinition(BeethovenFactory beethovenFactory, 
      object[] partDefinitions,
      List<(string, Action<IEventTrigger>)> eventList)
    {
      this.partDefinitions = partDefinitions;
      this.eventList = eventList;
      this.beethovenFactory = beethovenFactory;
      interceptorMaps = new WrapperFactories<T>(
        WrapperGenerator<T>.CreateAndCheckWrappers(partDefinitions), eventInvokers)
        .ToArray();
    }

    public T Create(params object[] parameters)
    {
      T generated = beethovenFactory.Create<T>(partDefinitions, parameters, eventInvokers, interceptorMaps);
      eventList.ForEach(tuple => tuple.Item2(beethovenFactory.CreateEventTrigger(generated, tuple.Item1)));
      return generated;
    }
  }
}
