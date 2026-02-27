// Decompiled with JetBrains decompiler
// Type: CreateObjective
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CreateObjective : BaseMonoBehaviour
{
  [TermsPopup("")]
  public string GroupName;
  public List<CreateObjective.ObjectiveToGive> Objectives = new List<CreateObjective.ObjectiveToGive>();

  public void Play()
  {
    Debug.Log((object) "CREATE OBJECTIVE!");
    foreach (CreateObjective.ObjectiveToGive objective in this.Objectives)
    {
      switch (objective.Quest)
      {
        case CreateObjective.ObjectiveToGive.QuestType.CollectItem:
          ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem(this.GroupName, objective.Item, objective.Count));
          continue;
        case CreateObjective.ObjectiveToGive.QuestType.BuildStructure:
          ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure(this.GroupName, objective.Structure));
          continue;
        case CreateObjective.ObjectiveToGive.QuestType.RecruitFollower:
          ObjectiveManager.Add((ObjectivesData) new Objectives_RecruitFollower(this.GroupName));
          continue;
        case CreateObjective.ObjectiveToGive.QuestType.DepositFood:
          ObjectiveManager.Add((ObjectivesData) new Objectives_DepositFood(this.GroupName));
          continue;
        case CreateObjective.ObjectiveToGive.QuestType.Custom:
          Debug.Log((object) "CUSTOM!");
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom(this.GroupName, objective.CustomQuestType));
          continue;
        case CreateObjective.ObjectiveToGive.QuestType.PlaceDecoration:
          ObjectiveManager.Add((ObjectivesData) new Objectives_PlaceStructure(this.GroupName, StructureBrain.Categories.AESTHETIC, objective.DecorationCount, -1f));
          continue;
        case CreateObjective.ObjectiveToGive.QuestType.Knucklebones:
          ObjectiveManager.Add((ObjectivesData) new Objectives_DefeatKnucklebones(this.GroupName, objective.OpponentName));
          continue;
        default:
          continue;
      }
    }
  }

  [Serializable]
  public class ObjectiveToGive
  {
    public CreateObjective.ObjectiveToGive.QuestType Quest;
    public InventoryItem.ITEM_TYPE Item;
    public StructureBrain.TYPES Structure;
    public int Count = 10;
    public global::Objectives.CustomQuestTypes CustomQuestType;
    public int DecorationCount = 1;
    public bool ContainsSubQuest;
    public List<CreateObjective.ObjectiveToGive> SubObjectives = new List<CreateObjective.ObjectiveToGive>();
    [TermsPopup("")]
    public string OpponentName;

    public enum QuestType
    {
      CollectItem,
      BuildStructure,
      RecruitFollower,
      DepositFood,
      Custom,
      PlaceDecoration,
      Knucklebones,
    }
  }
}
