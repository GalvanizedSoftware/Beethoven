using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;

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
        .Add(new PropertyDefinition<string>("FirstName").
            ValidityCheck(name => !string.IsNullOrEmpty(name)).
            SkipIfEqual().
            SetterGetter().
            NotifyChanged())
        .Add(CreateMvvmStringProperty("LastName"));
    }

    public IPerson CreatePerson() => personTypeDefinition.Create();

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