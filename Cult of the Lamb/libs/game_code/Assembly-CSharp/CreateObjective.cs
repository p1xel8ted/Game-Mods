// Decompiled with JetBrains decompiler
// Type: CreateObjective
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
      ObjectiveManager.Add(CreateObjective.GetObjective(objective, this.GroupName), true, objective.IsMajorDLC);
  }

  public static ObjectivesData GetObjective(
    CreateObjective.ObjectiveToGive Objective,
    string GroupName)
  {
    switch (Objective.Quest)
    {
      case CreateObjective.ObjectiveToGive.QuestType.CollectItem:
        return (ObjectivesData) new Objectives_CollectItem(GroupName, Objective.Item, Objective.Count);
      case CreateObjective.ObjectiveToGive.QuestType.BuildStructure:
        return (ObjectivesData) new Objectives_BuildStructure(GroupName, Objective.Structure, includeAlreadyBuilt: Objective.IncludeStructuresAlreadyBuilt);
      case CreateObjective.ObjectiveToGive.QuestType.RecruitFollower:
        return (ObjectivesData) new Objectives_RecruitFollower(GroupName);
      case CreateObjective.ObjectiveToGive.QuestType.DepositFood:
        return (ObjectivesData) new Objectives_DepositFood(GroupName);
      case CreateObjective.ObjectiveToGive.QuestType.Custom:
        return (ObjectivesData) new Objectives_Custom(GroupName, Objective.CustomQuestType);
      case CreateObjective.ObjectiveToGive.QuestType.PlaceDecoration:
        return (ObjectivesData) new Objectives_PlaceStructure(GroupName, StructureBrain.Categories.AESTHETIC, Objective.DecorationType, Objective.DecorationCount, -1f, Objective.IncludeDecosAlreadyBuilt);
      case CreateObjective.ObjectiveToGive.QuestType.Knucklebones:
        return (ObjectivesData) new Objectives_DefeatKnucklebones(GroupName, Objective.OpponentName);
      case CreateObjective.ObjectiveToGive.QuestType.GetAnimal:
        return (ObjectivesData) new Objectives_GetAnimal(GroupName, Objective.Animal, Objective.Level);
      case CreateObjective.ObjectiveToGive.QuestType.ShowFleece:
        return (ObjectivesData) new Objectives_ShowFleece(GroupName, Objective.Fleece);
      case CreateObjective.ObjectiveToGive.QuestType.LegendaryWeaponRun:
        return (ObjectivesData) new Objectives_LegendaryWeaponRun(GroupName, Objective.legendaryWeapon);
      case CreateObjective.ObjectiveToGive.QuestType.FindChildren:
        return (ObjectivesData) new Objectives_FindChildren(GroupName, Objective.ChildLocation);
      case CreateObjective.ObjectiveToGive.QuestType.WinFlockadeBet:
        return (ObjectivesData) new Objectives_WinFlockadeBet(GroupName, Objective.FlockadeOpponentName, Objective.MinimumWoolBet, Objective.WoolWonCountVariable);
      default:
        return (ObjectivesData) null;
    }
  }

  [Serializable]
  public class ObjectiveToGive
  {
    public CreateObjective.ObjectiveToGive.QuestType Quest;
    public Objectives_FindChildren.ChildLocation ChildLocation;
    public PlayerFleeceManager.FleeceType Fleece;
    public InventoryItem.ITEM_TYPE Item;
    public StructureBrain.TYPES Structure;
    public bool IncludeStructuresAlreadyBuilt;
    public int Count = 10;
    public global::Objectives.CustomQuestTypes CustomQuestType;
    public Objectives_PlaceStructure.DecorationType DecorationType;
    public int DecorationCount = 1;
    public bool IncludeDecosAlreadyBuilt;
    public EquipmentType legendaryWeapon;
    public InventoryItem.ITEM_TYPE Animal;
    public int Level;
    public bool ContainsSubQuest;
    public List<CreateObjective.ObjectiveToGive> SubObjectives = new List<CreateObjective.ObjectiveToGive>();
    [TermsPopup("")]
    public string OpponentName;
    [TermsPopup("")]
    public string FlockadeOpponentName;
    public int MinimumWoolBet = 1;
    public DataManager.Variables WoolWonCountVariable;
    public bool IsMajorDLC;

    public enum QuestType
    {
      CollectItem,
      BuildStructure,
      RecruitFollower,
      DepositFood,
      Custom,
      PlaceDecoration,
      Knucklebones,
      GetAnimal,
      ShowFleece,
      LegendaryWeaponRun,
      FindChildren,
      WinFlockadeBet,
    }
  }
}
