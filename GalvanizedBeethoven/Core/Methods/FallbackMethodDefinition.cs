using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class FallbackMethodDefinition : MethodDefinition
  {
    private readonly MethodDefinition master;

    public FallbackMethodDefinition(MethodDefinition master) :
      base(master.Name, master.MethodMatcher)
    {
      this.master = master ?? throw new NullReferenceException();
    }

    public override int SortOrder => 2;

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters,
      Type[] genericArguments, MethodInfo methodInfo) =>
      master.Invoke(localInstance, ref returnValue, parameters, genericArguments, methodInfo);
  }
}