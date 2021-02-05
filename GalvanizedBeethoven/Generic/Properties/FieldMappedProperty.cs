using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class FieldMappedProperty : DefaultDefinition
  {
    private readonly PropertyInfo propertyInfo;
    private readonly string fieldName;

    public FieldMappedProperty(PropertyInfo propertyInfo, string fieldName)
    {
      this.propertyInfo = propertyInfo;
      this.fieldName = fieldName;
    }

    public override int SortOrder => 2;

    public override bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo.IsMatch(propertyInfo);

    public override ICodeGenerator GetGenerator(MemberInfo memberInfo) =>
      new FieldMappedPropertyGenerator( propertyInfo, fieldName);
  }
}
