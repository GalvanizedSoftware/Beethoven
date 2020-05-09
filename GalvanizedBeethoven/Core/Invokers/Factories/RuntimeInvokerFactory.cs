using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Factories
{
  public class RuntimeInvokerFactory
  {
    private readonly object masterInvoker;

    public RuntimeInvokerFactory(string uniqueName)
    {
      masterInvoker = InvokerList.GetInvoker(uniqueName);
    }

    public T Create<T>() =>
      (masterInvoker as Func<T> ?? (() => default))();
  }
}
