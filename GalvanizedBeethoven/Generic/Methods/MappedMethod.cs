using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethod : Method
  {
    private readonly object instance;
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;

    public MappedMethod(string name, object instance) :
      this(name, instance, name)
    {
    }

    public MappedMethod(MethodInfo methodInfo, object instance) :
      this(methodInfo.Name, instance, methodInfo)
    {
    }

    public MappedMethod(string mainName, object instance, string targetName) :
      this(mainName, instance, GetMethod(instance, targetName))
    {
    }


    private MappedMethod(string mainName, object instance, MethodInfo methodInfo) :
      base(mainName, new MatchMethodInfoExact(methodInfo))
    {
      this.instance = instance;
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.HasReturnType();
    }

    private static MethodInfo GetMethod(object instance, string targetName)
    {
      return instance
        .GetType()
        .FindSingleMethod(targetName);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      object returnValue = methodInfo.Invoke(instance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}