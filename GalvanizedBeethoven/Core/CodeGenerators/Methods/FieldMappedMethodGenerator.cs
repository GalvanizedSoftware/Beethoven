using System.Reflection;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Linq;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class FieldMappedMethodGenerator : ICodeGenerator
  {
    private readonly string fieldName;

    public FieldMappedMethodGenerator(string fieldName)
    {
      this.fieldName = fieldName;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext.CodeType != MethodsCode)
        return Enumerable.Empty<(CodeType, string)?>();
      return Generate().Select(code => ((CodeType, string)?)(MethodsCode, code));
      IEnumerable<string> Generate()
      {
        MethodInfo methodInfo = generatorContext?.MemberInfo as MethodInfo;
        foreach (string line in new MethodSignatureGenerator(methodInfo).GenerateDeclaration())
          yield return line;
        yield return "=>".Format(1);
        yield return $"{fieldName}.;".Format(1);
      }
    }
  }
}