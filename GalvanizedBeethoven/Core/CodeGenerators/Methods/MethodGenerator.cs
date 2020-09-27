﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers;
using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodGenerator : ICodeGenerator
  {
    public MethodGenerator(MethodDefinition methodDefinition)
    {
      this.methodDefinition = methodDefinition;
    }

    private readonly MethodDefinition methodDefinition;

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      MethodInfo methodInfo = generatorContext?.MemberInfo as MethodInfo;
      if (methodInfo == null || generatorContext == null)
        yield break;
      string methodName = $"{methodInfo.Name}{generatorContext.MethodIndex}";
      string uniqueInvokerName = $"{generatorContext.GeneratedClassName}{methodName}_{new TagGenerator(generatorContext)}";
      InvokerList.SetInvoker(uniqueInvokerName,
        InvokerFactory.CreateMethodInvoker(methodInfo, methodDefinition));
      string invokerName = $"invoker{methodName}";
      string invokerTypeName = typeof(MethodInvoker).GetFullName();
      ParameterInfo[] parameters = methodInfo.GetParametersSafe().ToArray();
      Type returnType = methodInfo.ReturnType;
      yield return $@"private {invokerTypeName} {invokerName} = new {invokerTypeName}(""{uniqueInvokerName}"");";
      MethodSignatureGenerator methodSignatureGenerator = new MethodSignatureGenerator(methodInfo);
      foreach (string line in methodSignatureGenerator.GenerateDeclaration())
        yield return line;
      yield return "{";
      string parametersName = $"{invokerName}Parameters";
      ParametersGenerator parametersGenerator = new ParametersGenerator(parameters, parametersName);
      yield return parametersGenerator.GeneratePreInvoke().Format(1);
      string returnValueName = $"{invokerName}Result";
      yield return $"object {returnValueName} = {invokerName}.Invoke(this, {GetGenericTypes(methodInfo)}, {parametersName});".Format(1);
      foreach (string line in parametersGenerator.GeneratePostInvoke())
        yield return line.Format(1);
      if (returnType != typeof(void))
        yield return $"return ({returnType.GetFullName()}){returnValueName};".Format(1);
      yield return "}";
      yield return "";
    }

    private string GetGenericTypes(MethodInfo methodInfo) =>
      $"new System.Type[]{{{string.Join(", ", methodInfo.GetGenericArguments().Select(GetTypeof))}}}";

    private string GetTypeof(Type type) =>
      $"typeof({type.GetFullName()})";
  }
}