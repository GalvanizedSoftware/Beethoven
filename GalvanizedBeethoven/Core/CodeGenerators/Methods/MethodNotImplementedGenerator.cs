using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  public sealed class MethodNotImplementedGenerator : ICodeGenerator
  {
    private readonly MethodInfo methodInfo;

    public MethodNotImplementedGenerator(MethodInfo methodInfo)
    {
      this.methodInfo = methodInfo;
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      return GenerateLocal().Select(code => ((CodeType, string)?)(MethodsCode, code));
      IEnumerable<string> GenerateLocal()
      {
        foreach (string line in new MethodSignatureGenerator(methodInfo).GenerateDeclaration())
          yield return line;
        yield return "=>".Format(1);
        yield return "throw new System.MissingMethodException();".Format(1);
        yield return "";
      }
    }
  }
}