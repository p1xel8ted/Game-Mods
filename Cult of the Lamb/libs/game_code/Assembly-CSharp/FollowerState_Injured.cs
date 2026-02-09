// Decompiled with JetBrains decompiler
// Type: FollowerState_Injured
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
