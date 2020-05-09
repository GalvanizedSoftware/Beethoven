using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class ExactPropertyComparer : IEqualityComparer<PropertyInfo>
  {
    public bool Equals(PropertyInfo x, PropertyInfo y) =>
      ReferenceEquals(x, y) ||
      x != null &&
      y != null &&
      x.Name == y.Name &&
      x.PropertyType == y.PropertyType;

    public int GetHashCode(PropertyInfo obj)
    {
      unchecked // Overflow is fine
      {
        return obj.Name.GetHashCode() * 16777619 ^ obj.PropertyType.GetHashCode();
      }
    }
  }
}