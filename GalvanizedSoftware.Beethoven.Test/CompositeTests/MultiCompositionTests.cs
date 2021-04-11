using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GalvanizedSoftware.Beethoven.Test.CompositeTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.CompositeTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.CompositeTests
{
  [TestClass]
  public sealed class MultiCompositionTests
  {
    [TestMethod]
    public void MultiCompositionTest1()
    {
      Factory factory = new Factory();
      IPersonCollection personCollection = factory.CreatePersonCollection();
      personCollection.Add(factory.CreatePerson("John", "Smith"));
      CollectionAssert.AreEquivalent(
        new[] { "John Smith" },
        personCollection.Select(person => $"{person.FirstName} {person.LastName}").ToArray());
    }

    [TestMethod]
    public void MultiCompositionTest2()
    {
      Factory factory = new Factory();
      IPersonCollection personCollection = factory.CreatePersonCollection();
      List<IPerson> added = new List<IPerson>();
      personCollection.CollectionChanged += (_, args) => added.AddRange(args.NewItems.OfType<IPerson>());
      IPerson person = factory.CreatePerson("John", "Smith");
      personCollection.Add(person);
      Assert.AreEqual(person, added.Single());
    }

    [TestMethod]
    public void MultiCompositionTest3()
    {
      Factory factory = new Factory();
      IPersonCollection personCollection = factory.CreatePersonCollection();
      personCollection.Add(factory.CreatePerson("John", "Smith"));
      IPerson person = factory.CreatePerson("Alice", "Smith");
      personCollection.Add(person);
      personCollection.Add(factory.CreatePerson("Johnny", "Smith"));
      personCollection.Add(factory.CreatePerson("JJ", "Smith"));
      int removedIndex = -1;
      personCollection.CollectionChanged += (_, args) => removedIndex = args.OldStartingIndex;
      personCollection.Remove(person);
      Assert.IsFalse(personCollection.Any(person1 => person1.FirstName == "Alice"));
      Assert.AreEqual(1, removedIndex);
    }

    [TestMethod]
    public void MultiCompositionTest4()
    {
      Factory factory = new Factory();
      IPersonCollection personCollection = factory.CreatePersonCollection();
      personCollection.Add(factory.CreatePerson("John", "Smith"));
      personCollection.CollectionChanged += (sender, args) => Trace.WriteLine($"sender: {sender}, type: {args.Action}");
    }

    //[TestMethod]
    public void MultiCompositionTest5()
    {
      Factory factory = new();
      IPersonCollection personCollection = factory.CreatePersonCollection();
      personCollection.Add(factory.CreatePerson("John", "Smith"));
      IPerson person = factory.CreatePerson("Alice", "Smith");
      personCollection.Add(person);
      personCollection.Add(factory.CreatePerson("Johnny", "Smith"));
      personCollection.Add(factory.CreatePerson("JJ", "Smith"));
      object instance = null;
      personCollection.CollectionChanged += (currentInstance, _) => instance = currentInstance;
      personCollection.Remove(person);
      Assert.IsNotNull(instance);
      Assert.AreEqual(personCollection, instance);
    }
  }
}
