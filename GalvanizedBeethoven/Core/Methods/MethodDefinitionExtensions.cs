using System;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public static class MethodDefinitionExtensions
  {
    public static MethodDefinition CreateFallback(this MethodDefinition methodDefinition) =>
      new FallbackMethodDefinition(methodDefinition ?? throw new NullReferenceException());
  }
}