using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class MemberInfoExtensions
  {
    public static bool IsMatch(this MemberInfo memberInfo, PropertyInfo propertyInfo) =>
      memberInfo.Name == propertyInfo.Name &&
      (memberInfo as PropertyInfo)?.PropertyType == propertyInfo.PropertyType;
  }
}
