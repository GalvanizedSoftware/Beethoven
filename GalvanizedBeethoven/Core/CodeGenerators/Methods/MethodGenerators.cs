using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodGenerators : ICodeGenerators
  {
    private readonly MethodInfo[] methodInfos;
    private readonly Dictionary<MethodInfo, int> methodIndexes;
    private readonly MethodGeneratorFactory methodGeneratorFactory;

    public MethodGenerators(MethodInfo[] methodInfos, Dictionary<MethodInfo, int> methodIndexes, IEnumerable<IDefinition> definitions)
    {
      this.methodInfos = methodInfos;
      this.methodIndexes = methodIndexes;
      methodGeneratorFactory = new(definitions);
    }

    public IEnumerable<ICodeGenerator> GetGenerators() =>
      methodInfos
        .Select(CreateFactory);

    private ICodeGenerator CreateFactory(MethodInfo methodInfo) =>
      methodGeneratorFactory.Create(methodInfo, methodIndexes[methodInfo]);
  }
}
