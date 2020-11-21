using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Implementations.Properties
{
  public class InitialParameterValueInstance<T> : IPropertyInstance<T>
  {
    private bool valueSet;

    public bool InvokeGetter(ref T returnValue) => valueSet;

    public bool InvokeSetter(T newValue) => valueSet = true;
  }
}