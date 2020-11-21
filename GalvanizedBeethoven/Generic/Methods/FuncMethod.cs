using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FuncMethod : MethodDefinition
  {
    private readonly MethodInfo methodInfo;
    private readonly object target;
    private readonly (Type, string)[] localParameters;

    public static FuncMethod Create<TReturn>(string mainName, Func<TReturn> func) =>
      new FuncMethod(mainName, func);

    public static FuncMethod Create<T1, TReturn>(string mainName, Func<T1, TReturn> func) =>
      new FuncMethod(mainName, func);

    public static FuncMethod Create<T1, T2, TReturn>(string mainName, Func<T1, T2, TReturn> func) =>
      new FuncMethod(mainName, func);

    public FuncMethod(string mainName, Delegate func) :
      this(mainName, func?.Target, func?.Method)
    {
    }

    private FuncMethod(string mainName, object target, MethodInfo methodInfo) :
      base(mainName, new MatchFuncPartially(methodInfo))
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