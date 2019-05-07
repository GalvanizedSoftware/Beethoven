using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchAllButLastParamerter: IMethodMatcher
  {
    private readonly MethodInfo methodInfo;

    public MatchAllButLastParamerter(Delegate lambdaDelegate)
    {
      methodInfo = lambdaDelegate.Method;
    }

    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType) => 
      methodInfo.IsMatch(parameters.SkipLast(), genericArguments, returnType);
  }
}