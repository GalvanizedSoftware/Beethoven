using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchLambdaPartiallyNoReturn : IMethodMatcher
  {
    private readonly MethodInfo methodInfo;
    private readonly (Type, string)[] localParameters;

    public MatchLambdaPartiallyNoReturn(Delegate lambdaDelegate)
    {
      methodInfo = lambdaDelegate.Method;
      localParameters = methodInfo.GetParameterTypeAndNames();
    }

    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      if (methodInfo.ReturnType == typeof(bool) && !returnType.IsByRef)
        return false;
      return localParameters.All(parameters.Contains);
    }
  }
}