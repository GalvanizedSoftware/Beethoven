using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class CodeGeneratorExtensions
  {
    internal static ICodeGenerator WrapLocal(this ICodeGenerator codeGenerator,
      GeneratorContext generatorContext, MethodInfo methodInfo, int? index) =>
      new LocalMethodCodeGenerator(generatorContext, codeGenerator, methodInfo, index);

    internal static ICodeGenerator WrapLocal(this ICodeGenerator codeGenerator, GeneratorContext generatorContext, PropertyInfo propertyInfo) =>
      new LocalPropertyCodeGenerator(generatorContext, codeGenerator, propertyInfo);
  }
}
