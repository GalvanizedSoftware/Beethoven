using GalvanizedSoftware.Beethoven.Core.Methods;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class PropertyInfoExtensions
  {
    internal static IEnumerable<PropertyInfo> DistinctProperties(this IEnumerable<PropertyInfo> properties) =>
      properties.Distinct(new ExactPropertyComparer());
  }
}
