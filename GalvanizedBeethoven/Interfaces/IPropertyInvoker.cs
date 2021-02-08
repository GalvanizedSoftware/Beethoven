namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IPropertyInvoker<T>
  {
    bool InvokeGetter(ref T returnValue);
    bool InvokeSetter(T newValue);
  }
}