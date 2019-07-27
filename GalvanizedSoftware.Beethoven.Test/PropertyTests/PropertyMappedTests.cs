using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
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
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
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
        new PropertyDefinition<string>(nameof(ITestProperties.Property2))
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
        new PropertyDefinition<string>(nameof(ITestProperties.Property2))
          .MappedFrom(obj));
      ITestProperties test2 = factory.Generate<ITestProperties>(
        new PropertyDefinition<string>(nameof(ITestProperties.Property2))
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
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
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
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
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
        new PropertyDefinition<string>(nameof(ITestProperties.Property2))
          .MappedFrom(obj));
      Assert.AreEqual(null, test.Property2);
      obj.Property2 = "42";
      Assert.AreEqual("42", test.Property2);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped7()
    {
      IParameter parameter = ConstructorParameter.Create<MappedTestClass>();
      TypeDefinition<ITestProperties> typeDefinition = new TypeDefinition<ITestProperties>
      (
        parameter,
        new DefinitionImport(parameter)
      );
      ITestProperties test = typeDefinition.Create(new MappedTestClass());
      test.Property1 = 42;
      Assert.AreEqual(42, test.Property1);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped8()
    {
      IParameter parameter = ConstructorParameter.Create<MappedTestClass>();
      TypeDefinition<ITestProperties> typeDefinition = new TypeDefinition<ITestProperties>
      (
        parameter,
        new DefinitionImport(parameter)
      );
      MappedTestClass mappedTestClass = new MappedTestClass();
      ITestProperties test = typeDefinition.Create(mappedTestClass);
      test.Property1 = 42;
      Assert.AreEqual(42, mappedTestClass.Property1);
    }

    [TestMethod]
    public void TestMethodPropertyPropertyMapped9()
    {
      IParameter parameter = ConstructorParameter.Create<MappedTestClass>();
      TypeDefinition<ITestProperties> typeDefinition = new TypeDefinition<ITestProperties>
      (
        parameter,
        new DefinitionImport(parameter)
      );
      MappedTestClass mappedTestClass1 = new MappedTestClass();
      ITestProperties test1 = typeDefinition.Create(mappedTestClass1);
      MappedTestClass mappedTestClass2 = new MappedTestClass();
      ITestProperties test2 = typeDefinition.Create(mappedTestClass2);
      test1.Property1 = 42;
      Assert.AreEqual(0, test2.Property1);
      test2.Property1 = 2;
      Assert.AreEqual(42, test1.Property1);
    }
  }
}