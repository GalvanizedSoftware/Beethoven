using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
  public static class GeneratorHelper
  {
    internal static IEnumerable<string> AsCollection(string code)
    {
      if (!string.IsNullOrEmpty(code))
        yield return code;
    }
  }
}
