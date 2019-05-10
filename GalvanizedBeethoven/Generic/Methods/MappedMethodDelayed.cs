﻿using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethodDelayed : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;

    public MappedMethodDelayed(string name, Type type) :
      this(name, type, name)
    {
    }

    public MappedMethodDelayed(MethodInfo methodInfo) :
      this(methodInfo.Name, methodInfo)
    {
    }

    public MappedMethodDelayed(string mainName, Type type, string targetName) :
      this(mainName, GetMethod(type, targetName))
    {
    }


    private MappedMethodDelayed(string mainName, MethodInfo methodInfo) :
      base(mainName, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.HasReturnType();
    }

    private static MethodInfo GetMethod(Type type, string targetName) => 
      type.FindSingleMethod(targetName);

    public object Instance { private get; set; }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      object returnValue = methodInfo.Invoke(Instance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}