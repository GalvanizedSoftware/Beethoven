using System;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven
{
  public class CompiledTypeDefinition<T> where T : class
  {
    private readonly Type compiledType;
    private readonly IBindingParent bindingParents;

    public CompiledTypeDefinition(Type compiledType, IBindingParent bindingParents)
    {
      this.compiledType = compiledType;
      this.bindingParents = bindingParents;
    }

    public T Create(params object[] parameters)
    {
      object instance = compiledType.Create(parameters);
      bindingParents.Bind(instance);
      return instance as T;
    }
  }
}
