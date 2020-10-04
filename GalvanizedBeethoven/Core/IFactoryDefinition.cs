using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
  public interface IFactoryDefinition<TInterface>
  {
    string Namespace { get; }
    string ClassName { get; }
    IEnumerable<object> PartDefinitions { get; }
  }
}
