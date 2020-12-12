using System;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class EquivalentTypeComparer : IEqualityComparer<Type>
  {
    public bool Equals(Type x, Type y)
    {
      if (ReferenceEquals(x, y))
        return true;
      return x?.FullName == y?.FullName;
    }

    public int GetHashCode(Type obj) => 
      obj.FullName?.GetHashCode() ?? 0.GetHashCode();
  }
}