﻿using GalvanizedSoftware.Beethoven.Core.Invokers;
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
    private readonly PropertyInfo propertyInfo;
    private readonly GeneratorContext generatorContext;

    internal PropertyGenerator(
      GeneratorContext generatorContext, PropertyInfo propertyInfo, PropertyDefinition propertyDefinition)
    {
      definitions = propertyDefinition.Definitions;
      invokerTypeName = typeof(PropertyInvoker<>)
        .MakeGenericType(propertyDefinition.PropertyType)
        .GetFullName();
      this.generatorContext = generatorContext;
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
