namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations
{
  public class ValueCheck
  {
    public bool HasValue1(string text1)
    {
      return !string.IsNullOrEmpty(text1);
    }

    public bool HasValue2(string text2)
    {
      return !string.IsNullOrEmpty(text2);
    }
  }
}
