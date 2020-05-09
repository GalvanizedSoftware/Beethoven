using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Core.Events;

namespace GalvanizedSoftware.Beethoven.Test.EventTests
{
  [TestClass]
  public class EventTests
  {

    [TestMethod]
    public void EventSimple2()
    {
      TypeDefinition<ITestEvents> typeDefinition = new TypeDefinition<ITestEvents>();
      ITestEvents test = typeDefinition.Create();
      Action trigger =
        new EventTrigger(test, nameof(ITestEvents.Simple)).ToAction();
      bool simpleEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { simpleEventCalled = true; };
      test.WithParameters += delegate { otherEventCalled = true; };
      test.WithReturnValue += delegate { return otherEventCalled = true; };
      trigger();
      Assert.IsTrue(simpleEventCalled);
      Assert.IsFalse(otherEventCalled);
    }

    [TestMethod]
    public void EventSimpleRemoved1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      Action trigger = new EventTrigger(test, nameof(ITestEvents.Simple)).ToAction();
      bool simpleEventCalled = false;
      Action delegate1 = delegate { simpleEventCalled = true; };
      test.Simple += delegate1;
      test.Simple -= delegate1;
      trigger();
      Assert.IsFalse(simpleEventCalled);
    }

    [TestMethod]
    [ExpectedException(typeof(TargetParameterCountException))]
    public void EventSimpleError()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>(new DefaultSimpleEvent());
      Action<int> trigger = 
        new EventTrigger(test, nameof(ITestEvents.Simple)).ToAction<int>();
      bool simpleEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { simpleEventCalled = true; };
      test.WithParameters += delegate { otherEventCalled = true; };
      test.WithReturnValue += delegate { return otherEventCalled = true; };
      trigger(123);
      Assert.IsTrue(simpleEventCalled);
      Assert.IsFalse(otherEventCalled);
    }

    [TestMethod]
    public void EventWithParameters1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      Action<double, string> trigger =
        new EventTrigger(test, nameof(ITestEvents.WithParameters)).ToAction<double, string>();
      bool withParametersEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { otherEventCalled = true; };
      test.WithParameters += delegate { withParametersEventCalled = true; };
      test.WithReturnValue += delegate { return otherEventCalled = true; };
      trigger(4.4, "");
      Assert.IsTrue(withParametersEventCalled);
      Assert.IsFalse(otherEventCalled);
    }

    [TestMethod]
    public void EventWithParameters2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      Action<double, string> trigger =
        new EventTrigger(test, nameof(ITestEvents.WithParameters)).ToAction<double, string>();
      double gotValue1 = 0;
      string gotValue2 = null;
      test.WithParameters += (value1, value2) => gotValue1 = value1;
      test.WithParameters += (value1, value2) => gotValue2 = value2;
      trigger(54.0, "abe");
      Assert.AreEqual(54.0, gotValue1);
      Assert.AreEqual("abe", gotValue2);
    }

    [TestMethod]
    public void EventWithReturnValue1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      Action<string> trigger =
        new EventTrigger(test, nameof(ITestEvents.WithReturnValue)).ToAction<string>();
      bool withReturnValueEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { otherEventCalled = true; };
      test.WithParameters += delegate { otherEventCalled = true; };
      test.WithReturnValue += delegate { return withReturnValueEventCalled = true; };
      trigger("");
      Assert.IsTrue(withReturnValueEventCalled);
      Assert.IsFalse(otherEventCalled);
    }

    [TestMethod]
    public void EventWithReturnValue2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      Func<string, bool> trigger = 
        new EventTrigger(test, nameof(ITestEvents.WithReturnValue)).ToFunc<string, bool>();
      string gotValue = null;
      test.WithReturnValue += value =>
      {
        gotValue = value;
        return true;
      };
      bool returnValue = trigger("123");
      Assert.AreEqual(true, returnValue);
      Assert.AreEqual("123", gotValue);
    }

    [TestMethod]
    public void EventWithReturnValue3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      Func<string, bool> trigger = 
        new EventTrigger(test, nameof(ITestEvents.WithReturnValue)).ToFunc<string, bool>();
      test.WithReturnValue += value => false;
      test.WithReturnValue += value => true;
      bool returnValue = trigger("123");
      Assert.AreEqual(true, returnValue);
    }
  }
}
