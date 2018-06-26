using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extentions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MethodMapper : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;
    private readonly object instance;

    public MethodMapper(string name, Delegate methodDelegate) :
      base(name)
    {
      methodInfo = methodDelegate.Method;
      instance = methodDelegate.Target;
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public override bool IsMatch(IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      return methodInfo.IsMatch(parameters, genericArguments, returnType);
    }

    protected override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments)
    {
      object returnValue = methodInfo.Invoke(instance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}