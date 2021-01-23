using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.Definitions;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public sealed class GenericMethodsWrapper<T> : MethodDefinition, IFieldMaps
  {
    private readonly MethodInfo methodInfo;
    private readonly IEnumerable<GenericMethodWrapper> wrappers;

    public GenericMethodsWrapper(MethodInfo methodInfo, IEnumerable<GenericMethodWrapper> wrappers):
      base(methodInfo.Name, null)
    {
      this.methodInfo = methodInfo;
      this.wrappers = wrappers;
      Set(typeof(T));
    }

    public override bool CanGenerate(MemberInfo memberInfo) =>
      methodInfo == memberInfo;

    public override ICodeGenerator GetGenerator(GeneratorContext generatorContext) =>
      new MethodGenerator(generatorContext);

    public override IEnumerable<(string, object)> GetFields() =>
      wrappers.SelectMany(wrapper => wrapper
          .MethodDefinition
          .GetFields())
        .Distinct();

    public override void Set(Type setMainType)
    {
      base.Set(setMainType);
      MemberInfoList memberInfoList = MemberInfoListCache.Get(setMainType);
      invokerName = memberInfoList.GetMethodInvokerName(linkedMethodInfo);
      invokerFactory = () => new RealMethodInvokerFactory(linkedMethodInfo, this);
    }
  }
}