// Decompiled with JetBrains decompiler
// Type: ObjectivesDataFinalized
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Union(0, typeof (Objective_FindRelic.FinalizedData_FindRelic))]
[Union(1, typeof (Objectives_AssignClothing.FinalizedData_AssignClothing))]
[Union(2, typeof (Objectives_BedRest.FinalizedData_BedRest))]
[Union(3, typeof (Objectives_BlizzardOffering.FinalizedData_BlizzardOffering))]
[Union(4, typeof (Objectives_BuildStructure.FinalizedData_BuildStructure))]
[Union(5, typeof (Objectives_CollectItem.FinalizedData_CollectItem))]
[Union(6, typeof (Objectives_CookMeal.FinalizedData_CookMeal))]
[Union(7, typeof (Objectives_CraftClothing.FinalizedData_CraftClothing))]
[Union(8, typeof (Objectives_Custom.FinalizedData_Custom))]
[Union(9, typeof (Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones))]
[Union(10, typeof (Objectives_DepositFood.FinalizedData_DepositFood))]
[Union(11, typeof (Objectives_Drink.FinalizedData_Drink))]
[Union(12, typeof (Objectives_EatMeal.FinalizedData_EatMeal))]
[Union(13, typeof (Objectives_FindChildren.FinalizedData_FindChildren))]
[Union(14, typeof (Objectives_FindFollower.FinalizedData_FindFollower))]
[Union(15, typeof (Objectives_FinishRace.FinalizedData_Objectives_FinishRace))]
[Union(16 /*0x10*/, typeof (Objectives_FlowerBaskets.FinalizedData_FlowerBaskets))]
[Union(17, typeof (Objectives_GetAnimal.FinalizedData_GetAnimal))]
[Union(18, typeof (Objectives_GiveItem.FinalizedData_GiveItem))]
[Union(19, typeof (Objectives_KillEnemies.FinalizedData_KillEnemies))]
[Union(20, typeof (Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun))]
[Union(21, typeof (Objectives_Mating.FinalizedData_Mating))]
[Union(22, typeof (Objectives_PerformRitual.FinalizedData_PerformRitual))]
[Union(23, typeof (Objectives_PlaceStructure.FinalizedData_PlaceStructure))]
[Union(24, typeof (Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower))]
[Union(25, typeof (Objectives_RecruitFollower.FinalizedData_RecruitFollower))]
[Union(26, typeof (Objectives_RemoveStructure.FinalizedData_RemoveStructure))]
[Union(27, typeof (Objectives_ShootDummy.FinalizedData_ShootDummy))]
[Union(28, typeof (Objectives_ShowFleece.FinalizedData_ShowFleece))]
[Union(29, typeof (Objectives_Story.FinalizedData))]
[Union(30, typeof (Objectives_TalkToFollower.FinalizedData_TalkToFollower))]
[Union(31 /*0x1F*/, typeof (Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade))]
[Union(32 /*0x20*/, typeof (Objectives_UseRelic.FinalizedData_UseRelic))]
[Union(33, typeof (Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet))]
[Union(34, typeof (Objectives_NoDodge.FinalizedData_NoDodge))]
[Union(35, typeof (Objectives_NoDamage.FinalizedData_NoDamage))]
[Union(36, typeof (Objectives_NoCurses.FinalizedData_NoCurses))]
[Union(37, typeof (Objectives_NoHealing.FinalizedData_NoHealing))]
[Union(38, typeof (Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations))]
[Union(39, typeof (Objectives_FeedAnimal.FinalizedData_FeedAnimal))]
[Union(40, typeof (Objectives_WalkAnimal.FinalizedData_WalkAnimal))]
[Union(41, typeof (Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn))]
[Serializable]
public abstract class ObjectivesDataFinalized
{
  [Key(0)]
  public string GroupId;
  [Key(1)]
  public int Index;
  [Key(2)]
  public string UniqueGroupID;

  public abstract string GetText();
}
