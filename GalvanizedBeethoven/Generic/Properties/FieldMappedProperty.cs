﻿using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class FieldMappedProperty : IDefinition
  {
    private PropertyInfo propertyInfo;
    private string fieldName;

    public FieldMappedProperty(PropertyInfo propertyInfo, string fieldName)
    {
      this.propertyInfo = propertyInfo;
      this.fieldName = fieldName;
    }

    public int SortOrder => 2;

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo switch
      {
        PropertyInfo propertyInfo =>
             propertyInfo.Name == this.propertyInfo.Name &&
             propertyInfo.PropertyType == this.propertyInfo.PropertyType,
        _ => false,
      };

    public ICodeGenerator GetGenerator(GeneratorContext _) =>
      new FieldMappedPropertyGenerator(fieldName);
  }
}