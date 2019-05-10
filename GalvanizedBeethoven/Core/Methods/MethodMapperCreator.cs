using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Methods;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public class MethodMapperCreator<TMain, TChild> : IEnumerable<Method>, IBindingParent where TMain : class
  {
    private readonly Func<TMain, TChild> creatorFunc;
    private readonly List<MappedMethodDelayed> methods;

    public MethodMapperCreator(Func<TMain, TChild> creatorFunc)
    {
      this.creatorFunc = creatorFunc;
      methods = new List<MappedMethodDelayed>(typeof(TChild)
          .GetNotSpecialMethods()
          .Select(methodInfo => new MappedMethodDelayed(methodInfo)));
    }

    public IEnumerator<Method> GetEnumerator()
    {
      return methods.OfType<Method>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Bind(object target)
    {
      object methodInstance = creatorFunc(target as TMain);
      foreach (MappedMethodDelayed method in methods)
        method.Instance = methodInstance;
    }
  }
}