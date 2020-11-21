using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodGenerator : ICodeGenerator
  {
    private readonly GeneratorContext generatorContext;

    public MethodGenerator(GeneratorContext generatorContext, MethodDefinition methodDefinition)
    {
      this.methodDefinition = methodDefinition;
      this.generatorContext = generatorContext;
    }

    private readonly MethodDefinition methodDefinition;

    public IEnumerable<(CodeType, string)?> Generate()
    {
      MethodInfo methodInfo = generatorContext?.MemberInfo as MethodInfo;
      string methodName = $"{methodInfo.Name}{generatorContext.MethodIndex}";
      string uniqueInvokerName = $"{generatorContext.GeneratedClassName}{methodName}_{new TagGenerator(generatorContext)}";
      string invokerName = $"invoker{methodName}";
      ICodeGenerator invorkerGenerator = new MethodInvokerGenerator(
        uniqueInvokerName, methodInfo, invokerName, methodDefinition);
      return invorkerGenerator.Generate()
        .Concat(
          Generate()
            .Select(code => ((CodeType, string)?)(MethodsCode, code)));
      IEnumerable<string> Generate()
      {
        ParameterInfo[] parameters = methodInfo.GetParametersSafe().ToArray();
        Type returnType = methodInfo.ReturnType;
        MethodSignatureGenerator methodSignatureGenerator = new MethodSignatureGenerator(methodInfo);
        foreach (string line in methodSignatureGenerator.GenerateDeclaration())
          yield return line;
        yield return "{";
        string parametersName = $"{invokerName}Parameters";
        ParametersGenerator parametersGenerator = new ParametersGenerator(parameters, parametersName);
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