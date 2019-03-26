using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Generic;

// ReSharper disable StringLiteralTypo
// ReSharper disable AccessToModifiedClosure

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

    [TestMethod]
    public void LinkedMethodsTest6()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      int calledCount = 0;
      ValueCheck valueCheck = new ValueCheck();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethods(nameof(ITestMethods.NoReturnValue))
          .SkipIf(valueCheck, nameof(valueCheck.HasNoValue1))
          .Lambda<Action<string, string>>(delegate { calledCount++; }));
      instance.NoReturnValue("", "afasf");
      instance.NoReturnValue(null, "afasf");
      Assert.AreEqual(0, calledCount);
      instance.NoReturnValue("fdgdf", "afasf");
      Assert.AreEqual(1, calledCount);
    }

    [TestMethod]
    public void LinkedMethodsTest7()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      int calledCount = 0;
      ValueCheck valueCheck = new ValueCheck();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.WithParameters))
          .SkipIf(valueCheck, nameof(valueCheck.HasNoValue2))
          .Lambda<Func<string, string, int>>((arg1, arg2) => calledCount++));
      instance.WithParameters("", "");
      instance.WithParameters("fegf", null);
      Assert.AreEqual(0, calledCount);
      instance.WithParameters("fdgdf", "afasf");
      Assert.AreEqual(1, calledCount);
    }

    [TestMethod]
    public void LinkedMethodsTest8()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      int calledCount = 0;
      WithParametersImplementation implentation = new WithParametersImplementation();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedMethodsReturnValue(nameof(ITestMethods.WithParameters))
          .PartialMatchLambda<Func<int, int>>(count => count)
          .SkipIfResultCondition<int>(count => count == 0)
          .AutoMappedMethod(implentation)
          .PartialMatchLambda<Action>(() => calledCount++));
      int result1 = instance.WithParameters("fegf", "ggn", 0);
      Assert.AreEqual(0, calledCount);
      Assert.AreEqual(0, result1);
      int result2 = instance.WithParameters("fdgdf", "afasf", 3);
      Assert.AreEqual(1, calledCount);
      Assert.AreEqual(30, result2);
    }

    [TestMethod]
    public void LinkedMethodsTest9()
    {
      List<string> log = new List<string>();
      SimpleImplementation implementation = new SimpleImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      string name = nameof(ITestMethods.Simple);
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedObjects(
            new LambdaMethod<Action>(name, () => log.Add("Before")),
            new MappedMethod(name, implementation),
            new LambdaMethod<Action>(name, () => log.Add("After"))));
      instance.Simple();
      CollectionAssert.AreEquivalent(new[] { "Before", "After" }, log);
    }
  }
}
