using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class NotSupported<T> : IPropertyDefinition<T>
  {
    public bool InvokeGetter(object _, ref T __)
    {
      throw new NotSupportedException("Property is not supported.");
    }

    public bool InvokeSetter(object _, T __)
    {
      throw new NotSupportedException("Property is not supported.");
    }
  }
}