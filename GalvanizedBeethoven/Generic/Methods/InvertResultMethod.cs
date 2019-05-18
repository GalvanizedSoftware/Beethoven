using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class InvertResultMethod : Method
  {
    private readonly Method method;

    public InvertResultMethod(Method method) : 
      base(method.Name, method.MethodMatcher)
    {
      this.method = method;
    }

    public override void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      bool returnValue = false;
      method.Invoke(localInstance, value => InvertValue(value, out returnValue), parameters, genericArguments, methodInfo);
      returnAction(!returnValue);
    }
    
    private static void InvertValue(object value, out bool returnValue)
    {
      if (!(value is bool boolValue))
        throw new ArgumentException("Method must return bool to use InvertResultMethod");
      returnValue = boolValue;
    }
  }
}
