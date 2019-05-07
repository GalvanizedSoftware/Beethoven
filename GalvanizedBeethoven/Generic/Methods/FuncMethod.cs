using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FuncMethod<TReturnType> : Method
  {
    private readonly Func<TReturnType> func;

    public FuncMethod(string name, Func<TReturnType> func) : 
      base(name, new MatchNoParametersAndReturnType<TReturnType>())
    {
      this.func = func;
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _) => 
      returnAction(func());
  }
}