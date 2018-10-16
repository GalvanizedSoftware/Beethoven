using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ConditionCheckMethod : Method
  {
    private readonly Delegate lambdaDelegate;
    private readonly MethodInfo methodInfo;

    public static ConditionCheckMethod Create(string name, Func<bool> func)
    {
      return new ConditionCheckMethod(name, func);
    }

    public static ConditionCheckMethod Create<T1>(string name, Func<T1, bool> func)
    {
      return new ConditionCheckMethod(name, func);
    }

    public static ConditionCheckMethod Create<T1, T2>(string name, Func<T1, T2, bool> func)
    {
      return new ConditionCheckMethod(name, func);
    }

    public ConditionCheckMethod(string name, Delegate lambdaDelegate) : base(name)
    {
      this.lambdaDelegate = lambdaDelegate;
      if (lambdaDelegate == null)
        throw new InvalidCastException("You must supply an action, func or delegate");
      methodInfo = lambdaDelegate.Method;
    }

    public override bool IsMatch(IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      return methodInfo.IsMatch(parameters.SkipLast(), genericArguments, returnType);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      object returnValue = methodInfo.Invoke(lambdaDelegate.Target, parameters.SkipLast().ToArray(), genericArguments);
      returnAction(returnValue);
    }
  }
}