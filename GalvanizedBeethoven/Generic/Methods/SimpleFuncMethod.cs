using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class SimpleFuncMethod<TReturnType> : Method
  {
    private readonly Func<TReturnType> func;

    public SimpleFuncMethod(string name, Func<TReturnType> func) : 
      base(name, new MatchNoParametersAndReturnType<TReturnType>())
    {
      this.func = func;
    }

    public override void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo _) => 
      returnAction(func());
  }
}