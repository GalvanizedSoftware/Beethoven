using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialParameterValue<T> : IPropertyDefinition<T>
  {
    private bool valueSet;

    public bool InvokeGetter(object master, ref T returnValue) => valueSet;

    public bool InvokeSetter(object master, T newValue) => valueSet = true;
  }
}