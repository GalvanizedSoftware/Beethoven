using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialValue<T> : IPropertyDefinition<T>
  {
    private readonly T value;
    private bool valueSet;

    public InitialValue(T value)
    {
      this.value = value;
    }

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      if (valueSet)
        return true;
      returnValue = value;
      return false;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      valueSet = true;
      return true;
    }
  }
}