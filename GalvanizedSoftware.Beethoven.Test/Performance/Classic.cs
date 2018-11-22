using GalvanizedSoftware.Beethoven.Test.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GalvanizedSoftware.Beethoven.Test.Performance
{
  internal class Classic : IPerformanceTest, INotifyPropertyChanged
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
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
