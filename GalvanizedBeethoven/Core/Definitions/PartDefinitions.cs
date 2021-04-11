using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  internal class PartDefinitions : IEnumerable<object>
  {
    private readonly object[] partDefinitions;

    internal PartDefinitions(IEnumerable<object> newPartDefinitions)
    {
      partDefinitions = newPartDefinitions.ToArray();
    }

    internal PartDefinitions Concat(params object[] concatPartDefinitions) =>
      new(partDefinitions.Concat(concatPartDefinitions));

    public PartDefinitions Set<T>(T definition) where T : class =>
      new(partDefinitions
        .Where(o => o is not T)
        .Append(definition));

    public IEnumerator<object> GetEnumerator() => 
      ((IEnumerable<object>) partDefinitions).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
      GetEnumerator();
  }
}
