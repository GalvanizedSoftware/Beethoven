using GalvanizedSoftware.Beethoven.Generic.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class DefaultPropertyTests
  {
    [TestMethod]
    public void TestMethodDefaultPropertyValidityCheck1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .ValidityCheck(this, nameof(NonDefaultChecker)));
      test.Property1 = 5;
      test.Property2 = "";
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodDefaultPropertyValidityCheck2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .ValidityCheck(this, nameof(NonDefaultChecker)));
      test.Property1 = 0;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodDefaultPropertyValidityCheck3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .ValidityCheck(this, nameof(NonDefaultChecker)));
      test.Property2 = null;
    }

    public bool NonDefaultChecker<T>(T value)
    {
      return value != null && !value.Equals(default(T));
    }

    [TestMethod]
    public void TestMethodDefaultPropertyConstant1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .Constant(ConstantValue));
      Assert.AreEqual(0, test.Property1);
      Assert.AreEqual("Unknown", test.Property2);
    }

    public object ConstantValue(Type type)
    {
      if (type == typeof(string))
        return "Unknown";
      if (type.IsValueType)
        return Activator.CreateInstance(type);
      return null;
    }
  }
}
