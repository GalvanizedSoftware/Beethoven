namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IPropertyDefinition<T>
  {
    IPropertyInstance<T> Create(object master);
  }
}