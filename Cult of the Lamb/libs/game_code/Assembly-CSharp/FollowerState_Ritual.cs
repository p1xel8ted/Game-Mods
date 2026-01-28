// Decompiled with JetBrains decompiler
// Type: FollowerState_Ritual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Ritual : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Ritual;

  public override float MaxSpeed => 1.5f;

  public override string OverrideIdleAnim => "pray";

  public override string OverrideWalkAnim => "walk-hooded-ritual";
}
