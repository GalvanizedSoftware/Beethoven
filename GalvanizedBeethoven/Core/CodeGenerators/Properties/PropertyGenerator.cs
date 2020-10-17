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
    private readonly PropertyInfo propertyInfo;
    private readonly Type propertyType;
    private readonly string invokerName;
    private readonly string propertyInfoName;
    private readonly GeneratorContext generatorContext;
    private readonly PropertyInvokerGenerator invorkerGenerator;

    internal PropertyGenerator(
      GeneratorContext generatorContext, PropertyInfo propertyInfo, PropertyDefinition propertyDefinition)
    {
      definitions = propertyDefinition.Definitions;
      this.generatorContext = generatorContext;
      this.propertyInfo = propertyInfo;
      propertyType = propertyInfo.PropertyType;
      propertyInfoName = propertyInfo.Name;
      invokerName = $"invoker{propertyInfoName}";
      invorkerGenerator = new PropertyInvokerGenerator(
        $"{generatorContext.GeneratedClassName}{propertyInfoName}{new TagGenerator(generatorContext)}",
        invokerName,
        propertyType,
        definitions);
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
      invorkerGenerator.Generate()
        .Concat(
          GeneratePropertyCode().TagCode(PropertiesCode));

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
