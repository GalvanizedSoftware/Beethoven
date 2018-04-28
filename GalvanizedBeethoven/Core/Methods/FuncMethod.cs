using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public class FuncMethod<TReturnType> : Method
  {
    private readonly Func<TReturnType> func;

    public FuncMethod(string name, Func<TReturnType> func) : base(name)
    {
      this.func = func;
    }

    public override bool IsMatch(IEnumerable<Type> parameters)
    {
      return !parameters.Any();
    }

    protected override void Invoke(Action<object> returnAction, object[] parameters)
    {
      returnAction(func());
    }
  }
}