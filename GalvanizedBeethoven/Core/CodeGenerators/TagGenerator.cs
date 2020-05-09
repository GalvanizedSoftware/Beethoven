using System;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  internal class TagGenerator
  {
    private static readonly Random random = new Random();

    public override string ToString() => $"{random.Next(0, int.MaxValue):x08}";
  }
}