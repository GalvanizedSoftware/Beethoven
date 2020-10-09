using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  public sealed class MethodNotImplementedGenerator : ICodeGenerator
  {
    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext?.CodeType != CodeType.Methods)
        yield break;
      MethodInfo methodInfo = generatorContext.MemberInfo as MethodInfo;
      foreach (string line in new MethodSignatureGenerator(methodInfo).GenerateDeclaration())
        yield return line;
      yield return "=>".Format(1);
      yield return "throw new System.MissingMethodException();".Format(1);
      yield return "";
    }
  }
}