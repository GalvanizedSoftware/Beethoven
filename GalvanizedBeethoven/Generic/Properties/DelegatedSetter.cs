using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DelegatedSetter<T> : IPropertyDefinition<T>
  {
    private readonly Action<T> delegateAction;

    public DelegatedSetter(Action<T> delegateAction)
    {
      this.delegateAction = delegateAction;
    }

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      return true;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      delegateAction(newValue);
      return true;
    }

    public static DelegatedSetter<T> CreateWithReflection(object target, string methodName, string propertyName)
    {
      if (target == null)
        throw new NullReferenceException();
      MethodInfo methodInfo = target
        .GetType()
        .GetMethod(methodName, Constants.ResolveFlags)
        .MakeGeneric<T>();
      return new DelegatedSetter<T>(CreateAction(target, methodInfo, propertyName));
    }

    public static Action<T> CreateAction(object target, MethodInfo methodInfo, string propertyName)
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
