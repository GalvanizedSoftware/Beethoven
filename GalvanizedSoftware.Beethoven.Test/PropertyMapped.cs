using GalvanizedSoftware.Beethoven.Core.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalvanizedSoftware.Beethoven.Extentions;

namespace GalvanizedSoftware.Beethoven.Test
{
  [TestClass]
  public class PropertyMapped
  {
    [TestMethod]
    public void TestMethodPropertyPropertyMapped1()
    {
      MappedTestClass obj = new MappedTestClass();
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
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
      ITest test = factory.Generate<ITest>(
        new Property<string>(nameof(ITest.Property2))
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
      ITest test = factory.Generate<ITest>(
        new Property<string>(nameof(ITest.Property2))
          .MappedFrom(obj));
      ITest test2 = factory.Generate<ITest>(
        new Property<string>(nameof(ITest.Property2))
          .MappedFrom(obj));
      test.Property2 = "abc";
      Assert.AreEqual("abc", test2.Property2);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped4()
    {
      MappedTestClass obj = new MappedTestClass();
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
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
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
          .MappedFrom(obj));
      Assert.AreEqual(0, test.Property1);
     obj.Property1 = 42;
      Assert.AreEqual(42, test.Property1);
    }
  }

  public class MappedTestClass
  {
    public int Property1 { get; set; }
    public string Property2 { get; set; }
  }
}
