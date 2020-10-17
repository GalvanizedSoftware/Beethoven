using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public interface IMethodInvoker
  {
    IMethodInvokerInstance CreateInstance(object master);
  }
}