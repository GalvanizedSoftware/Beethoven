using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extentions
{
  public static class MethodInfoExtentions
  {
    public static IEnumerable<Type> GetParameterTypes(this MethodInfo methodInfo)
    {
      return methodInfo.GetParameters().Select(info => info.ParameterType);
    }
  }
}
