namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IPropertyDefinition<T>
  {
    bool InvokeGetter(object master, ref T returnValue);
    bool InvokeSetter(object master, T newValue);
  }
}