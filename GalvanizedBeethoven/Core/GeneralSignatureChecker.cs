using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class GeneralSignatureChecker
  {
    private readonly MethodInfo[] interfaceMethods;
    private readonly MethodInfo[] classMethods;

    public GeneralSignatureChecker(Type interfaceType, Type classType)
    {
      interfaceMethods = interfaceType.GetAllMethodsAndInherited().ToArray();
      classMethods = classType.GetAllMethodsAndInherited().ToArray();
    }

    public IEnumerable<MethodInfo> FindMissing()
    {
      return interfaceMethods.Except(classMethods, new ExactMethodComparer());
    }
  }
}
