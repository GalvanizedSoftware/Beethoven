using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Generic.Events;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class EventGenerator
  {
    private readonly IDefinition[] definitions;
    private readonly EventInfo eventInfo;

    public EventGenerator(IDefinition[] definitions, EventInfo eventInfo)
    {
	    this.eventInfo = eventInfo;
      this.definitions = definitions;
    }

    internal ICodeGenerator CreateCodeGenerator() =>
      definitions
        .GetGenerators(eventInfo)
        .FirstOrDefault() ??
        new DefaultEvent().GetGenerator(eventInfo);
  }
}

