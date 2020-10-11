using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class CodeGeneratorExtensions
  {
    internal static ICodeGenerator WrapLocal(this ICodeGenerator codeGenerator, MethodInfo methodInfo, int? index) =>
      new LocalMethodCodeGenerator(codeGenerator, methodInfo, index);

    internal static ICodeGenerator WrapLocal(this ICodeGenerator codeGenerator, PropertyInfo propertyInfo) =>
     new LocalPropertyCodeGenerator(codeGenerator, propertyInfo);

    internal static ICodeGenerator WrapLocal(this ICodeGenerator codeGenerator, EventInfo evnetInfo) =>
       new LocalEventCodeGenerator(codeGenerator, evnetInfo);
  }
}
