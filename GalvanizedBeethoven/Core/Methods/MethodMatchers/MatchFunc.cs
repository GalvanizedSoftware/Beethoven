using System;
using System.Linq;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchFunc : IMethodMatcher
  {
    private readonly Type funcReturnType;
    private readonly Type[] parameterTypes;

    public MatchFunc(params Type[] types)
    {
      funcReturnType = types.Last();
      parameterTypes = types.SkipLast().ToArray();
    }

    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType) =>
      !returnType.IsByRef &&
      returnType == funcReturnType &&
      parameters.Select(tuple => tuple.Item1).SequenceEqual(parameterTypes);
  }
}