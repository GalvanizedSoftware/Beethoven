using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MappedDefaultMethod : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly Func<MethodInfo, object[], object> mainFunc;
    private readonly Type returnType;

    public MappedDefaultMethod(MethodInfo methodInfo, Func<MethodInfo, object[], object> mainFunc) :
      base(methodInfo.Name)
    {
      this.methodInfo = methodInfo;
      this.mainFunc = mainFunc;
      returnType = methodInfo.ReturnType;
    }

    public override bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type matchReturnType)
    {
      return methodInfo.IsMatch(parameters, genericArguments, matchReturnType);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      object returnValue = mainFunc(methodInfo, parameters);
      if (returnType == typeof(void))
        return;
      if (returnValue == null)
        returnValue = returnType.GetDefaultValue();
      returnAction(returnValue);
    }
  }
}