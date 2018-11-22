using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
// ReSharper disable ImplicitlyCapturedClosure
// ReSharper disable CollectionNeverQueried.Local

namespace GalvanizedSoftware.Beethoven.Test.Performance
{
  [TestClass]
  public class PerformanceTests
  {
    private const double TimeScale = 1000.0;
    private readonly Factory factory = new Factory();

    private static long TimeAction(Action testAction)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      testAction();
      stopwatch.Stop();
      return stopwatch.ElapsedMilliseconds;
    }

    [TestMethod]
    // On my system this is about 100 times slower than classic implementation, or on average 1.3 µs per call
    public void PerformanceTestGetValue()
    {
      IPerformanceTest classic = new Classic { Name = "Name" };
      const int callCount = 1000000;
      List<string> dummyList = new List<string>(callCount);
      long time1 = TimeAction(() =>
      {
        for (int i = 0; i < callCount; i++)
          dummyList.Add(classic.Name);
      });
      Console.WriteLine($"Classic get: {time1}ms ({TimeScale * time1 / callCount} µs)");
      IPerformanceTest newWay = factory.Create();
      newWay.Name = "Name";
      dummyList.Clear();
      long time2 = TimeAction(() =>
      {
        for (int i = 0; i < callCount; i++)
          dummyList.Add(newWay.Name);
      });
      Console.WriteLine($"New get: {time2}ms ({TimeScale * time2 / callCount} µs)");
      Console.WriteLine($"{time2 / time1} Times slower");
    }

    [TestMethod]
    // On my system this is about 5 times slower than classic implementation, or on average 2.2 µs per call
    public void PerformanceTestSetValue()
    {
      IPerformanceTest classic = new Classic { Name = "Name" };
      const int callCount = 1000000;
      long time1 = TimeAction(() =>
      {
        for (int i = 0; i < callCount; i++)
          classic.Name = $"Name{i}";
      });
      Console.WriteLine($"Classic get: {time1}ms ({TimeScale * time1 / callCount} µs)");
      IPerformanceTest newWay = factory.Create();
      newWay.Name = "Name";
      long time2 = TimeAction(() =>
      {
        for (int i = 0; i < callCount; i++)
          newWay.Name = $"Name{i}";
      });
      Console.WriteLine($"New get: {time2}ms ({TimeScale * time2 / callCount} µs)");
      Console.WriteLine($"{time2 / time1} Times slower");
    }

    [TestMethod]
    // On my system this is about 10 times slower than classic implementation, or on average 3.6 µs per call
    public void PerformanceTestCallMethod()
    {
      IPerformanceTest classic = new Classic { Name = "Name" };
      const int callCount = 1000000;
      List<string> dummyList = new List<string>(callCount);
      long time1 = TimeAction(() =>
      {
        for (int i = 0; i < callCount; i++)
          dummyList.Add(classic.Format("Something {0}"));
      });
      Console.WriteLine($"Classic get: {time1}ms ({TimeScale * time1 / callCount} µs)");
      IPerformanceTest newWay = factory.Create();
      newWay.Name = "Name";
      dummyList.Clear();
      long time2 = TimeAction(() =>
      {
        for (int i = 0; i < callCount; i++)
          dummyList.Add(newWay.Format("Something {0}"));
      });
      Console.WriteLine($"New get: {time2}ms ({TimeScale * time2 / callCount} µs)");
      Console.WriteLine($"{time2 / time1} Times slower");
    }

    [TestMethod]
    // On my system this is about 1000-1400 times slower (yikes!) than classic implementation, or on average 130-170 µs per call
    public void PerformanceTestCreation()
    {
      const int callCount1 = 1000000;
      List<object> dummyList = new List<object>(callCount1);
      long time1 = TimeAction(() =>
      {
        for (int i = 0; i < callCount1; i++)
          dummyList.Add(new Classic());
      });
      Console.WriteLine($"Classic get: {time1}ms ({TimeScale * time1 / callCount1} µs)");
      dummyList.Clear();
      const int scale = 100;
      const int callCount2 = callCount1 / scale;
      long time2 = TimeAction(() =>
      {
        for (int i = 0; i < callCount2; i++)
          dummyList.Add(factory.Create());
      });
      Console.WriteLine($"New get: {time2}ms ({TimeScale * time2 / callCount2} µs)");
      Console.WriteLine($"{scale * time2 / time1} Times slower");
    }
  }
}
