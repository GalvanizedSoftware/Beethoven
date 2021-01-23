namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class MethodInvoker : IMethodInvoker
  {
    private readonly IMethodInvoker masterInvoker;

    public MethodInvoker(string uniqueName)
    {
      masterInvoker = InvokerList.CreateInvoker(uniqueName) as IMethodInvoker;
    }

    public MethodInvokerInstance CreateInstance(object master) =>
      masterInvoker.CreateInstance(master);
  }
}