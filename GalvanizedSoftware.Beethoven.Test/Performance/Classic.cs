using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalvanizedSoftware.Beethoven.Test.Properties;

namespace GalvanizedSoftware.Beethoven.Test.Performance
{
  internal sealed class Classic : IPerformanceTest
  {
    private string name;

    public string Name
    {
      get => name;
      set
      {
        if (value == name) return;
        name = value;
        OnPropertyChanged();
      }
    }

    public string Format(string format)
    {
      return string.Format(format, name);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
