using System.ComponentModel;
using GalvanizedSoftware.Beethoven.Core.Events;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class NotifyChanged<T> :
    IPropertyDefinition<T>
  {
    private readonly EventInvoker eventInvoker;
    private readonly string name;

    public NotifyChanged(string name)
    {
      this.name = name;
      eventInvoker = new EventInvoker(nameof(INotifyPropertyChanged.PropertyChanged));
    }

    public bool InvokeGetter(object _, ref T __) => true;

    public bool InvokeSetter(object master, T newValue)
    {
      eventInvoker?.Invoke(master, master, new PropertyChangedEventArgs(name));
      return true;
    }
  }
}