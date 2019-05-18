using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchLambdaPartiallyNoReturn : IMethodMatcher
  {
    private readonly (Type, string)[] localParameters;
    private readonly bool isFlowControlled;

    public MatchLambdaPartiallyNoReturn(params (Type, string)[] parameters) :
      this(null, parameters)
    {
    }

    public MatchLambdaPartiallyNoReturn(Type returnType, params (Type, string)[] parameters)
    {
      localParameters = parameters;
      isFlowControlled = returnType == typeof(bool);
    }

    public MatchLambdaPartiallyNoReturn(Delegate lambdaDelegate) :
      this(lambdaDelegate.Method)
    {
    }

    public MatchLambdaPartiallyNoReturn(MethodInfo methodInfo)
    {
      localParameters = methodInfo.GetParameterTypeAndNames();
      isFlowControlled = methodInfo.ReturnType == typeof(bool);
    }

    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      return isFlowControlled == returnType.IsByRef &&
             localParameters.All(parameters.Contains);
    }
  }
}