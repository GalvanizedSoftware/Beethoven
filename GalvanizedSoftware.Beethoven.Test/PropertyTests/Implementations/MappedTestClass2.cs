using GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces;
using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests.Implementations
{
  public class MappedTestClass2 : ITestProperties
  {
    public int Property1 { get; set; }
    public string Property2 { get; set; }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };
  }
}
