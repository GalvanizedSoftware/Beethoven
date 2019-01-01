using System;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class ParameterTypeAndNamesExtensions
  {
    internal static (Type, string)[] AppendReturnValue(this (Type, string)[] parameterTypeAndNames, Type returnType)
    {
      return returnType == typeof(void) ?
        parameterTypeAndNames :
        parameterTypeAndNames.Append((returnType.MakeByRefType(), "returnValue")).ToArray();
    }
  }
}
