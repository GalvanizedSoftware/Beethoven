using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class PropertyGenerator : ICodeGenerator
  {
    private readonly Type propertyType;
    private readonly string invokerName;
    private readonly string propertyInfoName;
    private readonly Type invokerInstanceType;

    internal PropertyGenerator(PropertyInfo propertyInfo)
    {
      propertyType = propertyInfo.PropertyType;
      propertyInfoName = propertyInfo.Name;
      invokerName = $"invoker{propertyInfoName}";
      invokerInstanceType = typeof(IPropertyInvokerInstance<>).MakeGenericType(propertyType);
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      CodeGeneratorList invokerGenerators = new
      (
        new FieldDeclarationGenerator(invokerInstanceType, invokerName),
        new PropertyInvokerGenerator(invokerName, propertyType)
      );
      return invokerGenerators.Generate()
        .Concat(
          GeneratePropertyCode().TagCode(PropertiesCode));
    }

    private IEnumerable<string> GeneratePropertyCode()
    {
      yield return $@"public {propertyType.GetFullName()} {propertyInfoName}";
      yield return "{";
      yield return $"get => {invokerName}.InvokeGetter();".Format(1);
      yield return $"set => {invokerName}.InvokeSetter(value);".Format(1);
      yield return "}";
    }
  }
}
