using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.ReflectionConstants;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class ValidityCheckFactory
  {
    private static readonly MethodInfo createMethod =
      typeof(ValidityCheckFactory).GetMethod(nameof(CreateWithReflection), StaticResolveFlags) ??
      throw new NullReferenceException();

    public static object Create(Type type, object target, string methodName) =>
      createMethod
        .MakeGenericMethod(type)
        .Invoke(type, new[] { target, methodName });

    private static ValidityCheck<T> CreateWithReflection<T>(object target, string methodName)
    {
      Func<T, bool> CheckFunc(MethodInfo methodInfo1) => 
        newValue => (bool)methodInfo1.Invoke(target, new object[] { newValue });
      MethodInfo methodInfo = target?
        .GetType()
        .GetMethod(methodName, ResolveFlags)
        .MakeGeneric<T>();
      return new ValidityCheck<T>(CheckFunc(methodInfo ?? throw new NullReferenceException()));
    }

  }
}