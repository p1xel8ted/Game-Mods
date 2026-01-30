// Decompiled with JetBrains decompiler
// Type: FollowerState_Freezing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Freezing : FollowerState
{
  public float SpeedMultiplier = 1f;

  public override FollowerStateType Type => FollowerStateType.Freezing;

  public override float XPMultiplierAddition => -0.5f;

  public override float MaxSpeed => 0.75f * this.SpeedMultiplier;

  public override string OverrideIdleAnim => "Freezing/idle";

  public override string OverrideWalkAnim => "Freezing/walk";
}
