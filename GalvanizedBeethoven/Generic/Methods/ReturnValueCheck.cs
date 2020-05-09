using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ReturnValueCheck<T> : MethodDefinition
  {
    private readonly Func<T, bool> condition;

    public ReturnValueCheck(string name, Func<T, bool> condition) :
      base(name, new MatchFlowControl())
    {
      this.condition = condition;
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) => 
      returnValue = condition((T)parameters.LastOrDefault());
  }
}