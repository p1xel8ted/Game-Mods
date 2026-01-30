// Decompiled with JetBrains decompiler
// Type: DLCEnterBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DLCEnterBuilding : EnterBuilding
{
  public override void OnTriggerEnter2D(Collider2D collision)
  {
    base.OnTriggerEnter2D(collision);
    this.playerFarming = collision.GetComponent<PlayerFarming>();
    if (!((Object) this.playerFarming != (Object) null))
      return;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.VisitLambTown);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnLambTown);
  }
}
