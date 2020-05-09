using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ParameterMappedMethod : MethodDefinition
  {
    private readonly MethodInfo methodInfo;

    public ParameterMappedMethod(MethodInfo methodInfo) :
      this(methodInfo?.Name, methodInfo)
    {
    }

    private ParameterMappedMethod(string mainName, MethodInfo methodInfo) :
      base(mainName, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo;
      methodInfo.HasReturnType();
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      returnValue = methodInfo.Invoke(localInstance, parameters, genericArguments);
  }
}