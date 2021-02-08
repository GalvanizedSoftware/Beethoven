using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class SetterGetterInvoker<T> : IPropertyInvoker<T>
  {
    private T value;

    public bool InvokeGetter(ref T returnValue)
    {
      returnValue = value;
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      value = newValue;
      return true;
    }
  }
}