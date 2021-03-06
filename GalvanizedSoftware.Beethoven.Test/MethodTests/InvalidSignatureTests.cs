﻿using System;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class InvalidSignatureTests
  {
    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MethodSimpleInvalid()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      test.Simple();
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MethodReturnValueInvalid()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      test.ReturnValue();
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MethodWithParametersInvalid1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      test.WithParameters("123", "abc");
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MethodWithParametersInvalid2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      test.WithParameters("123", "abc", 5);
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MethodOutAndRefInvalid()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      string text1 = "abc";
      test.OutAndRef(out string _, ref text1, 5);
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MethodSimpleMissing()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new object());
      test.Simple();
      Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void MethodRefInvalid()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(new InvalidSignature());
      int value = 5;
      test.Ref(ref value);
      Assert.Fail();
    }
  }
}
