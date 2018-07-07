using System.Reflection;
using static System.Reflection.BindingFlags;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal static class Constants
  {
    public const BindingFlags ResolveFlags = Instance | Public | NonPublic;
    public const BindingFlags StaticResolveFlags = Static | Public | NonPublic;
  }
}
