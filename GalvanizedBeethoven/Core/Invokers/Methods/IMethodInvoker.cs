namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public interface IMethodInvoker
  {
    MethodInvokerInstance CreateInstance(object master);
  }
}