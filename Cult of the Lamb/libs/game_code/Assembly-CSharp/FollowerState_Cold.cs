// Decompiled with JetBrains decompiler
// Type: FollowerState_Cold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Cold : FollowerState
{
  public int Variant;

  public override FollowerStateType Type => FollowerStateType.Cold;

  public override float MaxSpeed => 1f;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (follower.Brain.CanFreeze())
      this.Variant = 1;
    else
      this.Variant = 0;
  }

  public override string OverrideIdleAnim
  {
    get => this.Variant <= 0 ? "Snow/idle-smile" : "Snow/idle-sad";
  }

  public override string OverrideWalkAnim
  {
    get => this.Variant <= 0 ? "Snow/walk-smile" : "Snow/walk-sad";
  }
}
