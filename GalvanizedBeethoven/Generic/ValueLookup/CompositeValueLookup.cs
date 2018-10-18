using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Generic.ValueLookup
{
  public class CompositeValueLookup : IValueLookup
  {
    private readonly IValueLookup[] valueLookups;

    public CompositeValueLookup(params IValueLookup[] valueLookups)
    {
      this.valueLookups = valueLookups;
    }

    public IEnumerable<T> Lookup<T>(string name)
    {
      return valueLookups.SelectMany(lookup => lookup.Lookup<T>(name));
    }
  }
}
