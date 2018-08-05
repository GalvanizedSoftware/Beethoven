using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.DemoApp.EqualsGetHashImport
{
  class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public IValueHolder Create(string name, int value, byte[] data)
    {
      IValueHolder valueHolder = beethovenFactory.Generate<IValueHolder>(
        new DefaultProperty()
          .InitialValue(new byte[0])
          .SetterGetter(),
        new EqualsGetHash<IValueHolder>(ValuesGetterFunc));
      if (name != null)
        valueHolder.Name = name;
      valueHolder.Value = value;
      if (data != null)
        valueHolder.Data = data;
      return valueHolder;
    }

    private static IEnumerable<object> ValuesGetterFunc(IValueHolder arg)
    {
      yield return arg.Name;
      yield return arg.Value;
      foreach (byte b in arg.Data)
        yield return b;
    }
  }
}
