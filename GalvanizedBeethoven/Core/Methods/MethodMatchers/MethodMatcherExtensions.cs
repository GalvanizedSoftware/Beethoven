using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public static class MethodMatcherExtensions
  {
    public static bool IsMatchToFlowControlled(this IMethodMatcher methodMattcher, (Type, string)[] parameterTypeAndNames, Type[] genericArguments, Type returnType) =>
      methodMattcher.IsMatch(
        parameterTypeAndNames.AppendReturnValue(returnType).ToArray(),
        genericArguments,
        typeof(bool).MakeByRefType());

    public static bool IsNonGenericMatch(this IMethodMatcher methodMattcher, MethodInfo methodInfo) =>
      methodMattcher.IsMatch(
        methodInfo.GetParameterTypeAndNames(),
        new Type[0],
        methodInfo.ReturnType);
  }
}
