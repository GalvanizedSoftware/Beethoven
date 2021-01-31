using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class MethodInvokerInstance
  {
    private readonly object master;
    private readonly MethodInfo methodInfo;
    private readonly IInvoker[] methodInvokers;

    public MethodInvokerInstance(object master, MethodInfo methodInfo, IEnumerable<IInvoker> methodInvokers)
    {
      this.master = master;
      this.methodInfo = methodInfo;
      this.methodInvokers = methodInvokers.ToArray();
    }

    // ReSharper disable once UnusedMember.Global
    public object Invoke(Type[] genericTypes, object[] parameters)
    {
      MethodInfo realMethodInfo = methodInfo.IsGenericMethod ?
        methodInfo.MakeGenericMethod(genericTypes) :
        methodInfo;
      if (methodInvokers?.Any() != true)
        throw new MissingMethodException();
      object returnValue = realMethodInfo.ReturnType.GetDefaultValue();
      foreach (IInvoker invoker in methodInvokers)
        if (!invoker.Invoke(master, ref returnValue, parameters, genericTypes, realMethodInfo))
          break;
      return returnValue;
    }
  }
}