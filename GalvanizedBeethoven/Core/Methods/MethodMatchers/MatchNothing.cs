using System;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchNothing : IMethodMatcher
  {
    public bool IsMatch((Type, string)[] __, Type[] ___, Type ____) =>
      false;
  }
}