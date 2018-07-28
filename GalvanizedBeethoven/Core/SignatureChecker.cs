using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class SignatureChecker<T>
  {
    private readonly Dictionary<string, Type> properties;
    private readonly MethodInfo[] methods;

    public SignatureChecker()
    {
      Type type = typeof(T);
      properties = type.GetProperties(ResolveFlags).ToDictionary(info => info.Name, info => info.PropertyType);
      methods = type.GetMethods(ResolveFlags);
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

    private void CheckProperty(string name, Type actualType, Property[] wrappers)
    {
      int matchingCount = wrappers
        .Where(property => property.Name == name)
        .Where(property => property.PropertyType == actualType)
        .Count();
      switch (matchingCount)
      {
        case 0:
          throw new NotImplementedException($"Implementation not found for property: {actualType} {name}");
        case 1:
          return;
        default:
          throw new NotImplementedException($"Multiple implementation found for property: {actualType} {name}");
      }
    }
  }
}