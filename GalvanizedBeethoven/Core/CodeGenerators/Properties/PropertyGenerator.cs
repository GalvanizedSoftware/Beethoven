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
    private readonly string name;
    private readonly object[] definitions;
    private readonly string invokerTypeName;

    internal PropertyGenerator(PropertyDefinition propertyDefinition)
    {
      this.name = propertyDefinition.Name;
      this.definitions = propertyDefinition.Definitions;
      invokerTypeName = typeof(PropertyInvoker<>)
        .MakeGenericType(propertyDefinition.PropertyType)
        .GetFullName();
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      GeneratorContext localContext = generatorContext.CreateLocal(name);
      PropertyInfo propertyInfo = localContext.MemberInfo as PropertyInfo;
      Type propertyType = propertyInfo.PropertyType;
      string typeName = propertyType.FullName;
      string propertyName = propertyInfo.Name;
      string uniqueInvokerName = $"{generatorContext.GeneratedClassName}{propertyName}{new TagGenerator()}";
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
