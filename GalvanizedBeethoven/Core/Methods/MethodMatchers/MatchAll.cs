using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchAll: IMethodMatcher
  {
    private readonly IEnumerable<IMethodMatcher> matchers;

    public MatchAll(IEnumerable<IMethodMatcher> matchers)
    {
      this.matchers = matchers;
    }

    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType) =>
      matchers.All(matcher =>
        matcher.IsMatch(parameters, genericArguments, returnType));
  }
}