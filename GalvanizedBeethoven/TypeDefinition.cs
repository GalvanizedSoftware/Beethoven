using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Generic.Events;

namespace GalvanizedSoftware.Beethoven
{
  public class TypeDefinition<T> where T : class
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();
    private readonly object[] partDefinitions;
    private readonly List<(string, Action<IEventTrigger>)> eventList = new List<(string, Action<IEventTrigger>)>();

    public TypeDefinition(params object[] newPartDefinitions)
    {
      partDefinitions = newPartDefinitions;
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newPartDefinitions) :
      this(previousDefinition.partDefinitions.Concat(newPartDefinitions).ToArray())
    {
    }

    public CompiledTypeDefinition<T> Compile() =>
      new CompiledTypeDefinition<T>(beethovenFactory, partDefinitions, eventList);

    public TypeDefinition<T> Add(params object[] newImplementationObjects) =>
      new TypeDefinition<T>(this, newImplementationObjects);

    public void RegisterEvent(string name, Action<IEventTrigger> triggerFunc) =>
      eventList.Add((name, triggerFunc));


    public T Create(params object[] parameters)
    {
      return Compile().Create(parameters);
    }

    public IEventTrigger CreateEventTrigger(object mainObject, string name) =>
      beethovenFactory.CreateEventTrigger(mainObject, name);

    public TypeDefinition<T> AddMethodMapper<TChild>(Func<T, TChild> creatorFunc) =>
      new TypeDefinition<T>(this, new object[] { new MethodMapperCreator<T, TChild>(creatorFunc) });
  }
}
