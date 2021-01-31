using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
	internal static class Enumerable
	{
		internal static IEnumerable<T> SingleEnumerable<T>(T value)
		{
			yield return value;
		}
	}
}
