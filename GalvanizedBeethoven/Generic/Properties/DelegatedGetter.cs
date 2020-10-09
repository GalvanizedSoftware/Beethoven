using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DelegatedGetter<T> : IPropertyDefinition<T>
  {
    private readonly Func<T> delegateFunc;

    public DelegatedGetter(Func<T> delegateFunc)
    {
      this.delegateFunc = delegateFunc;
    }

    // ReSharper disable once RedundantAssignment
    public bool InvokeGetter(object _, ref T returnValue)
    {
      returnValue = delegateFunc();
      return true;
    }

    public bool InvokeSetter(object _, T newValue)
    {
      return true;
    }
  }
}
