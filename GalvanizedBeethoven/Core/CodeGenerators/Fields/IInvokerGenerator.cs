using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  public interface IInvokerGenerator
  {
    string InvokerName { get; }

    IEnumerable<(CodeType, string)?> Generate();
  }
}