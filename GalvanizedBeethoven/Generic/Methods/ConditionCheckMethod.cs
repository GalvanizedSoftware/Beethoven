using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ConditionCheckMethod : Method
  {
    private readonly Delegate lambdaDelegate;
    private readonly MethodInfo methodInfo;

    public static ConditionCheckMethod Create(string name, Func<bool> func) => 
      new ConditionCheckMethod(name, func);

    public static ConditionCheckMethod Create<T1>(string name, Func<T1, bool> func) => 
      new ConditionCheckMethod(name, func);

    public static ConditionCheckMethod Create<T1, T2>(string name, Func<T1, T2, bool> func) => 
      new ConditionCheckMethod(name, func);

    public ConditionCheckMethod(string name, Delegate lambdaDelegate) : 
      base(name, new MatchAllButLastParamerter(lambdaDelegate))
    {
      this.lambdaDelegate = lambdaDelegate;
      methodInfo = lambdaDelegate.Method;
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _) =>
      returnAction(methodInfo.Invoke(
        lambdaDelegate.Target, parameters.SkipLast().ToArray(), genericArguments));
  }
}