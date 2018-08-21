namespace GalvanizedSoftware.Beethoven.DemoApp.Decorator_Pattern
{
  internal class GiftWrapping
  {
    private readonly IOrderedItem mainItem;

    public GiftWrapping(IOrderedItem mainItem)
    {
      this.mainItem = mainItem;
    }

    public double Price => mainItem.Price + (mainItem.Price > 1000 ? 0 : 10); // Anything costing more than 1000 comes with free gift wrapping

    public double Weight => mainItem.Weight + 0.1; // Might be important for shipping cost estimation
  }
}
