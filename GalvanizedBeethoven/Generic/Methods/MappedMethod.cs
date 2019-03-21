﻿using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using Castle.DynamicProxy.Internal;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethod : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;

    public MappedMethod(MethodInfo methodInfo) :
      base(methodInfo.Name)
    {
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.HasReturnType();
    }

    public MappedMethod(string name, object instance) :
      this(name, instance, name)
    {
    }

    public MappedMethod(string mainName, object instance, string targetName) :
      base(mainName)
    {
      Instance = instance;
      methodInfo = instance
        .GetType()
        .FindSingleMethod(targetName);
      hasReturnType = methodInfo.HasReturnType();
    }

    public MappedMethod(object instance, MethodInfo methodInfo) :
      base(methodInfo.Name)
    {
      Instance = instance;
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.HasReturnType();
    }

    public object Instance { private get; set; }

    public override bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      return methodInfo.IsMatch(parameters, genericArguments, returnType);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      object returnValue = methodInfo.Invoke(Instance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}