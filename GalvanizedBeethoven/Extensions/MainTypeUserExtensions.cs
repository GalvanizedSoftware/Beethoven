using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class MainTypeUserExtensions
  {
	  internal static void SetAll<T>(this IEnumerable<IMainTypeUser> mainTypeUsers) => 
		  mainTypeUsers.ForEach(mainTypeUser => mainTypeUser.Set(typeof(T)));
  }
}
