using GalvanizedSoftware.Beethoven.Core.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalvanizedSoftware.Beethoven.Extentions;

namespace GalvanizedSoftware.Beethoven.Test
{
  [TestClass]
  public class PropertySkipIfChanged
  {

    [TestMethod]
    public void TestMethodPropertySkipIfChanged1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .SkipIfChanged()
          .DelegatedSetter(value => setCount++));
      test.Property1 = 5;
      test.Property1 = 5;
      Assert.AreEqual(1, setCount);
    }

    [TestMethod]
    public void TestMethodPropertySkipIfChanged2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .DelegatedSetter(value => setCount++)
          .SkipIfChanged());
      test.Property1 = 5;
      test.Property1 = 5;
      Assert.AreEqual(2, setCount);
    }

    [TestMethod]
    public void TestMethodPropertySkipIfChanged3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .SkipIfChanged()
          .DelegatedSetter(value => setCount++));
      test.Property1 = 5;
      test.Property1 = 6;
      Assert.AreEqual(2, setCount);
    }
  }
}
