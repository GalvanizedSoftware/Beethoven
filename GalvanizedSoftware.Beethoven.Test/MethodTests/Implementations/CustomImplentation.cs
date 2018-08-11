// ReSharper disable UnusedMember.Global

namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations
{
  class CustomImplentation
  {

    internal int GetLength(string text1, string text2, int count)
    {
      return text1.Length + text2.Length + count;
    }

    // ReSharper disable once RedundantAssignment
    internal bool GetLength2(ref int length, string text1, string text2, int count)
    {
      length = text1.Length + text2.Length + count;
      return true;
    }
  }
}
