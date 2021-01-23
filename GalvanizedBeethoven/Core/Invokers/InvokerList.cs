using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;

namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  public static class InvokerList
  {
    private static readonly Dictionary<string, Func<object>> invokers = new();

    internal static void SetFactory(string uniqueName, Func<object> factory)
    {
      if (!invokers.ContainsKey(uniqueName))
        invokers.Add(uniqueName, factory);
    }

    internal static object CreateInvoker(string uniqueName) => 
      invokers.TryGetValue(uniqueName, out Func<object> value) ? 
        value?.Invoke() : 
        new RealMethodInvoker(null, null);
  }
}