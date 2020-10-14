using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  public sealed class MethodNotImplementedGenerator : ICodeGenerator
  {
    private readonly GeneratorContext generatorContext;

    public MethodNotImplementedGenerator(GeneratorContext generatorContext)
    {
      this.generatorContext = generatorContext;
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      return Generate().Select(code => ((CodeType, string)?)(MethodsCode, code));
      IEnumerable<string> Generate()
      {
        MethodInfo methodInfo = generatorContext.MemberInfo as MethodInfo;
        foreach (string line in new MethodSignatureGenerator(methodInfo).GenerateDeclaration())
          yield return line;
        yield return "=>".Format(1);
        yield return "throw new System.MissingMethodException();".Format(1);
        yield return "";
      }
    }
  }
}