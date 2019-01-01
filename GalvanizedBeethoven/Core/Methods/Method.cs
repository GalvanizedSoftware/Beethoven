﻿using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class Method : IInterceptor
  {
    protected Method(string name)
    {
      Name = name;
    }

    public string Name { get; }

    public abstract bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType);

    internal abstract void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo);

    public void Intercept(IInvocation invocation)
    {
      Invoke(value => invocation.ReturnValue = value, invocation.Arguments, invocation.GenericArguments, invocation.Method);
    }
  }
}