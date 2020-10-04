using System.ComponentModel;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class NotifyChanged<T> : IPropertyDefinition<T>
  {
    private const string PropertyChangedName = nameof(INotifyPropertyChanged.PropertyChanged);
    private readonly PropertyChangedEventArgs eventArgs;

    public NotifyChanged(string name)
    {
      eventArgs = new PropertyChangedEventArgs(name);
    }

    public bool InvokeGetter(object _, ref T __) => true;

    public bool InvokeSetter(object master, T newValue)
    {
      (master as IGeneratedClass)?.NotifyEvent(PropertyChangedName, new object[] { master, eventArgs });
      return true;
    }
  }
}