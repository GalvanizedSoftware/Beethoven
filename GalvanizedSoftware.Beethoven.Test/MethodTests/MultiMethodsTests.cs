using System;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class MultiMethodsTests
  {
    [TestMethod]
    public void MultiMethodsTest1()
    {
      MultiImplementationInt implentation = new MultiImplementationInt();
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(implentation);
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo(5);
      CollectionAssert.AreEquivalent(new[] { 5 }, implentation.CallList);
    }

    [TestMethod]
    public void MultiMethodsTest2()
    {
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          new LambdaMethod<Action<int>>("Foo", value => { }));
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo(5);
    }

    [TestMethod]
    public void MultiMethodsTest3()
    {
      int callCount = 0;
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          PartialMatchAction.Create("Foo", () => callCount++));
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo();
      instance.Foo(5);
      instance.Foo("5");
      Assert.AreEqual(3, callCount);
    }

    [TestMethod]
    public void MultiMethodsTest4()
    {
      int callCount = 0;
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          PartialMatchAction.Create("Foo", (int a) => callCount++)
          ,PartialMatchAction.Create("Foo", () => {}));
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo();
      instance.Foo(5);
      instance.Foo("5");
      Assert.AreEqual(3, callCount); // TODO: Change the code so this succeeds with 1
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void MultiMethodsTestInvalid1()
    {
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          new LambdaMethod<Action<string>>("Foo", value => { }));
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo(5);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void MultiMethodsTestInvalid2()
    {
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          new LambdaMethod<Action<short>>("Foo", value => { }));
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo(5);
    }
  }
}
