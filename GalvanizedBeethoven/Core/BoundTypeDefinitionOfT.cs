using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Binding;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class BoundTypeDefinitionOfT<T> where T : class
  {
    private readonly BindingParents bindingParents;
    private readonly NameDefinition nameDefinition;

    public BoundTypeDefinitionOfT(NameDefinition nameDefinition, TypeDefinitionGeneratorOfT<T> generator)
    {
      this.nameDefinition = nameDefinition;
      bindingParents = new BindingParents(generator.AllDefinitions);
    }

    internal CompiledTypeDefinition<T> Link(Assembly assembly) => 
      new CompiledTypeDefinition<T>(
        assembly.GetType($"{nameDefinition.ClassNamespace}.{nameDefinition.ClassName}"), 
        bindingParents);
  }
}
