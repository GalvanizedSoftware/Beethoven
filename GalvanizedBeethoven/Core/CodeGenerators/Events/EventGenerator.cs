using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Linq;

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

