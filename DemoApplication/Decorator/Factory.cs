using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extentions;

namespace GalvanizedSoftware.Beethoven.DemoApp.Decorator
{
  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public IOrderedItem AddGiftWrapping(IOrderedItem mainItem)
    {
      return beethovenFactory.Generate<IOrderedItem>(
        new GiftWrapping(mainItem), // It's not implementing Name, but why should it do that?
        new Property<string>(nameof(IOrderedItem.Name)).MappedFrom(mainItem)
      );
    }
  }
}
