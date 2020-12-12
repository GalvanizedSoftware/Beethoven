using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class CodeGeneratorExtensions
  {
    internal static ICodeGenerator WrapLocal(this ICodeGenerator codeGenerator,
      GeneratorContext generatorContext, MethodInfo methodInfo, int? index) =>
      new LocalMethodCodeGenerator(codeGenerator);

    internal static ICodeGenerator WrapLocal(this ICodeGenerator codeGenerator, GeneratorContext generatorContext, PropertyInfo propertyInfo) =>
      new LocalPropertyCodeGenerator(codeGenerator);
  }
}
