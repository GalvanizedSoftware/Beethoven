using System;
using GalvanizedSoftware.Beethoven.Implementations.Properties;
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

    public IPropertyInstance<T> CreateInstance(object master) =>
      new DelegatedSetterInstance<T>(delegateAction);
  }
}
