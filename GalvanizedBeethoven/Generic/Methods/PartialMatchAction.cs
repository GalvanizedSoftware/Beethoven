using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class PartialMatchAction<T> : Method
  {
    private readonly Delegate action;
    private readonly (Type, string)[] localParameters;

    public PartialMatchAction(string mainName, ConstructorParameter parameter, Delegate action) :
      base(mainName, new MatchLambdaPartiallyNoReturn(), parameter)
    {
      this.action = action;
      localParameters = new (Type, string)[0];
    }

    public PartialMatchAction(string mainName, Delegate action) :
      base(mainName, new MatchLambdaPartiallyNoReturn(), new ConstructorParameter(action.GetFirstParameterName(), typeof(T)))
    {
      this.action = action;
      localParameters = new (Type, string)[0];
    }

    public override void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(masterMethodInfo.ReturnType)
        .ToArray();
      int[] indexes = localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .ToArray();
      object[] localParameterValues = indexes
        .Select(index => parameters[index])
        .ToArray();
      action.DynamicInvoke(localParameterValues.Prepend(localInstance).ToArray());
    }
  }
}