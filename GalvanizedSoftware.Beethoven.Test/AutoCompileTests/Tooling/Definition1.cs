using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Test.AutoCompileTests.Tooling
{
  internal class Definition1 : IFactoryDefinition<ITestProperties1>
  {
    [Factory]
    public Definition1()
    {
    }

    public string Namespace => null;

    public string ClassName => null;

    public IEnumerable<object> PartDefinitions
    {
      get
      {
        yield return new PropertyDefinition<int>
          (nameof(ITestProperties1.Property1))
          .Constant(5);
        yield return new PropertyDefinition<string>
          (nameof(ITestProperties1.Property2))
          .SetterGetter();
      }
    }
  }
}
