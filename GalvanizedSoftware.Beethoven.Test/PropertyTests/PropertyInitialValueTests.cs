using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyInitialValueTests
  {

    [TestMethod]
    public void TestMethodInitialValueTest1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
        .InitialValue(5)
        .SetterGetter());
      Assert.AreEqual(5, test.Property1);
    }

    [TestMethod]
    public void TestMethodInitialValueTest2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
        .InitialValue(5)
        .SetterGetter());
      test.Property1 = 50;
      Assert.AreEqual(50, test.Property1);
    }
  }
}
