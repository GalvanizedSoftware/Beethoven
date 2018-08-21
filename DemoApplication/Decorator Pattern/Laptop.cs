namespace GalvanizedSoftware.Beethoven.DemoApp.Decorator_Pattern
{
  internal class Laptop : IOrderedItem
  {
    public string Name { get; } = "Nice Laptop";
    public double Price { get; } = 2000;
    public double Weight { get; } = 4;
  }
}
