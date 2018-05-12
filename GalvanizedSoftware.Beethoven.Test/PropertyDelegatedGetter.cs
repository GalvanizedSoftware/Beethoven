using GalvanizedSoftware.Beethoven.Core.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalvanizedSoftware.Beethoven.Extentions;
using System;

namespace GalvanizedSoftware.Beethoven.Test
{
  [TestClass]
  public class PropertyDelegatedGetter
  {

    [TestMethod]
    public void TestMethodPropertyDelegatedGetter1()
    {
      int value = 0;
      // ReSharper disable once AccessToModifiedClosure
      Func<int> action = () => value;
      BeethovenFactory factory = new BeethovenFactory();
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
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
      ITest test = factory.Generate<ITest>(
        new Property<int>(nameof(ITest.Property1))
          .DelegatedGetter(action));
      Assert.AreEqual(0, getCount);
      Assert.AreEqual(55, test.Property1);
      Assert.AreEqual(1, getCount);
    }
  }
}
