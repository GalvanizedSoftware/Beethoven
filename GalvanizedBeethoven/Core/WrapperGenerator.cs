using System;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class WrapperGenerator<T> : IDefinitions where T : class
  {
    private readonly IEnumerable<IDefinition> wrappers;

    internal WrapperGenerator(object[] partDefinitions)
    {
      wrappers = partDefinitions
        .SelectMany(definition => FilterNonDefinitions(definition))
        .SelectMany(GetDefinitionWrappers)
        .ToArray();
      PropertiesSignatureChecker<T>.CheckSignatures(this);
    }

    public IEnumerable<IDefinition> GetDefinitions() => wrappers;

    internal static IEnumerable<object> FilterNonDefinitions(object instance)
    {
      switch (instance)
      {
        case null:
        case IDefinition _:
        case IEnumerable<IDefinition> __:
          break;
        default:
          yield return instance;
          break;
      }
    }

    internal static IEnumerable<IDefinition> GetDefinitionWrappers(object definition) =>
      Array.Empty<IDefinition>()
        .Concat(new PropertiesMapper(typeof(T), definition))
        .Concat(new MethodsMapper<T>(definition));
  }
}
