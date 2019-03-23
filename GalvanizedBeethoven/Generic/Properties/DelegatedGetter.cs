using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DelegatedGetter<T> : IPropertyDefinition<T>
  {
    private readonly Func<T> delegateFunc;

    public DelegatedGetter(Func<T> delegateFunc)
    {
      this.delegateFunc = delegateFunc;
    }

    // ReSharper disable once RedundantAssignment
    public bool InvokeGetter(ref T returnValue)
    {
      returnValue = delegateFunc();
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      return true;
    }

    public static DelegatedGetter<T> CreateWithReflection(object target, string methodName, string propertyName)
    {
      MethodInfo methodInfo = target
        .GetType()
        .GetMethod(methodName, Constants.ResolveFlags)
        .MakeGeneric<T>();
      return new DelegatedGetter<T>(CreateFunc(target, methodInfo, propertyName));
    }

    public static Func<T> CreateFunc(object target, MethodInfo methodInfo, string propertyName)
    {
      Type returnType = methodInfo.ReturnType;
      if (methodInfo.ReturnType != typeof(T))
        throw new ArgumentException($"Method: {methodInfo.Name} has incorrect return type expected: {typeof(T).FullName}, actual: {returnType.FullName}");
      Type[] parameterTypes = methodInfo.GetParameterTypes().ToArray();
      switch (parameterTypes.Length)
      {
        case 0:
          return () => (T)methodInfo.Invoke(target, new object[0]);
        case 1:
          Debug.Assert(parameterTypes[0] == typeof(string));
          return () => (T)methodInfo.Invoke(target, new object[] { propertyName, });
        default:
          throw new ArgumentException($"Method: {methodInfo.Name} not found or has incorrect signature");
      }
    }
  }
}
