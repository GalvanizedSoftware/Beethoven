using System;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchFlowControl : IMethodMatcher
  {
    public bool IsMatch((Type, string)[] __, Type[] ___, Type returnType) => 
      returnType.IsByRef;
  }
}