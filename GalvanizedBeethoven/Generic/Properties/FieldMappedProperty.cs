using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class FieldMappedProperty : IDefinition
  {
    private readonly PropertyInfo propertyInfo;
    private readonly string fieldName;

    public FieldMappedProperty(PropertyInfo propertyInfo, string fieldName)
    {
      this.propertyInfo = propertyInfo;
      this.fieldName = fieldName;
    }

    public int SortOrder => 2;

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo switch
      {
        PropertyInfo checkPropertyInfo =>
             checkPropertyInfo.Name == this.propertyInfo.Name &&
             checkPropertyInfo.PropertyType == this.propertyInfo.PropertyType,
        _ => false,
      };

    public ICodeGenerator GetGenerator(GeneratorContext _) =>
      new FieldMappedPropertyGenerator( propertyInfo, fieldName);
  }
}