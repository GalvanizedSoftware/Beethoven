using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
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
