using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyRangeCheckTests
  {
    [TestMethod]
    public void TestMethodPropertyRangeCheck1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .RangeCheck(0, 42)
        .SetterGetter());
      Assert.AreEqual(0, test.Property1);
      test.Property1 = 42;
      Assert.AreEqual(42, test.Property1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodPropertyRangeCheck2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .RangeCheck(0, 42));
      test.Property1 = 43;
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodPropertyRangeCheck3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .RangeCheck(0, 42));
      test.Property1 = -1;
      Assert.Fail();
    }
  }
}
