using GalvanizedSoftware.Beethoven.Core.Methods;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  internal class RealMethodInvokerFactory : IMethodInvokerFactory
  {
    private readonly MethodInfo methodInfo;
    private readonly MethodDefinition methodDefinition;

    public RealMethodInvokerFactory(MethodInfo methodInfo, MethodDefinition methodDefinition)
    {
      this.methodInfo = methodInfo;
      this.methodDefinition = methodDefinition;
    }

    public MethodInvokerInstance CreateInstance(object master) =>
      new(master, methodInfo, methodDefinition);
  }
}