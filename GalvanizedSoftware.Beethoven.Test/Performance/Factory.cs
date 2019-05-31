using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Test.Performance
{
  internal class Factory
  {
    private readonly BeethovenFactory factory = new BeethovenFactory();

    public IPerformanceTest Create() =>
      new TypeDefinition<IPerformanceTest>
      (
        new DefaultProperty()
          .SkipIfEqual()
          .SetterGetter()
          .NotifyChanged()
      )
      .AddMethodMapper(main => new FormatClass(main))
      .Create();

    private class FormatClass
    {
      private readonly IPerformanceTest main;

      public FormatClass(IPerformanceTest main)
      {
        this.main = main;
      }

      internal string Format(string format)
      {
        return string.Format(format, main.Name);
      }
    }
  }

}