using System;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchNoParametersAndReturnType<TReturnType>: IMethodMatcher
  {
    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      return typeof(TReturnType) == returnType && !parameters.Any();
    }

  }
}