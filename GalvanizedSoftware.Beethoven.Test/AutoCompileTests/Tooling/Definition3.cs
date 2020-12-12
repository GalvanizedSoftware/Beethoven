using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Test.AutoCompileTests.Tooling
{
  internal static class Definition3
  {
    private static readonly object[] partDefinitions =
        { new PropertyDefinition<int>
          (nameof(ITestProperties2.Property1))
          .Constant(5),
          new PropertyDefinition<string>
          (nameof(ITestProperties2.Property2))
          .SetterGetter()
      };

    [Factory]
    public static IFactoryDefinition<ITestProperties3> CreateDefinition() =>
      new SimpleDefinition<ITestProperties3>(partDefinitions);
  }
}
