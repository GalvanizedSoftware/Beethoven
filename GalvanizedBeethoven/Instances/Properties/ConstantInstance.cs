using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Implementations.Properties
{
  public class ConstantInstance<T> : IPropertyInstance<T>
  {
    private readonly Action<T> errorHandler;
    private readonly T value;

    public ConstantInstance(T value) :
      this(value, null)
    {
    }

    public ConstantInstance(T value, Action<T> errorHandler)
    {
      this.value = value;
      this.errorHandler = errorHandler ??
                          (invalidValue =>
                            throw new ArgumentOutOfRangeException($"Value cannot be changed to {invalidValue}"));
    }

    // ReSharper disable once RedundantAssignment
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