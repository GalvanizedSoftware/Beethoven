using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Fields
{
  internal class FieldMappedInstance : DefaultDefinition
  {
    private readonly PropertyInfo propertyInfo;
    private readonly string fieldName;

    public FieldMappedInstance(PropertyInfo propertyInfo, string fieldName)
    {
      this.propertyInfo = propertyInfo;
      this.fieldName = fieldName;
    }

    public override bool CanGenerate(MemberInfo memberInfo) =>

      memberInfo.IsMatch(propertyInfo);
    public override ICodeGenerator GetGenerator(MemberInfo memberInfo) =>
      new FieldMappedPropertyGenerator(propertyInfo, fieldName);
  }
}
