using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  public interface ICodeGenerator<T> : ICodeGenerator where T : MemberInfo
  {
  }
}
