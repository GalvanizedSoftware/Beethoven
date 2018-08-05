using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalvanizedSoftware.Beethoven.DemoApp.Common;
using GalvanizedSoftware.Beethoven.DemoApp.Properties;

namespace GalvanizedSoftware.Beethoven.DemoApp.EqualsGetHashImport
{
  public class EqualsViewModel : INotifyPropertyChanged
  {
    private readonly Factory factory = new Factory();
    private readonly HashSet<IValueHolder> valueHolders;
    private IValueHolder[] values;

    public EqualsViewModel()
    {
      AddCommand = new Command(Add);
      valueHolders = new HashSet<IValueHolder>(new EqualsValueComparer<IValueHolder>()); // Unfortunately the == operator is difficult to override
    }

    private void Add()
    {
      IValueHolder value = factory.Create(Name, Value, Data?.Select(c => unchecked((byte)c)).ToArray());
      if (valueHolders.Contains(value))
        MessageBox.Show("Element already exists");
      valueHolders.Add(value);
      Values = valueHolders.ToArray();
    }

    public IValueHolder[] Values
    {
      get => values;
      set
      {
        if (Equals(value, values)) return;
        values = value;
        OnPropertyChanged();
      }
    }

    public string Name { get; set; }

    public int Value { get; set; }

    public string Data { get; set; }

    public ICommand AddCommand { get; }
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}