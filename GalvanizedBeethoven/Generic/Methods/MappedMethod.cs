using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethod : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;

    public MappedMethod(MethodInfo methodInfo) :
      base(methodInfo.Name)
    {
      this.methodInfo = methodInfo;
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public MappedMethod(string mainName, object instance, string targetName) :
      base(mainName)
    {
      Instance = instance;
      MethodInfo[] methodInfos = instance
        .GetType()
        .GetAllMethods(targetName)
        .ToArray();
      switch (methodInfos.Length)
      {
        case 0:
          throw new MissingMethodException($"The method {targetName} was not found");
        case 1:
          methodInfo = methodInfos.Single();
          break;
        default:
          throw new MissingMethodException($"Multiple versions of the method {targetName} were found");
      }
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public object Instance { private get; set; }

    public override bool IsMatch(IEnumerable<Type> parameters, Type[] genericArguments, Type returnType)
    {
      return methodInfo.IsMatch(parameters, genericArguments, returnType);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      object returnValue = methodInfo.Invoke(Instance, parameters, genericArguments);
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}