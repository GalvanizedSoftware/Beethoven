using GalvanizedSoftware.Beethoven.Test.VariousTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.VariousTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests
{
  [TestClass]
  public class ImplementsTests
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    [TestMethod]
    public void ImplementsTest1()
    {
      Assert.IsTrue(beethovenFactory.Implements<ITestImplements, FullImplements>());
    }

    [TestMethod]
    public void ImplementsTest2()
    {
      Assert.IsFalse(beethovenFactory.Implements<ITestImplements, InvalidImplementation1>());
    }

    [TestMethod]
    public void ImplementsTest3()
    {
      Assert.IsFalse(beethovenFactory.Implements<ITestImplements, InvalidImplementation2>());
    }

    [TestMethod]
    public void ImplementsTest4()
    {
      Assert.IsFalse(beethovenFactory.Implements<ITestImplements, InvalidImplementation3>());
    }

    [TestMethod]
    public void ImplementsTest5()
    {
      Assert.IsFalse(beethovenFactory.Implements<ITestImplements, InvalidImplementation4>());
    }

    [TestMethod]
    public void ImplementsTest6()
    {
      ITestImplements duckWrapper = beethovenFactory.Generate<ITestImplements>(new FullImplements());
      duckWrapper.Property1 = 5;
      Assert.AreEqual(5, duckWrapper.Property1);
      duckWrapper.Property2 = "Some string";
      Assert.AreEqual("Some string", duckWrapper.Property2);
      duckWrapper.Method1(1, 2);
      string str = "";
      duckWrapper.Method2(ref str);
      duckWrapper.Method3();
    }

  }
}
