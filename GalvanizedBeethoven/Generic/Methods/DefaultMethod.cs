using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class DefaultMethod
  {
    private readonly Func<MethodInfo, object[], object> mainFunc;

    public DefaultMethod(Func<MethodInfo, object[], object> mainFunc)
    {
      this.mainFunc = mainFunc ?? ((unused1, unused2) => null);
    }

    public DefaultMethod(Action<MethodInfo, object[]> mainAction)
    {
      mainFunc = (methodInfo, objects) =>
      {
        mainAction?.Invoke(methodInfo, objects);
        return methodInfo.GetDefaultReturnValue();
      };
    }

    public Method CreateMapped(MethodInfo methodInfo) => 
      new MappedDefaultMethod(methodInfo, mainFunc);
  }
}
