using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  internal class NotImplementedMethodInvokerInstace : IMethodInvokerInstance
  {
    public object Invoke(Type[] _, object[] __) => 
      throw new MissingMethodException();
  }
}