using GalvanizedSoftware.Beethoven.Core.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalvanizedSoftware.Beethoven.Extentions;
using System;

namespace GalvanizedSoftware.Beethoven.Test
{
  [TestClass]
  public class PropertySetterGetter
  {
    [TestMethod]
    public void TestMethodProperty1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
        .SetterGetter());
      Assert.AreEqual(0, test.Property1);
      test.Property1 = 42;
      Assert.AreEqual(42, test.Property1);
    }

    [TestMethod]
    public void TestMethodProperty2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<string>(nameof(ITest.Property2))
          .SetterGetter());
      Assert.AreEqual(null, test.Property2);
      test.Property2 = "abc";
      Assert.AreEqual("abc", test.Property2);
    }

    [TestMethod]
    public void TestMethodProperty3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<string>(nameof(ITest.Property2))
          .SetterGetter());
      ITest test2 = factory.Generate<ITest>(
        new Property<string>(nameof(ITest.Property2))
          .SetterGetter());
      test.Property2 = "abc";
      Assert.AreEqual(null, test2.Property2);
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void TestMethodProperty4()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property2))
          .SetterGetter());
      Assert.AreEqual(null, test.Property2);
    }
  }
}
