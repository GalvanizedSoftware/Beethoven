using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class ValidityCheck<T> : IPropertyDefinition<T>
  {
    private readonly Func<T, bool> checkFunc;

    public ValidityCheck(Func<T, bool> checkFunc)
    {
      this.checkFunc = checkFunc;
    }

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      return true;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      if (!checkFunc(newValue))
        throw new ArgumentOutOfRangeException($"Value {newValue} invalid");
      return true;
    }

    public static ValidityCheck<T> CreateWithReflection(object target, string methodName)
    {
      Func<T, bool> CheckFunc(MethodInfo methodInfo1) => 
        newValue => (bool)methodInfo1.Invoke(target, new object[] { newValue });

      MethodInfo methodInfo = target?
        .GetType()
        .GetMethod(methodName, Constants.ResolveFlags)
        .MakeGeneric<T>();
      return new ValidityCheck<T>(CheckFunc(methodInfo ?? throw new NullReferenceException()));
    }
  }
}