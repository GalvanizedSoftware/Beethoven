using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class CodeGeneratorExtensions
  {
    internal static IEnumerable<string> ConditionalGenerate(this ICodeGenerator codeGenerator, GeneratorContext generatorContext) =>
      codeGenerator.Generate(generatorContext);
  }
}
