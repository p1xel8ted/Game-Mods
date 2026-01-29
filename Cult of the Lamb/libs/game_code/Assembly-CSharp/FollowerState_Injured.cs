// Decompiled with JetBrains decompiler
// Type: FollowerState_Injured
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Injured : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Injured;

  public override float XPMultiplierAddition => -0.5f;

  public override float MaxSpeed => 0.75f;

  public override string OverrideIdleAnim => "Injured/idle";

  public override string OverrideWalkAnim => "Injured/walk";
}
