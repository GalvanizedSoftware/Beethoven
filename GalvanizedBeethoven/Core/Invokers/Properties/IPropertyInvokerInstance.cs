namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public interface IPropertyInvokerInstance<T>
  {
    T InvokeGetter();
    void InvokeSetter(T newValue);
  }
}