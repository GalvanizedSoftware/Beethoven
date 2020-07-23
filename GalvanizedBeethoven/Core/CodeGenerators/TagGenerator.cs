using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  internal class TagGenerator
  {
    private static readonly HashSet<int> generated = new HashSet<int>();
    private readonly string hashCodeText;

    public TagGenerator(object master)
    {
      int hashCode = master.GetHashCode();
      while (generated.Contains(hashCode))
        hashCode = unchecked(++hashCode).GetHashCode();
      generated.Add(hashCode);
      hashCodeText = hashCode.ToString("X08", CultureInfo.InvariantCulture);
    }

    public override string ToString() => hashCodeText;
  }
}