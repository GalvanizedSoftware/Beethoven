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
    private readonly List<MappedMethod> methods;

    public MethodMapperCreator(Func<TMain, TChild> creatorFunc)
    {
      this.creatorFunc = creatorFunc;
      methods = new List<MappedMethod>(typeof(TChild)
          .GetNotSpecialMethods()
          .Select(methodInfo => new MappedMethod(methodInfo)));
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
      foreach (MappedMethod method in methods)
        method.Instance = methodInstance;
    }
  }
}