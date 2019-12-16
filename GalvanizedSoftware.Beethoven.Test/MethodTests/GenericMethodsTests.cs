using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class GenericMethodsTests
  {
    [TestMethod]
    public void GenericSimpleTest1()
    {
      GenericMethods genericMethods = new GenericMethods();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      IGenericMethods instance = beethovenFactory.Generate<IGenericMethods>(genericMethods);
      List<string> calledMethods = new List<string>();
      genericMethods.MethodCalled += s => calledMethods.Add(s);
      Assert.AreEqual(5, instance.Simple<int>());
      Assert.AreEqual(0, instance.Simple<short>());
      Assert.AreEqual("abcd", instance.Simple<string>());
      CollectionAssert.AreEquivalent(new[] { "Simple", "Simple", "Simple" }, calledMethods);
    }

    [TestMethod]
    public void GenericSimpleTest2()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      IGenericMethods instance = beethovenFactory.Generate<IGenericMethods>(
        FuncMethod.Create("Simple", () => "abcd"),
        FuncMethod.Create("Simple", () => 5),
        FuncMethod.Create("Simple", () => 0)
        );
      Assert.AreEqual(5, instance.Simple<int>());
      Assert.AreEqual(0, instance.Simple<short>());
      Assert.AreEqual("abcd", instance.Simple<string>());
    }

    [TestMethod]
    public void GenericSimpleTest3()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      IGenericMethods instance = beethovenFactory.Generate<IGenericMethods>(
        new SimpleFuncMethod<string>("Simple", () => "abcd"),
        new SimpleFuncMethod<int>("Simple", () => 5),
        new SimpleFuncMethod<short>("Simple", () => 0)
        );
      Assert.AreEqual(5, instance.Simple<int>());
      Assert.AreEqual(0, instance.Simple<short>());
      Assert.AreEqual("abcd", instance.Simple<string>());
    }

    [TestMethod]
    public void GenericSimpleTest4()
    {
      GenericMethods2 genericMethods2 = new GenericMethods2();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      IGenericMethods instance = beethovenFactory.Generate<IGenericMethods>(
        FuncMethod.Create("Simple", genericMethods2.SimpleString),
        FuncMethod.Create("Simple", genericMethods2.SimpleInt),
        genericMethods2);
      Assert.AreEqual(5, instance.Simple<int>());
      Assert.AreEqual("abcd", instance.Simple<string>());
    }
  }
}
