using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class NotSupportedInvoker<T> : IPropertyInvoker<T>
  {
    public bool InvokeGetter(ref T _) => 
      throw new NotSupportedException("Property is not supported.");

    public bool InvokeSetter(T _) => 
      throw new NotSupportedException("Property is not supported.");
  }
}