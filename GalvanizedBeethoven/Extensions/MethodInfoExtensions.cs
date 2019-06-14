using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class MethodInfoExtensions
  {
    public static IEnumerable<Type> GetParameterTypes(this MethodInfo methodInfo) =>
      methodInfo.GetParameters().Select(info => info.ParameterType);

    public static IEnumerable<Type> GetParameterTypesIgnoreGeneric(this MethodInfo methodInfo) =>
      methodInfo
        .GetParameters()
        .Select(info => info.ParameterType)
        .Select(TypeExtensions.RemoveGeneric);

    public static (Type, string)[] GetParameterTypeAndNames(this MethodInfo methodInfo)
    {
      return methodInfo
        .GetParameters()
        .Select(info => (info.ParameterType.RemoveGeneric(), info.Name))
        .ToArray();
    }

    public static bool IsMatch(this MethodInfo methodInfo, IEnumerable<(Type, string)> parameters, Type[] genericArguments, Type returnType) =>
      IsMatch(methodInfo, parameters.Select(tuple => tuple.Item1), genericArguments, returnType);

    public static bool IsMatchIgnoreNames(this MethodInfo methodInfo1, MethodInfo methodInfo2)
    {
      return methodInfo1.GetParameterTypesIgnoreGeneric()
               .SequenceEqual(methodInfo2.GetParameterTypesIgnoreGeneric()) &&
             methodInfo1.ReturnType.IsMatchReturnType(methodInfo2.ReturnType);
    }

    public static bool IsMatch(this MethodInfo methodInfo, IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      MethodInfo actualMethod = methodInfo.GetActualMethod(genericArguments);
      bool isMatchReturnTypeIgnoreGeneric = actualMethod
                                              .GetParameterTypes()
                                              .SequenceEqual(parameters) &&
                                            returnType.IsMatchReturnTypeIgnoreGeneric(actualMethod.ReturnType);
      return isMatchReturnTypeIgnoreGeneric;
    }

    public static bool TypeIgnoreGeneric(this MethodInfo methodInfo, IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      MethodInfo actualMethod = methodInfo.GetActualMethod(genericArguments);
      return actualMethod
               .GetParameterTypes()
               .SequenceEqual(parameters) &&
             returnType.IsMatchReturnTypeIgnoreGeneric(actualMethod.ReturnType);
    }

    public static object Invoke(this MethodInfo methodInfo, object instance, object[] parameters, Type[] genericArguments) =>
      methodInfo.GetActualMethod(genericArguments).Invoke(instance, parameters);

    private static MethodInfo GetActualMethod(this MethodInfo methodInfo, Type[] genericArguments)
    {
      return !methodInfo.IsGenericMethod ?
        methodInfo :
        genericArguments == null ?
          methodInfo :
          methodInfo.MakeGenericMethod(genericArguments);
    }

    public static MethodInfo MakeGeneric<T>(this MethodInfo methodInfo) =>
      methodInfo.IsGenericMethod ? methodInfo.MakeGenericMethod(typeof(T)) : methodInfo;

    public static bool HasReturnType(this MethodInfo methodInfo)
      => methodInfo.ReturnType != typeof(void);
  }
}
