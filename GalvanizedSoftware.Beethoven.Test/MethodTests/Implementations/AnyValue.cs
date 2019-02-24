namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations
{
  public class ValueCheck
  {
    public bool HasNoValue1(string text1)
    {
      return string.IsNullOrEmpty(text1);
    }

    public bool HasNoValue2(string text2)
    {
      return string.IsNullOrEmpty(text2);
    }
  }
}
