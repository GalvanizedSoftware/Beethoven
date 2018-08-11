using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extentions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class LinkedMethodsReturnValue : Method
  {
    private readonly Method[] methodList;

    public LinkedMethodsReturnValue(string name) : base(name)
    {
      methodList = new Method[0];
    }

    private LinkedMethodsReturnValue(LinkedMethodsReturnValue previous, Method newMethod) :
      base(previous.Name)
    {
      methodList = previous.methodList.Append(newMethod).ToArray();
    }

    public LinkedMethodsReturnValue Func<TReturnType>(Func<TReturnType> func) =>
      new LinkedMethodsReturnValue(this, new FuncMethod<TReturnType>(Name, func));

    public LinkedMethodsReturnValue Lambda<T>(T actionOrFunc) =>
      new LinkedMethodsReturnValue(this, new LambdaMethod<T>(Name, actionOrFunc));

    public LinkedMethodsReturnValue MappedMethod(object instance, string targetName) =>
      new LinkedMethodsReturnValue(this, new MappedMethod(Name, instance, targetName));

    public LinkedMethodsReturnValue AutoMappedMethod(object instance) =>
      new LinkedMethodsReturnValue(this, new MappedMethod(Name, instance, Name));

    public override bool IsMatch(IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      List<Type> parameterList = parameters.ToList();
      return methodList.Any(method => method.IsMatch(parameterList, genericArguments, returnType)) ||
             methodList.Any(method => method.IsMatch(parameterList.Append(returnType), genericArguments, typeof(bool)));
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo)
    {
      object returnValue = methodInfo.ReturnType.GetDefaultValue();
      Type[] parameterTypes = methodInfo.GetParameterTypes().ToArray();
      foreach (Method method in methodList)
      {
        if (!InvokeFirstMatch(method, ref returnValue, parameters, parameterTypes, genericArguments, methodInfo))
          break;
      }
      returnAction(returnValue);
    }

    private bool InvokeFirstMatch(Method method, ref object returnValue, object[] parameters, Type[] parameterTypes,
      Type[] genericArguments, MethodInfo methodInfo)
    {
      Type returnType = methodInfo.ReturnType;
      if (method.IsMatch(parameterTypes, genericArguments, returnType))
      {
        object returnValueLocal = returnValue;
        Action<object> returnAction = newValue => returnValueLocal = newValue;
        method.Invoke(returnAction, parameters, genericArguments, methodInfo);
        returnValue = returnValueLocal;
        return true;
      }
      if (!method.IsMatch(parameterTypes.Append(returnType.MakeByRefType()), genericArguments, typeof(bool)))
        throw new MissingMethodException();
      bool result = true;
      Action<object> localReturn = newValue => result = (bool)newValue;
      object[] newParameters = parameters.Append(returnValue).ToArray();
      method.Invoke(localReturn, newParameters, genericArguments, methodInfo);
      returnValue = newParameters.Last();
      return result;
    }
  }
}