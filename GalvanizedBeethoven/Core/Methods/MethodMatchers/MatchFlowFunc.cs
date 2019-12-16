using System;
using System.Linq;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchFlowFunc : IMethodMatcher
  {
    private readonly Type[] parameterTypes;

    public MatchFlowFunc(params Type[] types)
    {
      parameterTypes = types;
    }

    public bool IsMatch((Type, string)[] parameters, Type[] __, Type returnType) =>
      returnType.IsByRefence() &&
      parameters.SkipLast().Select(tuple => tuple.Item1).SequenceEqual(parameterTypes);
  }
}