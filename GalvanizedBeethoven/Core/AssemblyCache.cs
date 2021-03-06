﻿using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class AssemblyCache : IEnumerable<Assembly>
  {
    private readonly List<Assembly> analyzingAssemblies = new();
    private readonly Dictionary<Assembly, Assembly[]> assemblyCache = new();
    private readonly Dictionary<string, Assembly> loadedAssemblyNames = new();
    private readonly Assembly[] assemblies;
    private readonly Assembly[] domainAssemblies;

    internal AssemblyCache(Assembly mainAssembly, Assembly callingAssembly)
    {
      if (callingAssembly.GetName().Name == "GalvanizedSoftware.Beethoven")
        throw new InvalidOperationException("callingAssembly is GalvanizedSoftware.Beethoven");
      assemblies =
        new[] 
          {
            typeof(object).Assembly, 
            Assembly.Load(new AssemblyName("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51")),
            mainAssembly, 
            callingAssembly 
          }
          .Distinct()
          .ToArray();
      domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
      foreach (Assembly assembly in domainAssemblies)
        loadedAssemblyNames.Add(assembly.GetName().FullName, assembly);
      analyzingAssemblies.AddRange(assemblies);
    }
    public IEnumerator<Assembly> GetEnumerator() =>
      assemblies
        .SelectMany(GetAssemblies)
        .Where(assembly => domainAssemblies.Contains(assembly))
        .Distinct()
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IEnumerable<Assembly> GetAssemblies(Assembly assembly)
    {
      if (assemblyCache.TryGetValue(assembly, out Assembly[] result))
        return result;
      result = FindReferencedAssemblies(assembly)
        .Append(assembly)
        .ToArray();
      assemblyCache.Add(assembly, result);
      return result;
    }

    private IEnumerable<Assembly> FindReferencedAssemblies(Assembly assembly) =>
      assembly
        .GetReferencedAssemblies()
        .Select(item => item.FullName)
        .Select(loadedAssemblyNames.TryGetValue)
        .SelectMany(FindNew)
        .SelectMany(GetAssemblies);

    private IEnumerable<Assembly> FindNew(Assembly assembly)
    {
      if (analyzingAssemblies.Contains(assembly) || assembly == null)
        yield break;
      analyzingAssemblies.Add(assembly);
      yield return assembly;
    }
  }
}
