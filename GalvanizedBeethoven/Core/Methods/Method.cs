using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class Method : IInterceptor
  {
    protected Method(string name)
    {
      Name = name;
    }

    public string Name { get; }

    public abstract bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType);

    internal abstract void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo);

    public void Intercept(IInvocation invocation) =>
      Invoke(value => invocation.ReturnValue = value, invocation.Arguments, invocation.GenericArguments, invocation.Method);

    public bool IsMatchToFlowControlled((Type, string)[] parameterTypeAndNames, Type[] genericArguments, Type returnType) =>
      IsMatch(
        parameterTypeAndNames.AppendReturnValue(returnType).ToArray(),
        genericArguments,
        typeof(bool).MakeByRefType());

    public bool IsNonGenericMatch(MethodInfo methodInfo) =>
        IsMatch(
          methodInfo.GetParameterTypeAndNames(),
          new Type[0],
          methodInfo.ReturnType);
  }
}