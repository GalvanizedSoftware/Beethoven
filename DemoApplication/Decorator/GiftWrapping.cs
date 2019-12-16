// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace GalvanizedSoftware.Beethoven.DemoApp.Decorator
{
  internal class GiftWrapping// It's not implementing Name, but why should it do that?
  {
    public GiftWrapping(IOrderedItem mainItem)
    {
      Price = mainItem.Price + (mainItem.Price > 1000 ? 0 : 10); // Anything costing more than 1000 comes with free gift wrapping
      Weight = mainItem.Weight + 0.1; // Might be important for shipping cost estimation
    }

    public double Price { get; }

    public double Weight { get; }
  }
}
