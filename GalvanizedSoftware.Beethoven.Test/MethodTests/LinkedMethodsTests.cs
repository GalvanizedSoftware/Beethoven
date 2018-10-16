﻿using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
// ReSharper disable StringLiteralTypo

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

    [TestMethod]
    public void LinkedMethodsTest3()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.WithParameters))
          .SkipIf<string, string>((text1, text2) => string.IsNullOrEmpty(text1))
          .SkipIf<string, string>((text1, text2) => string.IsNullOrEmpty(text2))
          .Lambda<Func<string, string, int>>((text1, text2) => text1.Length + text2.Length));
      Assert.AreEqual(0, instance.WithParameters(null, null));
      Assert.AreEqual(0, instance.WithParameters("", "dsfgdsfhsd"));
      Assert.AreEqual(15, instance.WithParameters("fdsfd", "dsfgdsfhsd"));
    }

    // ReSharper disable AccessToModifiedClosure
    [TestMethod]
    public void LinkedMethodsTest4()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      bool skip = true;
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.ReturnValue))
          .SkipIf(() => skip)
          .Lambda<Func<int>>(() => 477));
      Assert.AreEqual(0, instance.ReturnValue());
      skip = false;
      Assert.AreEqual(477, instance.ReturnValue());
    }

    [TestMethod]
    public void LinkedMethodsTest5()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      bool skip = true;
      int calledCount = 0;
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethods(nameof(ITestMethods.Simple))
          .SkipIf(() => skip)
          .Lambda<Action>(() => calledCount++));
      instance.Simple();
      instance.Simple();
      Assert.AreEqual(0, calledCount);
      skip = false;
      instance.Simple();
      Assert.AreEqual(1, calledCount);
    }
    // ReSharper restore AccessToModifiedClosure
  }
}
