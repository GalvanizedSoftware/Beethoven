using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Fields
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

    public IEnumerable<IDefinition> GetDefinitions<T>() where T : class =>
      mainType
        .GetAllTypes()
        .SelectMany(type => type.GetProperties())
        .Select(propertyInfo => new FieldMappedInstance(propertyInfo, fieldName))
        .ToArray();
  }
}