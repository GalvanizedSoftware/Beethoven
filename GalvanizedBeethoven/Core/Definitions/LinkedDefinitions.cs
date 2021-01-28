using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Methods;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  internal class LinkedDefinitions<T> : IEnumerable<IDefinition> where T : class
  {
    private readonly IDefinition[] definitions;

    internal LinkedDefinitions(IEnumerable<object> newPartDefinitions)
    {
      Type type = typeof(T);
      object[] allInstances = newPartDefinitions.ToArray();
      IDefinition[] mapped = new MappedDefinitions<T>(allInstances)
        .GetDefinitions<T>()
        .ToArray();
      allInstances = allInstances.Concat(mapped).ToArray();
      IEnumerable<object> allObjects = allInstances
        .SelectMany(GetAll)
        .Distinct()
        .ToArray();
      FieldMaps = allObjects
        .OfType<IFieldMaps>()
        .ToArray();
      definitions = allObjects
        .OfType<IDefinition>()
        .Distinct()
        .OrderBy(definition => definition.SortOrder)
        .ToArray();
      GenericMethodWrapper[] generic = definitions
        .OfType<GenericMethodWrapper>()
        .ToArray();
      GenericMethodsWrapper<T>[] genericWrappers = generic
        .Select(wrapper => wrapper.MethodInfo)
        .Distinct()
        .Select(info =>
          new GenericMethodsWrapper<T>(info, generic
            .Where(wrapper => wrapper.MethodInfo == info)))
        .ToArray();
      definitions = definitions
        .Concat(genericWrappers)
        .ToArray();
      definitions
        .OfType<IMainTypeUser>()
        .SetAll(type);
    }

    public IReadOnlyCollection<IFieldMaps> FieldMaps { get; }

    public IEnumerator<IDefinition> GetEnumerator() =>
      definitions.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
      GetEnumerator();

    private static IEnumerable<object> GetAll(object part)
    {
	    if (part is IFieldMaps)
		    yield return part;
	    switch (part)
      {
        case IDefinitions definitions:
          foreach (IDefinition definition in definitions.GetDefinitions<T>())
            yield return definition;
          break;
        case IEnumerable childObjects:
          foreach (object child in childObjects)
            yield return child;
          break;
        default:
          yield return part;
          break;
      }
    }
  }
}
