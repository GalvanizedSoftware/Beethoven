using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class ParameterTypeAndNamesExtensions
  {
    internal static IEnumerable<(Type, string)> AppendReturnValue(this IEnumerable<(Type, string)> parameterTypeAndNames, Type returnType)
    {
      if (returnType == null)
        throw new NullReferenceException();
      return (returnType == typeof(void) ?
        parameterTypeAndNames :
        parameterTypeAndNames.Append((returnType.MakeByRefType(), "returnValue")));
    }
  }
}
