using System;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchFlowControl : IMethodMatcher
  {
    public bool IsMatch(string methodName, (Type, string)[] parameters, Type[] genericArguments, Type returnType) => 
      returnType.IsByRef;
  }
}