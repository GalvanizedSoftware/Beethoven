using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  public interface ITestProperties : INotifyPropertyChanged
  {
    int Property1 { get; set; }
    string Property2 { get; set; }
  }
}
