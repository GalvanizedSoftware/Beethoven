using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class MappedInvokerDelayed : IInvoker
  {
    private readonly Func<object, object> creatorFunc;
    private readonly MethodInfo methodInfo;
    private object instance;

    public MappedInvokerDelayed(MethodInfo methodInfo, Func<object, object> creatorFunc)
    {
      this.methodInfo = methodInfo;
      this.creatorFunc = creatorFunc;
    }

    private object GetInstance(object value) =>
      instance ??= creatorFunc(value);

    public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _)
    {
      returnValue = methodInfo.Invoke(GetInstance(localInstance), parameters, genericArguments);
      return true;
    }
  }
}