// Decompiled with JetBrains decompiler
// Type: CompleteSozoQuest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CompleteSozoQuest : MonoBehaviour
{
  public Structure Structure;

  private void OnEnable() => this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);

  private void OnDisable() => this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);

  private void OnBrainAssigned()
  {
    if (DataManager.Instance.BuiltMushroomDecoration)
      return;
    DataManager.Instance.BuiltMushroomDecoration = true;
    DataManager.Instance.SozoDecorationQuestActive = false;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Objectives.CustomQuestTypes.SozoReturn));
  }
}
