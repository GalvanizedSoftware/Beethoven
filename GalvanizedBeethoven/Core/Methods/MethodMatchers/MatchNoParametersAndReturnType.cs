using System;
using System.Linq;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchNoParametersAndReturnType<TReturnType>: IMethodMatcher
  {
    public bool IsMatch((Type, string)[] parameters, Type[] __, Type returnType) =>
      returnType.IsMatchReturnTypeIgnoreGeneric<TReturnType>() && !parameters.Any();
  }
}