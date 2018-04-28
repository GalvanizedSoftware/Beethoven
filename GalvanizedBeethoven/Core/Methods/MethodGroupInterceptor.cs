using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Extentions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public class MethodGroupInterceptor : IInterceptor
  {
    private readonly List<Tuple<object, MethodInfo>> methodList;

    public MethodGroupInterceptor(string name, params object[] objects)
    {
      methodList = new List<Tuple<object, MethodInfo>>
      (
        from obj in objects
        from methodInfo in obj.GetType().GetAllMethods(name)
        select new Tuple<object, MethodInfo>(obj, methodInfo)
      );
    }

    public void Intercept(IInvocation invocation)
    {
      MethodInfo methodInfo = invocation.Method;
      Tuple<object, MethodInfo> tuple = methodList.FirstOrDefault(item => methodInfo == item.Item2);
      if (tuple == null)
        throw new MissingMethodException();
      object obj = tuple.Item1;
      if (methodInfo.ReturnType == typeof(void))
        methodInfo.Invoke(obj, invocation.Arguments);
      else
        invocation.ReturnValue = methodInfo.Invoke(obj, invocation.Arguments);
    }
  }
}
