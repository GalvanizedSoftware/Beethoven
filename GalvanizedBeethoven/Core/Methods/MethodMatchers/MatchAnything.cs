using System;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  internal class MatchAnything : IMethodMatcher
  {
    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType) =>
      true;
  }
}