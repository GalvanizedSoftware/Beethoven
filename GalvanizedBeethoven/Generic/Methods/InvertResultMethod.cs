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

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      method.Invoke(localInstance, ref returnValue, parameters, genericArguments, methodInfo);
      returnValue = InvertValue(returnValue);
    }
    
    private static bool InvertValue(object value)
    {
      if (!(value is bool boolValue))
        throw new ArgumentException("Method must return bool to use InvertResultMethod");
      return !boolValue;
    }
  }
}
