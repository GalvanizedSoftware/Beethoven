using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertySkipIfEqualTests
  {

    [TestMethod]
    public void TestMethodPropertySkipIfEqual1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .SkipIfEqual()
          .DelegatedSetter(value => setCount++));
      test.Property1 = 5;
      test.Property1 = 5;
      Assert.AreEqual(1, setCount);
    }

    [TestMethod]
    public void TestMethodPropertySkipIfEqual2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .DelegatedSetter(value => setCount++)
          .SkipIfEqual());
      test.Property1 = 5;
      test.Property1 = 5;
      Assert.AreEqual(2, setCount);
    }

    [TestMethod]
    public void TestMethodPropertySkipIfEqual3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .SkipIfEqual()
          .DelegatedSetter(value => setCount++));
      test.Property1 = 5;
      test.Property1 = 6;
      Assert.AreEqual(2, setCount);
    }
  }
}
