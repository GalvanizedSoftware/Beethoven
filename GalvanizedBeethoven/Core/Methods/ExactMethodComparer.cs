using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class ExactMethodComparer : IEqualityComparer<MethodInfo>
  {
    private readonly EquivalentTypeComparer equivalentTypeComparer = new EquivalentTypeComparer();

    public bool Equals(MethodInfo x, MethodInfo y)
    {
      if (ReferenceEquals(x, y))
        return true;
      if (x == null) return false;
      if (y == null) return false;
      if (x.Name != y.Name) return false;
      if (!equivalentTypeComparer.Equals(x.ReturnType, y.ReturnType)) return false;
      return x.GetParameterTypes().SequenceEqual(y.GetParameterTypes(), equivalentTypeComparer);
    }

    public int GetHashCode(MethodInfo obj)
    {
      unchecked // Overflow is fine
      {
        return new[] { obj.Name.GetHashCode(), equivalentTypeComparer.GetHashCode(obj.ReturnType) }
          .Concat(obj.GetParameterTypes()
            .Select(type => equivalentTypeComparer.GetHashCode(type)))
          .Aggregate((int)2166136261, (seed, value) => (seed * 16777619) ^ value);
      }
    }
  }
}