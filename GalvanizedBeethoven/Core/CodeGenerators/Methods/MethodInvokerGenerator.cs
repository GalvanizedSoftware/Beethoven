using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using static GalvanizedSoftware.Beethoven.Core.GeneratorHelper;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodInvokerGenerator : ICodeGenerator
  {
    private readonly string invokerName;
    private static readonly string invokerType = typeof(IMethodInvoker).GetFullName();
    private static readonly string invokerInstanceType = typeof(IMethodInvokerInstance).GetFullName();

    internal MethodInvokerGenerator(string invokerName)
    {
      this.invokerName = invokerName;
    }


    public IEnumerable<(CodeType, string)?> Generate()
    {
      yield return (FieldsCode, $@"private {invokerInstanceType} {invokerName};");
      yield return (ConstructorCode,
        $@"{invokerName} = {InstanceListName}.GetInstance<{invokerType}>(""{invokerName}"").CreateInstance(this); ");
    }
  }
}
