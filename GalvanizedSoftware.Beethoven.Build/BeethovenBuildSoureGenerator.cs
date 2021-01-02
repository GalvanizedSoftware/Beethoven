using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.AutoFactories;

namespace GalvanizedSoftware.Beethoven.Build
{
  [Generator]
  public class BeethovenBuildSoureGenerator : ISourceGenerator
  {
    public string FactoryFile { get; set; }
    public string OutputFile { get; set; }

    public void Initialize(GeneratorInitializationContext _)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
      (string filename, Assembly)[] assemblies = context
        .Compilation
        .References
        .Select(assembly => assembly.Display)
        .Where(filename => filename?.Contains(@"\.nuget\packages\") != true)
        .ToArray()
        .Select(filename => (filename, AssemblyLoad(filename)))
        .ToArray();
      AutoFactories[] factoryTypes = assemblies
        .Select(tuple => Assembly.LoadFrom(tuple.filename))
        .Select(CreateFactories)
        .SkipNull()
        .ToArray();
      (string filename, string code)[] codeFiles = factoryTypes
        .SelectMany(factories => factories.Factories)
        .Select(tuple => CodeGenerator.Create(tuple.Item1, tuple.Item2, tuple.Item3))
        .Select(generator => generator.GenerateCode())
        .ToArray();
      foreach ((string filename, string code) tuple in codeFiles)
      {
        (string filename, string code) = tuple;
        context.AddSource(filename, code);
        //using StreamWriter streamWriter = new StreamWriter(filename);
        //streamWriter.Write(code);
      }
    }

    private Assembly AssemblyLoad(string filename)
    {
      byte[] data = null;
      try
      {
        data = ReadFile(filename);
        return Assembly.Load(data);
      }
      catch (Exception e)
      {
        Debug.WriteLine($"Assembly load failed: {filename}, {data?.Length}");
        return null;
      }
    }

    private static byte[] ReadFile(string filename)
    {
      using FileStream stream = new FileStream(filename, FileMode.Open);
      int length = (int)stream.Length;
      byte[] data = new byte[length];
      stream.Read(data, 0, length);
      return data;
    }
  }
}
