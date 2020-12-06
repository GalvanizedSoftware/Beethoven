﻿using System.Collections.Generic;
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
      TypeDefinition<IPerson>.Create(new DefaultProperty().
        SkipIfEqual().
        SetterGetter().
        NotifyChanged());

    public IPersonCollection CreatePersonCollection()
    {
      List<IPerson> persons = new List<IPerson>();
      CollectionChangedImplementation<IPerson> collectionChanged =
        new CollectionChangedImplementation<IPerson>(person => persons.IndexOf(person));
      TypeDefinition<IPersonCollection> typeDefinition =
        TypeDefinition<IPersonCollection>.Create(new LinkedObjects(
          new MappedMethod("Remove", collectionChanged, nameof(collectionChanged.PreRemove)),
          persons,
          collectionChanged));
      IPersonCollection personCollection = typeDefinition.CreateNew();
      IEventTrigger trigger = new EventTrigger(personCollection, nameof(IPersonCollection.CollectionChanged));
      collectionChanged.CollectionChanged += (sender, args) => trigger.Notify(sender, args);
      return personCollection;
    }

    public IPerson CreatePerson() =>
      personTypeDefinition.CreateNew();

    public IPerson CreatePerson(string firstName, string lastName)
    {
      IPerson person = personTypeDefinition.CreateNew();
      person.FirstName = firstName;
      person.LastName = lastName;
      return person;
    }
  }
}
