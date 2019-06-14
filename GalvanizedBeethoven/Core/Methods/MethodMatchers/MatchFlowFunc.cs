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

    public bool IsMatch(string methodName, (Type, string)[] parameters, Type[] genericArguments, Type returnType) => 
      returnType.IsByRef && 
      parameters.SkipLast().Select(tuple => tuple.Item1).SequenceEqual(parameterTypes);
  }
}