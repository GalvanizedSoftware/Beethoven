using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DelegatedSetter<T> : IPropertyDefinition<T>
  {
    private readonly Action<T> delegateAction;

    public DelegatedSetter(Action<T> delegateAction)
    {
      this.delegateAction = delegateAction;
    }

    public bool InvokeGetter(object _, ref T returnValue)
    {
      return true;
    }

    public bool InvokeSetter(object _, T newValue)
    {
      delegateAction(newValue);
      return true;
    }
  }
}
