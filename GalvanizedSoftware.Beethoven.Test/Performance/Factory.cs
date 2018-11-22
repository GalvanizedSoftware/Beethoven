using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Test.Performance
{
  internal class Factory
  {
    private readonly BeethovenFactory factory = new BeethovenFactory();

    public IPerformanceTest Create()
    {
      FactoryHelper<IPerformanceTest> factoryHelper = new FactoryHelper<IPerformanceTest>();
      return factory.Generate<IPerformanceTest>(
        new DefaultProperty()
          .SkipIfEqual()
          .SetterGetter()
          .NotifyChanged(),
        factoryHelper.MethodMapper(main => new FormatClass(main)));
    }

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