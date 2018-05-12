using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Methods;

namespace GalvanizedSoftware.Beethoven.Core
{
  public class MethodMapperCreator<TMain, TChild> : IEnumerable<Method>, IBindingParent where TMain : class
  {
    private readonly Func<TMain, TChild> creatorFunc;
    private readonly List<MethodsWithInstance> methods;

    public MethodMapperCreator(Func<TMain, TChild> creatorFunc)
    {
      this.creatorFunc = creatorFunc;
      methods = new List<MethodsWithInstance>(
        from methodInfo in typeof(TChild).GetMethods()
        where !methodInfo.IsSpecialName
        select new MethodsWithInstance(methodInfo));
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
      foreach (MethodsWithInstance method in methods)
        method.Instance = methodInstance;
    }
  }
}