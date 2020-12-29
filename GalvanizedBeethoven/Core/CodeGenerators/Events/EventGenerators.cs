using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class EventGenerators : ICodeGenerators
  {
    private readonly EventInfo[] eventInfos;
    private readonly IDefinition[] definitions;

    public EventGenerators(EventInfo[] eventInfos, IEnumerable<IDefinition> definitions)
    {
      this.eventInfos = eventInfos;
      this.definitions = definitions.ToArray();
    }

    public IEnumerable<ICodeGenerator> GetGenerators(GeneratorContext generatorContext)
    {
      foreach (EventInfo eventInfo in eventInfos)
        yield return new EventGenerator(          
          definitions,
          generatorContext.CreateLocal(eventInfo))
          .CreateCodeGenerator();
      yield return new NotifyEventGenerator(eventInfos);
    }
  }
}

