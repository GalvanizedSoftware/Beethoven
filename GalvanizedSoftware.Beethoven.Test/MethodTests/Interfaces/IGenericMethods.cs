namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  public interface IGenericMethods
  {
    T Simple<T>();
    void Parameter<T>(T value);
    void StructParameter<T>(T value) where T : struct;
  }
}