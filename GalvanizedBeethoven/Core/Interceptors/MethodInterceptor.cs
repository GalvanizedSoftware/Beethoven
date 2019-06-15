using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class MethodInterceptor : IGeneralInterceptor
  {
    private readonly Method method;

    internal MethodInterceptor(Method method)
    {
      MainDefinition = this.method = method;
    }

    public void Invoke(InstanceMap instanceMap, Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo)
    {
      object returnValue = methodInfo.ReturnType.GetDefaultValue();
      method.InvokeFindInstance(instanceMap, ref returnValue, parameters, genericArguments, methodInfo);
      returnAction(returnValue);
    }

    public object MainDefinition { get; }
  }
}
