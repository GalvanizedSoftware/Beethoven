using System;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
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
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
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
        new PropertyDefinition<string>(nameof(ITestProperties.Property2))
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
        new PropertyDefinition<string>(nameof(ITestProperties.Property2))
          .SetterGetter());
      ITestProperties test2 = factory.Generate<ITestProperties>(
        new PropertyDefinition<string>(nameof(ITestProperties.Property2))
          .SetterGetter());
      test.Property2 = "abc";
      Assert.AreEqual(null, test2.Property2);
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void TestMethodProperty4()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property2))
          .SetterGetter());
      Assert.AreEqual(null, test.Property2);
    }

    [TestMethod]
    public void TestMethodProperty5()
    {
      TypeDefinition<ITestProperties> definition = new TypeDefinition<ITestProperties>()
        .Add(new PropertyDefinition<int>(nameof(ITestProperties.Property1))
        .SetterGetter());
      CompiledTypeDefinition<ITestProperties> compiledTypeDefinition = definition.Compile();
      ITestProperties test1 = compiledTypeDefinition.Create();
      ITestProperties test2 = compiledTypeDefinition.Create();
      test1.Property1 = 42;
      Assert.AreEqual(0, test2.Property1);
    }
  }
}
