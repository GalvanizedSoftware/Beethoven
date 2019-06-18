using System;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
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
      ITestMethods unused = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.WithParameters))
          .MappedMethod(logger, nameof(logger.LogBefore))
          .AutoMappedMethod(implementation)
          .MappedMethod(logger, nameof(logger.LogAfter)));
      Assert.Fail();
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest4()
    {
      CustomImplentation implementation = new CustomImplentation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.OutAndRef))
          .MappedMethod(implementation, nameof(CustomImplentation.OutAndRef))
          .InvertResult()
          .Action(Assert.Fail));
      string text2 = "wetwt";
      instance.OutAndRef(out string _, ref text2, 5);
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest5()
    {
      CustomImplentation implementation = new CustomImplentation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      bool called = false;
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.OutAndRef))
          .MappedMethod(implementation, nameof(CustomImplentation.OutAndRef))
          .Action(() => called = true));
      string text2 = "wetwt";
      instance.OutAndRef(out string _, ref text2, 5);
      Assert.IsTrue(called);
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest6()
    {
      CustomImplentation implementation = new CustomImplentation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      bool called = false;
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.OutAndRef))
          .MappedMethod(implementation, nameof(CustomImplentation.OutAndRef))
          .FlowControl(() => false)
          .Action(() => called = true));
      string text2 = "wetwt";
      instance.OutAndRef(out string _, ref text2, 5);
      Assert.IsFalse(called);
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest7()
    {
      CustomImplentation implementation = new CustomImplentation();
      ConstructorParameter parameter = ConstructorParameter.Create<BoolContainer>("container");
      TypeDefinition<ITestMethods> typeDefinition = new TypeDefinition<ITestMethods>(
          parameter,
          new LinkedMethodsReturnValue(nameof(ITestMethods.OutAndRef))
            .MappedMethod(implementation, nameof(CustomImplentation.OutAndRef))
            .Action<BoolContainer>(container => container.Value = true, parameter));
      BoolContainer boolContainer = new BoolContainer();
      ITestMethods instance = typeDefinition.Create(boolContainer);
      BoolContainer boolContainer2 = new BoolContainer();
      typeDefinition.Create(boolContainer2);
      string text2 = "wetwt";
      instance.OutAndRef(out string _, ref text2, 5);
      Assert.IsTrue(boolContainer.Value);
      Assert.IsFalse(boolContainer2.Value);
    }
  }
}