using GalvanizedSoftware.Beethoven.Extentions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public class MethodsWithInstance : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;

    public MethodsWithInstance(MethodInfo methodInfo) :
      base(methodInfo.Name)
    {
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public object Instance { private get; set; }

    public override bool IsMatch(IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      return methodInfo.IsMatch(parameters, genericArguments, returnType);
    }

    protected override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments)
    {
      object returnValue = methodInfo.Invoke(Instance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}