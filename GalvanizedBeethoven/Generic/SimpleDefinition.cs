using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class SimpleDefinition<T> : IFactoryDefinition<T> where T : class
  {
    public SimpleDefinition(IEnumerable<object> partDefinitions, 
      string className = null, string classNamespace = null)
    {
      PartDefinitions = partDefinitions;
      ClassName = className;
      Namespace = classNamespace;
    }

    public string Namespace { get; }

    public string ClassName { get; }

    public IEnumerable<object> PartDefinitions { get; }
  }
}
