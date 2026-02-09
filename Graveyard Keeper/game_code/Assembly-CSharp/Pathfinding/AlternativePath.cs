// Decompiled with JetBrains decompiler
// Type: Pathfinding.AlternativePath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[AddComponentMenu("Pathfinding/Modifiers/Alternative Path")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_alternative_path.php")]
[Serializable]
public class AlternativePath : MonoModifier
{
  public int penalty = 1000;
  public int randomStep = 10;
  public GraphNode[] prevNodes;
  public int prevSeed;
  public int prevPenalty;
  public bool waitingForApply;
  public object lockObject = new object();
  public System.Random rnd = new System.Random();
  public System.Random seedGenerator = new System.Random();
  public bool destroyed;
  public GraphNode[] toBeApplied;

  public override int Order => 10;

  public override void Apply(Path p)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    lock (this.lockObject)
    {
      this.toBeApplied = p.path.ToArray();
      if (this.waitingForApply)
        return;
      this.waitingForApply = true;
      AstarPath.OnPathPreSearch += new OnPathDelegate(this.ApplyNow);
    }
  }

  public new void OnDestroy()
  {
    this.destroyed = true;
    lock (this.lockObject)
    {
      if (!this.waitingForApply)
      {
        this.waitingForApply = true;
        AstarPath.OnPathPreSearch += new OnPathDelegate(this.ClearOnDestroy);
      }
    }
    base.OnDestroy();
  }

  public void ClearOnDestroy(Path p)
  {
    lock (this.lockObject)
    {
      AstarPath.OnPathPreSearch -= new OnPathDelegate(this.ClearOnDestroy);
      this.waitingForApply = false;
      this.InversePrevious();
    }
  }

  public void InversePrevious()
  {
    this.rnd = new System.Random(this.prevSeed);
    if (this.prevNodes == null)
      return;
    bool flag = false;
    for (int index = this.rnd.Next(this.randomStep); index < this.prevNodes.Length; index += this.rnd.Next(1, this.randomStep))
    {
      if ((long) this.prevNodes[index].Penalty < (long) this.prevPenalty)
      {
        flag = true;
        this.prevNodes[index].Penalty = 0U;
      }
      else
        this.prevNodes[index].Penalty = (uint) ((ulong) this.prevNodes[index].Penalty - (ulong) this.prevPenalty);
    }
    if (!flag)
      return;
    Debug.LogWarning((object) "Penalty for some nodes has been reset while this modifier was active. Penalties might not be correctly set.", (UnityEngine.Object) this);
  }

  public void ApplyNow(Path somePath)
  {
    lock (this.lockObject)
    {
      this.waitingForApply = false;
      AstarPath.OnPathPreSearch -= new OnPathDelegate(this.ApplyNow);
      this.InversePrevious();
      if (this.destroyed)
        return;
      int Seed = this.seedGenerator.Next();
      this.rnd = new System.Random(Seed);
      if (this.toBeApplied != null)
      {
        for (int index = this.rnd.Next(this.randomStep); index < this.toBeApplied.Length; index += this.rnd.Next(1, this.randomStep))
          this.toBeApplied[index].Penalty = (uint) ((ulong) this.toBeApplied[index].Penalty + (ulong) this.penalty);
      }
      this.prevPenalty = this.penalty;
      this.prevSeed = Seed;
      this.prevNodes = this.toBeApplied;
    }
  }
}
