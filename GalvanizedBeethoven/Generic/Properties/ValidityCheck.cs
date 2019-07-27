using System;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class ValidityCheck<T> : IPropertyDefinition<T>
  {
    private readonly Func<T, bool> checkFunc;

    public ValidityCheck(Func<T, bool> checkFunc)
    {
      this.checkFunc = checkFunc;
    }

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue) => 
      true;

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      if (!checkFunc(newValue))
        throw new ArgumentOutOfRangeException($"Value {newValue} invalid");
      return true;
    }
  }
}