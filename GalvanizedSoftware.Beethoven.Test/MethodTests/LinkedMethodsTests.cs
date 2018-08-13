using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class LinkedMethodsTests
  {
    [TestMethod]
    public void LinkedMethodsTest1()
    {
      List<string> log = new List<string>();
      SimpleImplementation implementation = new SimpleImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethods(nameof(ITestMethods.Simple))
          .Lambda<Action>(() => log.Add("Before"))
          .AutoMappedMethod(implementation)
          .Lambda<Action>(() => log.Add("After")));
      instance.Simple();
      CollectionAssert.AreEquivalent(new[] { "Before", "After" }, log);
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void LinkedMethodsTest2()
    {
      SimpleImplementation implementation = new SimpleImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethods(nameof(ITestMethods.Simple))
          .AutoMappedMethod(implementation)
          .Lambda<Action<int>>(value => { }));
      instance.Simple();
      Assert.Fail();
    }
  }
}
