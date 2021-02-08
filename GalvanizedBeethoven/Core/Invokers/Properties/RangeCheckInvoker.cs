using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class RangeCheckInvoker<T> : IPropertyInvoker<T> where T : IComparable
  {
    private readonly T maximum;
    private readonly T minimum;

    public RangeCheckInvoker(T minimum, T maximum)
    {
      this.minimum = minimum;
      this.maximum = maximum;
    }

    public bool InvokeGetter(ref T __)
    {
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      if (newValue.CompareTo(minimum) < 0)
        throw new ArgumentOutOfRangeException(nameof(newValue), "Value too low");
      if (newValue.CompareTo(maximum) > 0)
        throw new ArgumentOutOfRangeException(nameof(newValue), "Value too high");
      return true;
    }
  }
}