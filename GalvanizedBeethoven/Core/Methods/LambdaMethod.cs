using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public class LambdaMethod<T> : Method
  {
    private readonly Delegate lambdaDelegate;
    private readonly Type returnType;

    public LambdaMethod(string name, T actionOrFunc) : base(name)
    {
      lambdaDelegate = actionOrFunc as Delegate;
      if (lambdaDelegate == null)
        throw new InvalidCastException("You must supplt an action, func or delegate");
      returnType = lambdaDelegate.Method.ReturnType;
    }

    public override bool IsMatch(IEnumerable<Type> parameters)
    {
      return !parameters.Any();
    }

    protected override void Invoke(Action<object> returnAction, object[] parameters)
    {
      object returnValue = lambdaDelegate.DynamicInvoke(parameters);
      if (returnType != null)
        returnAction(returnValue);
    }
  }
}