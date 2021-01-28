using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
          .Action(() => log.Add("Before"))
          .AutoMappedMethod(implementation)
          .Action(() => log.Add("After")));
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
          .Action((int value) => { }));
      instance.Simple();
      Assert.Fail();
    }

    [TestMethod]
    public void LinkedMethodsTest3()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.WithParameters))
          .SkipIf<string, string>((text1, text2) => string.IsNullOrEmpty(text1))
          .SkipIf<string, string>((text1, text2) => string.IsNullOrEmpty(text2))
          .Func((string text1, string text2) => text1.Length + text2.Length));
      //Assert.AreEqual(0, instance.WithParameters(null, null));
      //Assert.AreEqual(0, instance.WithParameters("", "dsfgdsfhsd"));
      //Assert.AreEqual(0, instance.WithParameters("gjgkffg", ""));
      Assert.AreEqual(15, instance.WithParameters("fdsfd", "dsfgdsfhsd"));
    }

    [TestMethod]
    public void LinkedMethodsTest4()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      bool skip = true;
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.ReturnValue))
          .SkipIf(() => skip)
          .Func(() => 477));
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
          .Action(() => calledCount++));
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
          .Action(delegate { calledCount++; }));
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
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.WithParameters))
          .SkipIf(valueCheck, nameof(valueCheck.HasNoValue2))
          .Func((string text1, string text2) => calledCount++));
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
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.WithParameters), 1)
          .Func((int count) => count)
          .SkipIfResultCondition((int count) => count == 0)
          .AutoMappedMethod(implentation)
          .Action(() => calledCount++));
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
      const string name = nameof(ITestMethods.Simple);
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedObjects(
            ActionMethod.Create(name, () => log.Add("Before")),
            new MappedMethod(name, implementation),
            ActionMethod.Create(name, () => log.Add("After"))));
      instance.Simple();
      CollectionAssert.AreEquivalent(new[] { "Before", "After" }, log);
    }

    [TestMethod]
    public void LinkedMethodsTest10()
    {
      List<string> log = new List<string>();
      SimpleImplementation implementation = new SimpleImplementation();
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        new LinkedObjects(
          ActionMethod.Create("Simple", () => log.Add("Before")),
          implementation,
          ActionMethod.Create("Simple", () => log.Add("After"))));
      instance.Simple();
      CollectionAssert.AreEquivalent(new[] { "Before", "After" }, log);
    }


    [TestMethod]
    public void LinkedMethodsTest11()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      WithParametersImplementation implentation = new WithParametersImplementation();
      ITestMethods instance = beethovenFactory.Generate<ITestMethods>(
        LinkedMethodsReturnValue.Create<ITestMethods>(nameof(ITestMethods.WithParameters), 1)
          .AutoMappedMethod(implentation)
          .Func(() => 5));
      int result = instance.WithParameters("fdgdf", "afasf", 3);
      Assert.AreEqual(5, result);
    }

    [TestMethod]
    public void LinkedMethodsTest12()
    {
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      IGenericMethods instance = beethovenFactory.Generate<IGenericMethods>(
        LinkedMethodsReturnValue.Create<IGenericMethods>(nameof(IGenericMethods.Simple))
          .Func(() => true)
          .FlowControl(() => false)
          .Func(() => false));
      Assert.AreEqual(true, instance.Simple<bool>());
    }

    [TestMethod]
    public void LinkedMethodsTest13()
    {
      List<string> log = new List<string>();
      List<int> implementation = new List<int> { 5, 2, 17 };
      BeethovenFactory beethovenFactory = new BeethovenFactory();
      IEnumerable<int> instance = beethovenFactory.Generate<IEnumerable<int>>(
        new LinkedObjects(
          ActionMethod.Create("GetEnumerator", () => log.Add("Before")),
          implementation,
          ActionMethod.Create("GetEnumerator", () => log.Add("After"))));
      CollectionAssert.AreEqual(new[] { 5, 2, 17 }, instance.ToArray());
      CollectionAssert.AreEquivalent(new[] { "Before", "After" }, log);
    }
  }
}
