using System;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class LinkedMethodsReturnValueTests
  {
    [TestMethod]
    public void LinkedMethodsReturnValueTest1()
    {
      Logger logger = new Logger();
      CustomImplentation implementation = new CustomImplentation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.WithParameters))
          .MappedMethod(logger, nameof(logger.LogBefore))
          .MappedMethod(implementation, nameof(CustomImplentation.GetLength))
          .MappedMethod(logger, nameof(logger.LogAfter)));
      Assert.AreEqual(10, instance.WithParameters("w", "sd", 7));
      Assert.AreEqual(2, logger.Log.Count);
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest2()
    {
      Logger logger = new Logger();
      CustomImplentation implementation = new CustomImplentation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.OutAndRef))
          .AutoMappedMethod(implementation)
          .MappedMethod(implementation, nameof(implementation.OutAndRef1)));
      string text1 = "abc";
      Assert.AreEqual(20, instance.OutAndRef(out string text2, ref text1, 5));
      Assert.AreEqual("cba", text1);
      Assert.AreEqual("abc abc abc abc abc", text2);
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void LinkedMethodsReturnValueTest3()
    {
      Logger logger = new Logger();
      CustomImplentation implementation = new CustomImplentation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.WithParameters))
          .MappedMethod(logger, nameof(logger.LogBefore))
          .AutoMappedMethod(implementation)
          .MappedMethod(logger, nameof(logger.LogAfter)));
      Assert.Fail();
    }
  }
}
