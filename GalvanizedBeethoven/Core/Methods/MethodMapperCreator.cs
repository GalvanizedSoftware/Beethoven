using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodMapperCreator<TMain, TChild> : IDefinitions where TMain : class
  {
    private readonly List<LinkedMappedMethod> methods;

    public MethodMapperCreator(Func<TMain, TChild> creatorFunc)
    {
      methods = new List<LinkedMappedMethod>(typeof(TChild)
          .GetNotSpecialMethods()
          .Select(methodInfo => new LinkedMappedMethod(methodInfo, (target) => creatorFunc(target as TMain))));
    }

    public IEnumerable<IDefinition> GetDefinitions<T>() where T : class => methods;
  }
}