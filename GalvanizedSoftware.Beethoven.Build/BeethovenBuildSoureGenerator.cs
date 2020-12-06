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
      context.AddSource("source", "namespace DefinitionLibrary { public class C1 {}}");
      var assemblies1 = context
        .Compilation
        .References
        .Select(assembly => assembly.Display)
        .Where(filename => filename?.Contains(@"\.nuget\packages\") != true)
        .ToArray();
      var assemblies = assemblies1
        .Select(filename => (filename, AssemblyLoad(filename)))
        .ToArray();
      AutoFactories[] factoryTypes = assemblies
        .Select(tuple => Assembly.LoadFrom(tuple.filename))
        .Select(CreateFactories)
        .SkipNull()
        .ToArray();
      foreach (AutoFactories autoFactories in factoryTypes)
      {
        //autoFactories.CreateTypeDefinition<>()
      }
      //Debug.Assert(false);
      if (Debugger.IsAttached)
      {
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
      using (FileStream stream = new FileStream(filename, FileMode.Open))
      {
        int length = (int)stream.Length;
        byte[] data = new byte[length];
        stream.Read(data, 0, length);
        return data;
      }
    }
  }
}
