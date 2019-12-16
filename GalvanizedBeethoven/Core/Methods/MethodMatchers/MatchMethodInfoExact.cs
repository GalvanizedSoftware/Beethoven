using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchMethodInfoExact: IMethodMatcher
  {
    private readonly MethodInfo methodInfo;

    public MatchMethodInfoExact(MethodInfo methodInfo)
    {
      this.methodInfo = methodInfo;
    }

    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType) => 
      methodInfo.GetActualMethod(genericArguments).IsMatch(parameters, genericArguments, returnType);
  }
}
