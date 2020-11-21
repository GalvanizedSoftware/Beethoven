namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  internal class NotImplementedMethodInvoker : IMethodInvoker
  {
    public IMethodInvokerInstance CreateInstance(object master) =>
      new NotImplementedMethodInvokerInstace();
  }
}