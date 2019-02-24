namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations
{
  public class WithParametersImplementation
  {
    public int WithParameters(string text1, string text2, int count)
    {
      return (text1.Length + text2.Length) * count;
    }
  }
}
