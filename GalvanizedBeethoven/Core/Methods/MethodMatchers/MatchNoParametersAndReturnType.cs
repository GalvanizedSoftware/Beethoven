using System;
using System.Linq;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchNoParametersAndReturnType<TReturnType>: IMethodMatcher
  {
    public bool IsMatch(string methodName, (Type, string)[] parameters, Type[] genericArguments, Type returnType) =>
      returnType.IsMatchReturnTypeIgnoreGeneric<TReturnType>() && !parameters.Any();
  }
}