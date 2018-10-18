using GalvanizedSoftware.Beethoven.Generic.ValueLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  // ReSharper disable once InconsistentNaming
  public static class IValueLookupExtensions
  {
    private static readonly MethodInfo lookupMethod;

    static IValueLookupExtensions()
    {
      lookupMethod = typeof(IValueLookup).GetMethod(nameof(IValueLookup.Lookup));
    }

    public static IEnumerable<object> Lookup(this IValueLookup lookup, Type type, string name)
    {
      IEnumerator enumerator = ((IEnumerable)lookupMethod
        .MakeGenericMethod(type)
        .Invoke(lookup, new object[] { name }))
        .GetEnumerator();
      if (enumerator.MoveNext())
        yield return enumerator.Current;
    }
  }
}
