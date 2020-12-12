using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodInvokerGenerator : ICodeGenerator
  {
    private readonly string invokerName;
    private readonly string uniqueName;
    private readonly Func<object> invokerFacory;

    internal MethodInvokerGenerator(
      string uniqueName, MethodInfo methodInfo, string invokerName, MethodDefinition methodDefinition)
    {
      this.uniqueName = uniqueName;
      invokerFacory = () => new RealMethodInvoker(methodInfo, methodDefinition);
      this.invokerName = invokerName;
    }


    public IEnumerable<(CodeType, string)?> Generate()
    {
      InvokerList.SetFactory(uniqueName, invokerFacory);
      string invokerTypeName = typeof(MethodInvoker).GetFullName();
      string instanceTypeName = typeof(IMethodInvokerInstance).GetFullName();
      yield return (FieldsCode, $@"private {instanceTypeName} {invokerName};");
      yield return (ConstructorFields, $@"{invokerName} = new {invokerTypeName}(""{uniqueName}"").CreateInstance(this);");
    }
  }
}
