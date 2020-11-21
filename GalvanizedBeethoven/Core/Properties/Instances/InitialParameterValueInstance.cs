using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
{
  public class InitialParameterValueInstance<T> : IPropertyInstance<T>
  {
    private bool valueSet;

    public bool InvokeGetter(ref T returnValue) => valueSet;

    public bool InvokeSetter(T newValue) => valueSet = true;
  }
}