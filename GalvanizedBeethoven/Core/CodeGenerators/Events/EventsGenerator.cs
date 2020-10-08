using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class EventsGenerator : ICodeGenerator
  {
    private readonly EventInfo[] membersInfos;
    private readonly IDefinition[] definitions;

    public EventsGenerator(MemberInfo[] membersInfos, IEnumerable<IDefinition> definitions)
    {
      this.membersInfos = membersInfos
        .OfType<EventInfo>()
        .ToArray();
      this.definitions = definitions.ToArray();
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      foreach (string line in GenerateDeclaration(generatorContext))
        yield return line;
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

    private IEnumerable<string> GenerateDeclaration(GeneratorContext generatorContext) =>
      membersInfos
        .OfType<EventInfo>()
        .SelectMany(eventInfo => GenerateInnerEventCode(generatorContext, eventInfo));

    private IEnumerable<string> GenerateInnerEventCode(GeneratorContext generatorContext, EventInfo eventInfo)
    {
      GeneratorContext localContext = generatorContext.CreateLocal(eventInfo, CodeType.Events);
      ICodeGenerator codeGenerator = definitions
        .Where(definition => definition.CanGenerate(eventInfo))
        .Append(new DefaultEvent())
        .FirstOrDefault()
        .GetGenerator(localContext);
      foreach (string item in codeGenerator?.Generate(localContext))
        yield return item;
    }

    internal IEnumerable<string> GenerateNotifyCode() =>
      membersInfos
        .Select(eventInfo => eventInfo.Name)
        .Select(name => @$"case ""{name}"": return {name}?.DynamicInvoke(values);");
  }
}

