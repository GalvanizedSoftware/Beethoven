using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Generic.Events;

namespace GalvanizedSoftware.Beethoven
{
  public class TypeDefinition<T> where T : class
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();
    private readonly object[] partDefinitions;
    private readonly List<(string, Action<IEventTrigger>)> eventList = new List<(string, Action<IEventTrigger>)>();
    private readonly List<object> wrappers;

    public TypeDefinition(params object[] newPartDefinitions)
    {
      partDefinitions = newPartDefinitions;
      wrappers = WrapperGenerator<T>.GetWrappers(partDefinitions).ToList();
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newPartDefinitions) :
      this(previousDefinition.partDefinitions.Concat(newPartDefinitions).ToArray())
    {
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

    public TypeDefinition<T> AddMethodMapper<TChild>(Func<T, TChild> creatorFunc) =>
      new TypeDefinition<T>(this, new object[] { new MethodMapperCreator<T, TChild>(creatorFunc) });
  }
}
