// Decompiled with JetBrains decompiler
// Type: FollowerState_Midas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Midas : FollowerState
{
  public override string OverrideWalkAnim => "Midas/run-away";

  public override float MaxSpeed => 2f;

  public override FollowerStateType Type => FollowerStateType.Default;
}
