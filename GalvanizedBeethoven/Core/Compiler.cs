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
using static System.Reflection.Assembly;

namespace GalvanizedSoftware.Beethoven.Core
{
  class Compiler
  {
    private static int assemblyNumber = 1;
    private readonly string code;
    private readonly IEnumerable<Assembly> assemblyCache;

    public Compiler(string code, IEnumerable<Assembly> assemblyCache)
    {
      this.code = code;
      this.assemblyCache = assemblyCache;
      string data = 
        string.Join(
          Environment.NewLine,
          assemblyCache
            .Select(assembly => assembly.GetName().FullName)) +
        code;
      using SHA256 sha = SHA256.Create();
      Hash = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    public byte[] Hash { get; }

    internal Assembly GenerateAssembly()
    {
      MetadataReference[] references = assemblyCache
        .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
        .ToArray();
      SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
      CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
      CSharpCompilation compilation = CSharpCompilation.Create(
        $"TmpAssembly{++assemblyNumber}",
        new[] { syntaxTree },
        references,
        options);
      using MemoryStream assemblyStream = new MemoryStream();
      using MemoryStream pbdStream = new MemoryStream();
      EmitResult result = compilation.Emit(assemblyStream, pbdStream);
      string[] erros = result
        .Diagnostics
        .Select(error => error.ToString())
        .ToArray();
      return result.Success ?
        Load(assemblyStream.ToArray(), pbdStream.ToArray()) :
        throw new Exception("Internal compilation error"); ;
    }
  }
}
