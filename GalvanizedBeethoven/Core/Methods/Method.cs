using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class Method
  {
    protected Method(string name)
    {
      Name = name;
      MethodMatcher = null;
    }

    protected Method(string name, IMethodMatcher methodMatcher)
    {
      Name = name;
      MethodMatcher = methodMatcher;
    }

    public string Name { get; }
    public IMethodMatcher MethodMatcher { get; }

    public abstract void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo);
  }
}