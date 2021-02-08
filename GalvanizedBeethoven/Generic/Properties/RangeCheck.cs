using System;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class RangeCheck<T> : IPropertyDefinition<T> where T : IComparable
  {
    private readonly T maximum;
    private readonly T minimum;

    public RangeCheck(T minimum, T maximum)
    {
      this.minimum = minimum;
      this.maximum = maximum;
    }

    public IPropertyInvoker<T> Create(object master) =>
      new RangeCheckInvoker<T>(minimum, maximum);
  }
}