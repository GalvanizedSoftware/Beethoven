using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests.EqualsGetHashImport
{
	internal class Factory
	{
		private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

		public IValueHolder Create(string name, int value, byte[] data)
		{
			IValueHolder valueHolder = beethovenFactory.Generate<IValueHolder>(
				new DefaultProperty()
					.InitialValue(new byte[0])
					.SetterGetter(),
				LinkedMappedMethods.Create(ComparerCreator));
			if (name != null)
				valueHolder.Name = name;
			valueHolder.Value = value;
			if (data != null)
				valueHolder.Data = data;
			return valueHolder;
		}

		private static IEqualsGetHash ComparerCreator(object instance) =>
			new EqualsGetHash<IValueHolder>(ValuesGetterFunc, (IValueHolder)instance);

		private static IEnumerable<object> ValuesGetterFunc(IValueHolder arg)
		{
			yield return arg.Name;
			yield return arg.Value;
			foreach (byte b in arg.Data)
				yield return b;
		}
	}
}
