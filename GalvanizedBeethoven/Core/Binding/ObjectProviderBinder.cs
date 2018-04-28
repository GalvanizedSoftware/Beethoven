using System;

namespace GalvanizedSoftware.Beethoven.Core.Binding
{
  internal class ObjectProviderBinder<T>
  {
    private readonly IObjectProvider objectProvider;

    public ObjectProviderBinder(IObjectProvider objectProvider)
    {
      this.objectProvider = objectProvider;
    }

    public void Bind(Action<T> bindingAction)
    {
      foreach (T binding in objectProvider.Get<T>())
        bindingAction(binding);
    }
  }
}