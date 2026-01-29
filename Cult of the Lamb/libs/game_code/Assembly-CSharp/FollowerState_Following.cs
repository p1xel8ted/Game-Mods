// Decompiled with JetBrains decompiler
// Type: FollowerState_Following
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Following : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Following;

  public override float MaxSpeed => 3f;

  public override string OverrideWalkAnim => "run-fast";
}
