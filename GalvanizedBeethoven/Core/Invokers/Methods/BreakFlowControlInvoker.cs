using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
	internal class BreakFlowControlInvoker : IInvoker
	{
		public bool Invoke(object _, ref object __, object[] ___, Type[] ____, MethodInfo _____) =>
			false;
	}
}
