namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal abstract class PropertyInterceptor
  {
    internal Property Property { get; }

    public PropertyInterceptor(Property property)
    {
      Property = property;
    }
  }
}