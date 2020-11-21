using System;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  internal class MatchNothing : IMethodMatcher
  {
    public bool IsMatch((Type, string)[] __, Type[] ___, Type ____) =>
      false;
  }
}