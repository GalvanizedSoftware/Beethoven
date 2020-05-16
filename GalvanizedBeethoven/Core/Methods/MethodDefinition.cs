using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class MethodDefinition : IDefinition
  {
    protected MethodDefinition(string name, IMethodMatcher methodMatcher)
    {
      Name = name;
      MethodMatcher = methodMatcher ?? new MatchNothing();
    }

    public string Name { get; }
    public IMethodMatcher MethodMatcher { get; }

    public virtual void Invoke(object localInstance,
      ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo) =>
      throw new MissingMemberException(methodInfo?.DeclaringType?.FullName, methodInfo?.Name);

    public virtual int SortOrder => 1;

    public bool CanGenerate(MemberInfo memberInfo) => memberInfo switch
    {
      MethodInfo methodInfo => MethodMatcher.IsMatchIgnoreGeneric(methodInfo, Name),
      _ => false,
    };

    public ICodeGenerator GetGenerator(GeneratorContext _) => null;
  }
}