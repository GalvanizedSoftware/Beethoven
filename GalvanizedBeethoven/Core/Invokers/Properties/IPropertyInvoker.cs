namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  public interface IPropertyInvoker<T>
  {
    IPropertyInvokerInstance<T> CreateInstance(object master);
  }
}