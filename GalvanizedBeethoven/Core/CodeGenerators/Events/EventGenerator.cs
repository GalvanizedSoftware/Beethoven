using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Linq;
using GalvanizedSoftware.Beethoven.Generic.Events;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class EventGenerator
  {
    private readonly IDefinition[] definitions;
    private readonly GeneratorContext generatorContext;

    public EventGenerator(IDefinition[] definitions, GeneratorContext generatorContext)
    {
      this.generatorContext = generatorContext;
      this.definitions = definitions;
    }

    internal ICodeGenerator CreateCodeGenerator() =>
      definitions
        .GetGenerators(generatorContext)
        .FirstOrDefault() ??
        new DefaultEvent().GetGenerator(generatorContext);
  }
}

