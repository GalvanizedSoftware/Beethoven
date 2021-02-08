using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class Mapped<T> : IPropertyDefinition<T>
  {
    private readonly object main;
    private readonly string name;

    public Mapped(object target, string name)
    {
      this.name = name;
      main = target;
    }

    public IPropertyInvoker<T> Create(object master) =>
      new MappedInvoker<T>(main, name);

    public PropertyDefinition CreateMasterProperty() =>
      new PropertyDefinition<T>(new PropertyDefinition<T>(name), this);
  }
}