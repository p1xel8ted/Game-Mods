// Decompiled with JetBrains decompiler
// Type: FollowerState_Midas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Midas : FollowerState
{
  public override string OverrideWalkAnim => "Midas/run-away";

  public override float MaxSpeed => 2f;

  public override FollowerStateType Type => FollowerStateType.Default;
}
