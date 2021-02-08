using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  internal class MultiplePropertyInvoker<T> : IPropertyInvoker<T>
  {
    private readonly IPropertyInvoker<T>[] implementation;

    public MultiplePropertyInvoker(IEnumerable<IPropertyInvoker<T>> implementation)
    {
      this.implementation = implementation.ToArray();
    }

    public bool InvokeGetter(ref T returnValue)
    {
      foreach (IPropertyInvoker<T> definition in implementation)
        if (!definition.InvokeGetter(ref returnValue))
          return false;
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      foreach (IPropertyInvoker<T> definition in implementation)
        if (!definition.InvokeSetter(newValue))
          return false;
      return true;
    }
  }
}
