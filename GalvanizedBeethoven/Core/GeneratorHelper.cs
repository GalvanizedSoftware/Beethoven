using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core
{
  public static class GeneratorHelper
  {
    internal static readonly string InvokerTypeName = typeof(RuntimeInvokerFactory).GetFullName();
    internal const string InstanceListName = "instanceList";
  }
}
