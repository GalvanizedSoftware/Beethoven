using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class MethodInvokerInstance
  {
    private readonly object master;
    private readonly MethodInfo methodInfo;
    private readonly MethodDefinition methodDefinition;

    public MethodInvokerInstance(object master, MethodInfo methodInfo, MethodDefinition methodDefinition)
    {
      this.master = master;
      this.methodInfo = methodInfo;
      this.methodDefinition = methodDefinition;
    }

    public object Invoke(Type[] genericTypes, object[] parameters)
    {
      object returnValue = methodInfo.ReturnType.GetDefaultValue();
      methodDefinition.Invoke(master, ref returnValue, parameters, genericTypes, methodInfo);
      return returnValue;
    }
  }
}