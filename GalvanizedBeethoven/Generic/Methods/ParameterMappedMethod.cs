using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ParameterMappedMethod : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;

    public ParameterMappedMethod(MethodInfo methodInfo, IParameter parameter) :
      this(methodInfo.Name, parameter, methodInfo)
    {
    }

    private ParameterMappedMethod(string mainName, IParameter parameter, MethodInfo methodInfo) :
      base(mainName, new MatchMethodInfoExact(methodInfo), parameter)
    {
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.HasReturnType();
    }

    public override void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo _)
    {
      object returnValue = methodInfo.Invoke(localInstance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}