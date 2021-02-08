namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IPropertyDefinition<T>
  {
    IPropertyInvoker<T> Create(object master);
  }
}