using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class TypeExtensions
  {
    internal static Type RemoveGeneric(this Type type) =>
      type.FullName == null ? typeof(AnyGenericType) : type;


    internal static IEnumerable<Type> GetAllTypes(this Type type) =>
      type == null ?
        Array.Empty<Type>() :
        new[] { type }
          .Concat(type.GetInterfaces()
            .SelectMany(subType => subType.GetAllTypes()))
          .Concat(type.BaseType.GetAllTypes())
          .Distinct();

    internal static IEnumerable<MethodInfo> GetAllMethods(this Type masterType, string name) =>
      masterType
        .GetAllTypes()
        .SelectMany(type => type
          .GetMethods(ResolveFlags)
          .Where(methodInfo => methodInfo.Name == name))
        .Distinct(new ExactMethodComparer());

    internal static EventInfo GetEventInfo(this Type type, string name) =>
      type
        .GetAllTypes()
        .SelectMany(childType => childType.GetEvents(ResolveFlags))
        .SingleOrDefault(eventInfo => eventInfo.Name == name);

    internal static IEnumerable<MethodInfo> GetNotSpecialMethods(this Type type) =>
      type.GetMethods(ResolveFlags)
        .Where(methodInfo => !methodInfo.IsSpecialName);

    internal static IEnumerable<MethodInfo> GetAllMethodsAndInherited(this Type type) =>
      type.GetAllTypes()
        .SelectMany(childType => childType
          .GetMethods(ResolveFlags));

    internal static IEnumerable<MethodInfo> GetMethodsAndInherited(this Type type) =>
      type.GetAllTypes()
        .SelectMany(childType => childType.GetMethods(ResolveFlags))
        .Where(info => !info.IsSpecialName);

    internal static object Create1(this Type type, Type genericType1, params object[] constructorParameters)
    {
      Type genericType = type.MakeGenericType(genericType1);
      ConstructorInfo[] constructors = genericType.GetConstructors(ResolveFlags);
      return constructors.Length == 1 ?
        constructors.First().Invoke(constructorParameters) :
        genericType
          .GetConstructor(constructorParameters
            .Select(obj => obj?.GetType())
            .ToArray())?
          .Invoke(constructorParameters);
    }

    internal static object InvokeStatic(this Type type, string methodName, params object[] parameters) =>
      type.GetMethod(methodName, StaticResolveFlags)?
        .Invoke(type, parameters);

    internal static object GetDefaultValue(this Type type) => 
      type == typeof(void) || !type.IsValueType ? null : Activator.CreateInstance(type);

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

    internal static bool IsMatchReturnType(this Type returnType, Type actualReturnType) =>
      returnType?.FullName?.TrimEnd('&') == actualReturnType?.FullName;

    internal static bool IsMatchReturnTypeIgnoreGeneric<TActual>(this Type mainReturnType) =>
      mainReturnType.IsMatchReturnTypeIgnoreGeneric(typeof(TActual));

    internal static bool IsMatchReturnTypeIgnoreGeneric(this Type mainReturnType, Type actualReturnType) =>
      mainReturnType.IsGeneric() ?
        actualReturnType != typeof(void) :
        mainReturnType.IsMatchReturnType(actualReturnType);

    internal static bool IsMatchReturnTypeIgnoreGeneric(this Type mainReturnType, MethodInfo actualMethod) =>
      mainReturnType.IsMatchReturnTypeIgnoreGeneric(actualMethod?.ReturnType);

    internal static bool IsByRefence(this Type type) =>
      type?.IsByRef == true;

    private static bool IsGeneric(this Type type) =>
      type == typeof(AnyGenericType) || type.FullName == null;

  }
}