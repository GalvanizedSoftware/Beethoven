using System.Reflection;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Linq;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class FieldMappedMethodGenerator : ICodeGenerator
  {
    private readonly string fieldName;
    private readonly MethodInfo methodInfo;

    public FieldMappedMethodGenerator(string fieldName, GeneratorContext generatorContext)
    {
      this.fieldName = fieldName;
      methodInfo = generatorContext?.MemberInfo as MethodInfo;
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      return Generate().Select(code => ((CodeType, string)?)(MethodsCode, code));
      IEnumerable<string> Generate()
      {
        foreach (string line in new MethodSignatureGenerator(methodInfo).GenerateDeclaration())
          yield return line;
        yield return "=>".Format(1);
        yield return $"{fieldName}.;".Format(1);
      }
    }
  }
}