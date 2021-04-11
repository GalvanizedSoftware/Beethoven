namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public interface IMethodInvokerFactory
  {
    MethodInvokerInstance Create(object master);
  }
}