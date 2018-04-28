using System.ComponentModel;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Events;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.MVVM.Properties
{
  internal class NotifyChanged<T> :
    IPropertyDefinition<T>,
    ITypeBinding<EventInvokers>,
    ITargetBinding
  {
    private readonly EventInvoker<PropertyChangedEventHandler> eventInvoker;
    private readonly string name;
    private object targetObject;

    public NotifyChanged(string name)
    {
      this.name = name;
      eventInvoker = new EventInvoker<PropertyChangedEventHandler>(nameof(INotifyPropertyChanged.PropertyChanged));
    }

    public bool InvokeGetter(ref T returnValue)
    {
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      eventInvoker?.Invoke(targetObject, new PropertyChangedEventArgs(name));
      return true;
    }

    public void Bind(object target)
    {
      targetObject = target;
    }

    public void Bind(EventInvokers master)
    {
      eventInvoker.Bind(master);
    }
  }
}