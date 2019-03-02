using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable UnusedVariable

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class DefaultMethodTests
  {
    [TestMethod]
    public void TestMethodDefaultMethod1()
    {
      List<string> methodsCalled = new List<string>();
      object LogCall(MethodInfo methodInfo, object[] _)
      {
        methodsCalled.Add(methodInfo.Name);
        return null;
      }

      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(
        new DefaultMethod(LogCall));
      test.NoReturnValue("asd", "gggggdsss");
      CollectionAssert.AreEquivalent(new[] { "NoReturnValue" }, methodsCalled);
    }

    [TestMethod]
    public void TestMethodDefaultMethod2()
    {
      List<string> methodsCalled = new List<string>();
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(
        new DefaultMethod((methodInfo, _) => methodsCalled.Add(methodInfo.Name)));
      Assert.AreEqual(0, test.ReturnValue());
      CollectionAssert.AreEquivalent(new[] { "ReturnValue" }, methodsCalled);
    }

    [TestMethod]
    public void TestMethodDefaultMethod3()
    {
      List<string> methodsCalled = new List<string>();
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(
        new DefaultMethod((methodInfo, _) => methodsCalled.Add(methodInfo.Name)));
      test.Simple();
      test.ReturnValue();
      test.WithParameters("as", "wefewf");
      test.WithParameters("", null, 5);
      string b = "";
      test.OutAndRef(out string a, ref b, 5);
      int value = 0;
      test.Ref(ref value);
      test.GetMain("klkl", "kklklklkk");
      test.NoReturnValue("", "");
      CollectionAssert.AreEquivalent(new[]
        {
          "Simple",
          "ReturnValue",
          "WithParameters",
          "WithParameters",
          "OutAndRef",
          "Ref",
          "GetMain",
          "NoReturnValue"
        },
        methodsCalled);
    }

    [TestMethod]
    public void TestMethodDefaultMethod4()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(
        new DefaultMethod((methodInfo, parameters) =>
        {
          Type intRefType = typeof(int).MakeByRefType();
          ParameterInfo[] parameterInfos = methodInfo.GetParameters();
          foreach (ParameterInfo parameterInfo in parameterInfos.Where(info => info.ParameterType == intRefType))
            parameters[parameterInfo.Position] = 5;
        }));
      string b = "";
      test.OutAndRef(out string a, ref b, 5);
      int value = 0;
      test.Ref(ref value);
      Assert.AreEqual(5, value);
    }

    [TestMethod]
    public void TestMethodDefaultMethod5()
    {
      List<string> methodsCalled = new List<string>();
      object LogCall(MethodInfo methodInfo, object[] _)
      {
        methodsCalled.Add(methodInfo.Name);
        return null;
      }

      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(
        new LambdaMethod<Action>(nameof(ITestMethods.Simple), () => { }),
        new DefaultMethod(LogCall));
      test.Simple();
      CollectionAssert.AreEquivalent(new string[0], methodsCalled);
    }
  }
}
