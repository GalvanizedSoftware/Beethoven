using System;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class NotSupported<T> : IPropertyDefinition<T>
  {
    public bool InvokeGetter(ref T returnValue)
    {
      throw new NotSupportedException("Property is not supported.");
    }

    public bool InvokeSetter(T newValue)
    {
      throw new NotSupportedException("Property is not supported.");
    }
  }
}