using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class InitialValueInvoker<T> : IPropertyInvoker<T>
  {
    private readonly T value;
    private bool valueSet;

    public InitialValueInvoker(T value)
    {
      this.value = value;
    }

    public bool InvokeGetter(ref T returnValue)
    {
      if (valueSet)
        return true;
      returnValue = value;
      return false;
    }

    public bool InvokeSetter(T newValue)
    {
      valueSet = true;
      return true;
    }
  }
}