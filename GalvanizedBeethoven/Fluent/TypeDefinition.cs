using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Generic.Events;

namespace GalvanizedSoftware.Beethoven.Fluent
{
  public class TypeDefinition<T> where T : class
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();
    private readonly object[] implementationObjects;
    private readonly List<(string, Action<IEventTrigger>)> eventList = new List<(string, Action<IEventTrigger>)>();

    public TypeDefinition(params object[] newImplementationObjects)
    {
      implementationObjects = newImplementationObjects;
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newImplementationObjects)
    {
      beethovenFactory = previousDefinition.beethovenFactory;
      implementationObjects = previousDefinition.implementationObjects.Concat(newImplementationObjects).ToArray();
    }

    public TypeDefinition<T> Add(params object[] newImplementationObjects) =>
      new TypeDefinition<T>(this, newImplementationObjects);

    public void RegisterEvent(string name, Action<IEventTrigger> triggerFunc) =>
      eventList.Add(ValueTuple.Create(name, triggerFunc));

    public T Create()
    {
      T generated = beethovenFactory.Generate<T>(implementationObjects);
      eventList.ForEach(tuple => tuple.Item2(beethovenFactory.CreateEventTrigger(generated, tuple.Item1)));
      return generated;
    }

    public IEventTrigger CreateEventTrigger(object mainObject, string name) =>
      beethovenFactory.CreateEventTrigger(mainObject, name);
  }
}
