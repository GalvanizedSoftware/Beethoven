using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Test.AutoCompileTests.Tooling
{
  internal class Definition2 : SimpleDefinition<ITestProperties2>
  {
    private static readonly object[] partDefinitions =
    { 
      new PropertyDefinition<int>
        (nameof(ITestProperties2.Property1))
        .Constant(5),
      new PropertyDefinition<string>
        (nameof(ITestProperties2.Property2))
        .SetterGetter()
    };

    [Factory]
    public Definition2() : base(partDefinitions)
    {
    }
  }
}
