// Decompiled with JetBrains decompiler
// Type: FollowerState_Child
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerState_Child : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Child;

  public override float MaxSpeed => 0.5f;

  public override string OverrideWalkAnim
  {
    get
    {
      return this.brain != null && this.brain.Info != null && this.brain.Info.Age < 10 ? this.GetCrawlAnim() : this.GetWalkAnim();
    }
  }

  public override string OverrideIdleAnim
  {
    get
    {
      if (this.brain == null || this.brain.Info == null || this.brain.Info.Age >= 10)
        return this.GetIdleStandAnim();
      return (double) Random.value < 0.5 ? this.GetIdleAnim() : this.GetIdleSitAnim();
    }
  }

  public string GetCrawlAnim()
  {
    string crawlAnim = "Baby/baby-crawl";
    if (this.follower.IsBabyAngry())
      crawlAnim = "Baby/Baby-angry/baby-crawl-angry";
    else if (this.follower.IsBabySad())
      crawlAnim = "Baby/Baby-sad/baby-crawl-sad";
    return crawlAnim;
  }

  public string GetWalkAnim()
  {
    string walkAnim = "Baby/baby-walk";
    if ((bool) (Object) this.follower)
    {
      if (this.follower.IsBabyAngry())
        walkAnim = "Baby/Baby-angry/baby-walk-angry";
      else if (this.follower.IsBabySad())
        walkAnim = "Baby/Baby-sad/baby-walk-sad";
    }
    return walkAnim;
  }

  public string GetIdleAnim()
  {
    string idleAnim = "Baby/baby-idle";
    if ((bool) (Object) this.follower)
    {
      if (this.follower.IsBabyAngry())
        idleAnim = "Baby/Baby-angry/baby-idle-angry";
      else if (this.follower.IsBabySad())
        idleAnim = "Baby/Baby-sad/baby-idle-sad";
    }
    return idleAnim;
  }

  public string GetIdleSitAnim()
  {
    string[] strArray1 = new string[3]
    {
      "Baby/baby-idle-sit",
      "Baby/baby-idle-sit2",
      "Baby/baby-idle-sit-swing"
    };
    string[] strArray2 = new string[2]
    {
      "Baby/Baby-angry/baby-idle-sit-angry",
      "Baby/Baby-angry/baby-angry-tantrum"
    };
    string[] strArray3 = new string[2]
    {
      "Baby/Baby-sad/baby-idle-sit-sad",
      "Baby/Baby-sad/baby-idle-sit-sad-tantrum"
    };
    if (this.follower.IsBabyAngry())
      return strArray2[Random.Range(0, strArray2.Length)];
    return this.follower.IsBabySad() ? strArray3[Random.Range(0, strArray3.Length)] : strArray1[Random.Range(0, strArray1.Length)];
  }

  public string GetIdleStandAnim()
  {
    string idleStandAnim = "Baby/baby-idle-stand";
    if ((Object) this.follower != (Object) null)
    {
      if (this.follower.IsBabyAngry())
        idleStandAnim = "Baby/Baby-angry/baby-idle-stand-angry";
      else if (this.follower.IsBabySad())
        idleStandAnim = "Baby/Baby-sad/baby-idle-stand-sad";
    }
    return idleStandAnim;
  }
}
