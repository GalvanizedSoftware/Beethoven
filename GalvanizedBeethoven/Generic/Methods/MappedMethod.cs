using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Reflection;
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
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public MappedMethod(string mainName, object instance) :
      this(mainName, instance, mainName)
    {
    }

    public MappedMethod(string mainName, object instance, string targetName) :
      base(mainName)
    {
      Instance = instance;
      methodInfo = instance
        .GetType()
        .FindSingleMethod(targetName);
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public object Instance { private get; set; }

    public override bool IsMatch(IEnumerable<(Type, string)> parameters, Type[] genericArguments, Type returnType)
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