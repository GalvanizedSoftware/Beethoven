using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Events;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Test.CompositeTests.Interfaces;

namespace GalvanizedSoftware.Beethoven.Test.CompositeTests.Implementations
{
  internal class Factory
  {
    private readonly TypeDefinition<IPerson> personTypeDefinition =
      new TypeDefinition<IPerson>(
        new DefaultProperty().
          SkipIfEqual().
          SetterGetter().
          NotifyChanged());

    public IPersonCollection CreatePersonCollection()
    {
      List<IPerson> persons = new List<IPerson>();
      CollectionChangedImplementation<IPerson> collectionChanged =
        new CollectionChangedImplementation<IPerson>(person => persons.IndexOf(person));
      TypeDefinition<IPersonCollection> typeDefinition =
        new TypeDefinition<IPersonCollection>(
          new LinkedObjects(
            new MappedMethod("Remove", collectionChanged, nameof(collectionChanged.PreRemove)),
            persons,
            collectionChanged));
      IEventTrigger trigger = null;
      typeDefinition.RegisterEvent(
        nameof(IPersonCollection.CollectionChanged),
        eventTrigger => trigger = eventTrigger);
      IPersonCollection personCollection = typeDefinition.Create();
      collectionChanged.CollectionChanged += (sender, args) => trigger.Notify(sender, args);
      return personCollection;
    }

    public IPerson CreatePerson() =>
      personTypeDefinition.Create();

    public IPerson CreatePerson(string firstName, string lastName)
    {
      IPerson person = personTypeDefinition.Create();
      person.FirstName = firstName;
      person.LastName = lastName;
      return person;
    }
  }
}
