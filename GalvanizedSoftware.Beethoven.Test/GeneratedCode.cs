using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests.Interfaces
{
  public class ITestPropertiesImplementation2 : ITestProperties, GalvanizedSoftware.Beethoven.Interfaces.IGeneratedClass
  {
    Implementations.MappedTestClass fieldMappedTestClass3D802AFC;

    public ITestPropertiesImplementation2()
    {
      RuntimeInvokerFactory runtimeInvokerFactory = new("ITestPropertiesImplementationfieldMappedTestClass1B6BE7C4Factory");
      fieldMappedTestClass3D802AFC = runtimeInvokerFactory.Create<Implementations.MappedTestClass>();
    }

    public int Property1
    {
      get => fieldMappedTestClass3D802AFC.Property1;
      set => fieldMappedTestClass3D802AFC.Property1 = value;
    }

    public string Property2
    {
      get => fieldMappedTestClass3D802AFC.Property2;
      set => fieldMappedTestClass3D802AFC.Property2 = value;
    }




    public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

    object GalvanizedSoftware.Beethoven.Interfaces.IGeneratedClass.NotifyEvent(string eventName, object[] values)
    {
      switch (eventName)
      {
        case "PropertyChanged": return PropertyChanged?.DynamicInvoke(values);
        default: return null;
      }
    }

  }
}