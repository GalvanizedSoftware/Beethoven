using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ReturnValueCheck<T> : Method
  {
    private readonly Func<T, bool> condition;

    public ReturnValueCheck(string name, Func<T, bool> condition) :
      base(name)
    {
      this.condition = condition;
    }

    public override bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      return returnType != typeof(bool) && parameters.LastOrDefault().Item1?.IsByRef == true;
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo _)
    {
      T value = (T)parameters.LastOrDefault();
      returnAction(condition(value));
    }
  }
}