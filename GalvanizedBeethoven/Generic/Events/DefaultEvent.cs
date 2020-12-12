using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Events
{
  public class DefaultEvent : IDefinition
  {
    private static readonly MethodInfo createMethodInfo =
      typeof(DefaultEvent).GetMethod(nameof(CreateGeneric), ReflectionConstants.ResolveFlags);

    public int SortOrder => 2;

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo switch
      {
        EventInfo _ => true,
        _ => false,
      };


    public ICodeGenerator GetGenerator(GeneratorContext generatorContext)
    {
      EventInfo eventInfo = generatorContext?.MemberInfo as EventInfo;
      return eventInfo == null ? null : 
        Create(eventInfo.EventHandlerType, eventInfo.Name);
    }

    private ICodeGenerator Create(Type type, string name) =>
       (ICodeGenerator)createMethodInfo.Invoke(this, new object[] { name }, new[] { type });

#pragma warning disable CA1822 // Mark members as static
    private ICodeGenerator CreateGeneric<T>(string name) =>
      new SimpleEventGenerator<T>(name);
#pragma warning restore CA1822 // Mark members as static
  }
}