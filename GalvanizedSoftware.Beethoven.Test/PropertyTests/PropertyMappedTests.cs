using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extentions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyMappedTests
  {
    [TestMethod]
    public void TestMethodPropertyPropertyMapped1()
    {
      MappedTestClass obj = new MappedTestClass();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .MappedFrom(obj));
      Assert.AreEqual(0, test.Property1);
      test.Property1 = 42;
      Assert.AreEqual(42, test.Property1);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped2()
    {
      MappedTestClass obj = new MappedTestClass();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<string>(nameof(ITestProperties.Property2))
          .MappedFrom(obj));
      Assert.AreEqual(null, test.Property2);
      test.Property2 = "abc";
      Assert.AreEqual("abc", test.Property2);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped3()
    {
      MappedTestClass obj = new MappedTestClass();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<string>(nameof(ITestProperties.Property2))
          .MappedFrom(obj));
      ITestProperties test2 = factory.Generate<ITestProperties>(
        new Property<string>(nameof(ITestProperties.Property2))
          .MappedFrom(obj));
      test.Property2 = "abc";
      Assert.AreEqual("abc", test2.Property2);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped4()
    {
      MappedTestClass obj = new MappedTestClass();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .MappedFrom(obj));
      Assert.AreEqual(0, obj.Property1);
      test.Property1 = 42;
      Assert.AreEqual(42, obj.Property1);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped5()
    {
      MappedTestClass obj = new MappedTestClass();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
          .MappedFrom(obj));
      Assert.AreEqual(0, test.Property1);
      obj.Property1 = 42;
      Assert.AreEqual(42, test.Property1);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped6()
    {
      MappedTestClass obj = new MappedTestClass();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<string>(nameof(ITestProperties.Property2))
          .MappedFrom(obj));
      Assert.AreEqual(null, test.Property2);
      obj.Property2 = "42";
      Assert.AreEqual("42", test.Property2);
    }

    public class MappedTestClass
    {
      public int Property1 { get; set; }
      internal string Property2 { get; set; }
    }
  }
}