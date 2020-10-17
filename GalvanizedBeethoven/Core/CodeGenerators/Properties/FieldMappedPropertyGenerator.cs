using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class FieldMappedPropertyGenerator : ICodeGenerator
  {
    private readonly string fieldName;
    private readonly PropertyInfo propertyInfo;

    public FieldMappedPropertyGenerator(PropertyInfo propertyInfo, string fieldName)
    {
      this.fieldName = fieldName;
      this.propertyInfo = propertyInfo;
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      return Generate().Select(code => ((CodeType, string)?)(PropertiesCode, code));
      IEnumerable<string> Generate()
      {
        Type propertyType = propertyInfo.PropertyType;
        string typeName = propertyType.GetFullName();
        string propertyName = propertyInfo.Name;
        yield return $@"public {typeName} {propertyName}";
        yield return "{";
        if (propertyInfo.CanRead)
          yield return $"get => {fieldName}.{propertyName};".Format(1);
        if (propertyInfo.CanWrite)
          yield return $"set => {fieldName}.{propertyName} = value;".Format(1);
        yield return "}";
        yield return "";
      }
    }
  }
}

