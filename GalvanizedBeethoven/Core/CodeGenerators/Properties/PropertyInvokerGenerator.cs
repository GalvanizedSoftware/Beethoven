using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using static GalvanizedSoftware.Beethoven.Core.GeneratorHelper;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class PropertyInvokerGenerator : ICodeGenerator
  {
    private readonly string invokerName;
    private readonly string invokerType;
    private readonly string invokerInstanceType;

    public PropertyInvokerGenerator(string invokerName, Type propertyType)
    {
      this.invokerName = invokerName;
      invokerType = typeof(IPropertyInvoker<>).MakeGenericType(propertyType).GetFullName();
      invokerInstanceType = typeof(IPropertyInvokerInstance<>).MakeGenericType(propertyType).GetFullName();
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      yield return (FieldsCode, $@"private {invokerInstanceType} {invokerName};");
      yield return (ConstructorFields,
        $@"{invokerName} = {InstanceListName}.GetInstance<{invokerType}>(""{invokerName}"").CreateInstance(this); ");
    }
  }
}
