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
  }
}
