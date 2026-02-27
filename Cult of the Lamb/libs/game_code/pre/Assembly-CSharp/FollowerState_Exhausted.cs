// Decompiled with JetBrains decompiler
// Type: FollowerState_Exhausted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Exhausted : FollowerState
{
  public const int EVERYONE_RECOVERED = -5;
  public const int EVERYONE_NEXT_DAY = -4;
  public const int EVERYONE_WOKEN_UP = -3;
  public const int EVERYONE_EXHAUSED = -2;
  public const int DEFAULT = -1;

  public override FollowerStateType Type => FollowerStateType.Exhausted;

  public override float XPMultiplierAddition => -0.5f;

  public override float MaxSpeed => 1.5f;

  public override string OverrideIdleAnim => "Fatigued/idle-fatigued";

  public override string OverrideWalkAnim => "Fatigued/walk-fatigued";
}
