using GalvanizedSoftware.Beethoven.Test.VariousTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalvanizedSoftware.Beethoven.Extensions;
using System.ComponentModel;
using GalvanizedSoftware.Beethoven.Generic.Events;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests
{
  [TestClass]
  public class MvvmTests
  {
    [TestMethod]
    public void ImplementsTest1()
    {
      BeethovenFactory factory = new BeethovenFactory();
      ITestImplements instance = factory.Generate<ITestImplements>(
        new PropertyDefinition<string>("Property2")
          .ValidityCheck(name => !string.IsNullOrEmpty(name))
          .SkipIfEqual()
          .SetterGetter()
          .NotifyChanged(),
        new SimpleEventDefinition<PropertyChangedEventHandler>(nameof(INotifyPropertyChanged.PropertyChanged)));
      int count = 0;
      instance.PropertyChanged += (_, __) => count++;
      instance.Property2 = "123";
      instance.Property2 = "123";
      instance.Property2 = "1e23";
      Assert.AreEqual(2, count);
    }
  }
}
