namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public interface IPropertyDefinition<T>
  {
    bool InvokeGetter(ref T returnValue);
    bool InvokeSetter(T newValue);
  }
}