using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Properties;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class Mapped<T> : IPropertyDefinition<T>
  {
    private readonly object main;
    private readonly string name;
    private readonly PropertyInfo propertyInfo;

    public Mapped(object target, string name)
    {
      this.name = name;
      main = target;
      propertyInfo = target.GetType().GetProperty(name, ResolveFlags);
    }

    // ReSharper disable once RedundantAssignment
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