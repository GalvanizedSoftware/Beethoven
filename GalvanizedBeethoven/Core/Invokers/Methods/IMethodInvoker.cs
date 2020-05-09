using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public interface IMethodInvoker
  {
    public object Invoke(object master, Type[] genericTypes, object[] parameters);
  }
}