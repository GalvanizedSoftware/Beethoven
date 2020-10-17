using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class InvokerGenerator : ICodeGenerator
  {
    private readonly string uniqueName;
    private readonly Func<object> invokerFacory;
    private readonly string invokerTypeName;
    private readonly string instanceTypeName;

    internal static InvokerGenerator CreatePropertyInvoker(
      string uniqueInvokerName, Type propertyType, string propertyName, object[] definitions) =>
      new InvokerGenerator(
        uniqueInvokerName,
        () => PropertyInvokerFactory.Create(propertyType, definitions),
        propertyName,
        typeof(PropertyInvoker<>).MakeGenericType(propertyType),
        typeof(IPropertyInvokerInstance<>).MakeGenericType(propertyType));

    internal static InvokerGenerator CreateMethodInvoker(
      string uniqueInvokerName, MethodInfo methodInfo, string methodName, MethodDefinition methodDefinition) =>
      new InvokerGenerator(
        uniqueInvokerName,
        () => new RealMethodInvoker(methodInfo, methodDefinition),
        methodName,
        typeof(MethodInvoker),
        typeof(IMethodInvokerInstance));

    private InvokerGenerator(
      string uniqueName,
      Func<object> invokerFacory,
      string name,
      Type invokerType,
      Type instanceType)
    {
      this.uniqueName = uniqueName;
      this.invokerFacory = invokerFacory;
      InvokerName = $"invoker{name}";
      invokerTypeName = invokerType.GetFullName();
      instanceTypeName = instanceType?.GetFullName();
    }

    public string InvokerName { get; }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      InvokerList.SetFactory(uniqueName, invokerFacory);
      yield return (FieldsCode, $@"private {instanceTypeName} {InvokerName};");
      yield return (ConstructorFields, $@"{InvokerName} = new {invokerTypeName}(""{uniqueName}"").CreateInstance(this);");
    }
  }
}
