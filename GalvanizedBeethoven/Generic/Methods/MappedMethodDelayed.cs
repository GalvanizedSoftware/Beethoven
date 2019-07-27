using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethodDelayed : Method
  {
    private readonly MethodInfo methodInfo;
    private object instance;

    public MappedMethodDelayed(MethodInfo methodInfo) :
      this(methodInfo?.Name, methodInfo)
    {
    }

    private MappedMethodDelayed(string mainName, MethodInfo methodInfo) :
      base(mainName, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo;
      methodInfo.HasReturnType();
    }

    public void SetInstance(object value) =>
      instance = value;

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      returnValue = methodInfo.Invoke(instance, parameters, genericArguments);
  }
}