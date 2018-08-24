namespace GalvanizedSoftware.Beethoven.DemoApp.DuckTyping
{
  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public IDisplayName CreateDisplayName(object instance)
    {
      return beethovenFactory.Generate<IDisplayName>(instance);
    }
  }
}
