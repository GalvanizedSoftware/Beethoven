using GalvanizedSoftware.Beethoven.Core;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;

namespace GalvanizedSoftware.Beethoven
{
  public class AssemblyDefinition
  {
    private static int assemblyNumber = 1;
    private readonly string assemblyName;
    private readonly TypeDefinitionGenerator[] typeDefinitionGenerators = Array.Empty<TypeDefinitionGenerator>();

    public AssemblyDefinition(string assemblyName = null)
    {
      this.assemblyName = assemblyName ?? $"TmpAssembly{assemblyNumber++}";
    }

    private AssemblyDefinition(AssemblyDefinition previous, TypeDefinitionGenerator newGenerator)
    {
      assemblyName = previous.assemblyName;
      typeDefinitionGenerators = previous
        .typeDefinitionGenerators
        .Append(newGenerator)
        .ToArray();
    }

    internal AssemblyDefinition Add<T>(TypeDefinitionGeneratorOfT<T> boundTypeDefinition) where T : class =>
      new(this, boundTypeDefinition);

    internal Assembly GenerateAssembly(Assembly mainAssembly, Assembly callingAssembly) =>
      new Compiler(
        mainAssembly, 
        callingAssembly, 
        typeDefinitionGenerators
          .Select(item => item.Generate())
          .ToArray())
      .GenerateAssembly(assemblyName);
  }
}
