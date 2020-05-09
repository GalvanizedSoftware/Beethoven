using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Core;
using System.Linq;
using System.Collections;
using GalvanizedSoftware.Beethoven.Generic.Methods;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class FieldMappedProperties : IEnumerable<IDefinition>
  {
    private readonly string fieldName;
    private Type mainType;

    public FieldMappedProperties(string fieldName, Type mainType)
    {
      this.fieldName = fieldName;
      this.mainType = mainType;
    }

    public IEnumerator<IDefinition> GetEnumerator()
    {
      IEnumerable<System.Reflection.PropertyInfo> enumerable = mainType
              .GetAllTypes()
              .SelectMany(type => type.GetProperties());
      IEnumerable<FieldMappedProperty> enumerable1 = enumerable
        .Select(propertyInfo => new FieldMappedProperty(propertyInfo, fieldName))
        .ToArray();
      return enumerable1.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}