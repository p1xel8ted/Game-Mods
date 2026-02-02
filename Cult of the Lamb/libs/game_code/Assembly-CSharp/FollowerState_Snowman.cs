// Decompiled with JetBrains decompiler
// Type: FollowerState_Snowman
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerState_Snowman : FollowerState
{
  public int Variant;

  public override FollowerStateType Type => FollowerStateType.Snowman;

  public override float MaxSpeed
  {
    get
    {
      if (this.brain.HasTrait(FollowerTrait.TraitType.MasterfulSnowman))
        return 1.5f;
      return this.brain.HasTrait(FollowerTrait.TraitType.ShoddySnowman) ? 0.8f : 1f;
    }
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.Variant = Random.Range(0, 3);
  }

  public override string OverrideIdleAnim
  {
    get
    {
      if (this.Variant == 0)
        return "Snowman/idle";
      if (this.Variant == 1)
        return "Snowman/idle2";
      return this.brain._directInfoAccess.Special == FollowerSpecialType.Snowman_Bad ? "Snowman/idle-head-small" : "Snowman/idle-head-big";
    }
  }

  public override string OverrideWalkAnim
  {
    get => this.Variant <= 0 ? "Snow/walk-smile" : "Snow/walk-sad";
  }
}
