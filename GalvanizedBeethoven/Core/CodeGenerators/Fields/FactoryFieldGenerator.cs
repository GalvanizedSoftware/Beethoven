using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class FactoryFieldGenerator : ICodeGenerator<FieldInfo>
  {
    private readonly Type type;
    private readonly string fieldName;
    private readonly Func<object> factoryFunc;
    private static readonly string invokerTypeName = typeof(RuntimeInvokerFactory).GetFullName();

    public FactoryFieldGenerator(Type type, string fieldName, Func<object> factoryFunc)
    {
      this.type = type ?? throw new NullReferenceException();
      this.fieldName = fieldName;
      this.factoryFunc = factoryFunc;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext.CodeType != CodeType.Fields)
        yield break;
      string uniqueBackingId = $"{generatorContext?.GeneratedClassName}{fieldName}Factory";
      InvokerList.SetInvoker(uniqueBackingId, factoryFunc);
      string typeName = type.GetFullName();
      yield return
        $@"{typeName} {fieldName} = new {invokerTypeName}(""{uniqueBackingId}"").Create<{typeName}>();";
    }
  }
}
