using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyLinkedObjectsTests
  {

    [TestMethod]
    public void TestMethodPropertyLinkedObjects1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new LinkedObjects(
          new Property<int>(nameof(ITestProperties.Property1))
            .SkipIfEqual(),
          new Property<int>(nameof(ITestProperties.Property1))
            .DelegatedSetter(value => setCount++)));
      test.Property1 = 5;
      test.Property1 = 5;
      Assert.AreEqual(1, setCount);
    }

    [TestMethod]
    public void TestMethodPropertyLinkedObjects2()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new LinkedObjects(
          new Property<int>(nameof(ITestProperties.Property1))
            .DelegatedSetter(value => setCount++),
          new Property<int>(nameof(ITestProperties.Property1))
            .SkipIfEqual()));
      test.Property1 = 5;
      test.Property1 = 5;
      Assert.AreEqual(2, setCount);
    }

    [TestMethod]
    public void TestMethodPropertyLinkedObjects3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      int setCount = 0;
      ITestProperties test = factory.Generate<ITestProperties>(
        new LinkedObjects(
          new Property<int>(nameof(ITestProperties.Property1))
            .SkipIfEqual(),
          new Property<int>(nameof(ITestProperties.Property1))
            .DelegatedSetter(value => setCount++)));
      test.Property1 = 5;
      test.Property1 = 6;
      Assert.AreEqual(2, setCount);
    }

    [TestMethod]
    public void TestMethodPropertyLinkedObjects4()
    {
      BeethovenFactory factory = new BeethovenFactory();
      var obj1 = new { Property1 = 6 };
      var obj2 = new { Property1 = 17 };
      ITestProperties test = factory.Generate<ITestProperties>(
        new LinkedObjects(obj1, obj2));
      Assert.AreEqual(17, test.Property1);
    }
  }
}
