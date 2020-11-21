using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  internal class MatchMethodNoReturn: IMethodMatcher
  {
    private readonly MethodInfo methodInfo;
    private readonly (Type, string)[] localParameters;
    private readonly string mainParameterName;

    public MatchMethodNoReturn(Type type, string targetName, string mainParameterName)
    {
      this.mainParameterName = mainParameterName;
      methodInfo = type.FindSingleMethod(targetName);
      localParameters = methodInfo.GetParameterTypeAndNames();
    }

    public bool IsMatch((Type, string)[] parameters, Type[] __, Type returnType) =>
      (methodInfo.ReturnType != typeof(bool) || returnType.IsByRefence()) &&
      localParameters
        .Where(tuple => tuple.Item2 != mainParameterName)
        .ToArray().All(parameters.Contains);
  }
}