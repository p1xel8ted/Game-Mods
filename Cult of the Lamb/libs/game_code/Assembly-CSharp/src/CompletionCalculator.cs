// Decompiled with JetBrains decompiler
// Type: src.CompletionCalculator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.BuildMenu;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
namespace src;

public class CompletionCalculator
{
  public static int Calculate(DataManager dataManager)
  {
    // ISSUE: variable of a compiler-generated type
    CompletionCalculator.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass00.currentPoints = 0.0f;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass00.totalPoints = 0.0f;
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DungeonCompleted(FollowerLocation.Dungeon1_1), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DungeonCompleted(FollowerLocation.Dungeon1_2), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DungeonCompleted(FollowerLocation.Dungeon1_3), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DungeonCompleted(FollowerLocation.Dungeon1_4), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DungeonCompleted(FollowerLocation.Dungeon1_1, true), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DungeonCompleted(FollowerLocation.Dungeon1_2, true), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DungeonCompleted(FollowerLocation.Dungeon1_3, true), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DungeonCompleted(FollowerLocation.Dungeon1_4, true), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.BeatenWitnessDungeon1, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.BeatenWitnessDungeon2, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.BeatenWitnessDungeon3, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.BeatenWitnessDungeon4, 1, ref cDisplayClass00);
    if (!dataManager.SurvivalModeActive)
    {
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ability_Eat), 1, ref cDisplayClass00);
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ability_Resurrection), 1, ref cDisplayClass00);
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ability_BlackHeart), 1, ref cDisplayClass00);
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ability_TeleportHome), 1, ref cDisplayClass00);
    }
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(0), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(1), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(2), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(3), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(4), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(5), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(6), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(7), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(8), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(9), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedFleeces.Contains(1000), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DiscoveredLocations.Contains(FollowerLocation.Hub1_RatauOutside), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DiscoveredLocations.Contains(FollowerLocation.Hub1_Sozo), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DiscoveredLocations.Contains(FollowerLocation.Dungeon_Decoration_Shop1), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DiscoveredLocations.Contains(FollowerLocation.Dungeon_Location_4), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DiscoveredLocations.Contains(FollowerLocation.HubShore), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES Types in FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Food))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(StructuresData.GetUnlocked(Types), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES Types in FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Items))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(StructuresData.GetUnlocked(Types), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES Types in FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Misc))
    {
      if (Types != StructureBrain.TYPES.LEADER_TENT)
        CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(StructuresData.GetUnlocked(Types), 1, ref cDisplayClass00);
    }
    foreach (StructureBrain.TYPES allStructure in FaithCategory.AllStructures())
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(StructuresData.GetUnlocked(allStructure), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES sinStructure in FaithCategory.SinStructures())
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(StructuresData.GetUnlocked(sinStructure), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Building_Temple2), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Temple_III), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Temple_IV), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES types in DataManager.DecorationsForType(DataManager.DecorationType.All))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedStructures.Contains(types), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES types in DataManager.DecorationsForType(DataManager.DecorationType.Path))
    {
      switch (types)
      {
        case StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR:
        case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR:
          continue;
        default:
          CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedStructures.Contains(types), 1, ref cDisplayClass00);
          continue;
      }
    }
    foreach (StructureBrain.TYPES types in DataManager.DecorationsForType(DataManager.DecorationType.Dungeon1))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedStructures.Contains(types), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES types in DataManager.DecorationsForType(DataManager.DecorationType.Mushroom))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedStructures.Contains(types), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES types in DataManager.DecorationsForType(DataManager.DecorationType.Crystal))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedStructures.Contains(types), 1, ref cDisplayClass00);
    foreach (StructureBrain.TYPES types in DataManager.DecorationsForType(DataManager.DecorationType.Spider))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedStructures.Contains(types), 1, ref cDisplayClass00);
    UpgradeSystem.Type[] typeArray = new UpgradeSystem.Type[31 /*0x1F*/]
    {
      UpgradeSystem.Type.PUpgrade_Heart_1,
      UpgradeSystem.Type.PUpgrade_WeaponCritHit,
      UpgradeSystem.Type.PUpgrade_WeaponPoison,
      UpgradeSystem.Type.PUpgrade_WeaponFervor,
      UpgradeSystem.Type.PUpgrade_WeaponNecromancy,
      UpgradeSystem.Type.PUpgrade_WeaponHeal,
      UpgradeSystem.Type.PUpgrade_WeaponGodly,
      UpgradeSystem.Type.PUpgrade_Ammo_1,
      UpgradeSystem.Type.PUpgrade_Ammo_2,
      UpgradeSystem.Type.PUpgrade_CursePack2,
      UpgradeSystem.Type.PUpgrade_CursePack5,
      UpgradeSystem.Type.PUpgrade_CursePack1,
      UpgradeSystem.Type.PUpgrade_CursePack4,
      UpgradeSystem.Type.PUpgrade_CursePack3,
      UpgradeSystem.Type.PUpgrade_StartingWeapon_1,
      UpgradeSystem.Type.PUpgrade_Heart_2,
      UpgradeSystem.Type.PUpgrade_StartingWeapon_2,
      UpgradeSystem.Type.PUpgrade_StartingWeapon_3,
      UpgradeSystem.Type.PUpgrade_StartingWeapon_4,
      UpgradeSystem.Type.PUpgrade_StartingWeapon_5,
      UpgradeSystem.Type.PUpgrade_HeavyAttacks,
      UpgradeSystem.Type.PUpgrade_HA_Sword,
      UpgradeSystem.Type.PUpgrade_HA_Axe,
      UpgradeSystem.Type.PUpgrade_HA_Dagger,
      UpgradeSystem.Type.PUpgrade_HA_Hammer,
      UpgradeSystem.Type.PUpgrade_HA_Gauntlets,
      UpgradeSystem.Type.Relic_Pack1,
      UpgradeSystem.Type.Relics_Blessed_1,
      UpgradeSystem.Type.Relics_Dammed_1,
      UpgradeSystem.Type.PUpgrade_ResummonWeapon,
      UpgradeSystem.Type.PUpgrade_HA_Blunderbuss
    };
    foreach (UpgradeSystem.Type type in typeArray)
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(type), 1, ref cDisplayClass00);
    foreach (DoctrineUpgradeSystem.DoctrineType doctrineType in DoctrineUpgradeSystem.GetDoctrinesForCategory(SermonCategory.Afterlife))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DoctrineUnlockedUpgrades.Contains(doctrineType), 1, ref cDisplayClass00);
    foreach (DoctrineUpgradeSystem.DoctrineType doctrineType in DoctrineUpgradeSystem.GetDoctrinesForCategory(SermonCategory.Food))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DoctrineUnlockedUpgrades.Contains(doctrineType), 1, ref cDisplayClass00);
    foreach (DoctrineUpgradeSystem.DoctrineType doctrineType in DoctrineUpgradeSystem.GetDoctrinesForCategory(SermonCategory.LawAndOrder))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DoctrineUnlockedUpgrades.Contains(doctrineType), 1, ref cDisplayClass00);
    foreach (DoctrineUpgradeSystem.DoctrineType doctrineType in DoctrineUpgradeSystem.GetDoctrinesForCategory(SermonCategory.Possession))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DoctrineUnlockedUpgrades.Contains(doctrineType), 1, ref cDisplayClass00);
    foreach (DoctrineUpgradeSystem.DoctrineType doctrineType in DoctrineUpgradeSystem.GetDoctrinesForCategory(SermonCategory.WorkAndWorship))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DoctrineUnlockedUpgrades.Contains(doctrineType), 1, ref cDisplayClass00);
    foreach (DoctrineUpgradeSystem.DoctrineType doctrineType in DoctrineUpgradeSystem.GetDoctrinesForCategory(SermonCategory.Pleasure))
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DoctrineUnlockedUpgrades.Contains(doctrineType), 1, ref cDisplayClass00);
    foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsAll())
    {
      if (skinAndData.DropLocation != WorshipperData.DropLocation.Major_DLC && skinAndData.DropLocation != WorshipperData.DropLocation.Dungeon5 && skinAndData.DropLocation != WorshipperData.DropLocation.Dungeon6 && skinAndData.DropLocation != WorshipperData.DropLocation.DLC && skinAndData.DropLocation != WorshipperData.DropLocation.SpecialEvents && skinAndData.Title != "Night Wolf")
        CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.FollowerSkinsUnlocked.Contains(skinAndData.Skin[0].Skin), 1, ref cDisplayClass00);
    }
    foreach (ClothingData clothingData in TailorManager.ClothingData)
    {
      if ((DataManager.Instance.DLC_Cultist_Pack || !DataManager.Instance.Cultist_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType)) && (DataManager.Instance.DLC_Heretic_Pack || !DataManager.Instance.Heretic_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType)) && (DataManager.Instance.DLC_Pilgrim_Pack || !DataManager.Instance.Pilgrim_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType)) && !DataManager.Instance.Major_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType) && (DataManager.Instance.DLC_Sinful_Pack || !DataManager.Instance.Sinful_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType)))
        CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType), 1, ref cDisplayClass00);
    }
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ritual_FirePit), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ritual_Sacrifice), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ritual_Brainwashing), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.Lighthouse_Lit, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.SozoNoLongerBrainwashed, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.HasReturnedAym, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.HasReturnedBaal, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.CurrentFoxEncounter >= 1, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.CurrentFoxEncounter >= 2, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.CurrentFoxEncounter >= 3, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.CurrentFoxEncounter >= 4, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.RatooGivenHeart, 1, ref cDisplayClass00);
    if (!dataManager.RatauKilled)
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.Knucklebones_Opponent_Ratau_Won, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.Knucklebones_Opponent_0_Won, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.Knucklebones_Opponent_1_Won, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.Knucklebones_Opponent_2_Won, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.DeathCatBeaten, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.HasDeathCatFollower(), 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.LeshyHealed, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.HeketHealed, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.KallamarHealed, 1, ref cDisplayClass00);
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.ShamuraHealed, 1, ref cDisplayClass00);
    foreach (TarotCards.Card allTrinket in DataManager.AllTrinkets)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(allTrinket) && !TarotCards.MajorDLCCards.Contains<TarotCards.Card>(allTrinket))
        CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.PlayerFoundTrinkets.Contains(allTrinket), 1, ref cDisplayClass00);
    }
    foreach (RelicData relicData in EquipmentManager.RelicData)
    {
      if (!EquipmentManager.CoopRelics.Contains(relicData.RelicType) && !EquipmentManager.MajorDLCRelics.Contains(relicData.RelicType))
        CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.PlayerFoundRelics.Contains(relicData.RelicType), 1, ref cDisplayClass00);
    }
    CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.ChoreXPLevel >= 9, 1, ref cDisplayClass00);
    for (int index = 0; index < 10; ++index)
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.TempleLevel > index, 1, ref cDisplayClass00);
    for (int index = 0; index < 15; ++index)
      CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(dataManager.LoreUnlocked.Contains(index), 1, ref cDisplayClass00);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    int num1 = (int) Mathf.Floor((float) ((double) cDisplayClass00.currentPoints / (double) cDisplayClass00.totalPoints * 100.0));
    // ISSUE: reference to a compiler-generated field
    cDisplayClass00.currentPoints = 0.0f;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass00.totalPoints = 0.0f;
    int num2 = 0;
    if (num1 >= 100)
    {
      for (int index1 = 0; index1 < 2; ++index1)
      {
        for (int index2 = 0; index2 < 10; ++index2)
        {
          for (int index3 = 0; index3 < 6; ++index3)
            CompletionCalculator.\u003CCalculate\u003Eg__AddPoint\u007C0_0(DungeonSandboxManager.GetProgressionForScenario((DungeonSandboxManager.ScenarioType) index1, (PlayerFleeceManager.FleeceType) index2).CompletedRuns > index3, 1, ref cDisplayClass00);
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      num2 = (int) Mathf.Floor((float) ((double) cDisplayClass00.currentPoints / (double) cDisplayClass00.totalPoints * 100.0 * 0.10000000149011612));
    }
    return num1 + num2;
  }

  public static int CalculateDLC(DataManager dataManager)
  {
    // ISSUE: variable of a compiler-generated type
    CompletionCalculator.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass10.currentPoints = 0.0f;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass10.totalPoints = 0.0f;
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.BeatenExecutioner, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.BeatenWolf, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.BeatenYngya, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(666), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(667), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(668), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(669), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(670), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(671), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(672), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(673), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(674), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(675), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(678), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(679), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(681), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedFleeces.Contains(12) || dataManager.UnlockedFleeces.Contains(11), 1, ref cDisplayClass10);
    foreach (StructureBrain.TYPES majorDlcStructure in DataManager.MajorDLCStructures)
      CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(StructuresData.GetUnlocked(majorDlcStructure), 1, ref cDisplayClass10);
    foreach (StructureBrain.TYPES types in DataManager.DecorationsForType(DataManager.DecorationType.Major_DLC))
      CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedStructures.Contains(types), 1, ref cDisplayClass10);
    foreach (DoctrineUpgradeSystem.DoctrineType doctrineType in DoctrineUpgradeSystem.GetDoctrinesForCategory(SermonCategory.Winter))
      CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.DoctrineUnlockedUpgrades.Contains(doctrineType), 1, ref cDisplayClass10);
    foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsAll())
    {
      if (skinAndData.DropLocation == WorshipperData.DropLocation.Major_DLC || skinAndData.DropLocation == WorshipperData.DropLocation.Dungeon5 || skinAndData.DropLocation == WorshipperData.DropLocation.Dungeon6)
        CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.FollowerSkinsUnlocked.Contains(skinAndData.Skin[0].Skin), 1, ref cDisplayClass10);
    }
    foreach (ClothingData clothingData in TailorManager.ClothingData)
    {
      if (DataManager.Instance.Major_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType))
        CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType), 1, ref cDisplayClass10);
    }
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ritual_Snowman), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.CompletedMidasFollowerQuest, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.BlizzardOfferingsCompleted >= 5, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.TookBopToTailor, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.CompletedDecoJobBoard, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.CompletedTarotJobBoard, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.CompletedBlacksmithJobBoard, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.CompletedFlockadeJobBoard, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.CompletedRanchingJobBoard, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.CompletedGraveyardJobBoard, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.LandPurchased >= 0, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.LandPurchased >= 1, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.LandPurchased >= 2, 1, ref cDisplayClass10);
    for (int index = 0; index < 36; ++index)
      CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.PlayerFoundPieces.Count > index, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ability_WinterChoice), 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.RancherShopFixed, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.BlacksmithShopFixed, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.DecoShopFixed, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.TarotShopFixed, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.GraveyardShopFixed, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.FlockadeShopFixed, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.WinterServerity >= 1, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.WinterServerity >= 2, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.WinterServerity >= 3, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.RepairedLegendaryAxe, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.RepairedLegendaryBlunderbuss, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.RepairedLegendaryChains, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.RepairedLegendaryDagger, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.RepairedLegendaryGauntlet, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.RepairedLegendaryHammer, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(DataManager.Instance.RepairedLegendarySword, 1, ref cDisplayClass10);
    CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.WeaponPool.Contains(EquipmentType.Sword_Ratau), 1, ref cDisplayClass10);
    foreach (TarotCards.Card allTrinket in DataManager.AllTrinkets)
    {
      if (TarotCards.MajorDLCCards.Contains<TarotCards.Card>(allTrinket))
        CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.PlayerFoundTrinkets.Contains(allTrinket), 1, ref cDisplayClass10);
    }
    foreach (RelicData relicData in EquipmentManager.RelicData)
    {
      if (EquipmentManager.MajorDLCRelics.Contains(relicData.RelicType))
        CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.PlayerFoundRelics.Contains(relicData.RelicType), 1, ref cDisplayClass10);
    }
    for (int index = 15; index < LoreSystem.LoreCount; ++index)
      CompletionCalculator.\u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(dataManager.LoreUnlocked.Contains(index), 1, ref cDisplayClass10);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return (int) Mathf.Floor((float) ((double) cDisplayClass10.currentPoints / (double) cDisplayClass10.totalPoints * 100.0));
  }

  [CompilerGenerated]
  public static void \u003CCalculate\u003Eg__AddPoint\u007C0_0(
    bool condition,
    int weight = 1,
    [In] ref CompletionCalculator.\u003C\u003Ec__DisplayClass0_0 obj2)
  {
    if (condition)
    {
      // ISSUE: reference to a compiler-generated field
      obj2.currentPoints += (float) weight;
    }
    // ISSUE: reference to a compiler-generated field
    obj2.totalPoints += (float) weight;
  }

  [CompilerGenerated]
  public static void \u003CCalculateDLC\u003Eg__AddPoint\u007C1_0(
    bool condition,
    int weight = 1,
    [In] ref CompletionCalculator.\u003C\u003Ec__DisplayClass1_0 obj2)
  {
    if (condition)
    {
      // ISSUE: reference to a compiler-generated field
      obj2.currentPoints += (float) weight;
    }
    // ISSUE: reference to a compiler-generated field
    obj2.totalPoints += (float) weight;
  }
}
