namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public interface IMethodInvokerFactory
  {
    // ReSharper disable once UnusedMember.Global
    MethodInvokerInstance CreateInstance(object master);
  }
}