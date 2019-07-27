using System;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchFlowControl : IMethodMatcher
  {
    public bool IsMatch((Type, string)[] __, Type[] ___, Type returnType) => 
      returnType.IsByRefence();
  }
}