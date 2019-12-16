using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyValidityCheckTests
  {
    [TestMethod]
    public void TestMethodPropertyValidityCheck1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
        .ValidityCheck(value => value % 2 == 0)
        .SetterGetter());
      Assert.AreEqual(0, test.Property1);
      test.Property1 = 10;
      Assert.AreEqual(10, test.Property1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodPropertyValidityCheck2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
          .ValidityCheck(value => value % 2 == 0));
      test.Property1 = 1;
      Assert.Fail();
    }
  }
}
