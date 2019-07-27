using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public static class DelegatedSetterFactory
  {
    private static readonly MethodInfo createMethod =
      typeof(DelegatedSetterFactory).GetMethod(nameof(CreateWithReflection), StaticResolveFlags) ??
      throw new NullReferenceException();

    public static object Create(Type type, object target, string methodName, string propertyName) =>
      createMethod
        .MakeGenericMethod(type)
        .Invoke(type, new[] { target, methodName, propertyName });

    public static DelegatedSetter<T> CreateWithReflection<T>(object target, string methodName, string propertyName)
    {
      if (target == null)
        throw new NullReferenceException();
      MethodInfo methodInfo = target
        .GetType()
        .GetMethod(methodName, ResolveFlags)
        .MakeGeneric<T>();
      return new DelegatedSetter<T>(CreateAction<T>(target, methodInfo, propertyName));
    }

    public static Action<T> CreateAction<T>(object target, MethodInfo methodInfo, string propertyName)
    {
      Type[] parameterTypes = methodInfo.GetParameterTypes().ToArray();
      switch (parameterTypes.Length)
      {
        case 1:
          Debug.Assert(parameterTypes[0] == typeof(T));
          return newValue => methodInfo.Invoke(target, new object[] { newValue });
        case 2:
          Debug.Assert(parameterTypes[0] == typeof(string));
          Debug.Assert(parameterTypes[1] == typeof(T));
          return newValue => methodInfo.Invoke(target, new object[] { propertyName, newValue });
        default:
          throw new ArgumentException($"Method: {methodInfo?.Name} not found or has incorrect signature");
      }
    }
  }
}
