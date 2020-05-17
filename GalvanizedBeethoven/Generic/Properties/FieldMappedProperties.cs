using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Core;
using System.Linq;
using System.Collections;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class FieldMappedProperties : IDefinitions
  {
    private readonly string fieldName;
    private Type mainType;

    public FieldMappedProperties(string fieldName, Type mainType)
    {
      this.fieldName = fieldName;
      this.mainType = mainType;
    }

    public IEnumerable<IDefinition> GetDefinitions() =>
      mainType
        .GetAllTypes()
        .SelectMany(type => type.GetProperties())
        .Select(propertyInfo => new FieldMappedProperty(propertyInfo, fieldName))
        .ToArray();
  }
}