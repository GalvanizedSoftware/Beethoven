﻿using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FuncMethod<TReturnType> : Method
  {
    private readonly Func<TReturnType> func;

    public FuncMethod(string name, Func<TReturnType> func) : base(name)
    {
      this.func = func;
    }

    public override bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      return typeof(TReturnType) == returnType && !parameters.Any();
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      returnAction(func());
    }
  }
}