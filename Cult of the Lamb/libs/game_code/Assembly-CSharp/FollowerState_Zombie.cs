// Decompiled with JetBrains decompiler
// Type: FollowerState_Zombie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerState_Zombie : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Zombie;

  public override float XPMultiplierAddition => -0.5f;

  public override float MaxSpeed => 0.5f;

  public override string OverrideIdleAnim => "Zombie/zombie-idle";

  public override string OverrideWalkAnim
  {
    get => (double) Random.value >= 0.5 ? "Zombie/zombie-walk" : "Zombie/zombie-walk-limp";
  }
}
