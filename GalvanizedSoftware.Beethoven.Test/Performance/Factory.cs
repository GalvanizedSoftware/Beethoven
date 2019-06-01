using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Test.Performance
{
  internal class Factory
  {
    private readonly TypeDefinition<IPerformanceTest> typeDefinition;

    public Factory()
    {
      typeDefinition = new TypeDefinition<IPerformanceTest>
      (
        new DefaultProperty()
          .SkipIfEqual()
          .SetterGetter()
          .NotifyChanged()
      )
      .AddMethodMapper(main => new FormatClass(main));
    }

    public IPerformanceTest Create() =>
      typeDefinition
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