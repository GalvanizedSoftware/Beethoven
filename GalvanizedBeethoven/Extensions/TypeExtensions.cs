using GalvanizedSoftware.Beethoven.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class TypeExtensions
  {
    internal static IEnumerable<Type> GetAllTypes(this Type type)
    {
      yield return type;
      foreach (Type subType in type.GetInterfaces())
        yield return subType;
      Type baseType = type.BaseType;
      while (baseType != null)
      {
        yield return baseType;
        baseType = baseType.BaseType;
      }
    }

    internal static IEnumerable<MethodInfo> GetAllMethods(this Type type, string name)
    {
      return from methodInfo in type.GetMethods(Constants.ResolveFlags)
             where methodInfo.Name == name
             select methodInfo;
    }

    internal static IEnumerable<MethodInfo> GetAllMethodsAndInherited(this Type type)
    {
      return type.GetAllTypes()
        .SelectMany(childType => childType
          .GetMethods(Constants.ResolveFlags));
    }

    public static object Create1(this Type type, Type genericType1, params object[] constructorParameters)
    {
      Type genericType = type.MakeGenericType(genericType1);
      ConstructorInfo[] constructors = genericType.GetConstructors(Constants.ResolveFlags);
      if (constructors.Length == 1)
        return constructors.First().Invoke(constructorParameters);
      return genericType
        .GetConstructor(constructorParameters
          .Select(obj => obj.GetType())
          .ToArray())
        ?.Invoke(constructorParameters);
    }

    public static object InvokeStatic(this Type type, string methodName, params object[] parameters)
    {
      return type.GetMethod(methodName, Constants.StaticResolveFlags)
        ?.Invoke(type, parameters);
    }

    public static object GetDefaultValue(this Type type)
    {
      return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    public static MethodInfo FindSingleMethod(this Type type, string targetName)
    {
      MethodInfo[] methodInfos = type
        .GetAllMethods(targetName)
        .ToArray();
      switch (methodInfos.Length)
      {
        case 0:
          throw new MissingMethodException($"The method {targetName} was not found");
        case 1:
          return methodInfos.Single();
        default:
          throw new MissingMethodException($"Multiple versions of the method {targetName} were found");
      }
    }
  }
}