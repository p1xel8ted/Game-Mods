// Decompiled with JetBrains decompiler
// Type: FollowerState_ExistentialDread
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_ExistentialDread : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.ExistentialDread;

  public override float XPMultiplierAddition => -0.5f;

  public override float MaxSpeed => 0.5f;

  public override string OverrideIdleAnim => "Existential Dread/dread-idle";

  public override string OverrideWalkAnim => "Existential Dread/dread-walk";
}
