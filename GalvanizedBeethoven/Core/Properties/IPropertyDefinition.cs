namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public interface IPropertyDefinition<T>
  {
    bool InvokeGetter(InstanceMap instanceMap,ref T returnValue);
    bool InvokeSetter(InstanceMap instanceMap, T newValue);
  }
}