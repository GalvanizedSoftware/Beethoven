using System;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
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

    public IPropertyInvoker<T> Create(object master) =>
      new DelegatedGetterInvoker<T>(delegateFunc);
  }
}
