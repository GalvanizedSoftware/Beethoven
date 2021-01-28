using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class ParameterMappedInvoker : IInvoker
  {
    private readonly MethodInfo methodInfo;

    public ParameterMappedInvoker(MethodInfo methodInfo) :
      this(methodInfo?.Name, methodInfo)
    {
    }

    private ParameterMappedInvoker(string mainName, MethodInfo methodInfo)
    {
      this.methodInfo = methodInfo;
      methodInfo.HasReturnType();
    }

    public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _)
    {
      returnValue = methodInfo.Invoke(localInstance, parameters, genericArguments);
      return true;
    }
  }
}