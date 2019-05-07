using System;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchFlowControl : IMethodMatcher
  {
    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      return returnType != typeof(bool) && parameters.LastOrDefault().Item1?.IsByRef == true;
    }
  }
}