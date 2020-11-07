using System;

namespace GalvanizedSoftware.Beethoven
{
  [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
  public class FactoryAttribute : Attribute
  {
  }
}
