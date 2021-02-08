using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class DelegatedSetterInvoker<T> : IPropertyInvoker<T>
  {
    private readonly Action<T> delegateAction;

    public DelegatedSetterInvoker(Action<T> delegateAction)
    {
      this.delegateAction = delegateAction;
    }

    public bool InvokeGetter(ref T returnValue)
    {
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      delegateAction(newValue);
      return true;
    }
  }
}
