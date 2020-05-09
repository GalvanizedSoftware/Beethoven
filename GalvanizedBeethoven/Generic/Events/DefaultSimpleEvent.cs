using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DefaultSimpleEvent : ICodeGenerator
  {
    private static readonly MethodInfo createMethodInfo =
      typeof(DefaultSimpleEvent).GetMethod(nameof(CreateGeneric), Constants.ResolveFlags);

    public DefaultSimpleEvent()
    {
    }

    public ICodeGenerator Create(Type type, string name) =>
      (ICodeGenerator)createMethodInfo.Invoke(this, new object[] { name }, new[] { type });

    public static ICodeGenerator Create(EventInfo eventInfo) =>
      new DefaultSimpleEvent().Create(eventInfo?.EventHandlerType, eventInfo?.Name);

    private SimpleEventGenerator<T> CreateGeneric<T>(string name) =>
      new SimpleEventGenerator<T>(name);

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      EventInfo eventInfo = generatorContext?.MemberInfo as EventInfo;
      return eventInfo == null ?
        Enumerable.Empty<string>() :
        Create(eventInfo.EventHandlerType, eventInfo.Name)
        .Generate(generatorContext);
    }
  }
}