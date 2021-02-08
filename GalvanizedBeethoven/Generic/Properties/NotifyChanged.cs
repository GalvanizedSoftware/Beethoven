using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class NotifyChanged<T> : IPropertyDefinition<T>
  {
    private readonly string name;

    public NotifyChanged(string name)
    {
      this.name = name;
    }

    public IPropertyInvoker<T> Create(object master) => 
      new NotifyChangedInvoker<T>(master, name);
  }
}