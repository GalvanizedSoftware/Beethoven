using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests.EqualsGetHashImport
{
	[TestClass]
	public class EqualsGetHashImportTests
	{
		[TestMethod]
		public void EqualsGetHashImportTest1()
		{
			Factory factory = new();
			IValueHolder value1 = factory.Create("name", 1, new byte[] {1, 2, 3});
			IValueHolder value2 = factory.Create("name", 1, new byte[] {1, 2, 3});
			EqualsGetHashComparer comparer = new();
			bool condition = comparer.Equals(value1, value2);
			Assert.IsTrue(condition);
		}
	}
}
