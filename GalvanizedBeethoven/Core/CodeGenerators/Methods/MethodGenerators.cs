using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodGenerators : ICodeGenerators
  {
    private readonly MethodInfo[] methodInfos;
    private readonly IEnumerable<MethodDefinition> definitions;
    private readonly Dictionary<MethodInfo, int> methodIndexes;

    public MethodGenerators(MethodInfo[] methodInfos, Dictionary<MethodInfo, int> methodIndexes, IEnumerable<IDefinition> definitions)
    {
      this.methodInfos = methodInfos;
      this.methodIndexes = methodIndexes;
      this.definitions = definitions
        .OfType<MethodDefinition>()
        .ToArray();
    }

    public IEnumerable<ICodeGenerator> GetGenerators(GeneratorContext generatorContext)
    {
      return methodInfos
        .Select(
          methodInfo =>
            CreateFactory(generatorContext, methodInfo));
    }

    private ICodeGenerator CreateFactory(GeneratorContext generatorContext, MethodInfo methodInfo) =>
      new MethodGeneratorFactory(generatorContext, methodInfo, methodIndexes[methodInfo], definitions)
        .Create();
  }
}
