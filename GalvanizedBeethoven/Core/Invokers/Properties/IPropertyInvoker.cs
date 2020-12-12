namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public interface IPropertyInvoker<T>
  {
    IPropertyInvokerInstance<T> CreateInstance(object master);
  }
}