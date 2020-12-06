using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Test.Performance
{
  internal class Factory
  {
    private readonly CompiledTypeDefinition<IPerformanceTest> compiledTypeDefinition;

    public Factory()
    {
      compiledTypeDefinition = TypeDefinition<IPerformanceTest>.Create(new DefaultProperty()
          .SkipIfEqual()
          .SetterGetter()
          .NotifyChanged())
      .AddMethodMapper(main => new FormatClass(main))
      .Compile();
    }

    public IPerformanceTest Create() =>
      compiledTypeDefinition.Create();

    private class FormatClass
    {
      private readonly IPerformanceTest main;

      public FormatClass(IPerformanceTest main)
      {
        this.main = main;
      }

      // ReSharper disable once UnusedMember.Local
      internal string Format(string format)
      {
        return string.Format(format, main.Name);
      }
    }
  }

}