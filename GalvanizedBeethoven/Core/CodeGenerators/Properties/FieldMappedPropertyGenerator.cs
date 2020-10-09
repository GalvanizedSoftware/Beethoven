using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class FieldMappedPropertyGenerator : ICodeGenerator
  {
    private readonly string fieldName;

    public FieldMappedPropertyGenerator(string fieldName)
    {
      this.fieldName = fieldName;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext.CodeType != CodeType.Properties)
        yield break;
      PropertyInfo propertyInfo = generatorContext?.MemberInfo as PropertyInfo;
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
