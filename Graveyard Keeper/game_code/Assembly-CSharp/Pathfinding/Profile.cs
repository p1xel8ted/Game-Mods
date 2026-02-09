// Decompiled with JetBrains decompiler
// Type: Pathfinding.Profile
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Pathfinding;

public class Profile
{
  public const bool PROFILE_MEM = false;
  public string name;
  public Stopwatch watch;
  public int counter;
  public long mem;
  public long smem;
  public int control = 1073741824 /*0x40000000*/;
  public const bool dontCountFirst = false;

  public int ControlValue() => this.control;

  public Profile(string name)
  {
    this.name = name;
    this.watch = new Stopwatch();
  }

  [Conditional("PROFILE")]
  public void Start() => this.watch.Start();

  [Conditional("PROFILE")]
  public void Stop()
  {
    ++this.counter;
    this.watch.Stop();
  }

  [Conditional("PROFILE")]
  public void Log() => UnityEngine.Debug.Log((object) this.ToString());

  [Conditional("PROFILE")]
  public void ConsoleLog() => Console.WriteLine(this.ToString());

  [Conditional("PROFILE")]
  public void Stop(int control)
  {
    ++this.counter;
    this.watch.Stop();
    if (this.control == 1073741824 /*0x40000000*/)
      this.control = control;
    else if (this.control != control)
      throw new Exception($"Control numbers do not match {this.control.ToString()} != {control.ToString()}");
  }

  [Conditional("PROFILE")]
  public void Control(Profile other)
  {
    if (this.ControlValue() != other.ControlValue())
      throw new Exception($"Control numbers do not match ({this.name} {other.name}) {this.ControlValue().ToString()} != {other.ControlValue().ToString()}");
  }

  public override string ToString()
  {
    return $"{this.name} #{this.counter.ToString()} {this.watch.Elapsed.TotalMilliseconds.ToString("0.0 ms")} avg: {(this.watch.Elapsed.TotalMilliseconds / (double) this.counter).ToString("0.00 ms")}";
  }
}
