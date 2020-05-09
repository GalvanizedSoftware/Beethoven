using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  internal class RealMethodInvoker : IMethodInvoker
  {
    private readonly MethodInfo methodInfo;
    private readonly MethodDefinition methodDefinition;

    public RealMethodInvoker(MethodInfo methodInfo, MethodDefinition methodDefinition)
    {
      this.methodInfo = methodInfo;
      this.methodDefinition = methodDefinition;
    }

    public object Invoke(object master, Type[] genericTypes, object[] parameters)
    {
      object returnValue = methodInfo.ReturnType.GetDefaultValue();
      methodDefinition.Invoke(master, ref returnValue, parameters, genericTypes, methodInfo);
      return returnValue;
    }
  }
}