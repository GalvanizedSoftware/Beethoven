using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
	internal static class Enumerable
	{
		internal static IEnumerable<T> Single<T>(T value)
		{
			yield return value;
		}
	}
}
