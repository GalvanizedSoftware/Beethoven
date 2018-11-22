using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.Performance
{
  public interface IPerformanceTest : INotifyPropertyChanged
  {
    string Name { get; set; }
    string Format(string format);
  }
}
