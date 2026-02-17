// Decompiled with JetBrains decompiler
// Type: FollowerState_ChildZombie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerState_ChildZombie : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.ChildZombie;

  public override float MaxSpeed => 0.35f;

  public override string OverrideWalkAnim
  {
    get
    {
      return this.brain != null && this.brain.Info != null && this.brain.Info.Age < 10 ? "Baby/Baby-zombie/baby-crawl-zombie" : "Baby/Baby-zombie/baby-walk-zombie";
    }
  }

  public override string OverrideIdleAnim
  {
    get
    {
      if (this.brain == null || this.brain.Info == null || this.brain.Info.Age >= 10)
        return "Baby/Baby-zombie/baby-idle-stand-zombie";
      if ((double) Random.value < 0.5)
        return "Baby/Baby-zombie/baby-idle-zombie";
      switch (Random.Range(0, 3))
      {
        case 0:
          return "Baby/Baby-zombie/baby-idle-sit-zombie";
        case 1:
          return "Baby/baby-idle-sit2";
        default:
          return "Baby/baby-idle-sit-swing";
      }
    }
  }
}
