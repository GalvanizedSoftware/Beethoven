using System;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Generic.Methods;
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
          ActionMethod.Create("Foo", (int a) => { }));
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo(5);
    }

    [TestMethod]
    public void MultiMethodsTest3()
    {
      int callCount = 0;
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          ActionMethod.Create("Foo", () => callCount++));
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
          ActionMethod.Create("Foo", (int a) => callCount++),
          ActionMethod.Create("Foo", () => {}).CreateFallback());
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo();
      instance.Foo(5);
      instance.Foo("5");
      instance.Foo(out string _);
      Assert.AreEqual(1, callCount);
    }

    [TestMethod]
    public void MultiMethodsTest5()
    {
      int callCount = 0;
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          ActionMethod.Create("Foo", (int a) => { }),
          ActionMethod.Create("Foo", () => callCount++).CreateFallback());
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo();
      instance.Foo(5);
      instance.Foo("5");
      instance.Foo(out string _);
      Assert.AreEqual(3, callCount);
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MultiMethodsTestInvalid1()
    {
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          ActionMethod.Create("Foo", (string value) => { }));
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo(5);
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MultiMethodsTestInvalid2()
    {
      TypeDefinition<IMultiMethods1> typeDefinition =
        new TypeDefinition<IMultiMethods1>(
          ActionMethod.Create("Foo", (short value) => { }));
      IMultiMethods1 instance = typeDefinition.Create();
      instance.Foo(5);
    }
  }
}
