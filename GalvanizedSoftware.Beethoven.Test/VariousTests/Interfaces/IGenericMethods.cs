namespace GalvanizedSoftware.Beethoven.Test.VariousTests.Interfaces
{
  public interface IGenericMethods
  {
    T Parameter<T>(T value);
    T TwoParameters1<T>(T value, short number);
    T TwoParameters2<T>(T value, int number);
  }
}