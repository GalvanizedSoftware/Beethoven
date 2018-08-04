using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.Constants;

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
      return from methodInfo in type.GetMethods(ResolveFlags)
             where methodInfo.Name == name
             select methodInfo;
    }

    internal static IEnumerable<MethodInfo> GetAllMethodsAndInherited(this Type type)
    {
      return type.GetMethods(ResolveFlags)
        .Concat(type.GetInterfaces()
          .SelectMany(interfaceType => interfaceType
            .GetMethods(ResolveFlags)));
    }

    public static object Create1(this Type type, Type genericType1, params object[] constructorParameters)
    {
      Type genericType = type.MakeGenericType(genericType1);

      ConstructorInfo[] constructors = genericType.GetConstructors(ResolveFlags);
      if (constructors.Length == 1)
        return constructors.First().Invoke(constructorParameters);
      return genericType
        .GetConstructor(constructorParameters
          .Select(obj => obj.GetType())
          .ToArray())
        ?.Invoke(constructorParameters);
    }

    public static object Create2(this Type type, Type genericType1, Type genericType2, params object[] constructorParameters)
    {
      return type.MakeGenericType(genericType1, genericType2)
        .GetConstructor(constructorParameters
          .Select(obj => obj.GetType())
          .ToArray())
        ?.Invoke(constructorParameters);
    }

    public static object InvokeStatic(this Type type, string methodName, params object[] parameters)
    {
      return type.GetMethod(methodName, StaticResolveFlags)
        ?.Invoke(type, parameters);
    }
  }
}