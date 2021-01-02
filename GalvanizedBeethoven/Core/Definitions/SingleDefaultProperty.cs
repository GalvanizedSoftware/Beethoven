using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  public class SingleDefaultProperty<T> : PropertyDefinition<T>
  {
    public static PropertyDefinition<T> CreateInstance(string name) => 
      new SingleDefaultProperty<T>(name);

    private SingleDefaultProperty(string name) : 
      base(name)
    {
    }

    public override int SortOrder => 2;
  }
}
