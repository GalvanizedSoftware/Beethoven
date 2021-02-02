using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using System.Collections.Generic;
using System.Linq;
using static GalvanizedSoftware.Beethoven.Core.Enumerable;

namespace GalvanizedSoftware.Beethoven.Extensions
{
	internal static class CodeTypeExtensions
	{
		internal static IEnumerable<(CodeType, string)?> EnumerateCode(this CodeType codeType, params object[] codeLines) =>
			codeLines
				.SelectMany(GetStrings)
				.Select(line => ((CodeType, string)?)(codeType, line));

		private static IEnumerable<string> GetStrings(object item) =>
			item switch
			{
				string text => Single(text),
				IEnumerable<string> texts => texts,
				_ => Enumerable.Empty<string>()
			};
	}
}
