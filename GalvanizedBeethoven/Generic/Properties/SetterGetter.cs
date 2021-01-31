using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class SetterGetter<T> : IPropertyDefinition<T>
  {
    public IPropertyInstance<T> Create(object master) =>
      new SetterGetterInstance<T>();
  }
}