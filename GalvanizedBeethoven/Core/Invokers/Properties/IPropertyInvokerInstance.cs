namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  public interface IPropertyInvokerInstance<T>
  {
    T InvokeGetter();
    void InvokeSetter(T newValue);
  }
}