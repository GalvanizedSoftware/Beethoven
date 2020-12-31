using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  internal class WrapperGenerator<T> : IDefinitions where T : class
  {
    private readonly IEnumerable<IDefinition> wrappers;

    internal WrapperGenerator(object[] partDefinitions)
    {
      wrappers = partDefinitions
        .SelectMany(FilterNonDefinitions)
        .SelectMany(GetDefinitionWrappers)
        .ToArray();
      PropertiesSignatureChecker<T>.CheckSignatures(this);
    }

    public IEnumerable<IDefinition> GetDefinitions() => wrappers;

    private static IEnumerable<object> FilterNonDefinitions(object instance)
    {
      switch (instance)
      {
        case null:
        case IDefinition:
        case IEnumerable<IDefinition>:
          break;
        default:
          yield return instance;
          break;
      }
    }

    private static IEnumerable<IDefinition> GetDefinitionWrappers(object definition) =>
      Array.Empty<IDefinition>()
        .Concat(new PropertiesMapper(typeof(T), definition))
        .Concat(new MethodsMapper<T>(definition));
  }
}
