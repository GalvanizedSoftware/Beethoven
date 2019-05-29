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

    public ParameterMappedMethod(string name, IParameter parameter) :
      this(name, parameter, name)
    {
    }

    public ParameterMappedMethod(MethodInfo methodInfo, IParameter parameter) :
      this(methodInfo.Name, parameter, methodInfo)
    {
    }

    public ParameterMappedMethod(string mainName, IParameter parameter, string targetName) :
      this(mainName, parameter, GetMethod(parameter.Type, targetName))
    {
    }


    private ParameterMappedMethod(string mainName, IParameter parameter, MethodInfo methodInfo) :
      base(mainName, new MatchMethodInfoExact(methodInfo), parameter)
    {
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.HasReturnType();
    }

    private static MethodInfo GetMethod(Type type, string targetName) => 
      type.FindSingleMethod(targetName);

    public override void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo _)
    {
      object returnValue = methodInfo.Invoke(localInstance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}