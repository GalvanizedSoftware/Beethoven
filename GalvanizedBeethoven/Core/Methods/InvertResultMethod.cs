using System;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public class InvertResultMethod : Method
  {
    private readonly Method method;

    public InvertResultMethod(Method method) : base(method.Name)
    {
      this.method = method;
    }

    public override bool IsMatch(IEnumerable<(Type, string)> parameters, Type[] genericArguments, Type returnType)
    {
      return method.IsMatch(parameters, genericArguments, returnType);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo)
    {
      bool returnValue = false;
      method.Invoke(value => InvertValue(value, out returnValue), parameters, genericArguments, methodInfo);
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
