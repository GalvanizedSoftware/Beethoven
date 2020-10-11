using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class PropertyGenerator : ICodeGenerator
  {
    private readonly object[] definitions;
    private readonly string invokerTypeName;

    internal PropertyGenerator(PropertyDefinition propertyDefinition)
    {
      definitions = propertyDefinition.Definitions;
      invokerTypeName = typeof(PropertyInvoker<>)
        .MakeGenericType(propertyDefinition.PropertyType)
        .GetFullName();
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext.CodeType != PropertiesCode)
        return Enumerable.Empty<(CodeType, string)?>();
      return Generate().Select(code => ((CodeType, string)?)(PropertiesCode, code));
      IEnumerable<string> Generate()
      {
        PropertyInfo propertyInfo = generatorContext.MemberInfo as PropertyInfo;
        Type propertyType = propertyInfo.PropertyType;
        string typeName = propertyType.GetFullName();
        string propertyName = propertyInfo.Name;
        string uniqueInvokerName = $"{generatorContext.GeneratedClassName}{propertyName}{new TagGenerator(generatorContext)}";
        InvokerList.SetInvoker(uniqueInvokerName,
          InvokerFactory.CreatePropertyInvoker(propertyType, definitions));
        string invokerName = $"invoker{propertyName}";
        yield return $@"private {invokerTypeName} {invokerName} = new {invokerTypeName}(""{uniqueInvokerName}"");";
        yield return $@"public {typeName} {propertyInfo.Name}";
        yield return "{";
        yield return $"get => {invokerName}.InvokeGet(this);".Format(1);
        yield return $"set => {invokerName}.InvokeSet(this, value);".Format(1);
        yield return "}";
      }
    }
  }
}
