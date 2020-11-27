using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
{
  internal class MultiplePropertyInstance<T> : IPropertyInstance<T>
  {
    private readonly IPropertyInstance<T>[] implementation;

    public MultiplePropertyInstance(IEnumerable<IPropertyInstance<T>> implementation)
    {
      this.implementation = implementation.ToArray();
    }

    public bool InvokeGetter(ref T returnValue)
    {
      foreach (IPropertyInstance<T> definition in implementation)
        if (!definition.InvokeGetter(ref returnValue))
          return false;
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      foreach (IPropertyInstance<T> definition in implementation)
        if (!definition.InvokeSetter(newValue))
          return false;
      return true;
    }
  }
}
