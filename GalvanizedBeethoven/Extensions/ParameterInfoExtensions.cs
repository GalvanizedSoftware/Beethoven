using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class ParameterInfoExtensions
  {
    internal static bool IsOutOrRef(this ParameterInfo parameterInfo) =>
      parameterInfo.IsOut || parameterInfo.ParameterType.IsByRef;
  }
}
