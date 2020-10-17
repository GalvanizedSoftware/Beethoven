using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Implementations.Properties
{
  public class DelegatedGetterInstance<T> : IPropertyInstance<T>
  {
    private readonly Func<T> delegateFunc;

    public DelegatedGetterInstance(Func<T> delegateFunc)
    {
      this.delegateFunc = delegateFunc;
    }

    // ReSharper disable once RedundantAssignment
    public bool InvokeGetter(ref T returnValue)
    {
      returnValue = delegateFunc();
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      return true;
    }
  }
}
