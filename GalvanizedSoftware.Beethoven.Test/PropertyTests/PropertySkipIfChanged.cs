using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extentions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertySkipIfSame
  {

    [TestMethod]
    public void TestMethodPropertySkipIfSame1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .SkipIfSame()
          .DelegatedSetter(value => setCount++));
      test.Property1 = 5;
      test.Property1 = 5;
      Assert.AreEqual(1, setCount);
    }

    [TestMethod]
    public void TestMethodPropertySkipIfSame2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .DelegatedSetter(value => setCount++)
          .SkipIfSame());
      test.Property1 = 5;
      test.Property1 = 5;
      Assert.AreEqual(2, setCount);
    }

    [TestMethod]
    public void TestMethodPropertySkipIfSame3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .SkipIfSame()
          .DelegatedSetter(value => setCount++));
      test.Property1 = 5;
      test.Property1 = 6;
      Assert.AreEqual(2, setCount);
    }
  }
}
