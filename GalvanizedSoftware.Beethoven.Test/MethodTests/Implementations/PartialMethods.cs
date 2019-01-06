using System;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations
{
  public class PartialMethods
  {
    public bool WithParameters1(string text1)
    {
      return !string.IsNullOrEmpty(text1);
    }

    public bool WithParameters2(string text2)
    {
      return !string.IsNullOrEmpty(text2);
    }

    public bool WithParametersCount(int count)
    {
      return count > 0;
    }

    public int WithParameters(string text1, string text2, int count)
    {
      return (text1.Length + text2.Length) * count;
    }

    // ReSharper disable once RedundantAssignment
    public void WithParametersReturnValue(ref int returnValue)
    {
      returnValue = 5;
    }

    public object GetMain(ITestMethods testMethods, string text1, string text2)
    {
      if (string.IsNullOrEmpty(text1))
        throw new Exception();
      if (string.IsNullOrEmpty(text2))
        throw new Exception();
      return testMethods;
    }
  }
}
