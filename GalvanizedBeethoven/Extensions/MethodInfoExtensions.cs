using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class MethodInfoExtensions
  {
    public static IEnumerable<Type> GetParameterTypes(this MethodInfo methodInfo)
    {
      return methodInfo.GetParameters().Select(info => info.ParameterType);
    }

    public static (Type, string)[] GetParameterTypeAndNames(this MethodInfo methodInfo)
    {
      return methodInfo
        .GetParameters()
        .Select(info => (info.ParameterType, info.Name))
        .ToArray();
    }

    public static bool IsMatch(this MethodInfo methodInfo, IEnumerable<(Type, string)> parameters, Type[] genericArguments, Type returnType) =>
      IsMatch(methodInfo, parameters.Select(tuple => tuple.Item1), genericArguments, returnType);

    public static bool IsMatch(this MethodInfo methodInfo, IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      MethodInfo actualMethod = methodInfo.GetActualMethod(genericArguments);
      return actualMethod
               .GetParameterTypes()
               .SequenceEqual(parameters) &&
             returnType.FullName?.TrimEnd('&') == actualMethod.ReturnType.FullName;
    }

    public static object Invoke(this MethodInfo methodInfo, object instance, object[] parameters, Type[] genericArguments) =>
      methodInfo.GetActualMethod(genericArguments).Invoke(instance, parameters);

    private static MethodInfo GetActualMethod(this MethodInfo methodInfo, Type[] genericArguments) =>
      methodInfo.IsGenericMethod ? methodInfo.MakeGenericMethod(genericArguments) : methodInfo;

    public static MethodInfo MakeGeneric<T>(this MethodInfo methodInfo) =>
      methodInfo.IsGenericMethod ? methodInfo.MakeGenericMethod(typeof(T)) : methodInfo;

    public static bool HasReturnType(this MethodInfo methodInfo)
      => methodInfo.ReturnType != typeof(void);
  }
}
