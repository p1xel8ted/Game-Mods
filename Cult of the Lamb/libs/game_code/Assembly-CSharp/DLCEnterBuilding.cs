// Decompiled with JetBrains decompiler
// Type: DLCEnterBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
