using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.GeneratorHelper;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class FactoryFieldGenerator : ICodeGenerator
  {
    private readonly Type type;
    private readonly string fieldName;
    private readonly Func<object> factoryFunc;
    private readonly string typeName;

    public FactoryFieldGenerator(Type type, string fieldName, Func<object> factoryFunc)
    {
      this.type = type ?? throw new NullReferenceException();
      this.fieldName = fieldName;
      this.factoryFunc = factoryFunc;
      typeName = type.GetFullName();
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      yield return generatorContext.CodeType switch
      {
        CodeType.Fields => $@"{typeName} {fieldName};",
        CodeType.ConstructorCode => GenerateConstructorCode(generatorContext),
        _ => null
      };
    }

    private string GenerateConstructorCode(GeneratorContext generatorContext)
    {
      string uniqueBackingId = $"{generatorContext?.GeneratedClassName}{fieldName}Factory";
      InvokerList.SetInvoker(uniqueBackingId, factoryFunc);
      return $@"{fieldName} = new {InvokerTypeName}(""{ uniqueBackingId}"").Create <{ typeName}> (); ";
    }
  }
}
