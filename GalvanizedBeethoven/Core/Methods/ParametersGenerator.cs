using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class ParametersGenerator
  {
    private readonly ParameterInfo[] parameters;
    private readonly string parametersName;

    public ParametersGenerator(ParameterInfo[] parameters, string parametersName)
    {
      this.parameters = parameters;
      this.parametersName = parametersName;
    }

    internal string GeneratePreInvoke() =>
      $"object[] {parametersName} = " +
      $"new object[]{{{string.Join(", ", parameters.Select(GetInField))}}};";

    internal IEnumerable<string> GeneratePostInvoke() => parameters
        .Select((parameterInfo, i) => (parameterInfo, i))
        .Where(tuple => tuple.parameterInfo.IsOutOrRef())
        .Select(tuple => GeneratePostSetter(tuple.parameterInfo, tuple.i));

    private string GeneratePostSetter(ParameterInfo parameterInfo, int index)
    {
      string parameterName = parameterInfo.Name;
      string typeName = parameterInfo.ParameterType.GetFullName();
      return $"{parameterName} = ({typeName}){parametersName}[{index}];";
    }

    private static string GetInField(ParameterInfo info) =>
      info.IsOut ? $"default({info.ParameterType.GetFullName()})" : info.Name;
  }
}