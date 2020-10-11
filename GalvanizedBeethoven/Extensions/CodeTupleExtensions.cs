using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class CodeTupleExtensions
  {
    internal static (CodeType, string) Format(this (CodeType, string) tuple, int indentCount) =>
      (tuple.Item1, tuple.Item2.Format(indentCount));

    internal static string Format(this IEnumerable<(CodeType, string)> lines, int indentCount) =>
      lines
        .Select(item => item.Item2)
        .Format(indentCount);

    internal static IEnumerable<(CodeType, string)> Filter(this IEnumerable<(CodeType, string)> lines, CodeType filter) =>
      lines.Where(item => item.Item1 == filter);

    internal static IEnumerable<string> ToCode(this IEnumerable<(CodeType, string)> lines) =>
      lines.Select(item => item.Item2);
  }
}
