using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Generic.ValueLookup
{
  public interface IValueLookup
  {
    IEnumerable<T> Lookup<T>(string name);
  }
}
