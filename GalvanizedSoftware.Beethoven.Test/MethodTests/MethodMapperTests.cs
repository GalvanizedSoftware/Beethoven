using System.Reflection;
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
      CustomImplentation customImplentation = new CustomImplentation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
       new MappedMethod(nameof(ITestMethods.WithParameters), customImplentation, nameof(customImplentation.GetLength)));
      Assert.AreEqual(10, instance.WithParameters("w", "sd", 7));
    }

    [TestMethod]
    [ExpectedException(typeof(TargetParameterCountException))]
    public void MethodMapperSimpleTest2()
    {
      CustomImplentation customImplentation = new CustomImplentation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new MappedMethod(nameof(ITestMethods.WithParameters), 
          customImplentation, nameof(customImplentation.GetLength2)));
      Assert.AreEqual(10, instance.WithParameters("w", "sd", 7));
    }
  }
}
