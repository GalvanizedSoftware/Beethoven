using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class SetterGetter<T> : IPropertyDefinition<T>
  {
    private T value;

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      returnValue = value;
      return true;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      value = newValue;
      return true;
    }
  }
}