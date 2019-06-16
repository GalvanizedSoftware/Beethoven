using System;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public interface IMethodMatcher
  {
    bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType);
  }
}
