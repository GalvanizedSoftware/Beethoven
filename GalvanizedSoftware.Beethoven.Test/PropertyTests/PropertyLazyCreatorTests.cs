using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyLazyCreatorTests
  {
    [TestMethod]
    public void TestMethodLazyCreatorProperty1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
          .LazyCreator(() => 5));
      Assert.AreEqual(5, test.Property1);
    }

    [TestMethod]
    public void TestMethodLazyCreatorProperty2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
          .LazyCreator(() =>
          {
            Assert.Fail();
            return 0;
          })
          .SetterGetter());
      test.Property1 = 7;
      Assert.AreEqual(7, test.Property1);
    }

    [TestMethod]
    public void TestMethodLazyCreatorProperty3()
    {
      int count = 0;
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
          .LazyCreator(() => ++count)
          .SetterGetter());
      Assert.AreEqual(1, test.Property1);
      Assert.AreEqual(1, test.Property1);
      Assert.AreEqual(1, test.Property1);
      Assert.AreEqual(1, count);
    }
  }
}
