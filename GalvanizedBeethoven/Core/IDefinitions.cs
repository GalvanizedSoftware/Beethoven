using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
  public interface IDefinitions
  {
    IEnumerable<IDefinition> GetDefinitions();
  }
}