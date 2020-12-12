using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class PropertyInfoExtensions
  {
    internal static IEnumerable<PropertyInfo> DistinctProperties(this IEnumerable<PropertyInfo> properties) =>
      properties.Distinct(new ExactPropertyComparer());
  }
}
