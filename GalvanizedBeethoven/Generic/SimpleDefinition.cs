using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Test.AutoCompileTests.Tooling
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
