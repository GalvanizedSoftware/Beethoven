using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;

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
    public void TestMethodDefaultPropertySkipIfEqual1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .SkipIfEqual()
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
      return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    [TestMethod]
    public void TestMethodDefaultPropertyDelegated1()
    {
      DefaultImplementation implementation = new DefaultImplementation();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .DelegatedSetter(implementation, nameof(implementation.DelegatedSetter)));
      Assert.AreEqual(0, test.Property1);
      test.Property2 = "Nothing";
      test.Property2 = "Some value";
      test.Property1 = 55;
      CollectionAssert.AreEquivalent(new object[] { 55, "Some value" }, implementation.GetObjects());
    }

    [TestMethod]
    public void TestMethodDefaultPropertyDelegated2()
    {
      DefaultImplementation implementation = new DefaultImplementation();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .DelegatedSetter(implementation, nameof(implementation.DelegatedSetter))
        .SetterGetter());
      Assert.AreEqual(0, test.Property1);
      test.Property2 = "Nothing";
      test.Property2 = "Some value";
      Assert.AreEqual("Some value", test.Property2);
      test.Property1 = 55;
      Assert.AreEqual(55, test.Property1);
    }

    [TestMethod]
    public void TestMethodDefaultPropertyDelegated3()
    {
      DefaultImplementation2 implementation = new DefaultImplementation2();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .DelegatedSetter(implementation, nameof(implementation.DelegatedSetter)));
      Assert.AreEqual(0, test.Property1);
      test.Property2 = "Nothing";
      test.Property2 = "Some value";
      test.Property1 = 55;
      CollectionAssert.AreEquivalent(new object[] { 55, "Some value" }, implementation.GetObjects());
    }

    [TestMethod]
    public void TestMethodDefaultPropertyDelegated4()
    {
      DefaultImplementation2 implementation = new DefaultImplementation2();
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new DefaultProperty()
        .DelegatedSetter(implementation, nameof(implementation.DelegatedSetter))
        .DelegatedGetter(implementation, nameof(implementation.DelegatedGetter)));
      Assert.AreEqual(0, test.Property1);
      test.Property2 = "Nothing";
      test.Property2 = "Some value";
      Assert.AreEqual("Some value", test.Property2);  
      test.Property1 = 55;
      Assert.AreEqual(55, test.Property1);
    }

    [TestMethod]
    public void TestMethodDefaultPropertyFallback1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
          .MappedGetter(() => 6),
        new DefaultProperty()
          .SetterGetter());
      Assert.AreEqual(6, test.Property1);
      test.Property2 = "Some value";
      Assert.AreEqual("Some value", test.Property2);
      test.Property1 = 55;
      Assert.AreEqual(6, test.Property1);
    }

    [TestMethod]
    public void TestMethodDefaultPropertyFallback2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
          .SetterGetter(),
        new DefaultProperty()
          .NotifyChanged()
          .SetterGetter());
      List<string> changes = new List<string>();
      test.PropertyChanged += (sender, args) => changes.Add(args.PropertyName);
      test.Property1 = 5;
      test.Property2 = "5";
      CollectionAssert.AreEquivalent(
        new[] { "Property2" },
        changes);
    }
  }
}
