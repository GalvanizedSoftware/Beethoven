using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class EquivalentMethodComparer : IEqualityComparer<MethodInfo>
  {
    private readonly EquivalentTypeComparer equivalentTypeComparer = new EquivalentTypeComparer();

    public bool Equals(MethodInfo x, MethodInfo y)
    {
      if (ReferenceEquals(x, y))
        return true;
      return x != null && y != null &&
             equivalentTypeComparer.Equals(x.ReturnType, y.ReturnType) &&
             x.GetParameterTypes().SequenceEqual(y.GetParameterTypes(), equivalentTypeComparer);
    }

    public int GetHashCode(MethodInfo obj)
    {
      unchecked // Overflow is fine
      {
        int hashCode = obj.GetParameterTypes()
          .Append(obj.ReturnType)
          .Select(type => equivalentTypeComparer.GetHashCode(type))
          .Aggregate((int)2166136261, (seed, value) => (seed * 16777619) ^ value);
        return hashCode;
      }
    }
  }
}