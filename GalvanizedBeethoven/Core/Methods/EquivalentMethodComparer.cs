using GalvanizedSoftware.Beethoven.Extentions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class EquivalentMethodComparer : IEqualityComparer<MethodInfo>
  {
    public bool Equals(MethodInfo x, MethodInfo y)
    {
      if (ReferenceEquals(x, y))
        return true;
      return x?.ReturnType == y?.ReturnType &&
             x.GetParameterTypes().SequenceEqual(y.GetParameterTypes());
    }

    public int GetHashCode(MethodInfo obj)
    {
      return new object[] { obj.Name, obj.ReturnType }
        .Concat(obj.GetParameterTypes())
        .Sum(type => (long)type.GetHashCode())
        .GetHashCode();
    }
  }
}