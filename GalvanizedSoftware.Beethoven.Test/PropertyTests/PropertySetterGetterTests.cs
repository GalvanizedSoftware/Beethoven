using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extentions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertySetterGetterTests
  {
    [TestMethod]
    public void TestMethodProperty1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .SetterGetter());
      Assert.AreEqual(0, test.Property1);
      test.Property1 = 42;
      Assert.AreEqual(42, test.Property1);
    }

    [TestMethod]
    public void TestMethodProperty2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<string>(nameof(ITestProperties.Property2))
          .SetterGetter());
      Assert.AreEqual(null, test.Property2);
      test.Property2 = "abc";
      Assert.AreEqual("abc", test.Property2);
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
