using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class ValidityCheck<T> : IPropertyDefinition<T>
  {
    private readonly Func<T, bool> checkFunc;

    public ValidityCheck(Func<T, bool> checkFunc)
    {
      this.checkFunc = checkFunc;
    }

    public bool InvokeGetter(object _, ref T __) => 
      true;

    public bool InvokeSetter(object _, T newValue)
    {
      if (!checkFunc(newValue))
        throw new ArgumentOutOfRangeException($"Value {newValue} invalid");
      return true;
    }
  }
}