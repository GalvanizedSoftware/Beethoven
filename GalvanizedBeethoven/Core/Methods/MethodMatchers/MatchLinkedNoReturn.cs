using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchLinkedNoReturn: IMethodMatcher, IObjectProvider
  {
    private readonly IMethodMatcher[] methodList;
    private readonly ObjectProviderHandler objectProviderHandler;

    public MatchLinkedNoReturn() :
      this(new IMethodMatcher[0])
    {
    }

    internal MatchLinkedNoReturn(MatchLinkedNoReturn previous, IMethodMatcher newMethod) :
      this(previous.methodList.Append(newMethod).ToArray())
    {
    }

    internal MatchLinkedNoReturn(IEnumerable<IMethodMatcher> methodList)
    {
      this.methodList = methodList.ToArray();
      objectProviderHandler = new ObjectProviderHandler(this.methodList);
    }

    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      return methodList.Any(method => method.IsMatch(parameters, genericArguments, returnType)) ||
             methodList.Any(method => method.IsMatchToFlowControlled(parameters, genericArguments, typeof(void)));
    }

    public IEnumerable<TChild> Get<TChild>() => 
      objectProviderHandler.Get<TChild>();
  }
}