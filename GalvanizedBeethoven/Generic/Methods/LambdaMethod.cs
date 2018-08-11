﻿using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extentions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class LambdaMethod<T> : Method
  {
    private readonly Delegate lambdaDelegate;
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;

    public LambdaMethod(string name, T actionOrFunc) : base(name)
    {
      lambdaDelegate = actionOrFunc as Delegate;
      if (lambdaDelegate == null)
        throw new InvalidCastException("You must supplt an action, func or delegate");
      methodInfo = lambdaDelegate.Method;
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public override bool IsMatch(IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      return methodInfo.IsMatch(parameters, genericArguments, returnType);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      object returnValue = this.methodInfo.Invoke(lambdaDelegate.Target, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}