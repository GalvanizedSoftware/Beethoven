using GalvanizedSoftware.Beethoven.Test.AutoCompileTests.Tooling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.AutoCompileTests
{
  [TestClass]
  public class AutoCompileTests
  {

    [TestMethod]
    public void TestMethodAutoCompileTest1()
    {
      AutoFactories autoFactories = AutoFactories.CreateFactories(GetType().Assembly);
      TypeDefinition<ITestProperties1> definition = autoFactories.CreateTypeDefinition<ITestProperties1>();
      ITestProperties1 test = definition
        .Compile()
        .Create();
      test.Property2 = "sdgsdg3";
      Assert.AreEqual(5, test.Property1);
      Assert.AreEqual("sdgsdg3", test.Property2);
    }

    [TestMethod]
    public void TestMethodAutoCompileTest2()
    {
      AutoFactories autoFactories = AutoFactories.CreateFactories(GetType().Assembly);
      TypeDefinition<ITestProperties2> definition = autoFactories.CreateTypeDefinition<ITestProperties2>();
      ITestProperties2 test = definition
        .Compile()
        .Create();
      test.Property2 = "sdgsdg3";
      Assert.AreEqual(5, test.Property1);
      Assert.AreEqual("sdgsdg3", test.Property2);
    }

    [TestMethod]
    public void TestMethodAutoCompileTest3()
    {
      AutoFactories autoFactories = AutoFactories.CreateFactories(GetType().Assembly);
      TypeDefinition<ITestProperties3> definition = autoFactories.CreateTypeDefinition<ITestProperties3>();
      ITestProperties3 test = definition
        .Compile()
        .Create();
      test.Property2 = "sdgsdg3";
      Assert.AreEqual(5, test.Property1);
      Assert.AreEqual("sdgsdg3", test.Property2);
    }
  }
}
