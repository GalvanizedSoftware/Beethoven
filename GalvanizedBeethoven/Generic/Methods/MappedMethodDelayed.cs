using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethodDelayed : MethodDefinition
  {
    private readonly Func<object, object> creatorFunc;
    private readonly MethodInfo methodInfo;
    private object instance;

    public MappedMethodDelayed(MethodInfo methodInfo, Func<object, object> creatorFunc) :
      this(methodInfo?.Name, methodInfo, creatorFunc)
    {
    }

    private MappedMethodDelayed(string mainName, MethodInfo methodInfo, Func<object, object> creatorFunc) :
      base(mainName, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo;
      this.creatorFunc = creatorFunc;
    }

    public object GetInstance(object value) =>
      instance ?? (instance = creatorFunc(value));

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      returnValue = methodInfo.Invoke(GetInstance(localInstance), parameters, genericArguments);
  }
}