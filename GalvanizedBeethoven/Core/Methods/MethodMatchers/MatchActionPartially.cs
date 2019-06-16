using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchActionPartially : IMethodMatcher
  {
    private readonly (Type, string)[] localParameters;
    private readonly bool isFlowControlled;

    public MatchActionPartially(IEnumerable<(Type, string)> parameters) :
      this(null, parameters)
    {
    }

    public MatchActionPartially(Type returnType, IEnumerable<(Type, string)> parameters)
    {
      localParameters = parameters.ToArray();
      isFlowControlled = returnType == typeof(bool);
    }

    public MatchActionPartially(Delegate lambdaDelegate) :
      this(lambdaDelegate.Method)
    {
    }

    public MatchActionPartially(MethodInfo methodInfo)
    {
      localParameters = methodInfo.GetParameterTypeAndNames();
      isFlowControlled = methodInfo.ReturnType == typeof(bool);
    }

    public bool IsMatch((Type, string)[] parameters, Type[] __, Type returnType) =>
      isFlowControlled == returnType.IsByRef &&
      localParameters.All(parameters.Contains);
  }
}