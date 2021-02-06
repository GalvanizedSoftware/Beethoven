using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests.EqualsGetHashImport
{
  public class EqualsGetHashComparer : IEqualityComparer<IEqualsGetHash>
  {
    public bool Equals(IEqualsGetHash x, IEqualsGetHash y) => 
	    x?.Equals(y) ?? y == null;

    public int GetHashCode(IEqualsGetHash obj) => 
	    obj.GetHashCode();
  }
}