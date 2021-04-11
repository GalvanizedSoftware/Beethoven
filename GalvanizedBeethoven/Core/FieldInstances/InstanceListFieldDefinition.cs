using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.Core.FieldInstances
{
  internal class InstanceListFieldDefinition<T> : DefaultDefinition where T : class
  {
    private readonly Func<ICodeGenerator> factory;

    internal static InstanceListFieldDefinition<T> Create(MemberInfo factoryMemberInfo, string id) =>
      factoryMemberInfo switch
      {
        null => new(() => InstanceListFieldGenerator<T>.Create(id)),
        _ => new(() => InstanceListFieldGenerator<T>.Create(factoryMemberInfo))
      };

    private InstanceListFieldDefinition(Func<ICodeGenerator> factory)
    {
      this.factory = factory;
    }

    public override ICodeGenerator GetGenerator(MemberInfo memberInfo) =>
      factory();
  }
}
