namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces
{
  public interface IGenericClass<T> where T : class
  {
    T Simple();
    void Parameter(T value);
    void StructParameter(T value);
  }
}