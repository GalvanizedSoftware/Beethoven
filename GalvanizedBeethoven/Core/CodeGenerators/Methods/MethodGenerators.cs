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
    private readonly Dictionary<MethodInfo, int?> methodIndexes;

    public MethodGenerators(MemberInfo[] membersInfos, IEnumerable<IDefinition> definitions)
    {
      methodInfos = membersInfos
                    .OfType<MethodInfo>()
                    .Where(methodInfo => !methodInfo.IsSpecialName)
                    .ToArray();
      this.definitions = definitions
        .OfType<MethodDefinition>()
        .ToArray();
      methodIndexes = methodInfos
        .ToDictionary(
          methodInfo => methodInfo,
          methodInfo => FindIndex(methodInfo));
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

    private int? FindIndex(MethodInfo methodInfo)
    {
      string name = methodInfo.Name;
      MethodInfo[] methodInfoArray = methodInfos
        .Where(info => info.Name == name)
        .ToArray();
      return methodInfoArray.Length == 1 ?
        0 :
        methodInfoArray
        .Select((info, i) => (info, i))
        .Where(tuple => tuple.info == methodInfo)
        .Select(tuple => (int?)tuple.i)
        .First();
    }
  }
}
