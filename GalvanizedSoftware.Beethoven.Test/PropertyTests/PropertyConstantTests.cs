using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyConstantTests
  {

    [TestMethod]
    public void TestMethodProperty1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .Constant(5));
      Assert.AreEqual(5, test.Property1);
    }

    [TestMethod]
    public void TestMethodProperty2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .Constant(5));
      test.Property1 = 5;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodProperty3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new Property<int>(nameof(ITestProperties.Property1))
        .Constant(5));
      test.Property1 = 42;
    }

    [TestMethod]
    public void TestMethodProperty4()
    {
      ITestProperties test = new TypeDefinition<ITestProperties>()
        .Add(new Property<int>(nameof(ITestProperties.Property1))
          .Constant(5))
        .Create();
      test.Property1 = 5;
    }
  }
}
