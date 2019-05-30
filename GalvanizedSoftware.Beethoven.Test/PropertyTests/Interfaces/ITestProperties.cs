using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces
{
  public interface ITestProperties : INotifyPropertyChanged
  {
    int Property1 { get; set; }
    string Property2 { get; set; }
  }
}
