using System;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class MethodMapperTests
  {
    [TestMethod]
    public void MethodMapperSimpleTest1()
    {
      CustomImplementation customImplementation = new CustomImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
       new MappedMethod(nameof(ITestMethods.WithParameters), customImplementation, nameof(customImplementation.GetLength)));
      Assert.AreEqual(10, instance.WithParameters("w", "sd", 7));
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MethodMapperSimpleTest2()
    {
      CustomImplementation customImplementation = new CustomImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new MappedMethod(nameof(ITestMethods.WithParameters), 
          customImplementation, nameof(customImplementation.GetLength2)));
      Assert.AreEqual(10, instance.WithParameters("w", "sd", 7));
    }
  }
}
