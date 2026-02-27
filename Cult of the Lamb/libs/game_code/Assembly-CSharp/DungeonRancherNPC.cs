// Decompiled with JetBrains decompiler
// Type: DungeonRancherNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
