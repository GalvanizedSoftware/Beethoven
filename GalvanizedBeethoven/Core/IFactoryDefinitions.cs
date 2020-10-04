using System;

namespace GalvanizedSoftware.Beethoven.Core
{
  public interface IFactoryDefinitions
  {
    Func<IFactoryDefinition<T>> GetFactoryCreator<T>();
  }
}