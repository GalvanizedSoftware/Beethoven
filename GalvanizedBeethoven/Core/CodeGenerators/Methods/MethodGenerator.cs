using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodGenerator : ICodeGenerator
  {
    private readonly MethodInfo methodInfo;
    private readonly int methodIndex;

    public MethodGenerator(GeneratorContext generatorContext) :
      this(
        generatorContext?.MemberInfo as MethodInfo, 
        generatorContext?.MethodIndex ?? 0)
    {
    }

    public MethodGenerator(MethodInfo methodInfo, int methodIndex)
    {
      this.methodInfo = methodInfo;
      this.methodIndex = methodIndex;
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      string methodName = $"{methodInfo?.Name}{methodIndex}";
      string invokerName = $"invoker{methodName}";
      CodeGeneratorList invokerGenerators = new
        (
        new FieldDeclarationGenerator(typeof(IMethodInvokerInstance), invokerName),
        new MethodInvokerGenerator(invokerName)
        );
      return invokerGenerators.Generate()
        .Concat(
          GenerateLocal()
            .Select(code => ((CodeType, string)?)(MethodsCode, code)));
      IEnumerable<string> GenerateLocal()
      {
        ParameterInfo[] parameters = methodInfo.GetParametersSafe().ToArray();
        Type returnType = methodInfo?.ReturnType;
        MethodSignatureGenerator methodSignatureGenerator = new(methodInfo);
        foreach (string line in methodSignatureGenerator.GenerateDeclaration())
          yield return line;
        yield return "{";
        string parametersName = $"{invokerName}Parameters";
        ParametersGenerator parametersGenerator = new(parameters, parametersName);
        yield return parametersGenerator.GeneratePreInvoke().Format(1);
        string returnValueName = $"{invokerName}Result";
        yield return $"object {returnValueName} = {invokerName}.Invoke({GetGenericTypes(methodInfo)}, {parametersName});".Format(1);
        foreach (string line in parametersGenerator.GeneratePostInvoke())
          yield return line.Format(1);
        if (returnType != typeof(void))
          yield return $"return ({returnType.GetFullName()}){returnValueName};".Format(1);
        yield return "}";
        yield return "";
      }
    }

    private string GetGenericTypes(MethodInfo methodInfo) =>
      $"new System.Type[]{{{string.Join(", ", methodInfo.GetGenericArguments().Select(GetTypeof))}}}";

    private string GetTypeof(Type type) =>
      $"typeof({type.GetFullName()})";
  }
}