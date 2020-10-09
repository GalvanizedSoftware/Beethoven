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

    public T Create<T>() => GetFunc<T>()();

    private Func<T> GetFunc<T>() => 
      masterInvoker is Func<T> func ? func : 
      masterInvoker is Func<object> funcObject ? (Func<T>)(() => (T)funcObject()) :
        (() => default);
  }
}
