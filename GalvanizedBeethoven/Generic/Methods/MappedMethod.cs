using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethod : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;

    public MappedMethod(MethodInfo methodInfo) :
      this(methodInfo.Name, null, methodInfo)
    {
    }

    public MappedMethod(string name, object instance) :
      this(name, instance, name)
    {
    }

    public MappedMethod(string mainName, object instance, string targetName) :
      this(mainName, instance, GetMethod(instance, targetName))
    {
    }


    public MappedMethod(object instance, MethodInfo methodInfo) :
      this(methodInfo.Name, instance, methodInfo)
    {
    }

    private MappedMethod(string mainName, object instance, MethodInfo methodInfo) :
      base(mainName, new MatchMethodInfoExact(methodInfo))
    {
      Instance = instance;
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.HasReturnType();
    }

    private static MethodInfo GetMethod(object instance, string targetName)
    {
      return instance
        .GetType()
        .FindSingleMethod(targetName);
    }

    public object Instance { private get; set; }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      object returnValue = methodInfo.Invoke(Instance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}