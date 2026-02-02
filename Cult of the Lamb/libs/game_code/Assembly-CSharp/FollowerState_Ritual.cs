// Decompiled with JetBrains decompiler
// Type: FollowerState_Ritual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Ritual : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Ritual;

  public override float MaxSpeed => 1.5f;

  public override string OverrideIdleAnim => "pray";

  public override string OverrideWalkAnim => "walk-hooded-ritual";
}
