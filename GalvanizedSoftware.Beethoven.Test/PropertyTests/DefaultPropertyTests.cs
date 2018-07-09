﻿using GalvanizedSoftware.Beethoven.Generic.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class DefaultPropertyTests
  {
    [TestMethod]
    public void TestMethodDefaultPropertyValidityCheck1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .ValidityCheck(this, nameof(NonDefaultChecker)));
      test.Property1 = 5;
      test.Property2 = "";
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodDefaultPropertyValidityCheck2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .ValidityCheck(this, nameof(NonDefaultChecker)));
      test.Property1 = 0;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethodDefaultPropertyValidityCheck3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .ValidityCheck(this, nameof(NonDefaultChecker)));
      test.Property2 = null;
    }

    public bool NonDefaultChecker<T>(T value)
    {
      return value != null && !value.Equals(default(T));
    }

    [TestMethod]
    public void TestMethodDefaultPropertySkipIfSame1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .SkipIfSame()
        .NotSupported());
      test.Property1 = 0;
    }

    [TestMethod]
    public void TestMethodDefaultPropertySetterGetter1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .SetterGetter());
      Assert.AreEqual(0, test.Property1);
      test.Property2 = "Some value";
      Assert.AreEqual("Some value", test.Property2);
      test.Property1 = 55;
      Assert.AreEqual(55, test.Property1);
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void TestMethodDefaultPropertyNotSupported1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .NotSupported());
      Assert.AreEqual(0, test.Property1);
    }

    [TestMethod]
    public void TestMethodDefaultPropertyNotifyChanged1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .NotifyChanged());
      List<string> changes = new List<string>();
      test.PropertyChanged += (sender, args) => changes.Add(args.PropertyName);
      test.Property1 = 5;
      test.Property2 = "5";
      CollectionAssert.AreEquivalent(
        new[] { "Property1", "Property2" },
        changes);
    }

    [TestMethod]
    public void TestMethodDefaultPropertyConstant1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .Constant(ConstantValue));
      Assert.AreEqual(0, test.Property1);
      Assert.AreEqual("Unknown", test.Property2);
    }

    public object ConstantValue(Type type)
    {
      if (type == typeof(string))
        return "Unknown";
      if (type.IsValueType)
        return Activator.CreateInstance(type);
      return null;
    }
  }
}
