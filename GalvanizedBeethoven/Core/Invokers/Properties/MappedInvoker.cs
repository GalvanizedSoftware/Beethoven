using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;
using static GalvanizedSoftware.Beethoven.Core.ReflectionConstants;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class MappedInvoker<T> : IPropertyInvoker<T>
  {
    private readonly object main;
    private readonly MethodInfo getMethod;
    private readonly MethodInfo setMethod;

    public MappedInvoker(object target, string name)
    {
      main = target;
      PropertyInfo propertyInfo = target?.GetType().GetProperty(name, ResolveFlags);
      if (propertyInfo == null)
        return;
      getMethod = propertyInfo.CanRead ? propertyInfo.GetMethod : null;
      setMethod = propertyInfo.CanWrite ? propertyInfo.SetMethod : null;
    }

    public bool InvokeGetter(ref T returnValue)
    {
      if (getMethod != null)
        returnValue = (T)getMethod.Invoke(main, Array.Empty<object>());
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      setMethod?.Invoke(main, new object[] { newValue });
      return true;
    }
  }
}