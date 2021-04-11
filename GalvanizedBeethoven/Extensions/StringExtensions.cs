using System.Collections.Generic;
using System.Linq;
using static System.Environment;
using static System.StringSplitOptions;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class StringExtensions
  {
	  private const string OneIndent = "\t";

    internal static string Format(this string text, int indentCount) =>
      Format(text.Split(new[] { NewLine }, RemoveEmptyEntries), indentCount);

    internal static string Format(this IEnumerable<string> lines, int indentCount)
    {
      string[] lineArray = lines?.ToArray();
      if (lineArray?.Any() != true)
        return "";
      string indent = indentCount switch
      {
        0 => "",
        1 => OneIndent,
        _ => string.Join("", Enumerable.Repeat(OneIndent, indentCount).ToArray())
      };
      return indent + string.Join(NewLine + indent, lineArray);
    }
  }
}
