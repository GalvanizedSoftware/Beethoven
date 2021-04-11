using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations
{
  class Logger
  {
    public List<string> Log { get; } = new List<string>();

    internal bool LogBefore(string text1, string text2, int count, ref int length)
    {
      Log.Add($"WithParameters called with \"{text1}\" \"{text2}\" {count}");
      return true;
    }

    internal bool LogAfter(string text1, string text2, int count, ref int length)
    {
      Log.Add($"WithParameters called with \"{text1}\" \"{text2}\" {count}, returning: {length}");
      return true;
    }
  }
}
