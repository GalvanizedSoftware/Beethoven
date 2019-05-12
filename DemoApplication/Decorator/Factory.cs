using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.DemoApp.Decorator
{
  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public IOrderedItem AddGiftWrapping(IOrderedItem mainItem) =>
      beethovenFactory.Generate<IOrderedItem>(
        new LinkedObjects(
          mainItem,
          new GiftWrapping(mainItem)
        ));
  }
}
