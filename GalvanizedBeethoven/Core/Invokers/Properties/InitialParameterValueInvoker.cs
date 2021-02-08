using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class InitialParameterValueInvoker<T> : IPropertyInvoker<T>
  {
    private bool valueSet;

    public bool InvokeGetter(ref T returnValue) => valueSet;

    public bool InvokeSetter(T newValue) => valueSet = true;
  }
}