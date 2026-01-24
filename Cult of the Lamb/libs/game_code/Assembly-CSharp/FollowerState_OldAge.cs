// Decompiled with JetBrains decompiler
// Type: FollowerState_OldAge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_OldAge : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.OldAge;

  public override float MaxSpeed => 0.5f;

  public override string OverrideIdleAnim => "Old/idle-old";

  public override string OverrideWalkAnim => "Old/walk-old";
}
