using GalvanizedSoftware.Beethoven.Interfaces;
using System;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
{
  public class ValidityCheckInstance<T> : IPropertyInstance<T>
  {
    private readonly Func<T, bool> checkFunc;

    public ValidityCheckInstance(Func<T, bool> checkFunc)
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