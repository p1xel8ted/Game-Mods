// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarProfiler
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Pathfinding;

public class AstarProfiler
{
  public static Dictionary<string, AstarProfiler.ProfilePoint> profiles = new Dictionary<string, AstarProfiler.ProfilePoint>();
  public static DateTime startTime = DateTime.UtcNow;
  public static AstarProfiler.ProfilePoint[] fastProfiles;
  public static string[] fastProfileNames;

  [Conditional("ProfileAstar")]
  public static void InitializeFastProfile(string[] profileNames)
  {
    AstarProfiler.fastProfileNames = new string[profileNames.Length + 2];
    Array.Copy((Array) profileNames, (Array) AstarProfiler.fastProfileNames, profileNames.Length);
    AstarProfiler.fastProfileNames[AstarProfiler.fastProfileNames.Length - 2] = "__Control1__";
    AstarProfiler.fastProfileNames[AstarProfiler.fastProfileNames.Length - 1] = "__Control2__";
    AstarProfiler.fastProfiles = new AstarProfiler.ProfilePoint[AstarProfiler.fastProfileNames.Length];
    for (int index = 0; index < AstarProfiler.fastProfiles.Length; ++index)
      AstarProfiler.fastProfiles[index] = new AstarProfiler.ProfilePoint();
  }

  [Conditional("ProfileAstar")]
  public static void StartFastProfile(int tag) => AstarProfiler.fastProfiles[tag].watch.Start();

  [Conditional("ProfileAstar")]
  public static void EndFastProfile(int tag)
  {
    AstarProfiler.ProfilePoint fastProfile = AstarProfiler.fastProfiles[tag];
    ++fastProfile.totalCalls;
    fastProfile.watch.Stop();
  }

  [Conditional("ASTAR_UNITY_PRO_PROFILER")]
  public static void EndProfile()
  {
  }

  [Conditional("ProfileAstar")]
  public static void StartProfile(string tag)
  {
    AstarProfiler.ProfilePoint profilePoint;
    AstarProfiler.profiles.TryGetValue(tag, out profilePoint);
    if (profilePoint == null)
    {
      profilePoint = new AstarProfiler.ProfilePoint();
      AstarProfiler.profiles[tag] = profilePoint;
    }
    profilePoint.tmpBytes = GC.GetTotalMemory(false);
    profilePoint.watch.Start();
  }

  [Conditional("ProfileAstar")]
  public static void EndProfile(string tag)
  {
    if (!AstarProfiler.profiles.ContainsKey(tag))
    {
      UnityEngine.Debug.LogError((object) $"Can only end profiling for a tag which has already been started (tag was {tag})");
    }
    else
    {
      AstarProfiler.ProfilePoint profile = AstarProfiler.profiles[tag];
      ++profile.totalCalls;
      profile.watch.Stop();
      profile.totalBytes += GC.GetTotalMemory(false) - profile.tmpBytes;
    }
  }

  [Conditional("ProfileAstar")]
  public static void Reset()
  {
    AstarProfiler.profiles.Clear();
    AstarProfiler.startTime = DateTime.UtcNow;
    if (AstarProfiler.fastProfiles == null)
      return;
    for (int index = 0; index < AstarProfiler.fastProfiles.Length; ++index)
      AstarProfiler.fastProfiles[index] = new AstarProfiler.ProfilePoint();
  }

  [Conditional("ProfileAstar")]
  public static void PrintFastResults()
  {
    if (AstarProfiler.fastProfiles == null)
      return;
    int num1 = 0;
    while (num1 < 1000)
      ++num1;
    double num2 = AstarProfiler.fastProfiles[AstarProfiler.fastProfiles.Length - 2].watch.Elapsed.TotalMilliseconds / 1000.0;
    TimeSpan timeSpan = DateTime.UtcNow - AstarProfiler.startTime;
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append("============================\n\t\t\t\tProfile results:\n============================\n");
    stringBuilder1.Append("Name\t\t|\tTotal Time\t|\tTotal Calls\t|\tAvg/Call\t|\tBytes");
    double num3;
    for (int index = 0; index < AstarProfiler.fastProfiles.Length; ++index)
    {
      string fastProfileName = AstarProfiler.fastProfileNames[index];
      AstarProfiler.ProfilePoint fastProfile = AstarProfiler.fastProfiles[index];
      int totalCalls = fastProfile.totalCalls;
      double num4 = fastProfile.watch.Elapsed.TotalMilliseconds - num2 * (double) totalCalls;
      if (totalCalls >= 1)
      {
        stringBuilder1.Append("\n").Append(fastProfileName.PadLeft(10)).Append("|   ");
        StringBuilder stringBuilder2 = stringBuilder1.Append(num4.ToString("0.0 ").PadLeft(10));
        num3 = fastProfile.watch.Elapsed.TotalMilliseconds;
        string str1 = num3.ToString("(0.0)").PadLeft(10);
        stringBuilder2.Append(str1).Append("|   ");
        stringBuilder1.Append(totalCalls.ToString().PadLeft(10)).Append("|   ");
        StringBuilder stringBuilder3 = stringBuilder1;
        num3 = num4 / (double) totalCalls;
        string str2 = num3.ToString("0.000").PadLeft(10);
        stringBuilder3.Append(str2);
      }
    }
    stringBuilder1.Append("\n\n============================\n\t\tTotal runtime: ");
    StringBuilder stringBuilder4 = stringBuilder1;
    num3 = timeSpan.TotalSeconds;
    string str = num3.ToString("F3");
    stringBuilder4.Append(str);
    stringBuilder1.Append(" seconds\n============================");
    UnityEngine.Debug.Log((object) stringBuilder1.ToString());
  }

  [Conditional("ProfileAstar")]
  public static void PrintResults()
  {
    TimeSpan timeSpan = DateTime.UtcNow - AstarProfiler.startTime;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
    int num = 5;
    foreach (KeyValuePair<string, AstarProfiler.ProfilePoint> profile in AstarProfiler.profiles)
      num = Math.Max(profile.Key.Length, num);
    stringBuilder.Append(" Name ".PadRight(num)).Append("|").Append(" Total Time\t".PadRight(20)).Append("|").Append(" Total Calls ".PadRight(20)).Append("|").Append(" Avg/Call ".PadRight(20));
    foreach (KeyValuePair<string, AstarProfiler.ProfilePoint> profile in AstarProfiler.profiles)
    {
      double totalMilliseconds = profile.Value.watch.Elapsed.TotalMilliseconds;
      int totalCalls = profile.Value.totalCalls;
      if (totalCalls >= 1)
      {
        string key = profile.Key;
        stringBuilder.Append("\n").Append(key.PadRight(num)).Append("| ");
        stringBuilder.Append(totalMilliseconds.ToString("0.0").PadRight(20)).Append("| ");
        stringBuilder.Append(totalCalls.ToString().PadRight(20)).Append("| ");
        stringBuilder.Append((totalMilliseconds / (double) totalCalls).ToString("0.000").PadRight(20));
        stringBuilder.Append(AstarMath.FormatBytesBinary((int) profile.Value.totalBytes).PadLeft(10));
      }
    }
    stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
    stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
    stringBuilder.Append(" seconds\n============================");
    UnityEngine.Debug.Log((object) stringBuilder.ToString());
  }

  public class ProfilePoint
  {
    public Stopwatch watch = new Stopwatch();
    public int totalCalls;
    public long tmpBytes;
    public long totalBytes;
  }
}
