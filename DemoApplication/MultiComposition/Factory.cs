﻿using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Events;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
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
    {// Make into TestMethod
      List<IPerson> persons = new List<IPerson>();
      CollectionChangedImplementation<IPerson> collectionChanged =
        new CollectionChangedImplementation<IPerson>(person => persons.IndexOf(person));
      TypeDefinition<IPersonCollection> typeDefinition =
        new TypeDefinition<IPersonCollection>(
          new LinkedObjects(
            new MappedMethod("Remove", collectionChanged, nameof(collectionChanged.PreRemove)),
            persons,
            collectionChanged));
      IPersonCollection personCollection = typeDefinition.Create();
      IEventTrigger trigger = new EventTrigger(personCollection, nameof(IPersonCollection.CollectionChanged));
      collectionChanged.CollectionChanged += (sender, args) => trigger.Notify(sender, args);
      return personCollection;
    }

    public IPerson CreatePerson() =>
      personTypeDefinition.Create();
  }
}
