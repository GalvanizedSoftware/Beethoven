using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
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
      EventInfo eventInfo = generatorContext?.MemberInfo as EventInfo ?? throw new NullReferenceException();
      return Create(eventInfo.EventHandlerType, eventInfo.Name);
    }

    private ICodeGenerator Create(Type type, string name) =>
       (ICodeGenerator)createMethodInfo.Invoke(this, new object[] { name }, new[] { type });

    private ICodeGenerator CreateGeneric<T>(string name) =>
      new SimpleEventGenerator<T>(name);
  }
}