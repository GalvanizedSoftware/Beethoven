using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class BoundTypeDefinitionOfT<T> where T : class
  {
    private readonly string className;
    private readonly string classNamespace;
    private readonly Type type = typeof(T);
    private readonly BindingParents bindingParents;

    internal BoundTypeDefinitionOfT(string className, string classNamespace, PartDefinitions partDefinitions)
    {
      this.className = className;
      this.classNamespace = classNamespace;
      object[] allPartDefinitions = partDefinitions.GetAll<T>();
      DefinitionGenerator = 
        new TypeDefinitionGeneratorOfT<T>(className, classNamespace, allPartDefinitions);
      bindingParents = new BindingParents(allPartDefinitions);
    }

    internal TypeDefinitionGeneratorOfT<T> DefinitionGenerator { get; }

    internal CompiledTypeDefinition<T> Link(Assembly assembly) => 
      new CompiledTypeDefinition<T>(
        assembly.GetType($"{classNamespace}.{className}"), 
        bindingParents);
  }
}
