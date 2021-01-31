using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class MethodInfoExtensions
  {
    public static IEnumerable<Type> GetParameterTypes(this MethodInfo methodInfo) =>
      methodInfo.GetParametersSafe().Select(info => info.ParameterType);

    public static IEnumerable<Type> GetParameterTypesIgnoreGeneric(this MethodInfo methodInfo) =>
      methodInfo
        .GetParametersSafe()
        .Select(info => info.ParameterType)
        .Select(TypeExtensions.RemoveGeneric);

    public static (Type, string)[] GetParameterTypeAndNames(this MethodInfo methodInfo)
    {
      return methodInfo
        .GetParametersSafe()
        .Select(info => (info.ParameterType.RemoveGeneric(), info.Name))
        .ToArray();
    }

    public static bool IsMatch(this MethodInfo methodInfo, IEnumerable<(Type, string)> parameters, Type[] genericArguments, Type returnType) =>
      IsMatch(methodInfo, parameters.Select(tuple => tuple.Item1), genericArguments, returnType);

    public static bool IsMatchIgnoreNames(this MethodInfo methodInfo1, MethodInfo methodInfo2) =>
      methodInfo1 != null &&
      methodInfo1.GetParameterTypesIgnoreGeneric().SequenceEqual(methodInfo2.GetParameterTypesIgnoreGeneric()) &&
      methodInfo1.ReturnType.IsMatchReturnType(methodInfo2?.ReturnType);

    public static bool IsMatch(this MethodInfo methodInfo, IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      MethodInfo actualMethod = methodInfo.GetActualMethod(genericArguments);
      return actualMethod
               .GetParameterTypes()
               .SequenceEqual(parameters) &&
             returnType.IsMatchReturnTypeIgnoreGeneric(actualMethod);
    }

    public static bool IsGenericSafe(this MethodInfo methodInfo) =>
      methodInfo?.IsGenericMethod == true;

    public static IEnumerable<ParameterInfo> GetParametersSafe(this MethodInfo methodInfo) =>
      methodInfo?.GetParameters() ?? Enumerable.Empty<ParameterInfo>();

    public static object Invoke(this MethodInfo methodInfo, object instance, object[] parameters, Type[] genericArguments) => 
	    methodInfo.GetActualMethod(genericArguments)?.Invoke(instance, parameters);

    internal static MethodInfo GetActualMethod(this MethodInfo methodInfo, Type[] genericArguments) =>
      methodInfo == null ? null :
      !methodInfo.IsGenericMethodDefinition ? methodInfo :
      genericArguments == null ? methodInfo :
      methodInfo.MakeGenericMethod(genericArguments);

    public static MethodInfo MakeGeneric<T>(this MethodInfo methodInfo) =>
      methodInfo.IsGenericSafe() ? methodInfo?.MakeGenericMethod(typeof(T)) : methodInfo;

    public static bool HasReturnType(this MethodInfo methodInfo)
      => methodInfo?.ReturnType != typeof(void);

    public static string GetReturnType(this MethodInfo methodInfo)
    {
      Type returnType = methodInfo?.ReturnType;
      return returnType != typeof(void) ? returnType.GetFullName() : "void";
    }

    public static object GetDefaultReturnValue(this MethodInfo methodInfo) =>
      methodInfo?.ReturnType.GetDefaultValue();

    public static object[] GetLocalParameters(this MethodInfo methodInfo,
      object[] parameters, (Type, string)[] localParameters)
    {
      if (methodInfo == null)
        throw new NullReferenceException();
      (Type, string)[] masterParameters = methodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(methodInfo.ReturnType)
        .ToArray();
      return localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .Select(index => index < 0 ? null : parameters[index])
        .ToArray();
    }
  }
}
