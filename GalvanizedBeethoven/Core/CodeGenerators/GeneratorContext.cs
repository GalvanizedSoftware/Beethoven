using System.Globalization;
using System.Reflection;
using static System.Environment;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  public class GeneratorContext
  {
    internal GeneratorContext()
    {
    }

    private GeneratorContext(GeneratorContext baseContext, MemberInfo memberInfo = null, int? methodIndex = null) :
      this()
    {
      MemberInfo = memberInfo;
      MethodIndex = methodIndex;
    }

    public MemberInfo MemberInfo { get; }

    public int? MethodIndex { get; }

    internal GeneratorContext CreateLocal<T>(T memberInfo, int? methodIndex = null) where T : MemberInfo =>
      new(this, memberInfo, methodIndex);
  }
}
