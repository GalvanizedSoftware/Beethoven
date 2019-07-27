using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  [TestClass]
  public class PropertyDelegatedGetterTests
  {

    [TestMethod]
    public void TestMethodPropertyDelegatedGetter1()
    {
      int value = 0;
      // ReSharper disable once AccessToModifiedClosure
      Func<int> action = () => value;
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
        .DelegatedGetter(action));
      Assert.AreEqual(0, test.Property1);
      value = 5;
      Assert.AreEqual(5, test.Property1);
    }

    [TestMethod]
    public void TestMethodPropertyDelegatedGetter2()
    {
      int getCount = 0;
      Func<int> action = () =>
      {
        getCount++;
        return 55;
      };
      BeethovenFactory factory = new BeethovenFactory();
      ITestProperties test = factory.Generate<ITestProperties>(
        new PropertyDefinition<int>(nameof(ITestProperties.Property1))
          .DelegatedGetter(action));
      Assert.AreEqual(0, getCount);
      Assert.AreEqual(55, test.Property1);
      Assert.AreEqual(1, getCount);
    }
  }
}
