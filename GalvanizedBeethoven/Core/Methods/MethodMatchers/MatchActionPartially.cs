using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  internal class MatchActionPartially : IMethodMatcher
  {
    private readonly (Type, string)[] localParameters;

    public MatchActionPartially(IEnumerable<(Type, string)> parameters)
    {
      localParameters = parameters.ToArray();
    }

    public bool IsMatch((Type, string)[] parameters, Type[] __, Type returnType) =>
      localParameters.All(parameters.Contains);
  }
}