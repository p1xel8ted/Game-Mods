// Decompiled with JetBrains decompiler
// Type: FollowerState_Aflame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Aflame : FollowerState
{
  public float SpeedMultiplier = 1f;

  public override FollowerStateType Type => FollowerStateType.Aflame;

  public override float XPMultiplierAddition => -0.5f;

  public override float MaxSpeed => 2f * this.SpeedMultiplier;

  public override string OverrideIdleAnim => "Insane/idle-insane";

  public override string OverrideWalkAnim => "Insane/run-insane";
}
