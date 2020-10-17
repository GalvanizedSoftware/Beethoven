using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public interface IMethodInvokerInstance
  {
     public object Invoke(Type[] genericTypes, object[] parameters);
  }
}