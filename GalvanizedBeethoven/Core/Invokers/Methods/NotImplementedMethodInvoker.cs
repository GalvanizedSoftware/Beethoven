using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  internal class NotImplementedMethodInvoker : IMethodInvoker
  {
    public object Invoke(object master, Type[] genericTypes, object[] parameters) => 
      throw new MissingMethodException();
  }
}