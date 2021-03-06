﻿using System;
using GalvanizedSoftware.Beethoven.Generic.Fields;
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
      CustomImplementation implementation = new CustomImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.WithParameters), 1)
          .MappedMethod(logger, nameof(logger.LogBefore))
          .MappedMethod(implementation, nameof(CustomImplementation.GetLength))
          .MappedMethod(logger, nameof(logger.LogAfter)));
      Assert.AreEqual(10, instance.WithParameters("w", "sd", 7));
      Assert.AreEqual(2, logger.Log.Count);
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest2()
    {
      CustomImplementation implementation = new CustomImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.OutAndRef))
          .MappedMethod(implementation, nameof(implementation.OutAndRef1))
          .AutoMappedMethod(implementation));
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
      CustomImplementation implementation = new CustomImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods unused = beethovenFactory.Generate<ITestMethods>(
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.WithParameters))
          .MappedMethod(logger, nameof(logger.LogBefore))
          .AutoMappedMethod(implementation)
          .MappedMethod(logger, nameof(logger.LogAfter)));
      Assert.Fail();
    }

		[TestMethod]
		public void LinkedMethodsReturnValueTest4()
		{
			CustomImplementation implementation = new CustomImplementation();
			BeethovenFactory beethovenFactory = new BeethovenFactory();
			ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
				LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.OutAndRef))
					.MappedMethod(implementation, nameof(CustomImplementation.OutAndRef))
					.Action(Assert.Fail));
			string text2 = "wetwt";
			instance.OutAndRef(out string _, ref text2, 5);
		}

		[TestMethod]
    public void LinkedMethodsReturnValueTest5()
    {
      CustomImplementation implementation = new CustomImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      bool called = false;
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.OutAndRef))
          .MappedMethod(implementation, nameof(CustomImplementation.OutAndRef1))
          .Action(() => called = true));
      string text2 = "wetwt";
      instance.OutAndRef(out string _, ref text2, 5);
      Assert.IsTrue(called);
    }

    [TestMethod]
    public void LinkedMethodsReturnValueTest6()
    {
      CustomImplementation implementation = new CustomImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      bool called = false;
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.OutAndRef))
          .MappedMethod(implementation, nameof(CustomImplementation.OutAndRef))
          .FlowControl(() => false)
          .Action(() => called = true));
      string text2 = "wetwt";
      instance.OutAndRef(out string _, ref text2, 5);
      Assert.IsFalse(called);
    }

    //[TestMethod]
    public void LinkedMethodsReturnValueTest7()
    {
      CustomImplementation implementation = new CustomImplementation();
      TypeDefinition<ITestMethods> typeDefinition = TypeDefinition<ITestMethods>.Create(FieldDefinition
          .CreateFromConstructorParameter<BoolContainer>()
          .ImportInMain(),
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.OutAndRef))
          .MappedMethod(implementation, nameof(CustomImplementation.OutAndRef)));
      BoolContainer boolContainer = new BoolContainer();
      ITestMethods instance = typeDefinition.CreateNew(boolContainer);
      BoolContainer boolContainer2 = new BoolContainer();
      typeDefinition.CreateNew(boolContainer2);
      string text2 = "wetwt";
      instance.OutAndRef(out string _, ref text2, 5);
      Assert.IsTrue(boolContainer.Value);
      Assert.IsFalse(boolContainer2.Value);
    }
  }
}