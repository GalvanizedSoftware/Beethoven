using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
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

    public IPropertyInstance<T> CreateInstance(object master) => 
      new NotifyChangedInstance<T>(master, name);
  }
}