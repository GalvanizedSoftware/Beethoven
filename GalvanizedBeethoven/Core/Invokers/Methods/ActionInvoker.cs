using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class ActionInvoker : IInvoker
  {
    private readonly Delegate action;
    private readonly (Type, string)[] localParameters;

    public static ActionInvoker Create(Delegate action) =>
      new(GetValues(action));

    private ActionInvoker(((Type, string)[] LocalParameters, Delegate Action) values)
    {
      action = values.Action;
      localParameters = values.LocalParameters;
    }

    private static ((Type, string)[], Delegate) GetValues(Delegate action)
    {
      (Type, string)[] localParameters = action?.Method.GetParameterTypeAndNames() ??
        throw new NullReferenceException();
      return (localParameters, action);
    }

    public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo)
    {
      action.DynamicInvoke(masterMethodInfo.GetLocalParameters(parameters, localParameters));
      return true;
    }
  }
}