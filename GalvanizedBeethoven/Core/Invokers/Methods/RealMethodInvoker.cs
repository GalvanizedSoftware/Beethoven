using GalvanizedSoftware.Beethoven.Core.Methods;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  internal class RealMethodInvoker : IMethodInvoker
  {
    private readonly MethodInfo methodInfo;
    private readonly MethodDefinition methodDefinition;

    public RealMethodInvoker(MethodInfo methodInfo, MethodDefinition methodDefinition)
    {
      this.methodInfo = methodInfo;
      this.methodDefinition = methodDefinition;
    }

    public IMethodInvokerInstance CreateInstance(object master) =>
      new MethodInvokerInstance(master, methodInfo, methodDefinition);
  }
}