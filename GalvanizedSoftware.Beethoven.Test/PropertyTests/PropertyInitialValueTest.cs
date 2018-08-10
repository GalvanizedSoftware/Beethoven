using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extentions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyInitialValueTest
  {

    [TestMethod]
    public void TestMethodInitialValueTest1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .InitialValue(5)
        .SetterGetter());
      Assert.AreEqual(5, test.Property1);
    }

    [TestMethod]
    public void TestMethodInitialValueTest2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .InitialValue(5)
        .SetterGetter());
      test.Property1 = 50;
      Assert.AreEqual(50, test.Property1);
    }
  }
}
