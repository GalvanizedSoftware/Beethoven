using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests
{
  internal interface ITestImplements : INotifyPropertyChanged
  {
    int Property1 { get; set; }
    string Property2 { get; set; }
    int Method1(int a, object b);
    int Method2(ref string c);
    void Method3();
  }
}
