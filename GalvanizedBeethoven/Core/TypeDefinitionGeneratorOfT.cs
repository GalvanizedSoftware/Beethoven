using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class TypeDefinitionGeneratorOfT<T> : TypeDefinitionGenerator where T : class
  {
    private readonly IDefinition[] definitions;
    private readonly string className;
    private readonly string classNamespace;
    private readonly Type type = typeof(T);

    internal TypeDefinitionGeneratorOfT(string className, string classNamespace, object[] partDefinitions)
    {
      this.className = className;
      this.classNamespace = classNamespace;
      definitions = partDefinitions.GetAllDefinitions();
    }


    internal override string Generate() =>
      new ClassGenerator(type, className, classNamespace, definitions)
        .Generate()
        .Format(0);
  }
}
