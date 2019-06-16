using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FuncMethod : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly object target;
    private readonly bool hasReturnType;
    private readonly (Type, string)[] localParameters;

    public FuncMethod(string mainName, Delegate lambdaDelegate, IParameter parameter) :
      this(mainName, lambdaDelegate.Target, lambdaDelegate.Method, parameter)
    {
    }

    private FuncMethod(string mainName, object target, MethodInfo lambdaMethodInfo, IParameter parameter) :
      base(mainName, new MatchActionPartially(lambdaMethodInfo), parameter)
    {
      methodInfo = lambdaMethodInfo;
      localParameters = lambdaMethodInfo.GetParameterTypeAndNames();
      hasReturnType = lambdaMethodInfo.HasReturnType();
      this.target = target;
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(masterMethodInfo.ReturnType)
        .ToArray();
      int[] indexes = localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .ToArray();
      object[] localParameterValues = indexes
        .Select(index => parameters[index])
        .ToArray();
      returnValue = methodInfo.Invoke(target, localParameterValues, genericArguments);
    }
  }
}