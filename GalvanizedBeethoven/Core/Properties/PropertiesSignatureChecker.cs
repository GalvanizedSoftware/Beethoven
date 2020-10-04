using GalvanizedSoftware.Beethoven.Generic.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal class PropertiesSignatureChecker<T>
  {
    private readonly Dictionary<string, Type> properties;

    private PropertiesSignatureChecker()
    {
      properties = typeof(T)
        .GetProperties(ReflectionConstants.ResolveFlags)
        .ToDictionary(info => info.Name, info => info.PropertyType);
    }

    public static void CheckSignatures(IDefinitions wrappers) => 
      new PropertiesSignatureChecker<T>().CheckSignaturesInternal(wrappers);

    private void CheckSignaturesInternal(IDefinitions wrappers) => 
      CheckProperty(wrappers.GetDefinitions().OfType<PropertyDefinition>().ToArray());

    private void CheckProperty(PropertyDefinition[] propertyWrappers)
    {
      foreach (KeyValuePair<string, Type> pair in properties)
        CheckProperty(pair.Key, pair.Value, propertyWrappers);
    }

    private static void CheckProperty(string name, Type actualType, PropertyDefinition[] wrappers)
    {
      int matchingCount = wrappers
        .Where(property => property.Name == name)
        .Count(property => property.PropertyType == actualType);
      switch (matchingCount)
      {
        case 0: // Tolerate some properties are not implemented, unless checked
        case 1:
          return;
        default:
          throw new MissingMethodException($"Multiple implementation found for property: {actualType} {name}");
      }
    }
  }
}