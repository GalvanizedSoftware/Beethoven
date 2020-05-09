using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedDefaultMethod : MethodDefinition
  {
    private readonly MethodInfo methodInfo;
    private readonly Func<MethodInfo, object[], object> mainFunc;

    public override int SortOrder => 2;

    public MappedDefaultMethod(MethodInfo methodInfo, Func<MethodInfo, object[], object> mainFunc) :
      base(methodInfo?.Name, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo ?? throw new NullReferenceException();
      this.mainFunc = mainFunc ?? throw new NullReferenceException();
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      returnValue = mainFunc(methodInfo, parameters);
  }
}