namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces
{
  public interface ITestMethods
  {
    void Simple();
    int ReturnValue();
    int WithParameters(string text1, string text2);
    int WithParameters(string text1, string text2, int count);
    int OutAndRef(out string text1, ref string text2, int count);
    void Ref(ref int value);
    object GetMain(string text1, string text2);
    void NoReturnValue(string text1, string text2);
  }
}
