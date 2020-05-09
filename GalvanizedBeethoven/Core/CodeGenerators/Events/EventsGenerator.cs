﻿using GalvanizedSoftware.Beethoven.Generic.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class EventsGenerator
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

    internal IEnumerable<string> Generate(GeneratorContext generatorContext)
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
        .SelectMany(eventInfo => GenerateInnerEventCode(
          generatorContext.CreateLocal(eventInfo), () => DefaultSimpleEvent.Create(eventInfo)));

    private IEnumerable<string> GenerateInnerEventCode(GeneratorContext generatorContext,
      Func<ICodeGenerator> defaultCreator)
    {
      ICodeGenerator codeGenerator = definitions
        .Where(definition => definition.CanGenerate(generatorContext.MemberInfo))
        .Select(generator => generator.GetGenerator())
        .FirstOrDefault() ?? defaultCreator();
      foreach (string item in codeGenerator?.Generate(generatorContext))
        yield return item;
    }

    internal IEnumerable<string> GenerateNotifyCode() =>
      membersInfos
        .Select(eventInfo => eventInfo.Name)
        .Select(name => @$"case ""{name}"": return {name}?.DynamicInvoke(values);");
  }
}

