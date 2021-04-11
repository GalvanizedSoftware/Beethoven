using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IDefinitions
  {
    IEnumerable<IDefinition> GetDefinitions<T>() where T : class;
  }
}