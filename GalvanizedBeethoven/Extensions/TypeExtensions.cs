﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods;
using static GalvanizedSoftware.Beethoven.Core.ReflectionConstants;

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

    internal static IEnumerable<PropertyInfo> GetAllProperties(this Type masterType) =>
      masterType
        .GetAllTypes()
        .SelectMany(type => type
          .GetProperties(ResolveFlags))
        .DistinctProperties();

    internal static IEnumerable<MethodInfo> GetNotSpecialMethods(this Type type) =>
      type.GetMethods(ResolveFlags)
        .Where(methodInfo => !methodInfo.IsSpecialName);

    internal static IEnumerable<MethodInfo> GetAllMethodsAndInherited(this Type type) =>
      type.GetAllTypes()
        .SelectMany(childType => childType
          .GetMethods(ResolveFlags));

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
      mainReturnType?.IsGeneric() == true ?
        actualReturnType != typeof(void) :
        mainReturnType.IsMatchReturnType(actualReturnType);

    internal static bool IsMatchReturnTypeIgnoreGeneric(this Type mainReturnType, MethodInfo actualMethod) =>
      mainReturnType?.IsMatchReturnTypeIgnoreGeneric(actualMethod?.ReturnType) == true;

    internal static bool IsByRefence(this Type type) =>
      type?.IsByRef == true;

    internal static string GetFullName(this Type type) =>
      Type.GetTypeCode(type) switch
      {
        TypeCode.String => "string",
        TypeCode.Boolean => "bool",
        TypeCode.Char => "char",
        TypeCode.SByte => "sbyte",
        TypeCode.Byte => "byte",
        TypeCode.Int16 => "short",
        TypeCode.UInt16 => "ushort",
        TypeCode.Int32 => "int",
        TypeCode.UInt32 => "uint",
        TypeCode.Int64 => "long",
        TypeCode.UInt64 => "ulong",
        TypeCode.Single => "float",
        TypeCode.Double => "double",
        TypeCode.Decimal => "decimal",
        _ => GetTypeNameFallback(type)
      };

    private static string GetTypeNameFallback(Type type)
    {
      if (type.FullName == null)
        return type.Name;
      string fullName = type.Name.TrimEnd('&');
      if (!type.IsGenericType)
        return $"{type.Namespace}.{fullName}";
      int index = fullName.IndexOf('`');
      fullName = fullName.Substring(0, index);
      return $"{type.Namespace}.{fullName}<{type.GetGenericTypes()}>";
    }

    internal static object Create(this Type type, object[] parameters) =>
      type.GetConstructors().Single().Invoke(parameters);

    internal static string GetBaseName(this Type type) =>
      type.BaseType == typeof(ValueType) ? "struct" : type.GetFullName();

    internal static string GetFormattedName(this Type type) => $"{type.Name.Replace("`", "_")}";

    private static string GetGenericTypes(this Type type) =>
      string.Join(",", type.GetGenericArguments().Select(itemType => itemType.GetFullName()));

    private static bool IsGeneric(this Type type) =>
      type == typeof(AnyGenericType) || type.FullName == null;
  }
}