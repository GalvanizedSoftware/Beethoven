using GalvanizedSoftware.Beethoven.Test.VariousTests.Implementations;
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
  }
}
