using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.AutoCompileTests.Tooling
{
  public interface ITestProperties1 : INotifyPropertyChanged
  {
    int Property1 { get; set; }
    string Property2 { get; set; }
  }
}
