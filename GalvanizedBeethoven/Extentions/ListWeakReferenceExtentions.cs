using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Extentions
{
  internal static class ListWeakReferenceExtentions
  {
    public static void RemoveDeadLinks(this ICollection<WeakReference> weakReferences)
    {
      foreach (WeakReference item in weakReferences.Where(weakReference => !weakReference.IsAlive).ToArray())
        weakReferences.Remove(item);
    }

    public static bool Exists(this ICollection<WeakReference> weakReferences, object obj)
    {
      return weakReferences.Any(weakReference => weakReference.Target == obj);
    }
  }
}
