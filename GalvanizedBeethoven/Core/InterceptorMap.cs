using System;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class InterceptorMap : Tuple<string, IInterceptor>
  {
    public InterceptorMap(string item1, IInterceptor item2) : base(item1, item2)
    {
    }
  }
}