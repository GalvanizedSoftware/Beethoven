using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extentions
{
  public static class MethodInfoExtentions
  {
    public static IEnumerable<Type> GetParameterTypes(this MethodInfo methodInfo)
    {
      return methodInfo.GetParameters().Select(info => info.ParameterType);
    }

    public static bool IsMatch(this MethodInfo methodInfo, IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      MethodInfo actualMethod = methodInfo.IsGenericMethod ?
        methodInfo.MakeGenericMethod(genericArguments) :
        methodInfo;
      return actualMethod
        .GetParameters()
        .Select(info => info.ParameterType)
        .SequenceEqual(parameters) &&
        returnType.FullName == actualMethod.ReturnType.FullName;
    }

    public static object Invoke(this MethodInfo methodInfo, object instance, object[] parameters, Type[] genericArguments)
    {
      MethodInfo actualMethod = methodInfo.IsGenericMethod ?
        methodInfo.MakeGenericMethod(genericArguments) :
        methodInfo;
      return actualMethod.Invoke(instance, parameters);
    }

    public static MethodInfo MakeGeneric<T>(this MethodInfo methodInfo)
    {
      return methodInfo.IsGenericMethod ? methodInfo.MakeGenericMethod(typeof(T)) : methodInfo;
    }
  }
}
