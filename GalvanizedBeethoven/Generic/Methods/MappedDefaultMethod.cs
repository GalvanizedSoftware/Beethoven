using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedDefaultMethod : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly Func<MethodInfo, object[], object> mainFunc;
    private readonly Type returnType;

    public MappedDefaultMethod(MethodInfo methodInfo, Func<MethodInfo, object[], object> mainFunc) :
      base(methodInfo.Name, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo;
      this.mainFunc = mainFunc;
      returnType = methodInfo.ReturnType;
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      returnValue = mainFunc(methodInfo, parameters);
  }
}