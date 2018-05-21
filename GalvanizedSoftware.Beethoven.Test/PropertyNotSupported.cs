using GalvanizedSoftware.Beethoven.Core.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalvanizedSoftware.Beethoven.Extentions;
using System;

namespace GalvanizedSoftware.Beethoven.Test
{
  [TestClass]
  public class PropertyNotSupported
  {
    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void TestMethodPropertyNotSupported1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .NotSupported());
      test.Property1 = 42;
      Assert.Fail("No exception");
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void TestMethodPropertyNotSupported2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<string>(nameof(ITestProperties.Property2))
          .NotSupported());
      Assert.AreNotEqual("abc", test.Property2);
      Assert.Fail("No exception");
    }

    [TestMethod]
    public void TestMethodProperty3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<string>(nameof(ITestProperties.Property2))
          .SetterGetter());
      ITestProperties test2 = factory.Generate<ITestProperties>(
        new Property<string>(nameof(ITestProperties.Property2))
          .SetterGetter());
      test.Property2 = "abc";
      Assert.AreEqual(null, test2.Property2);
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void TestMethodProperty4()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property2))
          .SetterGetter());
      Assert.AreEqual(null, test.Property2);
    }
  }
}
