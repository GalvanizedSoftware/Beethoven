using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using static System.Environment;
using static System.Reflection.Assembly;
// ReSharper disable UnusedVariable

namespace GalvanizedSoftware.Beethoven.Core
{
  class Compiler
  {
    private readonly string code;
    private readonly IEnumerable<Assembly> assemblyCache;

    public Compiler(Assembly mainAssembly, Assembly callingAssembly, string[] codeParts)
    {
      assemblyCache = new AssemblyCache(mainAssembly, callingAssembly);
      code = string.Join(NewLine, codeParts);
      string data =
        string.Join(
          NewLine,
          assemblyCache
            .Select(assembly => assembly.GetName().FullName)) +
        code;
      using SHA256 sha = SHA256.Create();
      Hash = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    public byte[] Hash { get; }

    internal Assembly GenerateAssembly(string assemblyName)
    {
      PortableExecutableReference[] references = assemblyCache
        .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
        .ToArray();
      SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
      CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
      CSharpCompilation compilation = CSharpCompilation.Create(
        assemblyName,
        new[] { syntaxTree },
        references,
        options);
      using MemoryStream assemblyStream = new MemoryStream();
      using MemoryStream pbdStream = new MemoryStream();
      EmitResult result = compilation.Emit(assemblyStream, pbdStream);
      string[] errors = result
        .Diagnostics
        .Select(error => error.ToString())
        .ToArray();
      return result.Success ?
        Load(assemblyStream.ToArray(), pbdStream.ToArray()) :
        throw new Exception("Internal compilation error");
    }
  }
}
