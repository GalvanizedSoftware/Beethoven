using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  internal class MatchActionPartially : IMethodMatcher
  {
    private readonly (Type, string)[] localParameters;


    public MatchActionPartially(Delegate lambdaDelegate) :
      this(lambdaDelegate?.Method)
    {
    }

    public MatchActionPartially(MethodInfo methodInfo):
      this(methodInfo.GetParameterTypeAndNames())
    {
      localParameters = methodInfo.GetParameterTypeAndNames();
    }

    public MatchActionPartially(IEnumerable<(Type, string)> parameters)
    {
      localParameters = parameters.ToArray();
    }

    public bool IsMatch((Type, string)[] parameters, Type[] __, Type returnType) =>
      localParameters.All(parameters.Contains);
  }
}