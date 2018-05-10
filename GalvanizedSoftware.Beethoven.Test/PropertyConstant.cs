using GalvanizedSoftware.Beethoven.Core.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalvanizedSoftware.Beethoven.Extentions;
using System;

namespace GalvanizedSoftware.Beethoven.Test
{
  [TestClass]
  public class PropertyConstant
  {

    [TestMethod]
    public void TestMethodProperty1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
        .Constant(5));
      Assert.AreEqual(5, test.Property1);
    }

    [TestMethod]
    public void TestMethodProperty2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
        .Constant(5));
      test.Property1 = 5;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodProperty3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
        .Constant(5));
      test.Property1 = 42;
    }
  }
}
