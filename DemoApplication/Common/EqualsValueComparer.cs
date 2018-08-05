using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.DemoApp.Common
{
  public class EqualsValueComparer<T> : IEqualityComparer<T>
  {
    public bool Equals(T x, T y)
    {
      return x?.Equals(y) ?? y == null;
    }

    public int GetHashCode(T obj)
    {
      return obj.GetHashCode();
    }
  }
}