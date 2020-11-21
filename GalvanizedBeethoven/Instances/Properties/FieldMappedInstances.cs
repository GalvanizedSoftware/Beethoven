using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Linq;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Implementations.Properties
{
  public class FieldMappedInstances : IDefinitions
  {
    private readonly string fieldName;
    private readonly Type mainType;

    public FieldMappedInstances(string fieldName, Type mainType)
    {
      this.fieldName = fieldName;
      this.mainType = mainType;
    }

    public IEnumerable<IDefinition> GetDefinitions() =>
      mainType
        .GetAllTypes()
        .SelectMany(type => type.GetProperties())
        .Select(propertyInfo => new FieldMappedInstance(propertyInfo, fieldName))
        .ToArray();
  }
}