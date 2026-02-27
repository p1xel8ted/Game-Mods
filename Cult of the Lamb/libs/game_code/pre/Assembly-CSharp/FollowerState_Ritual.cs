// Decompiled with JetBrains decompiler
// Type: FollowerState_Ritual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Ritual : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Ritual;

  public override float MaxSpeed => 1.5f;

  public override string OverrideIdleAnim => "pray";

  public override string OverrideWalkAnim => "walk-hooded-ritual";
}
