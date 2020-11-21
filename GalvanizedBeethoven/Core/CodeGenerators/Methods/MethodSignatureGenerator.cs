using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodSignatureGenerator
  {
    private readonly MethodInfo methodInfo;
    private readonly string returnTypeName;
    private readonly string genericParameters;
    private readonly string inputParameters;

    public MethodSignatureGenerator(MethodInfo methodInfo)
    {
      this.methodInfo = methodInfo;
      Type returnType = methodInfo.ReturnType;
      bool returnsValue = returnType != typeof(void);
      returnTypeName = !returnsValue ? "void" : returnType.GetFullName();
      ParameterInfo[] parameters = methodInfo.GetParametersSafe().ToArray();
      inputParameters = string.Join(", ",
        parameters
          .Select(info => $"{GetParameter(info)}"));
      genericParameters = methodInfo.IsGenericMethod ?
        "<" +
        string.Join(", ",
        methodInfo
          .GetGenericArguments()
          .Select(p => p.Name)) +
        ">" :
          "";
    }

    internal IEnumerable<string> GenerateDeclaration()
    {
      string declaringType = methodInfo.DeclaringType.GetFullName();
      yield return $@"{returnTypeName} {declaringType}.{methodInfo.Name}{genericParameters}({inputParameters})";
      foreach (string line in GenerateConstraints())
        yield return line.Format(1);
    }

    private IEnumerable<string> GenerateConstraints() =>
      methodInfo
        .GetGenericArguments()
        .Where(type => type.BaseType != typeof(object))
        .Select(type => $" where {type.Name} : {type.GetBaseName()}");

    private static string GetParameter(ParameterInfo info) =>
      string.Join(" ", GetParameterParts(info));

    private static IEnumerable<string> GetParameterParts(ParameterInfo info)
    {
      Type parameterType = info.ParameterType;
      if (info.IsOut)
        yield return "out";
      else if (parameterType.IsByRef)
        yield return "ref";
      yield return parameterType.GetFullName();
      yield return info.Name;
    }
  }
}