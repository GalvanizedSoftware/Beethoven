using FakeItEasy;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Events;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Test.FieldTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GalvanizedSoftware.Beethoven.Test.FieldTests
{
  [TestClass]
  public class ConstructorInitializedFieldTests
  {

    [TestMethod]
    public void ConstructorInitializedFieldInitialValueTest1()
    {
      CompiledTypeDefinition<ITestProperties> definition = new TypeDefinition<ITestProperties>()
          .Add(new PropertyDefinition<int>(nameof(ITestProperties.Property1))
            .ConstructorParameter()
            .SetterGetter())
        .Compile();
      ITestProperties test = definition.Create(5);
      Assert.AreEqual(5, test.Property1);
    }

    [TestMethod]
    public void ConstructorInitializedFieldInitialValueTest2()
    {
      CompiledTypeDefinition<ITestProperties> definition = new TypeDefinition<ITestProperties>()
          .Add(new PropertyDefinition<int>(nameof(ITestProperties.Property1))
            .ConstructorParameter()
            .SetterGetter())
        .Compile();
      ITestProperties test = definition.Create(17);
      test.Property1 = 50;
      Assert.AreEqual(50, test.Property1);
    }

    [TestMethod]
    public void ConstructorInitializedFieldInitialValueTest3()
    {
      CompiledTypeDefinition<ITestProperties> compiledTypeDefinition = new TypeDefinition<ITestProperties>()
          .Add(new PropertyDefinition<int>(nameof(ITestProperties.Property1))
            .ConstructorParameter()
            .SetterGetter())
        .Compile();
      ITestProperties test1 = compiledTypeDefinition.Create(5);
      ITestProperties test2 = compiledTypeDefinition.Create(4);
      //Assert.AreEqual(5, test1.Property1);
      //Assert.AreEqual(4, test2.Property1);
    }

    [TestMethod]
    public void ConstructorInitializedFieldMethodSimple()
    {
      ITestMethods fake = A.Fake<ITestMethods>();
      BeethovenFactory factory = new BeethovenFactory();
      ITestMethods test = factory.Generate<ITestMethods>(fake);
      test.Simple();
      A.CallTo(() => fake.Simple()).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void ConstructorInitializedFieldEventWithReturnValue1()
    {
      CompiledTypeDefinition<ITestEvents> compiledTypeDefinition = new TypeDefinition<ITestEvents>().Compile();
      ITestEvents test = compiledTypeDefinition.Create();
      Action<string> trigger =
        new EventTrigger(test, nameof(ITestEvents.WithReturnValue)).ToAction<string>();
      bool withReturnValueEventCalled = false;
      bool otherEventCalled = false;
      test.Simple += delegate { otherEventCalled = true; };
      test.WithParameters += delegate { otherEventCalled = true; };
      test.WithReturnValue += delegate { return withReturnValueEventCalled = true; };
      trigger("");
      Assert.IsTrue(withReturnValueEventCalled);
      Assert.IsFalse(otherEventCalled);
    }
  }
}
