using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class ValidityCheckInvoker<T> : IPropertyInvoker<T>
  {
    private readonly Func<T, bool> checkFunc;

    public ValidityCheckInvoker(Func<T, bool> checkFunc)
    {
      this.checkFunc = checkFunc;
    }

    public bool InvokeGetter(ref T __) => 
      true;

    public bool InvokeSetter(T newValue)
    {
      if (!checkFunc(newValue))
        throw new ArgumentOutOfRangeException($"Value {newValue} invalid");
      return true;
    }
  }
}