using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Properties;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class Mapped<T> : IPropertyDefinition<T>
  {
    private readonly object main;
    private readonly string name;
    private readonly MethodInfo getMethod;
    private readonly MethodInfo setMethod;

    public Mapped(object target, string name)
    {
      this.name = name;
      main = target;
      PropertyInfo propertyInfo = target.GetType().GetProperty(name, ResolveFlags);
      if (propertyInfo == null)
        return;
      getMethod = propertyInfo.CanRead ? propertyInfo.GetMethod : null;
      setMethod = propertyInfo.CanWrite ? propertyInfo.SetMethod : null;
    }

    public bool InvokeGetter(ref T returnValue)
    {
      if (getMethod != null)
        returnValue = (T)getMethod.Invoke(main, new object[0]);
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      setMethod?.Invoke(main, new object[] { newValue });
      return true;
    }

    public Property CreateMasterProperty() =>
      new Property<T>(new Property<T>(name), this);
  }
}