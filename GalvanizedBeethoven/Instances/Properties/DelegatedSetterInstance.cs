using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Implementations.Properties
{
  public class DelegatedSetterInstance<T> : IPropertyInstance<T>
  {
    private readonly Action<T> delegateAction;

    public DelegatedSetterInstance(Action<T> delegateAction)
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
