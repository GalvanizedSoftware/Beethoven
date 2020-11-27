namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IPropertyInstance<T>
  {
    bool InvokeGetter(ref T returnValue);
    bool InvokeSetter(T newValue);
  }
}