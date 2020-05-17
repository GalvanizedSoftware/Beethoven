using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodMapperCreator<TMain, TChild> : IDefinitions where TMain : class
  {
    private readonly Func<TMain, TChild> creatorFunc;
    private readonly List<MappedMethodDelayed> methods;

    public MethodMapperCreator(Func<TMain, TChild> creatorFunc)
    {
      this.creatorFunc = creatorFunc;
      methods = new List<MappedMethodDelayed>(typeof(TChild)
          .GetNotSpecialMethods()
          .Select(methodInfo => new MappedMethodDelayed(methodInfo, (target) => creatorFunc(target as TMain))));
    }

    public IEnumerable<IDefinition> GetDefinitions() => methods;
  }
}