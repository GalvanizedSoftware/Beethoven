using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extentions
{
  internal static class TypeExtentions
  {
    internal static IEnumerable<Type> GetAllTypes(this Type type)
    {
      yield return type;
      foreach (Type subType in type.GetInterfaces())
        yield return subType;
    }

    internal static IEnumerable<MethodInfo> GetAllMethods(this Type type, string name)
    {
      return from methodInfo in type.GetMethods()
             where methodInfo.Name == name
             select methodInfo;
    }

    public static object GetDefault(this Type type)
    {
      return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
  }
}