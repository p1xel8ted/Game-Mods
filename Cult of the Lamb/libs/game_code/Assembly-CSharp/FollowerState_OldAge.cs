// Decompiled with JetBrains decompiler
// Type: FollowerState_OldAge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_OldAge : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.OldAge;

  public override float MaxSpeed => 0.5f;

  public override string OverrideIdleAnim => "Old/idle-old";

  public override string OverrideWalkAnim => "Old/walk-old";
}
