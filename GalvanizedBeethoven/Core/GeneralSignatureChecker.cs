using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;

namespace GalvanizedSoftware.Beethoven.Core
{
  public class GeneralSignatureChecker<TInterface, TClass>
  {
    private readonly MethodInfo[] interfaceMethods;
    private readonly MethodInfo[] classMethods;

    public GeneralSignatureChecker()
    {
      Type interfaceType = typeof(TInterface);
      interfaceMethods = interfaceType.GetAllMethodsAndInherited().ToArray();
      classMethods = typeof(TClass).GetAllMethodsAndInherited().ToArray();
    }

    public IEnumerable<MethodInfo> FindMissing()
    {
      IEnumerable<MethodInfo> findMissing = interfaceMethods.Except(classMethods, new EquivalentMethodComparer()).ToArray();
      return findMissing;
    }
  }
}
