using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethod : MethodDefinition
  {
    private readonly object instance;
    private readonly MethodInfo methodInfo;

    public static MappedMethod Create(MethodInfo methodInfo, object instance) => 
      methodInfo is null ? 
        null : 
        new(methodInfo.Name, instance, methodInfo);

    public MappedMethod(string name, object instance) :
      this(name, instance, name)
    {
    }

    public MappedMethod(MethodInfo methodInfo, object instance) :
      this(methodInfo?.Name, instance, methodInfo)
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
      methodInfo.HasReturnType();
    }

    private static MethodInfo GetMethod(object instance, string targetName)
    {
      return instance?
        .GetType()
        .FindSingleMethod(targetName);
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      returnValue = methodInfo.Invoke(instance, parameters, genericArguments);
  }
}