using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class BoundTypeDefinition<T> : BoundTypeDefinition where T : class
  {
    private readonly IDefinition[] definitions;
    private readonly string className;
    private readonly string classNamespace;
    private readonly Type type = typeof(T);
    private readonly BindingParents bindingParents;

    internal BoundTypeDefinition(string className, string classNamespace, PartDefinitions partDefinitions)
    {
      this.className = className;
      this.classNamespace = classNamespace;
      partDefinitions.SetMainTypeUser(type);
      object[] allPartDefinitions = partDefinitions.GetAll<T>();
      definitions = allPartDefinitions.GetAllDefinitions();
      bindingParents = new BindingParents(allPartDefinitions);
    }


    internal override string Generate() =>
      new ClassGenerator(type, className, classNamespace, definitions)
        .Generate()
        .Format(0);

    internal CompiledTypeDefinition<T> Link(Assembly assembly) => 
      new CompiledTypeDefinition<T>(
        assembly.GetType($"{classNamespace}.{className}"), 
        bindingParents);
  }

  public abstract class BoundTypeDefinition
  {
    internal protected BoundTypeDefinition()
    {
    }
    internal abstract string Generate();
  }
}
