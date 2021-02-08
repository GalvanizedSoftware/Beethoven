using System;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
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

    public IPropertyInvoker<T> Create(object master) =>
      new ValidityCheckInvoker<T>(checkFunc);
  }
}