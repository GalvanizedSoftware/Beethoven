using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class GenericMethodDefinition : MethodDefinition
  {
    private readonly MethodDefinition[] definitions;

    public GenericMethodDefinition(MethodInfo methodInfo, MethodDefinition[] definitions) :
      base(methodInfo.Name, new MatchMethodInfoExact(methodInfo))
    {
      this.definitions = definitions;
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments, MethodInfo methodInfo)
    {
      MethodInfo actualMethodInfo = methodInfo.MakeGenericMethod(genericArguments);
      MethodDefinition match = definitions
        .FirstOrDefault(definition => definition.MethodMatcher.IsNonGenericMatch(actualMethodInfo)) ??
        throw new MissingMethodException();
      match.Invoke(localInstance, ref returnValue, parameters, Array.Empty<Type>(), actualMethodInfo);
    }
  }
}