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
      bool v = equivalentTypeComparer.Equals(x?.ReturnType, y?.ReturnType) &&
             x.GetParameterTypes().SequenceEqual(y.GetParameterTypes(), equivalentTypeComparer);
      return v;
    }

    public int GetHashCode(MethodInfo obj)
    {
      unchecked // Overflow is fine
      {
        int v = new[] { obj.Name.GetHashCode(), equivalentTypeComparer.GetHashCode(obj.ReturnType) }
          .Concat(obj.GetParameterTypes()
          .Select(type => equivalentTypeComparer.GetHashCode(type)))
          .Aggregate((int)2166136261, (seed, value) => (seed * 16777619) ^ value);
        return v;
      }
    }
  }
}