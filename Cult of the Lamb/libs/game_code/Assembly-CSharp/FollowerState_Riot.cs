// Decompiled with JetBrains decompiler
// Type: FollowerState_Riot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerState_Riot : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Nude;

  public override string OverrideIdleAnim => "Riot/riot-purge-idle" + Random.Range(1, 5).ToString();

  public override string OverrideWalkAnim
  {
    get => Random.Range(1, 5) > 2 ? "Riot/riot-purge-run2" : "Riot/riot-purge-run1";
  }
}
