using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class MainTypeUserExtensions
  {
    internal static void SetAll(this IEnumerable<IMainTypeUser> mainTypeUsers, Type type) => 
      mainTypeUsers.ForEach(mainTypeUser => mainTypeUser.Set(type));
  }
}
