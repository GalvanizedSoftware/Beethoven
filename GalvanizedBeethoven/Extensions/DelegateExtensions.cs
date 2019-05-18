using System;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class DelegateExtensions
  {
    internal static string GetFirstParameterName(this Delegate lambda) => 
      lambda.Method.GetParameters().FirstOrDefault()?.Name;
  }
}
