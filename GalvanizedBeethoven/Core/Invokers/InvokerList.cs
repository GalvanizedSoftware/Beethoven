using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  public static class InvokerList
  {
    private static readonly Dictionary<string, object> invokers = new Dictionary<string, object>();

    internal static void SetInvoker(string uniqueName, object instance)
    {
      if (!invokers.ContainsKey(uniqueName))
        invokers.Add(uniqueName, instance);
    }

    internal static object GetInvoker(string uniqueName)
    {
      invokers.TryGetValue(uniqueName, out object value);
      return value;
    }
  }
}