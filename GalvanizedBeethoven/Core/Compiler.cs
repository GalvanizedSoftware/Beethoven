using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Environment;
using static System.Reflection.Assembly;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal sealed class Compiler
  {
    private readonly string code;
    private readonly IEnumerable<Assembly> assemblyCache;

    public Compiler(Assembly mainAssembly, Assembly callingAssembly, string[] codeParts)
    {
      assemblyCache = new AssemblyCache(mainAssembly, callingAssembly);
      code = string.Join(NewLine, codeParts);
    }

    internal Assembly GenerateAssembly(string assemblyName)
    {
      PortableExecutableReference[] references = assemblyCache
        .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
        .ToArray();
      SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
      CSharpCompilationOptions options = new(OutputKind.DynamicallyLinkedLibrary);
      CSharpCompilation compilation = CSharpCompilation.Create(
        assemblyName,
        new[] { syntaxTree },
        references,
        options);
      using MemoryStream assemblyStream = new();
      using MemoryStream pbdStream = new();
      EmitResult result = compilation.Emit(assemblyStream, pbdStream);
      string[] errors = result
        .Diagnostics
        .Select(error => error.ToString())
        .ToArray();
      return result.Success ?
        Load(assemblyStream.ToArray(), pbdStream.ToArray()) :
        throw new($"Internal compilation error: {NewLine}{string.Join(NewLine, errors)}");
    }
  }
}
