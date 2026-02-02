// Decompiled with JetBrains decompiler
// Type: DungeonRancherNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
