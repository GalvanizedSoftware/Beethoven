using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class NotifyEventGenerator : ICodeGenerator
  {
    private readonly EventInfo[] eventInfos;

    public NotifyEventGenerator(EventInfo[] eventInfos)
    {
      this.eventInfos = eventInfos;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext)
    {
      return Generate().Select(code => ((CodeType, string)?)(EventsCode, code));
      IEnumerable<string> Generate()
      {
        yield return "";
        yield return $"object {ClassGenerator.GeneratedClassName}.NotifyEvent(string eventName, object[] values)";
        yield return "{";
        yield return "	switch (eventName)";
        yield return "	{";
        foreach (string line in GenerateNotifyCode())
          yield return $"		{line}";
        yield return "		default: return null;";
        yield return "	}";
        yield return "}";
      }
    }

    internal IEnumerable<string> GenerateNotifyCode() =>
      eventInfos
        .Select(eventInfo => eventInfo.Name)
        .Select(name => @$"case ""{name}"": return {name}?.DynamicInvoke(values);");
  }
}

