using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Properties;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class SignatureChecker<T>
  {
    private readonly Dictionary<string, Type> properties;

    public SignatureChecker()
    {
      Type type = typeof(T);
      properties = type.GetProperties(ResolveFlags).ToDictionary(info => info.Name, info => info.PropertyType);
    }

    public void CheckSignatures(IEnumerable<object> wrappers)
    {
      CheckProperty(wrappers.OfType<Property>().ToArray());
      // TODO: Check methods?
      // TODO: Check events?
    }

    private void CheckProperty(Property[] propertyWrappers)
    {
      foreach (KeyValuePair<string, Type> pair in properties)
        CheckProperty(pair.Key, pair.Value, propertyWrappers);
    }

    private static bool CheckProperty(string name, Type actualType, Property[] wrappers)
    {
      int matchingCount = wrappers
        .Where(property => property.Name == name)
        .Count(property => property.PropertyType == actualType);
      switch (matchingCount)
      {
        case 0: // Tolerate some properties are not implemented, unless checked
          return false;
        case 1:
          return true;
        default:
          throw new NotImplementedException($"Multiple implementation found for property: {actualType} {name}");
      }
    }
  }
}