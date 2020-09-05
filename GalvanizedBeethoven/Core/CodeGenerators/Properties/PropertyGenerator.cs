using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

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

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
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
