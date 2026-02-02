// Decompiled with JetBrains decompiler
// Type: FollowerState_Following
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Following : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Following;

  public override float MaxSpeed => 3f;

  public override string OverrideWalkAnim => "run-fast";
}
