using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class TypeDefinitionGeneratorOfT<T> : TypeDefinitionGenerator where T : class
  {
    private readonly IDefinition[] definitions;
    private readonly Type type = typeof(T);
    private readonly NameDefinition nameDefinition;

    internal TypeDefinitionGeneratorOfT(NameDefinition nameDefinition, PartDefinitions partDefinitions)
    {
      this.nameDefinition = nameDefinition;
      AllDefinitions = partDefinitions.GetAll<T>();
      definitions = AllDefinitions.GetAllDefinitions();
    }

    internal object[] AllDefinitions { get; }

    internal override string Generate() =>
      new ClassGenerator(type, nameDefinition, definitions)
        .Generate()
        .Format(0);
  }
}
