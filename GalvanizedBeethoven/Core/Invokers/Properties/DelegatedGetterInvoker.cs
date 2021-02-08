using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class DelegatedGetterInvoker<T> : IPropertyInvoker<T>
  {
    private readonly Func<T> delegateFunc;

    public DelegatedGetterInvoker(Func<T> delegateFunc)
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
