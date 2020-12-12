// ReSharper disable UnusedMember.Global

using System.Linq;
// ReSharper disable UnusedParameter.Global

namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations
{
  class CustomImplentation
  {
    internal int GetLength(string text1, string text2, int count)
    {
      return text1.Length + text2.Length + count;
    }

    internal bool GetLength2(ref int length, string text1, string text2, int count)
    {
      length = text1.Length + text2.Length + count;
      return true;
    }

    internal int WithParameters(string text1, string text2)
    {
      return 17;
    }

    internal int WithParameters(string text1, string text2, int count)
    {
      return 18;
    }

    public bool OutAndRef(out string text1, ref string text2, int count, ref int returnValue)
    { 
      string tmp1 = text2;
      text1 = string.Join(" ",
        Enumerable.Range(0, count)
          .Select(i => tmp1));
      text2 = new string(text2.Reverse().ToArray());
      returnValue = text1.Length;
      return true;
    }

    // Note to get value from previous call, text1 has to be ref not out (luckily both are valid)
    public bool OutAndRef1(ref string text1, ref string text2, int count, ref int returnValue)
    {
      returnValue += 1;
      return false;
    }
  }
}
