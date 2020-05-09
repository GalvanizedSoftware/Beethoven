using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class MethodInvoker : IMethodInvoker
  {
    private readonly IMethodInvoker masterInvoker;

    public MethodInvoker(string uniqueName)
    {
      masterInvoker = InvokerList.GetInvoker(uniqueName) as IMethodInvoker ??
        new NotImplementedMethodInvoker();
    }

    public object Invoke(object master, Type[] genericTypes, object[] parameters) =>
      masterInvoker.Invoke(master, genericTypes, parameters);
  }
}
