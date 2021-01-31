using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  internal class RealMethodInvokerFactory : IMethodInvokerFactory
  {
    private readonly MethodInfo methodInfo;
    private readonly MethodFieldInvoker methodFieldInvoker;

    public RealMethodInvokerFactory(MethodInfo methodInfo, MethodFieldInvoker methodFieldInvoker)
    {
      this.methodInfo = methodInfo;
      this.methodFieldInvoker = methodFieldInvoker;
    }

    public MethodInvokerInstance Create(object master) => 
	    new(master, methodInfo, methodFieldInvoker);
  }
}