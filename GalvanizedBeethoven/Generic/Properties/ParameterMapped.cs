using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class ParameterMapped<T> : IPropertyDefinition<T>
  {
    private readonly IParameter parameter;
    private readonly string name;
    private readonly MethodInfo getMethod;
    private readonly MethodInfo setMethod;

    public ParameterMapped(IParameter parameter, string name)
    {
      this.parameter = parameter;
      this.name = name;
      PropertyInfo propertyInfo = parameter.Type.GetProperty(name, ResolveFlags);
      if (propertyInfo == null)
        return;
      getMethod = propertyInfo.CanRead ? propertyInfo.GetMethod : null;
      setMethod = propertyInfo.CanWrite ? propertyInfo.SetMethod : null;
    }

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      if (getMethod != null)
        returnValue = (T)getMethod.Invoke(instanceMap.GetLocal(parameter), new object[0]);
      return true;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      setMethod?.Invoke(instanceMap.GetLocal(parameter), new object[] { newValue });
      return true;
    }

    public Property CreateMasterProperty() =>
      new Property<T>(new Property<T>(name), this);
  }
}