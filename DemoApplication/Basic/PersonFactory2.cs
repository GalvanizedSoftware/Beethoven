using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;

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
      personTypeDefinition = TypeDefinition<IPerson>.Create()
        .Add(new PropertyDefinition<string>("FirstName").
            ValidityCheck(name => !string.IsNullOrEmpty(name)).
            SkipIfEqual().
            SetterGetter().
            NotifyChanged())
        .Add(CreateMvvmStringProperty("LastName"));
    }

    public IPerson CreatePerson() => personTypeDefinition.CreateNew();

    private static PropertyDefinition CreateMvvmStringProperty(string propertyName)
    {
      return new PropertyDefinition<string>(propertyName).
        ValidityCheck(name => !string.IsNullOrEmpty(name)).
        SkipIfEqual().
        SetterGetter().
        NotifyChanged();
    }
  }
}