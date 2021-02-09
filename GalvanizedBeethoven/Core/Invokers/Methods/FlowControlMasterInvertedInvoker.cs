using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class FlowControlMasterInvertedInvoker : IInvoker
  {
	  private readonly IInvoker master;

	  public FlowControlMasterInvertedInvoker(IInvoker master)
	  {
		  this.master = master;
	  }

    public bool Invoke(object localInstance, ref object returnValue, 
	    object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo) =>
	    !master.Invoke(localInstance, ref returnValue, parameters, genericArguments, methodInfo);
  }
}
