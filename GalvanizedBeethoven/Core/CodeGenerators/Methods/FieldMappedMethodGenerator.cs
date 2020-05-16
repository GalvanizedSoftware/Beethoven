using System.Reflection;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class FieldMappedMethodGenerator : ICodeGenerator
  {
    private readonly string fieldName;

    public FieldMappedMethodGenerator(string fieldName)
    {
      this.fieldName = fieldName;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      MethodInfo methodInfo = generatorContext?.MemberInfo as MethodInfo;
      if (methodInfo == null)
        yield break;
      foreach (string line in new MethodSignatureGenerator(methodInfo).GenerateDeclaration())
        yield return line;
      yield return "=>".Format(1);
      yield return $"{fieldName}.;".Format(1);
      yield return "";
    }
  }
}