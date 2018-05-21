using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class MethodTestInvalidSignature
  {
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MethodSimpleInvalid()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      test.Simple();
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MethodReturnValueInvalid()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      test.ReturnValue();
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MethodWithParametersInvalid1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      test.WithParameters("123", "abc");
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MethodWithParametersInvalid2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      test.WithParameters("123", "abc", 5);
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MethodOutAndRefInvalid()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      string text1 = "abc";
      test.OutAndRef(out string _, ref text1, 5);
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MethodSimpleMissing()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new object());
      test.Simple();
      Assert.Fail();
    }
  }
}
