namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IPropertyDefinition<T>
  {
    IPropertyInstance<T> CreateInstance(object master);
  }
}