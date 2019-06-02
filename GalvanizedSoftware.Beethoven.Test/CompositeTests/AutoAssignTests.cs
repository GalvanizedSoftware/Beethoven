using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Generic.ValueLookup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
using Unity;

namespace GalvanizedSoftware.Beethoven.Test.CompositeTests
{
  [TestClass]
  public class AutoAssignTests
  {
    [TestMethod]
    public void AutoAssignTest1()
    {
      Dictionary<string, object> defaultValues = new Dictionary<string, object>
      {
        { "Name", "The evil company"},
        { "Address","2460 Sunshine road"}
      };
      BeethovenFactory factory = new BeethovenFactory();
      IValueLookup lookup = new CompositeValueLookup(
        new DictionaryValueLookup(defaultValues),
        new InterfaceFactoryValueLookup((type, name) => factory.Generate(type)));
      factory.GeneralPartDefinitions = new object[]
      {
        new DefaultProperty()
        .ValueLookup(lookup)
        .SetterGetter()
      };
      ICompany company = factory.Generate<ICompany>();
      Assert.AreEqual("The evil company", company.Information.Name);
      Assert.AreEqual("2460 Sunshine road", company.Information.Address);
    }

    [TestMethod]
    public void AutoAssignTest2()
    {
      var defaultValues = new
      {
        Name = "The evil company",
        Address = "2460 Sunshine road"
      };
      BeethovenFactory factory = new BeethovenFactory();
      IValueLookup lookup = new CompositeValueLookup(
        new AnonymousValueLookup(defaultValues),
        new InterfaceFactoryValueLookup((type, name) => factory.Generate(type)));
      factory.GeneralPartDefinitions = new object[]
      {
        new DefaultProperty()
          .ValueLookup(lookup)
          .SetterGetter()
      };
      ICompany company = factory.Generate<ICompany>();
      Assert.AreEqual("The evil company", company.Information.Name);
      Assert.AreEqual("2460 Sunshine road", company.Information.Address);
    }

    [TestMethod]
    public void AutoAssignTest3()
    {
      BeethovenFactory factory = new BeethovenFactory();
      var defaultValues1 = new
      {
        Name = "The evil company",
        Address = "2460 Sunshine road"
      };
      DefaultProperty defaultProperty1 = new DefaultProperty()
        .AnonymousValueLookup(defaultValues1)
        .SetterGetter();
      ICompanyInformation companyInformation = factory.Generate<ICompanyInformation>(defaultProperty1);
      Assert.AreEqual("The evil company", companyInformation.Name);
      Assert.AreEqual("2460 Sunshine road", companyInformation.Address);
      var defaultValues2 = new
      {
        Name = "",
      };
      DefaultProperty defaultProperty2 = new DefaultProperty()
        .AnonymousValueLookup(defaultValues2)
        .SetterGetter();
      companyInformation = factory.Generate<ICompanyInformation>(defaultProperty2);
      Assert.AreEqual("", companyInformation.Name);
      Assert.AreEqual(null, companyInformation.Address);
    }

    [TestMethod]
    public void AutoAssignTest4()
    {
      Dictionary<string, object> defaultValues = new Dictionary<string, object>
      {
        { "Name", "The evil company"},
        { "Address",2460}
      };
      BeethovenFactory factory = new BeethovenFactory();
      IValueLookup lookup = new CompositeValueLookup(
        new DictionaryValueLookup(defaultValues),
        new InterfaceFactoryValueLookup((type, name) => factory.Generate(type)));
      factory.GeneralPartDefinitions = new object[]
      {
        new DefaultProperty()
          .ValueLookup(lookup)
          .SetterGetter()
      };
      ICompany company = factory.Generate<ICompany>();
      Assert.AreEqual("The evil company", company.Information.Name);
      Assert.AreEqual(null, company.Information.Address);
    }

    [TestMethod]
    public void AutoAssignTest5()
    {
      IParameter nameParameter = ConstructorParameter.Create<string>("Name");
      IParameter addressParameter = ConstructorParameter.Create<string>("Address");
      TypeDefinition<ICompanyInformation> typeDefinition = new TypeDefinition<ICompanyInformation>(
        nameParameter,
        addressParameter,
        new Property<string>("Name")
          .InitialValue(nameParameter)
          .SetterGetter(),
        new Property<string>("Address")
          .InitialValue(addressParameter)
          .SetterGetter()
        );
      ICompanyInformation companyInformation = 
        typeDefinition.Create("The evil company", "2460 Sunshine road");
      Assert.AreEqual("The evil company", companyInformation.Name);
      Assert.AreEqual("2460 Sunshine road", companyInformation.Address);
    }

    [TestMethod]
    public void AutoAssignTest6()
    {
      IParameter nameParameter = ConstructorParameter.Create<string>("Name");
      TypeDefinition<ICompanyInformation> typeDefinition = new TypeDefinition<ICompanyInformation>(
        nameParameter,
        new Property<string>("Name")
          .InitialValue(nameParameter)
          .SetterGetter()      );
      ICompanyInformation companyInformation =
        typeDefinition.Create("The evil company");
      companyInformation.Name = "Generic Company B";
      Assert.AreEqual("Generic Company B", companyInformation.Name);
    }

    [TestMethod]
    public void AutoAssignTestIoc()
    {
      UnityContainer unityContainer = new UnityContainer();
      unityContainer.RegisterInstance("informationName", "The evil company");
      unityContainer.RegisterInstance("informationAddress", "2460 Sunshine road");
      unityContainer.Resolve<IocFactory>();
      ICompany company = unityContainer.Resolve<ICompany>();
      Assert.AreEqual("The evil company", company.Information.Name);
      Assert.AreEqual("2460 Sunshine road", company.Information.Address);
    }
  }
}
