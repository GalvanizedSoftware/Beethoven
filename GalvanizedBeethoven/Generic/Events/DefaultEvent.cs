using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Events
{
  public class DefaultEvent : DefaultDefinition
  {
    private static readonly MethodInfo createMethodInfo =
      typeof(DefaultEvent).GetMethod(nameof(CreateGeneric), ReflectionConstants.ResolveFlags);

    public override int SortOrder => 2;

    public override bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo is EventInfo;

    public override ICodeGenerator GetGenerator(GeneratorContext generatorContext)
    {
      EventInfo eventInfo = generatorContext?.MemberInfo as EventInfo;
      return eventInfo == null ? null :
        Create(eventInfo.EventHandlerType, eventInfo.Name);
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
      yield break;
    }

    private ICodeGenerator Create(Type type, string name) =>
       (ICodeGenerator)createMethodInfo.Invoke(this, new object[] { name }, new[] { type });

#pragma warning disable CA1822 // Mark members as static
    private ICodeGenerator CreateGeneric<T>(string name) =>
      new SimpleEventGenerator<T>(name);
#pragma warning restore CA1822 // Mark members as static
  }
}
