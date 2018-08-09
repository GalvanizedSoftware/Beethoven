using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.DemoApp.EqualsGetHashImport
{
  public class EqualsGetHashComparer : IEqualityComparer<IEqualsGetHash>
  {
    public bool Equals(IEqualsGetHash x, IEqualsGetHash y)
    {
      return x?.Equals(y) ?? y == null;
    }

    public int GetHashCode(IEqualsGetHash obj)
    {
      return obj.GetHashCode();
    }
  }
}