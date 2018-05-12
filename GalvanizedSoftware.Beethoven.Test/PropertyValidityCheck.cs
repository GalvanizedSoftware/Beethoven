using GalvanizedSoftware.Beethoven.Core.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalvanizedSoftware.Beethoven.Extentions;
using System;

namespace GalvanizedSoftware.Beethoven.Test
{
  [TestClass]
  public class PropertyValidityCheck
  {
    [TestMethod]
    public void TestMethodPropertyValidityCheck1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
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
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
          .ValidityCheck(value => value % 2 == 0));
      test.Property1 = 1;
      Assert.Fail();
    }
  }
}
