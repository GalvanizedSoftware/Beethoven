using System;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  internal class OutAndRefImplementation
  {
    public int OutAndRef(out string text1, ref string text2, int count)
    {
      string tmp1 = text2;
      text1 = string.Join(" ",
        Enumerable.Range(0, count)
          .Select(i => tmp1));
      text2 = new string(text2.Reverse().ToArray());
      return text1.Length;
    }

    public int OutAndRef(string text1, string text2, int count)
    {
      throw new NotImplementedException();
    }

    public int OutAndRef(ref string text1, out string text2)
    {
      throw new NotImplementedException();
    }
  }
}