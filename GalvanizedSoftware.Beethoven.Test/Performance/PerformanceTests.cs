using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
// ReSharper disable CommentTypo

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
    // On my system the result compared to classic implementation is:
    // Commit 0dca2387: 100 times slower, on average 1.3 µs per cal
    // Commit dcdf4581: 24 times slower, on average 1.31 µs per cal
    public void PerformanceTestGetValue()
    {
      IPerformanceTest classic = new Classic { Name = "Name" };
      IPerformanceTest newWay = factory.Create();
      newWay.Name = "Name";
      const int callCount1 = 10000000;
      const int callCount2 = 2000000;
      PerformanceTest(
        () => classic.Name, callCount1,
        () => newWay.Name, callCount2);
    }

    [TestMethod]
    // On my system the result compared to classic implementation is:
    // Commit 0dca2387: 5 times slower, on average 2.2 µs per cal
    // Commit dcdf4581: 31 times slower, on average 1.71 µs per cal
    public void PerformanceTestSetValue()
    {
      IPerformanceTest classic = new Classic { Name = "Name" };
      IPerformanceTest newWay = factory.Create();
      newWay.Name = "Name";
      const int callCount1 = 10000000;
      const int callCount2 = 3000000;
      PerformanceTest(
        () => classic.Name = "Name", callCount1,
        () => newWay.Name = "Name", callCount2);
    }

    [TestMethod]
    // On my system the result compared to classic implementation is:
    // Commit 0dca2387: 10 times slower, on average 3.6 µs per cal
    // Commit dcdf4581: 10 times slower, on average 4.0 µs per cal
    public void PerformanceTestCallMethod()
    {
      IPerformanceTest classic = new Classic { Name = "Name" };
      IPerformanceTest newWay = factory.Create();
      const int callCount1 = 1000000;
      const int callCount2 = 100000;
      PerformanceTest(
        () => classic.Format("Something {0}"), callCount1,
        () => newWay.Format("Something {0}"), callCount2);
    }

    [TestMethod]
    // On my system the result compared to classic implementation is:
    // Commit 0dca2387: 1000-1400 times slower (yikes!), on average 130-170 µs per cal
    // Commit dcdf4581: 500-700 times slower, on average 110-120 µs per cal
    public void PerformanceTestCreation()
    {
      const int callCount1 = 2000000;
      const int callCount2 = 200000;
      PerformanceTest(
        () => new Classic(), callCount1,
        () => factory.Create(), callCount2);
    }

    private void PerformanceTest(
      Func<object> classicAction, int classicCount,
      Func<object> newAction, int newCount)
    {
      double timePerCall1 = TimePerCall("Classic:", classicAction, classicCount);
      double timePerCall2 = TimePerCall("New:", newAction, newCount);
      Console.WriteLine($"{ timePerCall2 / timePerCall1:0.0} Times slower");
    }

    private static double TimePerCall(string header, Func<object> classicAction, int classicCount)
    {
      long time1 = TimeAction(() =>
        Enumerable.Range(0, classicCount)
          .Select(i => classicAction())
          // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
          .ToList()
      );
      double timePerCall1 = TimeScale * time1 / classicCount;
      Console.WriteLine(header + $" {time1}ms ({timePerCall1:0.00} µs)");
      return timePerCall1;
    }
  }
}
