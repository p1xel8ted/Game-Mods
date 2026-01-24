// Decompiled with JetBrains decompiler
// Type: DLCEnterBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
