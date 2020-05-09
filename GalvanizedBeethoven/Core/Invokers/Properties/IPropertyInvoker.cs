namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  public interface IPropertyInvoker<T>
  {
    T InvokeGet(object master);
    void InvokeSet(object master, T newValue);
  }
}