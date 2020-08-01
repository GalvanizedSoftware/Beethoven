using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.ReflectionConstants;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal static class DelegatedGetterFactory
  {
    private static readonly MethodInfo createMethod =
      typeof(DelegatedGetterFactory).GetMethod(nameof(CreateWithReflection), StaticResolveFlags) ??
      throw new NullReferenceException();

    public static object Create(Type type, object target, string methodName, string propertyName) =>
      createMethod
        .MakeGenericMethod(type)
        .Invoke(type, new[] { target, methodName, propertyName });

    private static DelegatedGetter<T> CreateWithReflection<T>(object target, string methodName, string propertyName)
    {
      return new DelegatedGetter<T>(CreateFunc<T>(target, target?
        .GetType()
        .GetMethod(methodName, ResolveFlags)
        .MakeGeneric<T>(), propertyName));
    }

    private static Func<T> CreateFunc<T>(object target, MethodInfo methodInfo, string propertyName)
    {
      Type returnType = (methodInfo ?? throw new NullReferenceException()).ReturnType;
      if (methodInfo.ReturnType != typeof(T))
        throw new ArgumentException($"Method: {methodInfo.Name} has incorrect return type expected: {typeof(T).FullName}, actual: {returnType.FullName}");
      Type[] parameterTypes = methodInfo.GetParameterTypes().ToArray();
      switch (parameterTypes.Length)
      {
        case 0:
          return () => (T)methodInfo.Invoke(target, Array.Empty<object>());
        case 1:
          Debug.Assert(parameterTypes[0] == typeof(string));
          return () => (T)methodInfo.Invoke(target, new object[] { propertyName, });
        default:
          throw new ArgumentException($"Method: {methodInfo.Name} not found or has incorrect signature");
      }
    }
  }
}