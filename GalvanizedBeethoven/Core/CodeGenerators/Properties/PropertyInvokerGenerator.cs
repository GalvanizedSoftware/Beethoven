using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class PropertyInvokerGenerator : ICodeGenerator
  {
    private readonly string uniqueName;
    private readonly Func<object> invokerFacory;
    private readonly string invokerTypeName;
    private readonly string instanceTypeName;
    private readonly string invokerName;

    public PropertyInvokerGenerator(
      string uniqueName, string invokerName, Type propertyType, object[] definitions)
    {
      this.uniqueName = uniqueName;
      invokerFacory = () => PropertyInvokerFactory.Create(propertyType, definitions);
      this.invokerName = invokerName;
      invokerTypeName = typeof(PropertyInvoker<>).MakeGenericType(propertyType).GetFullName();
      instanceTypeName = typeof(IPropertyInvokerInstance<>).MakeGenericType(propertyType).GetFullName();
    }


    public IEnumerable<(CodeType, string)?> Generate()
    {
      InvokerList.SetFactory(uniqueName, invokerFacory);
      yield return (FieldsCode, $@"private {instanceTypeName} {invokerName};");
      yield return (ConstructorFields, $@"{invokerName} = new {invokerTypeName}(""{uniqueName}"").CreateInstance(this);");
    }
  }
}
