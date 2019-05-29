using System;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class NotSupported<T> : IPropertyDefinition<T>
  {
    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      throw new NotSupportedException("Property is not supported.");
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      throw new NotSupportedException("Property is not supported.");
    }
  }
}