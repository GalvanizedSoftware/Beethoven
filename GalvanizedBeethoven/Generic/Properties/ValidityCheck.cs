using System;
using GalvanizedSoftware.Beethoven.Implementations.Properties;
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

    public IPropertyInstance<T> CreateInstance(object master) =>
      new ValidityCheckInstance<T>(checkFunc);
  }
}