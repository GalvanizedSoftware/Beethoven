using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyDelegatedSetterTests
  {

    [TestMethod]
    public void TestMethodProperty1()
    {
      int setValue = 0;
      Action<int> action = value => setValue = value;
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .DelegatedSetter(action));
      test.Property1 = 5;
      Assert.AreEqual(5, setValue);
    }

    [TestMethod]
    public void TestMethodProperty2()
    {
      int setCount = 0;
      Action<int> action = value => setCount++;
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .DelegatedSetter(action));
      test.Property1 = 5;
      Assert.AreEqual(1, setCount);
    }

    [TestMethod]
    public void TestMethodProperty3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .DelegatedSetter(value => { }));
      test.Property1 = 5;
      Assert.AreEqual(0, test.Property1);
    }
  }
}
