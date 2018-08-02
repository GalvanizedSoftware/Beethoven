using System;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.DemoApp.Mapped
{
  public static class AddressFormatter
  {
    public static string FormatAddress(params string[] fields)
    {
      return string.Join(Environment.NewLine, fields.Where(text => !string.IsNullOrEmpty(text)).ToArray());
    }
  }
}
