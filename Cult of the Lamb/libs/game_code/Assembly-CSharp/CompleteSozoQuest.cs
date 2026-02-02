// Decompiled with JetBrains decompiler
// Type: CompleteSozoQuest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CompleteSozoQuest : MonoBehaviour
{
  public Structure Structure;

  public void OnEnable() => this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);

  public void OnDisable() => this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);

  public void OnBrainAssigned()
  {
    if (DataManager.Instance.BuiltMushroomDecoration)
      return;
    DataManager.Instance.BuiltMushroomDecoration = true;
    DataManager.Instance.SozoDecorationQuestActive = false;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Objectives.CustomQuestTypes.SozoReturn));
  }
}
