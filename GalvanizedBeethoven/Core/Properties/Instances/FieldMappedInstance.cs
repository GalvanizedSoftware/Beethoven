using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
{
  internal class FieldMappedInstance : IDefinition
  {
    private readonly PropertyInfo propertyInfo;
    private readonly string fieldName;

    public FieldMappedInstance(PropertyInfo propertyInfo, string fieldName)
    {
      this.propertyInfo = propertyInfo;
      this.fieldName = fieldName;
    }

    public int SortOrder => 2;

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo.IsMatch(propertyInfo);

    public ICodeGenerator GetGenerator(GeneratorContext _) =>
      new FieldMappedPropertyGenerator(propertyInfo, fieldName);
  }
}