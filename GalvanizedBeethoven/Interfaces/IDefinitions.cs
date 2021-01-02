using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IDefinitions
  {
    IEnumerable<IDefinition> GetDefinitions<TInterface>() where TInterface : class;
  }
}