using System;
using GalvanizedSoftware.Beethoven.Core.Properties;

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

    public bool InvokeGetter(ref T returnValue)
    {
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      if (newValue.CompareTo(minimum) < 0)
        throw new ArgumentOutOfRangeException("Value too low");
      if (newValue.CompareTo(maximum) > 0)
        throw new ArgumentOutOfRangeException("Value too high");
      return true;
    }
  }
}