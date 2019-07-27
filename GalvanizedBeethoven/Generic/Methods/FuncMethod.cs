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
    private readonly (Type, string)[] localParameters;

    public static FuncMethod Create<TReturn>(string mainName, Func<TReturn> func, IParameter parameter = null) =>
      new FuncMethod(mainName, func, parameter);

    public static FuncMethod Create<T1, TReturn>(string mainName, Func<T1, TReturn> func, IParameter parameter = null) =>
      new FuncMethod(mainName, func, parameter);

    public static FuncMethod Create<T1, T2, TReturn>(string mainName, Func<T1, T2, TReturn> func, IParameter parameter = null) =>
      new FuncMethod(mainName, func, parameter);

    public FuncMethod(string mainName, Delegate func, IParameter parameter) :
      this(mainName, func?.Target, func?.Method, parameter)
    {
    }

    private FuncMethod(string mainName, object target, MethodInfo methodInfo, IParameter parameter) :
      base(mainName, new MatchFuncPartially(methodInfo), parameter)
    {
      this.target = target ?? throw new NullReferenceException();
      this.methodInfo = methodInfo ?? throw new NullReferenceException();
      localParameters = methodInfo.GetParameterTypeAndNames();
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(masterMethodInfo?.ReturnType)
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