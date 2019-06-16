using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public static class MethodMatcherExtensions
  {
    public static bool IsMatchToFlowControlled(this IMethodMatcher methodMatcher, (Type, string)[] parameterTypeAndNames,
      Type[] genericArguments, Type returnType) =>
      methodMatcher.IsMatch(parameterTypeAndNames.AppendReturnValue(returnType).ToArray(),
        genericArguments,
        typeof(bool).MakeByRefType());

    public static bool IsNonGenericMatch(this IMethodMatcher methodMatcher, MethodInfo methodInfo) =>
      methodMatcher.IsMatch(methodInfo.GetParameterTypeAndNames(),
        new Type[0],
        methodInfo.ReturnType);

    public static bool IsMatchIgnoreGeneric(this IMethodMatcher methodMatcher, MethodInfo methodInfo) =>
      methodMatcher.IsMatchEitherType(methodInfo.GetParameterTypeAndNames(), 
        null, methodInfo.ReturnType.RemoveGeneric());

    public static bool IsMatchEitherType(this IMethodMatcher methodMatcher,
      (Type, string)[] parameters, Type[] genericArguments, Type returnType) =>
      !returnType.IsByRef &&
      (methodMatcher.IsMatch(parameters, genericArguments, returnType) ||
       methodMatcher.IsMatchToFlowControlled(parameters, genericArguments, returnType));
  }
}
