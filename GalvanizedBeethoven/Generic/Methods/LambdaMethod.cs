﻿using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class LambdaMethod<T> : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;
    private readonly object target;

    static LambdaMethod()
    {
      typeof(T).CheckForDelegateType();
    }

    public LambdaMethod(string name, T actionOrFunc) :
      this(name, actionOrFunc as Delegate)
    {
    }

    private LambdaMethod(string name, Delegate lambdaDelegate) :
      base(name, new MatchMethodInfoExact(lambdaDelegate.Method))
    {
      methodInfo = lambdaDelegate.Method;
      target = lambdaDelegate.Target;
      hasReturnType = methodInfo.HasReturnType();
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      returnValue = methodInfo.Invoke(target, parameters, genericArguments);
  }
}