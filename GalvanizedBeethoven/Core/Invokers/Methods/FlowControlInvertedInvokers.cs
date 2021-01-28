using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class FlowControlInvertedInvokers : IInvoker
  {
    private readonly IInvoker[] invokers;

    public FlowControlInvertedInvokers(IEnumerable<IInvoker> invokers)
    {
      this.invokers = invokers.ToArray();
    }

    public bool Invoke(object localInstance, ref object returnValue,
      object[] parameters, Type[] genericArguments, MethodInfo masterMethodInfo)
    {
      object flowReturnValue = false;
      foreach (IInvoker invoker in invokers)
        if (!invoker.Invoke(localInstance, ref flowReturnValue, parameters, genericArguments, masterMethodInfo))
          break;
      return !(bool)flowReturnValue;
    }
  }
}