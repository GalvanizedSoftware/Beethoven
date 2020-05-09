using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  internal class NotImplementedPropertyInvoker<T> : IPropertyInvoker<T>
  {
    public T InvokeGet(object _) => throw new MissingMethodException();

    public void InvokeSet(object _, T __) => throw new MissingMethodException();
  }
}