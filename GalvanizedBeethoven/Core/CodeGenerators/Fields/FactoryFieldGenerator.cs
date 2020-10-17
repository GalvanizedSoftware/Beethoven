using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.GeneratorHelper;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class FactoryFieldGenerator : ICodeGenerator
  {
    private readonly string fieldName;
    private readonly Func<object> factoryFunc;
    private readonly string typeName;
    private string generatedClassName;

    public FactoryFieldGenerator(Type type, string fieldName, GeneratorContext generatorContext, Func<object> factoryFunc)
    {
      this.fieldName = fieldName;
      this.factoryFunc = factoryFunc;
      generatedClassName = generatorContext?.GeneratedClassName;
      typeName = type.GetFullName();
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      yield return (FieldsCode, $@"{typeName} {fieldName};");
      string uniqueBackingId = $"{generatedClassName}{fieldName}Factory";
      InvokerList.SetFactory(uniqueBackingId, () => factoryFunc);
      yield return (ConstructorCode,
        $@"{fieldName} = new {InvokerTypeName}(""{ uniqueBackingId}"").Create <{ typeName}> (); ");
    }
  }
}
