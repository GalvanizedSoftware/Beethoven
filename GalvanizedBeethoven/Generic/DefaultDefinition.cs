using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public abstract class DefaultDefinition : IDefinition
  {
    public virtual int SortOrder => 1;

    public virtual bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo is null;

    public abstract ICodeGenerator GetGenerator(GeneratorContext generatorContext);

    public virtual IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo) =>
      Enumerable.Empty<IInvoker>();
  }
}
