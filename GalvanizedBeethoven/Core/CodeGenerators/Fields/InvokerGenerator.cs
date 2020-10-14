using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  class InvokerGenerator : ICodeGenerator
  {
    private readonly string uniqueInvokerName;
    private readonly object invokerInstance;
    private readonly string invokerTypeName;

    public InvokerGenerator(string uniqueInvokerName, object invokerInstance, string invokerName, Type type)
    {
      this.uniqueInvokerName = uniqueInvokerName;
      this.invokerInstance = invokerInstance;
      this.InvokerName = invokerName;
      invokerTypeName = type.GetFullName();

    }

    public string InvokerName { get; }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      InvokerList.SetInvoker(uniqueInvokerName, invokerInstance);
      yield return (FieldsCode, $@"private {invokerTypeName} {InvokerName};");
      yield return (ConstructorFields, $@"{InvokerName} = new {invokerTypeName}(""{uniqueInvokerName}"");");
    }
  }
}
