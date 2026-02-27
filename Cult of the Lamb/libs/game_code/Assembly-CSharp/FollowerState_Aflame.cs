// Decompiled with JetBrains decompiler
// Type: FollowerState_Aflame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
