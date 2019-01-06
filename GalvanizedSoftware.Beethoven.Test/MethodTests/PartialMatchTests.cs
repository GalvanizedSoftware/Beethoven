using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class PartialMatchTests
  {
    [TestMethod]
    public void LinkedMethodsReturnValueTest1()
    {
      PartialMethods partialMethods = new PartialMethods();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.WithParameters))
          .PartialMatchMethod(partialMethods, nameof(partialMethods.WithParameters1))
          .PartialMatchMethod(partialMethods, nameof(partialMethods.WithParameters2))
          .PartialMatchMethod(partialMethods, nameof(partialMethods.WithParametersCount))
          .PartialMatchMethod(partialMethods, nameof(partialMethods.WithParameters)));
      Assert.AreEqual(9, instance.WithParameters("w", "sd", 3));
      Assert.AreEqual(0, instance.WithParameters(null, "sd", 3));
      Assert.AreEqual(0, instance.WithParameters("w", "sd", -68));
      Assert.AreEqual(0, instance.WithParameters("w", "", 7));
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest2()
    {
      PartialMethods partialMethods = new PartialMethods();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.WithParameters))
          .PartialMatchMethod(partialMethods, nameof(partialMethods.WithParametersReturnValue)));
      Assert.AreEqual(5, instance.WithParameters("w", "sd", 3));
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest3()
    {
      PartialMethods partialMethods = new PartialMethods();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.GetMain))
          .PartialMatchMethod<ITestMethods>(partialMethods, "testMethods"));
      object actual = instance.GetMain("w", "sd");
      Assert.AreEqual(instance, actual);
    }
  }
}
