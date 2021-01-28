using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class FuncInvoker : IInvoker
  {
    private readonly MethodInfo methodInfo;
    private readonly object target;
    private readonly (Type, string)[] localParameters;

    public FuncInvoker(Delegate func)
    {
      target = func?.Target ?? throw new NullReferenceException();
      methodInfo = func.Method;
      localParameters = methodInfo.GetParameterTypeAndNames();
    }

    public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
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
      return true;
    }
  }
}