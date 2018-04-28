using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class Method : IInterceptor
  {
    protected Method(string name)
    {
      Name = name;
    }

    public string Name { get; }

    public abstract bool IsMatch(IEnumerable<Type> parameters);

    protected abstract void Invoke(Action<object> returnAction, object[] parameters);

    public void Intercept(IInvocation invocation)
    {
      Invoke(value => invocation.ReturnValue = value, invocation.Arguments);
    }
  }
}