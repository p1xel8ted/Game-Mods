// Decompiled with JetBrains decompiler
// Type: FollowerState_Following
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Following : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Following;

  public override float MaxSpeed => 3f;

  public override string OverrideWalkAnim => "run-fast";
}
