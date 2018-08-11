using GalvanizedSoftware.Beethoven.Generic.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Test.EventTests
{
  [TestClass]
  public class EventTests
  {
    [TestMethod]
    public void EventSimple()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      IEventTrigger trigger = factory.CreateEventTrigger(test, nameof(ITestEvents.Simple));
      bool simpleEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { simpleEventCalled = true; };
      test.WithParameters += delegate { otherEventCalled = true; };
      test.WithReturnValue += delegate { return otherEventCalled = true; };
      trigger.Notify();
      Assert.IsTrue(simpleEventCalled);
      Assert.IsFalse(otherEventCalled);
    }

    [TestMethod]
    [ExpectedException(typeof(TargetParameterCountException))]
    public void EventSimpleError()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      IEventTrigger trigger = factory.CreateEventTrigger(test, nameof(ITestEvents.Simple));
      bool simpleEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { simpleEventCalled = true; };
      test.WithParameters += delegate { otherEventCalled = true; };
      test.WithReturnValue += delegate { return otherEventCalled = true; };
      trigger.Notify(123);
      Assert.IsTrue(simpleEventCalled);
      Assert.IsFalse(otherEventCalled);
    }

    [TestMethod]
    public void EventWithParameters1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      IEventTrigger trigger = factory.CreateEventTrigger(test, nameof(ITestEvents.WithParameters));
      bool withParametersEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { otherEventCalled = true; };
      test.WithParameters += delegate { withParametersEventCalled = true; };
      test.WithReturnValue += delegate { return otherEventCalled = true; };
      trigger.Notify(4.4, "");
      Assert.IsTrue(withParametersEventCalled);
      Assert.IsFalse(otherEventCalled);
    }

    [TestMethod]
    public void EventWithParameters2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      IEventTrigger trigger = factory.CreateEventTrigger(test, nameof(ITestEvents.WithParameters));
      double gotValue1 = 0;
      string gotValue2 = null;
      test.WithParameters += (value1, value2) => gotValue1 = value1;
      test.WithParameters += (value1, value2) => gotValue2 = value2;
      trigger.Notify(54.0, "abe");
      Assert.AreEqual(54.0, gotValue1);
      Assert.AreEqual("abe", gotValue2);
    }

    [TestMethod]
    public void EventWithReturnValue1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      IEventTrigger trigger = factory.CreateEventTrigger(test, nameof(ITestEvents.WithReturnValue));
      bool withReturnValueEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { otherEventCalled = true; };
      test.WithParameters += delegate { otherEventCalled = true; };
      test.WithReturnValue += delegate { return withReturnValueEventCalled = true; };
      trigger.Notify("");
      Assert.IsTrue(withReturnValueEventCalled);
      Assert.IsFalse(otherEventCalled);
    }

    [TestMethod]
    public void EventWithReturnValue2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      IEventTrigger trigger = factory.CreateEventTrigger(test, nameof(ITestEvents.WithReturnValue));
      string gotValue = null;
      test.WithReturnValue += value =>
      {
        gotValue = value;
        return true;
      };
      bool returnValue = (bool)trigger.Notify("123");
      Assert.AreEqual(true, returnValue);
      Assert.AreEqual("123", gotValue);
    }

    [TestMethod]
    public void EventWithReturnValue3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestEvents test = factory.Generate<ITestEvents>();
      IEventTrigger trigger = factory.CreateEventTrigger(test, nameof(ITestEvents.WithReturnValue));
      test.WithReturnValue += value => false;
      test.WithReturnValue += value => true;
      bool returnValue = (bool)trigger.Notify("123");
      Assert.AreEqual(true, returnValue);
    }
  }
}
