// Decompiled with JetBrains decompiler
// Type: DungeonRancherNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DungeonRancherNPC : MonoBehaviour
{
  public void AddAlert()
  {
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnLambTown))
      return;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitLambTown", Objectives.CustomQuestTypes.ReturnLambTown), true, true);
  }
}
