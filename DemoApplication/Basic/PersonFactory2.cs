using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.DemoApp.Basic
{
  /// <summary>
  /// An alternative implementation using TypeDefinition
  /// </summary>
  internal class PersonFactory2
  {
    private readonly TypeDefinition<IPerson> personTypeDefinition;

    public PersonFactory2()
    {
      personTypeDefinition = new TypeDefinition<IPerson>()
        .Add(new Property<string>("FirstName").
            ValidityCheck(name => !string.IsNullOrEmpty(name)).
            SkipIfEqual().
            SetterGetter().
            NotifyChanged())
        .Add(CreateMvvmStringProperty("LastName"));
    }

    public IPerson CreatePerson() => personTypeDefinition.Create();

    private static Property CreateMvvmStringProperty(string propertyName)
    {
      return new Property<string>(propertyName).
        ValidityCheck(name => !string.IsNullOrEmpty(name)).
        SkipIfEqual().
        SetterGetter().
        NotifyChanged();
    }
  }
}