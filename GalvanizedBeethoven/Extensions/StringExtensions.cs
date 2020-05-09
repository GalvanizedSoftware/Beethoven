﻿using System;
using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class StringExtensions
  {
    internal const string OneIndent = "\t";

    internal static string Indent(this string text, int indentCount) =>
      string.Join("", Enumerable.Repeat(OneIndent, indentCount)) + text;

    internal static string Format(this string text, int indentCount) =>
      Format(text.Split(new[] { NewLine }, StringSplitOptions.RemoveEmptyEntries), indentCount);

    internal static IEnumerable<string> SplitToLines(this string text) =>
      text.Split(new[] { NewLine }, StringSplitOptions.None);

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
