using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.AutoCompileTests.Tooling
{
  public interface ITestProperties3 : INotifyPropertyChanged
  {
    int Property1 { get; set; }
    string Property2 { get; set; }
  }
}
