using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class ConstantInvoker<T> : IPropertyInvoker<T>
  {
    private readonly Action<T> errorHandler;
    private readonly T value;

    public ConstantInvoker(T value, Action<T> errorHandler = null)
    {
      this.value = value;
      this.errorHandler = errorHandler ??
                          (invalidValue =>
                            throw new ArgumentOutOfRangeException($"Value cannot be changed to {invalidValue}"));
    }

    public bool InvokeGetter(ref T returnValue)
    {
      returnValue = value;
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      if (value.Equals(newValue))
        return true;
      errorHandler(newValue);
      return false;
    }
  }
}