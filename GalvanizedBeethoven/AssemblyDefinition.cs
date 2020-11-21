using GalvanizedSoftware.Beethoven.Core;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven
{
  public class AssemblyDefinition
  {
    private static int assemblyNumber = 1;
    private readonly string assemblyName;
    private readonly BoundTypeDefinition[] boundTypeDefinitions = Array.Empty<BoundTypeDefinition>();

    public AssemblyDefinition(string assemblyName = null)
    {
      this.assemblyName = assemblyName ?? $"TmpAssembly{assemblyNumber++}";
    }

    private AssemblyDefinition(AssemblyDefinition previous, BoundTypeDefinition newBoundTypeDefinition)
    {
      assemblyName = previous.assemblyName;
      boundTypeDefinitions = previous
        .boundTypeDefinitions
        .Append(newBoundTypeDefinition)
        .ToArray();
    }

    internal AssemblyDefinition Add<T>(BoundTypeDefinitionOfT<T> boundTypeDefinition) where T : class =>
      new AssemblyDefinition(this, boundTypeDefinition);

    internal Assembly GenerateAssembly(Assembly mainAssembly, Assembly callingAssembly) =>
      new Compiler(
        mainAssembly, 
        callingAssembly, 
        boundTypeDefinitions
          .Select(item => item.Generate())
          .ToArray())
      .GenerateAssembly(assemblyName);
  }
}
