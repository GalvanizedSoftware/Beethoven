using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Interfaces
{
  // ReSharper disable once UnusedTypeParameter
  interface IFactoryDefinition<T> where T : class
  {
    string Namespace { get; }
    string ClassName { get; }

    IEnumerable<object> PartDefinitions { get; }
  }
}
