using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodsWithInstance : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly Type returnType;

    public MethodsWithInstance(MethodInfo methodInfo):
      base(methodInfo.Name)
    {
      this.methodInfo = methodInfo;
      returnType = methodInfo.ReturnType;
    }

    public object Instance { private get; set; }
  
    public override bool IsMatch(IEnumerable<Type> parameters)
    {
      return methodInfo
        .GetParameters()
        .Select(info => info.ParameterType)
        .SequenceEqual(parameters);
    }

    protected override void Invoke(Action<object> returnAction, object[] parameters)
    {
      object returnValue = methodInfo.Invoke(Instance, parameters);
      if (returnType != typeof(void))
        returnAction(returnValue);
    }
  }
}