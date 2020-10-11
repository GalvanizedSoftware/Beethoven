using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class LocalEventCodeGenerator : ICodeGenerator
  {
    private readonly ICodeGenerator innerCodeGenerator;
    private readonly EventInfo eventInfo;

    public LocalEventCodeGenerator(ICodeGenerator innerCodeGenerator, EventInfo eventInfo)
    {
      this.innerCodeGenerator = innerCodeGenerator;
      this.eventInfo = eventInfo;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext) =>
      innerCodeGenerator.Generate(generatorContext.CreateLocal(eventInfo));
  }
}