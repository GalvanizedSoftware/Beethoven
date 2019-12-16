using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  internal class MatchFuncPartially : IMethodMatcher
  {
    private readonly Type funcReturnType;
    private readonly (Type, string)[] localParameters;

    public MatchFuncPartially(MethodInfo methodInfo):
      this(methodInfo.ReturnType,methodInfo.GetParameterTypeAndNames())
    {
    }

    public MatchFuncPartially(Type funcReturnType, IEnumerable<(Type, string)> parameters)
    {
      this.funcReturnType = funcReturnType;
      localParameters = parameters.ToArray();
    }

    public bool IsMatch((Type, string)[] parameters, Type[] __, Type returnType) =>
      !returnType.IsByRefence() &&
      returnType.IsMatchReturnTypeIgnoreGeneric(funcReturnType) &&
      localParameters.All(parameters.Contains);
  }
}