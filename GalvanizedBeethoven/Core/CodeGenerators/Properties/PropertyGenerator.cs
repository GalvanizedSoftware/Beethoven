using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
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
    private readonly GeneratorContext generatorContext;
    private readonly InvokerGenerator invorkerGenerator;

    internal PropertyGenerator(
      GeneratorContext generatorContext, PropertyInfo propertyInfo, PropertyDefinition propertyDefinition)
    {
      definitions = propertyDefinition.Definitions;
      this.generatorContext = generatorContext;
      this.propertyInfo = propertyInfo;
      propertyType = propertyInfo.PropertyType;
      invorkerGenerator = CreateInvokerGenerator();
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
      invorkerGenerator.Generate()
        .Concat(
          GeneratePropertyCode().TagCode(PropertiesCode));

    private IEnumerable<string> GeneratePropertyCode()
    {
      yield return $@"public {propertyType.GetFullName()} {propertyInfo.Name}";
      yield return "{";
      yield return $"get => {invorkerGenerator.InvokerName}.InvokeGetter();".Format(1);
      yield return $"set => {invorkerGenerator.InvokerName}.InvokeSetter(value);".Format(1);
      yield return "}";
    }

    private InvokerGenerator CreateInvokerGenerator() =>
      InvokerGenerator.CreatePropertyInvoker(
        $"{generatorContext.GeneratedClassName}{propertyInfo.Name}{new TagGenerator(generatorContext)}",
        propertyType, 
        propertyInfo.Name, 
        definitions);
  }
}
