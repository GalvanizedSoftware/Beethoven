using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class TypeExtensions
  {
    internal static Type RemoveGeneric(this Type type) =>
      type.FullName == null ? typeof(AnyGenericType) : type;


    internal static IEnumerable<Type> GetAllTypes(this Type type)
    {
      return type == null ?
        new Type[0] :
        new[] { type }
          .Concat(type.GetInterfaces()
            .SelectMany(subType => subType.GetAllTypes()))
          .Concat(type.BaseType.GetAllTypes())
          .Distinct();
    }

    internal static IEnumerable<MethodInfo> GetAllMethods(this Type type, string name)
    {
      return from methodInfo in type.GetMethods(ResolveFlags)
             where methodInfo.Name == name
             select methodInfo;
    }

    internal static EventInfo GetEventInfo(this Type type, string name) =>
      type
        .GetAllTypes()
        .SelectMany(childType => childType.GetEvents(ResolveFlags))
        .SingleOrDefault(eventInfo => eventInfo.Name == name);

    internal static IEnumerable<MethodInfo> GetNotSpecialMethods(this Type type)
    {
      return from methodInfo in type.GetMethods(ResolveFlags)
             where !methodInfo.IsSpecialName
             select methodInfo;
    }

    internal static IEnumerable<MethodInfo> GetAllMethodsAndInherited(this Type type)
    {
      return type.GetAllTypes()
        .SelectMany(childType => childType
          .GetMethods(ResolveFlags));
    }

    internal static IEnumerable<MethodInfo> GetMethodsAndInherited(this Type type)
    {
      return type.GetAllTypes()
        .SelectMany(childType => childType.GetMethods(ResolveFlags))
        .Where(info => !info.IsSpecialName);
    }

    internal static object Create1(this Type type, Type genericType1, params object[] constructorParameters)
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

    internal static object InvokeStatic(this Type type, string methodName, params object[] parameters)
    {
      return type.GetMethod(methodName, StaticResolveFlags)
        ?.Invoke(type, parameters);
    }

    internal static object GetDefaultValue(this Type type)
    {
      if (type == typeof(void))
      {
        return null;
      }
      return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    internal static MethodInfo FindSingleMethod(this Type type, string targetName)
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

    internal static void CheckForDelegateType(this Type type)
    {
      if (!typeof(Delegate).IsAssignableFrom(type))
        throw new InvalidCastException("You must supply an action, func or delegate");
    }

    internal static bool IsMatchReturnType(this Type returnType, Type actualReturnType) =>
      returnType.FullName?.TrimEnd('&') == actualReturnType.FullName;

    internal static bool IsMatchReturnTypeIgnoreGeneric<TActual>(this Type mainReturnType) =>
      mainReturnType.IsMatchReturnTypeIgnoreGeneric(typeof(TActual));

    internal static bool IsMatchReturnTypeIgnoreGeneric(this Type mainReturnType, Type actualReturnType) =>
      mainReturnType.IsGeneric() ?
        actualReturnType != typeof(void) :
        mainReturnType.IsMatchReturnType(actualReturnType);

    private static bool IsGeneric(this Type type) =>
      type == typeof(AnyGenericType) || type.FullName == null;
  }
}