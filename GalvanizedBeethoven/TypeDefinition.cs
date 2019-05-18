using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Generic.Events;

namespace GalvanizedSoftware.Beethoven
{
  public class TypeDefinition<T> where T : class
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();
    private readonly object[] partDefinitions;
    private readonly List<(string, Action<IEventTrigger>)> eventList = new List<(string, Action<IEventTrigger>)>();
    private readonly List<object> wrappers = new List<object>();

    public TypeDefinition(params object[] newPartDefinitions)
    {
      partDefinitions = newPartDefinitions;
      wrappers.AddRange(WrapperGenerator<T>.GetWrappers(partDefinitions));
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newPartDefinitions)
    {
      beethovenFactory = previousDefinition.beethovenFactory;
      partDefinitions = previousDefinition.partDefinitions.Concat(newPartDefinitions).ToArray();
      wrappers.AddRange(WrapperGenerator<T>.GetWrappers(newPartDefinitions));
    }

    public TypeDefinition<T> Add(params object[] newImplementationObjects) =>
      new TypeDefinition<T>(this, newImplementationObjects);

    public void RegisterEvent(string name, Action<IEventTrigger> triggerFunc) =>
      eventList.Add(ValueTuple.Create(name, triggerFunc));

    public T Create(params object[] parameters)
    {
      T generated = beethovenFactory.Create<T>(partDefinitions, wrappers, parameters);
      eventList.ForEach(tuple => tuple.Item2(beethovenFactory.CreateEventTrigger(generated, tuple.Item1)));
      return generated;
    }

    public IEventTrigger CreateEventTrigger(object mainObject, string name) =>
      beethovenFactory.CreateEventTrigger(mainObject, name);
  }
}
