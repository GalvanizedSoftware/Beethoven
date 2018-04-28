using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class PropertyMapped<T> : IPropertyDefinition<T>
  {
    private readonly object main;
    private readonly string name;
    private readonly PropertyInfo propertyInfo;

    public PropertyMapped(string name, object main)
    {
      this.name = name;
      this.main = main;
      propertyInfo = main.GetType().GetProperty(name);
    }

    public bool InvokeGetter(ref T returnValue)
    {
      returnValue = (T)propertyInfo.GetMethod.Invoke(main, new object[0]);
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      propertyInfo.SetMethod.Invoke(main, new object[] { newValue });
      return true;
    }

    public Property CreateMasterProperty()
    {
      return new Property<T>(new Property<T>(name), this);
    }
  }
}