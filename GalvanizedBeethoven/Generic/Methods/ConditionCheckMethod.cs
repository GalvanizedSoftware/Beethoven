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
      new ConditionCheckMethod(name, func, typeof(T1));

    public static ConditionCheckMethod Create<T1, T2>(string name, Func<T1, T2, bool> func) => 
      new ConditionCheckMethod(name, func, typeof(T1), typeof(T2));

   public ConditionCheckMethod(string name, Delegate lambdaDelegate, params Type[] types) : 
      base(name, new MatchFlowFunc(types))
    {
      this.lambdaDelegate = lambdaDelegate;
      methodInfo = lambdaDelegate.Method;
    }

    public override void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      returnAction(methodInfo.Invoke(
        lambdaDelegate.Target, parameters.SkipLast().ToArray(), genericArguments));
  }
}