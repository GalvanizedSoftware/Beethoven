using System;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Definitions;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  internal class TypeDefinitionGeneratorOfT<T> : TypeDefinitionGenerator where T : class
  {
    private readonly Type type = typeof(T);
    private readonly NameDefinition nameDefinition;
    private readonly MemberInfoList memberInfoList;

    internal TypeDefinitionGeneratorOfT(MemberInfoList memberInfoList, NameDefinition nameDefinition, PartDefinitions partDefinitions)
    {
      this.nameDefinition = nameDefinition;
      this.memberInfoList = memberInfoList;
      Definitions = new LinkedDefinitions<T>(partDefinitions).ToArray();
    }

    internal IDefinition[] Definitions { get; }

    internal override string Generate() =>
      new ClassGenerator(memberInfoList, type, nameDefinition, Definitions)
        .Generate()
        .Format(0);
  }
}
