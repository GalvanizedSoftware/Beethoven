using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class PartialMatchAction : Method
  {
    private readonly Delegate action;
    private readonly (Type, string)[] localParameters;
    private readonly int? parameterIndex;

    public PartialMatchAction(string mainName, IParameter parameter, Delegate action) :
      base(mainName, new MatchLambdaPartiallyNoReturn(), parameter)
    {
      this.action = action;
      localParameters = action.Method.GetParameterTypeAndNames();
      parameterIndex = localParameters
        .Select((tuple, i) => (tuple, (int?)i))
        .FirstOrDefault(outerTuple => parameter?.Equals(outerTuple.tuple) == true)
        .Item2;
    }

    public override void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(masterMethodInfo.ReturnType)
        .ToArray();
      object[] localParameterValues = localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .Select(index => index < 0 ? null : parameters[index])
        .ToArray();
      if (parameterIndex != null)
        localParameterValues[parameterIndex.Value] = localInstance;
      action.DynamicInvoke(localParameterValues);
    }
  }
}