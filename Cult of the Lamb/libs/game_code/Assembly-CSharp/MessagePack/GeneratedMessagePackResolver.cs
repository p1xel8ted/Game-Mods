// Decompiled with JetBrains decompiler
// Type: MessagePack.GeneratedMessagePackResolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.Serialization;
using Flockade;
using Lamb.UI;
using MessagePack.Formatters;
using MessagePack.Internal;
using src.Alerts;
using src.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace MessagePack;

public class GeneratedMessagePackResolver : IFormatterResolver
{
  public static IFormatterResolver Instance = (IFormatterResolver) new GeneratedMessagePackResolver();

  public IMessagePackFormatter<T> GetFormatter<T>()
  {
    return GeneratedMessagePackResolver.FormatterCache<T>.Formatter;
  }

  public static class FormatterCache<T>
  {
    public static IMessagePackFormatter<T> Formatter;

    static FormatterCache()
    {
      object formatter = GeneratedMessagePackResolver.GeneratedMessagePackResolverGetFormatterHelper.GetFormatter(typeof (T));
      if (formatter == null)
        return;
      GeneratedMessagePackResolver.FormatterCache<T>.Formatter = (IMessagePackFormatter<T>) formatter;
    }
  }

  public static class GeneratedMessagePackResolverGetFormatterHelper
  {
    public static Dictionary<System.Type, int> closedTypeLookup = new Dictionary<System.Type, int>(310)
    {
      {
        typeof (BuyEntry[]),
        0
      },
      {
        typeof (global::InventoryItem[]),
        1
      },
      {
        typeof (global::StructuresData.Ranchable_Animal[]),
        2
      },
      {
        typeof (Dictionary<global::StructureBrain.TYPES, int>),
        3
      },
      {
        typeof (Dictionary<int, int>),
        4
      },
      {
        typeof (List<global::BluePrint>),
        5
      },
      {
        typeof (List<BuyEntry>),
        6
      },
      {
        typeof (List<global::CrownAbilities>),
        7
      },
      {
        typeof (List<DayObject>),
        8
      },
      {
        typeof (List<FollowerInfo>),
        9
      },
      {
        typeof (List<global::IDAndRelationship>),
        10
      },
      {
        typeof (List<global::InventoryItem>),
        11
      },
      {
        typeof (List<JellyFishInvestmentDay>),
        12
      },
      {
        typeof (List<ObjectivesData>),
        13
      },
      {
        typeof (List<ObjectivesDataFinalized>),
        14
      },
      {
        typeof (List<ShopLocationTracker>),
        15
      },
      {
        typeof (List<ShrineUsageInfo>),
        16 /*0x10*/
      },
      {
        typeof (List<StoryData>),
        17
      },
      {
        typeof (List<StoryDataItem>),
        18
      },
      {
        typeof (List<global::StructureAndTime>),
        19
      },
      {
        typeof (List<StructureEffect>),
        20
      },
      {
        typeof (List<global::StructuresData>),
        21
      },
      {
        typeof (List<TaskAndTime>),
        22
      },
      {
        typeof (List<ThoughtData>),
        23
      },
      {
        typeof (List<TraderTracker>),
        24
      },
      {
        typeof (List<TraderTrackerItems>),
        25
      },
      {
        typeof (List<EquipmentType>),
        26
      },
      {
        typeof (List<FollowerClothingType>),
        27
      },
      {
        typeof (List<FollowerCommands>),
        28
      },
      {
        typeof (List<FollowerLocation>),
        29
      },
      {
        typeof (List<RelicType>),
        30
      },
      {
        typeof (List<SeasonalEventType>),
        31 /*0x1F*/
      },
      {
        typeof (List<TutorialTopic>),
        32 /*0x20*/
      },
      {
        typeof (List<global::DataManager.ClothingVariant>),
        33
      },
      {
        typeof (List<global::DataManager.DepositedFollower>),
        34
      },
      {
        typeof (List<global::DataManager.EnemyData>),
        35
      },
      {
        typeof (List<global::DataManager.LocationAndLayer>),
        36
      },
      {
        typeof (List<global::DataManager.LocationSeedsData>),
        37
      },
      {
        typeof (List<global::DataManager.Offering>),
        38
      },
      {
        typeof (List<global::DataManager.QuestHistoryData>),
        39
      },
      {
        typeof (List<global::DataManager.WoolhavenFlowerPot>),
        40
      },
      {
        typeof (List<global::DataManager.DungeonCompletedFleeces>),
        41
      },
      {
        typeof (List<global::DoctrineUpgradeSystem.DoctrineType>),
        42
      },
      {
        typeof (List<global::DungeonSandboxManager.ProgressionSnapshot>),
        43
      },
      {
        typeof (List<global::EnemyModifier.ModifierType>),
        44
      },
      {
        typeof (List<global::FollowerPet.FollowerPetType>),
        45
      },
      {
        typeof (List<global::FollowerPet.DLCPet>),
        46
      },
      {
        typeof (List<global::FollowerTrait.TraitType>),
        47
      },
      {
        typeof (List<global::Interaction_Kitchen.QueuedMeal>),
        48 /*0x30*/
      },
      {
        typeof (List<global::InventoryItem.ITEM_TYPE>),
        49
      },
      {
        typeof (List<global::MiniBossController.MiniBossData>),
        50
      },
      {
        typeof (List<global::MissionManager.Mission>),
        51
      },
      {
        typeof (List<global::SermonsAndRituals.SermonRitualType>),
        52
      },
      {
        typeof (List<global::StructureBrain.TYPES>),
        53
      },
      {
        typeof (List<global::StructuresData.LogisticsSlot>),
        54
      },
      {
        typeof (List<global::StructuresData.Ranchable_Animal>),
        55
      },
      {
        typeof (List<global::StructuresData.ResearchObject>),
        56
      },
      {
        typeof (List<global::StructuresData.ClothingStruct>),
        57
      },
      {
        typeof (List<global::StructuresData.PathData>),
        58
      },
      {
        typeof (List<global::TarotCards.Card>),
        59
      },
      {
        typeof (List<global::UITraitManipulatorMenuController.Type>),
        60
      },
      {
        typeof (List<global::UnlockManager.UnlockType>),
        61
      },
      {
        typeof (List<global::UpgradeSystem.UpgradeCoolDown>),
        62
      },
      {
        typeof (List<global::UpgradeSystem.Type>),
        63 /*0x3F*/
      },
      {
        typeof (List<Lamb.UI.ItemSelector.Category>),
        64 /*0x40*/
      },
      {
        typeof (List<FlockadePieceType>),
        65
      },
      {
        typeof (List<FinalizedNotification>),
        66
      },
      {
        typeof (List<Map.NodeType>),
        67
      },
      {
        typeof (List<Vector2>),
        68
      },
      {
        typeof (DayPhase),
        69
      },
      {
        typeof (Enemy),
        70
      },
      {
        typeof (EquipmentType),
        71
      },
      {
        typeof (FollowerClothingType),
        72
      },
      {
        typeof (FollowerCommands),
        73
      },
      {
        typeof (FollowerCustomisationType),
        74
      },
      {
        typeof (FollowerFaction),
        75
      },
      {
        typeof (FollowerHatType),
        76
      },
      {
        typeof (FollowerLocation),
        77
      },
      {
        typeof (FollowerOutfitType),
        78
      },
      {
        typeof (FollowerRole),
        79
      },
      {
        typeof (FollowerSpecialType),
        80 /*0x50*/
      },
      {
        typeof (FollowerTaskType),
        81
      },
      {
        typeof (GateType),
        82
      },
      {
        typeof (RelicType),
        83
      },
      {
        typeof (ResurrectionType),
        84
      },
      {
        typeof (SeasonalEventType),
        85
      },
      {
        typeof (SermonCategory),
        86
      },
      {
        typeof (Thought),
        87
      },
      {
        typeof (TutorialTopic),
        88
      },
      {
        typeof (WorkerPriority),
        89
      },
      {
        typeof (global::BluePrint.BluePrintType),
        90
      },
      {
        typeof (global::CrownAbilities.TYPE),
        91
      },
      {
        typeof (global::DataManager.CultLevel),
        92
      },
      {
        typeof (global::DataManager.OnboardingPhase),
        93
      },
      {
        typeof (global::DataManager.Variables),
        94
      },
      {
        typeof (global::DoctrineUpgradeSystem.DoctrineType),
        95
      },
      {
        typeof (global::DungeonSandboxManager.ScenarioType),
        96 /*0x60*/
      },
      {
        typeof (global::DungeonWorldMapIcon.NodeType),
        97
      },
      {
        typeof (global::EnemyModifier.ModifierType),
        98
      },
      {
        typeof (global::FollowerPet.FollowerPetType),
        99
      },
      {
        typeof (global::FollowerTrait.TraitType),
        100
      },
      {
        typeof (global::IDAndRelationship.RelationshipState),
        101
      },
      {
        typeof (global::InventoryItem.ITEM_TYPE),
        102
      },
      {
        typeof (global::InventoryWeapon.ITEM_TYPE),
        103
      },
      {
        typeof (global::MissionManager.MissionType),
        104
      },
      {
        typeof (global::NotificationFollower.Animation),
        105
      },
      {
        typeof (global::Objectives.CustomQuestTypes),
        106
      },
      {
        typeof (global::Objectives.TIMER_TYPE),
        107
      },
      {
        typeof (global::Objectives.TYPES),
        108
      },
      {
        typeof (global::Objectives_FindChildren.ChildLocation),
        109
      },
      {
        typeof (global::Objectives_PlaceStructure.DecorationType),
        110
      },
      {
        typeof (global::PlayerFleeceManager.FleeceType),
        111
      },
      {
        typeof (global::SeasonsManager.Season),
        112 /*0x70*/
      },
      {
        typeof (global::SeasonsManager.WeatherEvent),
        113
      },
      {
        typeof (global::SermonsAndRituals.SermonRitualType),
        114
      },
      {
        typeof (global::Shrines.ShrineType),
        115
      },
      {
        typeof (global::StructureAndTime.IDTypes),
        116
      },
      {
        typeof (global::StructureBrain.Categories),
        117
      },
      {
        typeof (global::StructureBrain.TYPES),
        118
      },
      {
        typeof (global::StructureEffectManager.EffectType),
        119
      },
      {
        typeof (global::StructuresData.Phase),
        120
      },
      {
        typeof (global::TarotCards.Card),
        121
      },
      {
        typeof (global::TarotCards.CardCategory),
        122
      },
      {
        typeof (global::UITraitManipulatorMenuController.Type),
        123
      },
      {
        typeof (global::UnlockManager.UnlockType),
        124
      },
      {
        typeof (global::UpgradeSystem.Type),
        125
      },
      {
        typeof (global::Villager_Info.Faction),
        126
      },
      {
        typeof (global::WeaponUpgradeSystem.WeaponType),
        (int) sbyte.MaxValue
      },
      {
        typeof (global::WeaponUpgradeSystem.WeaponUpgradeType),
        128 /*0x80*/
      },
      {
        typeof (global::WeatherSystemController.WeatherStrength),
        129
      },
      {
        typeof (global::WeatherSystemController.WeatherType),
        130
      },
      {
        typeof (global::WorshipperInfoManager.Outfit),
        131
      },
      {
        typeof (Lamb.UI.UpgradeTreeNode.TreeTier),
        132
      },
      {
        typeof (Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results),
        133
      },
      {
        typeof (FlockadePieceType),
        134
      },
      {
        typeof (Map.NodeType),
        135
      },
      {
        typeof (ObjectivesData),
        136
      },
      {
        typeof (ObjectivesDataFinalized),
        137
      },
      {
        typeof (Objectives_RoomChallenge),
        138
      },
      {
        typeof (global::Alerts),
        139
      },
      {
        typeof (global::BluePrint),
        140
      },
      {
        typeof (BuyEntry),
        141
      },
      {
        typeof (global::CrownAbilities),
        142
      },
      {
        typeof (global::DataManager),
        143
      },
      {
        typeof (DayObject),
        144 /*0x90*/
      },
      {
        typeof (DoctrineAlerts),
        145
      },
      {
        typeof (FollowerInfo),
        146
      },
      {
        typeof (FollowerInfoSnapshot),
        147
      },
      {
        typeof (FollowerInteractionAlerts),
        148
      },
      {
        typeof (FollowerRoleInfo),
        149
      },
      {
        typeof (FollowerThoughts),
        150
      },
      {
        typeof (global::IDAndRelationship),
        151
      },
      {
        typeof (Inventory),
        152
      },
      {
        typeof (global::InventoryItem),
        153
      },
      {
        typeof (global::InventoryWeapon),
        154
      },
      {
        typeof (JellyFishInvestment),
        155
      },
      {
        typeof (JellyFishInvestmentDay),
        156
      },
      {
        typeof (MidasDonation),
        157
      },
      {
        typeof (global::Objective_FindRelic),
        158
      },
      {
        typeof (global::Objectives_AssignClothing),
        159
      },
      {
        typeof (global::Objectives_BedRest),
        160 /*0xA0*/
      },
      {
        typeof (global::Objectives_BlizzardOffering),
        161
      },
      {
        typeof (global::Objectives_BuildStructure),
        162
      },
      {
        typeof (global::Objectives_BuildWinterDecorations),
        163
      },
      {
        typeof (global::Objectives_CollectItem),
        164
      },
      {
        typeof (global::Objectives_CookMeal),
        165
      },
      {
        typeof (global::Objectives_CraftClothing),
        166
      },
      {
        typeof (global::Objectives_Custom),
        167
      },
      {
        typeof (global::Objectives_DefeatKnucklebones),
        168
      },
      {
        typeof (global::Objectives_DepositFood),
        169
      },
      {
        typeof (global::Objectives_Drink),
        170
      },
      {
        typeof (global::Objectives_EatMeal),
        171
      },
      {
        typeof (global::Objectives_FeedAnimal),
        172
      },
      {
        typeof (global::Objectives_FindChildren),
        173
      },
      {
        typeof (global::Objectives_FindFollower),
        174
      },
      {
        typeof (global::Objectives_FinishRace),
        175
      },
      {
        typeof (global::Objectives_FlowerBaskets),
        176 /*0xB0*/
      },
      {
        typeof (global::Objectives_GetAnimal),
        177
      },
      {
        typeof (global::Objectives_GiveItem),
        178
      },
      {
        typeof (global::Objectives_KillEnemies),
        179
      },
      {
        typeof (global::Objectives_LegendarySwordReturn),
        180
      },
      {
        typeof (global::Objectives_LegendaryWeaponRun),
        181
      },
      {
        typeof (global::Objectives_Mating),
        182
      },
      {
        typeof (global::Objectives_NoCurses),
        183
      },
      {
        typeof (global::Objectives_NoDamage),
        184
      },
      {
        typeof (global::Objectives_NoDodge),
        185
      },
      {
        typeof (global::Objectives_NoHealing),
        186
      },
      {
        typeof (global::Objectives_PerformRitual),
        187
      },
      {
        typeof (global::Objectives_PlaceStructure),
        188
      },
      {
        typeof (global::Objectives_RecruitCursedFollower),
        189
      },
      {
        typeof (global::Objectives_RecruitFollower),
        190
      },
      {
        typeof (global::Objectives_RemoveStructure),
        191
      },
      {
        typeof (global::Objectives_ShootDummy),
        192 /*0xC0*/
      },
      {
        typeof (global::Objectives_ShowFleece),
        193
      },
      {
        typeof (global::Objectives_Story),
        194
      },
      {
        typeof (global::Objectives_TalkToFollower),
        195
      },
      {
        typeof (global::Objectives_UnlockUpgrade),
        196
      },
      {
        typeof (global::Objectives_UseRelic),
        197
      },
      {
        typeof (global::Objectives_WalkAnimal),
        198
      },
      {
        typeof (global::Objectives_WinFlockadeBet),
        199
      },
      {
        typeof (RitualAlerts),
        200
      },
      {
        typeof (ShopLocationTracker),
        201
      },
      {
        typeof (ShrineUsageInfo),
        202
      },
      {
        typeof (StoryData),
        203
      },
      {
        typeof (StoryDataItem),
        204
      },
      {
        typeof (StructureAlerts),
        205
      },
      {
        typeof (global::StructureAndTime),
        206
      },
      {
        typeof (StructureEffect),
        207
      },
      {
        typeof (global::StructuresData),
        208 /*0xD0*/
      },
      {
        typeof (global::TarotCards),
        209
      },
      {
        typeof (TaskAndTime),
        210
      },
      {
        typeof (ThoughtData),
        211
      },
      {
        typeof (TraderTracker),
        212
      },
      {
        typeof (TraderTrackerItems),
        213
      },
      {
        typeof (global::Villager_Info),
        214
      },
      {
        typeof (MetaData),
        215
      },
      {
        typeof (global::DataManager.ClothingVariant),
        216
      },
      {
        typeof (global::DataManager.DepositedFollower),
        217
      },
      {
        typeof (global::DataManager.LocationAndLayer),
        218
      },
      {
        typeof (global::DataManager.LocationSeedsData),
        219
      },
      {
        typeof (global::DataManager.Offering),
        220
      },
      {
        typeof (global::DataManager.QuestHistoryData),
        221
      },
      {
        typeof (global::DataManager.WoolhavenFlowerPot),
        222
      },
      {
        typeof (global::DungeonSandboxManager.ProgressionSnapshot),
        223
      },
      {
        typeof (global::DungeonWorldMapIcon.DLCTemporaryMapNode),
        224 /*0xE0*/
      },
      {
        typeof (global::FollowerPet.DLCPet),
        225
      },
      {
        typeof (global::Interaction_Kitchen.QueuedMeal),
        226
      },
      {
        typeof (global::MiniBossController.MiniBossData),
        227
      },
      {
        typeof (global::MissionManager.Mission),
        228
      },
      {
        typeof (global::Objective_FindRelic.FinalizedData_FindRelic),
        229
      },
      {
        typeof (global::Objectives_AssignClothing.FinalizedData_AssignClothing),
        230
      },
      {
        typeof (global::Objectives_BedRest.FinalizedData_BedRest),
        231
      },
      {
        typeof (global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering),
        232
      },
      {
        typeof (global::Objectives_BuildStructure.FinalizedData_BuildStructure),
        233
      },
      {
        typeof (global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations),
        234
      },
      {
        typeof (global::Objectives_CollectItem.FinalizedData_CollectItem),
        235
      },
      {
        typeof (global::Objectives_CookMeal.FinalizedData_CookMeal),
        236
      },
      {
        typeof (global::Objectives_CraftClothing.FinalizedData_CraftClothing),
        237
      },
      {
        typeof (global::Objectives_Custom.FinalizedData_Custom),
        238
      },
      {
        typeof (global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones),
        239
      },
      {
        typeof (global::Objectives_DepositFood.FinalizedData_DepositFood),
        240 /*0xF0*/
      },
      {
        typeof (global::Objectives_Drink.FinalizedData_Drink),
        241
      },
      {
        typeof (global::Objectives_EatMeal.FinalizedData_EatMeal),
        242
      },
      {
        typeof (global::Objectives_FeedAnimal.FinalizedData_FeedAnimal),
        243
      },
      {
        typeof (global::Objectives_FindChildren.FinalizedData_FindChildren),
        244
      },
      {
        typeof (global::Objectives_FindFollower.FinalizedData_FindFollower),
        245
      },
      {
        typeof (global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace),
        246
      },
      {
        typeof (global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets),
        247
      },
      {
        typeof (global::Objectives_GetAnimal.FinalizedData_GetAnimal),
        248
      },
      {
        typeof (global::Objectives_GiveItem.FinalizedData_GiveItem),
        249
      },
      {
        typeof (global::Objectives_KillEnemies.FinalizedData_KillEnemies),
        250
      },
      {
        typeof (global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn),
        251
      },
      {
        typeof (global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun),
        252
      },
      {
        typeof (global::Objectives_Mating.FinalizedData_Mating),
        253
      },
      {
        typeof (global::Objectives_NoCurses.FinalizedData_NoCurses),
        254
      },
      {
        typeof (global::Objectives_NoDamage.FinalizedData_NoDamage),
        (int) byte.MaxValue
      },
      {
        typeof (global::Objectives_NoDodge.FinalizedData_NoDodge),
        256 /*0x0100*/
      },
      {
        typeof (global::Objectives_NoHealing.FinalizedData_NoHealing),
        257
      },
      {
        typeof (global::Objectives_PerformRitual.FinalizedData_PerformRitual),
        258
      },
      {
        typeof (global::Objectives_PlaceStructure.FinalizedData_PlaceStructure),
        259
      },
      {
        typeof (global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower),
        260
      },
      {
        typeof (global::Objectives_RecruitFollower.FinalizedData_RecruitFollower),
        261
      },
      {
        typeof (global::Objectives_RemoveStructure.FinalizedData_RemoveStructure),
        262
      },
      {
        typeof (global::Objectives_ShootDummy.FinalizedData_ShootDummy),
        263
      },
      {
        typeof (global::Objectives_ShowFleece.FinalizedData_ShowFleece),
        264
      },
      {
        typeof (global::Objectives_Story.FinalizedData),
        265
      },
      {
        typeof (global::Objectives_TalkToFollower.FinalizedData_TalkToFollower),
        266
      },
      {
        typeof (global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade),
        267
      },
      {
        typeof (global::Objectives_UseRelic.FinalizedData_UseRelic),
        268
      },
      {
        typeof (global::Objectives_WalkAnimal.FinalizedData_WalkAnimal),
        269
      },
      {
        typeof (global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet),
        270
      },
      {
        typeof (global::StructuresData.EggData),
        271
      },
      {
        typeof (global::StructuresData.LogisticsSlot),
        272
      },
      {
        typeof (global::StructuresData.ResearchObject),
        273
      },
      {
        typeof (global::StructuresData.ClothingStruct),
        274
      },
      {
        typeof (global::StructuresData.PathData),
        275
      },
      {
        typeof (global::TarotCards.TarotCard),
        276
      },
      {
        typeof (global::UpgradeSystem.UpgradeCoolDown),
        277
      },
      {
        typeof (Lamb.UI.ItemSelector.Category),
        278
      },
      {
        typeof (MMBiomeGeneration.BiomeGenerator.VariableAndCondition),
        279
      },
      {
        typeof (MMBiomeGeneration.BiomeGenerator.VariableAndCount),
        280
      },
      {
        typeof (FinalizedFaithNotification),
        281
      },
      {
        typeof (FinalizedFollowerNotification),
        282
      },
      {
        typeof (FinalizedItemNotification),
        283
      },
      {
        typeof (FinalizedNotificationSimple),
        284
      },
      {
        typeof (FinalizedRelationshipNotification),
        285
      },
      {
        typeof (CharacterSkinAlerts),
        286
      },
      {
        typeof (ClothingAlerts),
        287
      },
      {
        typeof (ClothingAssignAlerts),
        288
      },
      {
        typeof (ClothingCustomiseAlerts),
        289
      },
      {
        typeof (CurseAlerts),
        290
      },
      {
        typeof (FlockadePieceAlerts),
        291
      },
      {
        typeof (InventoryAlerts),
        292
      },
      {
        typeof (LocationAlerts),
        293
      },
      {
        typeof (LoreAlerts),
        294
      },
      {
        typeof (PhotoGalleryAlerts),
        295
      },
      {
        typeof (RecipeAlerts),
        296
      },
      {
        typeof (RelicAlerts),
        297
      },
      {
        typeof (RunTarotCardAlerts),
        298
      },
      {
        typeof (TarotCardAlerts),
        299
      },
      {
        typeof (TraitManipulatorAlerts),
        300
      },
      {
        typeof (TutorialAlerts),
        301
      },
      {
        typeof (UpgradeAlerts),
        302
      },
      {
        typeof (WeaponAlerts),
        303
      },
      {
        typeof (TwitchSettings),
        304
      },
      {
        typeof (StoryObjectiveData),
        305
      },
      {
        typeof (global::DataManager.EnemyData),
        306
      },
      {
        typeof (global::DataManager.DungeonCompletedFleeces),
        307
      },
      {
        typeof (global::StructuresData.Ranchable_Animal),
        308
      },
      {
        typeof (FinalizedNotification),
        309
      }
    };

    public static object GetFormatter(System.Type t)
    {
      int num;
      if (!GeneratedMessagePackResolver.GeneratedMessagePackResolverGetFormatterHelper.closedTypeLookup.TryGetValue(t, out num))
        return (object) null;
      switch (num)
      {
        case 0:
          return (object) new ArrayFormatter<BuyEntry>();
        case 1:
          return (object) new ArrayFormatter<global::InventoryItem>();
        case 2:
          return (object) new ArrayFormatter<global::StructuresData.Ranchable_Animal>();
        case 3:
          return (object) new DictionaryFormatter<global::StructureBrain.TYPES, int>();
        case 4:
          return (object) new DictionaryFormatter<int, int>();
        case 5:
          return (object) new ListFormatter<global::BluePrint>();
        case 6:
          return (object) new ListFormatter<BuyEntry>();
        case 7:
          return (object) new ListFormatter<global::CrownAbilities>();
        case 8:
          return (object) new ListFormatter<DayObject>();
        case 9:
          return (object) new ListFormatter<FollowerInfo>();
        case 10:
          return (object) new ListFormatter<global::IDAndRelationship>();
        case 11:
          return (object) new ListFormatter<global::InventoryItem>();
        case 12:
          return (object) new ListFormatter<JellyFishInvestmentDay>();
        case 13:
          return (object) new ListFormatter<ObjectivesData>();
        case 14:
          return (object) new ListFormatter<ObjectivesDataFinalized>();
        case 15:
          return (object) new ListFormatter<ShopLocationTracker>();
        case 16 /*0x10*/:
          return (object) new ListFormatter<ShrineUsageInfo>();
        case 17:
          return (object) new ListFormatter<StoryData>();
        case 18:
          return (object) new ListFormatter<StoryDataItem>();
        case 19:
          return (object) new ListFormatter<global::StructureAndTime>();
        case 20:
          return (object) new ListFormatter<StructureEffect>();
        case 21:
          return (object) new ListFormatter<global::StructuresData>();
        case 22:
          return (object) new ListFormatter<TaskAndTime>();
        case 23:
          return (object) new ListFormatter<ThoughtData>();
        case 24:
          return (object) new ListFormatter<TraderTracker>();
        case 25:
          return (object) new ListFormatter<TraderTrackerItems>();
        case 26:
          return (object) new ListFormatter<EquipmentType>();
        case 27:
          return (object) new ListFormatter<FollowerClothingType>();
        case 28:
          return (object) new ListFormatter<FollowerCommands>();
        case 29:
          return (object) new ListFormatter<FollowerLocation>();
        case 30:
          return (object) new ListFormatter<RelicType>();
        case 31 /*0x1F*/:
          return (object) new ListFormatter<SeasonalEventType>();
        case 32 /*0x20*/:
          return (object) new ListFormatter<TutorialTopic>();
        case 33:
          return (object) new ListFormatter<global::DataManager.ClothingVariant>();
        case 34:
          return (object) new ListFormatter<global::DataManager.DepositedFollower>();
        case 35:
          return (object) new ListFormatter<global::DataManager.EnemyData>();
        case 36:
          return (object) new ListFormatter<global::DataManager.LocationAndLayer>();
        case 37:
          return (object) new ListFormatter<global::DataManager.LocationSeedsData>();
        case 38:
          return (object) new ListFormatter<global::DataManager.Offering>();
        case 39:
          return (object) new ListFormatter<global::DataManager.QuestHistoryData>();
        case 40:
          return (object) new ListFormatter<global::DataManager.WoolhavenFlowerPot>();
        case 41:
          return (object) new ListFormatter<global::DataManager.DungeonCompletedFleeces>();
        case 42:
          return (object) new ListFormatter<global::DoctrineUpgradeSystem.DoctrineType>();
        case 43:
          return (object) new ListFormatter<global::DungeonSandboxManager.ProgressionSnapshot>();
        case 44:
          return (object) new ListFormatter<global::EnemyModifier.ModifierType>();
        case 45:
          return (object) new ListFormatter<global::FollowerPet.FollowerPetType>();
        case 46:
          return (object) new ListFormatter<global::FollowerPet.DLCPet>();
        case 47:
          return (object) new ListFormatter<global::FollowerTrait.TraitType>();
        case 48 /*0x30*/:
          return (object) new ListFormatter<global::Interaction_Kitchen.QueuedMeal>();
        case 49:
          return (object) new ListFormatter<global::InventoryItem.ITEM_TYPE>();
        case 50:
          return (object) new ListFormatter<global::MiniBossController.MiniBossData>();
        case 51:
          return (object) new ListFormatter<global::MissionManager.Mission>();
        case 52:
          return (object) new ListFormatter<global::SermonsAndRituals.SermonRitualType>();
        case 53:
          return (object) new ListFormatter<global::StructureBrain.TYPES>();
        case 54:
          return (object) new ListFormatter<global::StructuresData.LogisticsSlot>();
        case 55:
          return (object) new ListFormatter<global::StructuresData.Ranchable_Animal>();
        case 56:
          return (object) new ListFormatter<global::StructuresData.ResearchObject>();
        case 57:
          return (object) new ListFormatter<global::StructuresData.ClothingStruct>();
        case 58:
          return (object) new ListFormatter<global::StructuresData.PathData>();
        case 59:
          return (object) new ListFormatter<global::TarotCards.Card>();
        case 60:
          return (object) new ListFormatter<global::UITraitManipulatorMenuController.Type>();
        case 61:
          return (object) new ListFormatter<global::UnlockManager.UnlockType>();
        case 62:
          return (object) new ListFormatter<global::UpgradeSystem.UpgradeCoolDown>();
        case 63 /*0x3F*/:
          return (object) new ListFormatter<global::UpgradeSystem.Type>();
        case 64 /*0x40*/:
          return (object) new ListFormatter<Lamb.UI.ItemSelector.Category>();
        case 65:
          return (object) new ListFormatter<FlockadePieceType>();
        case 66:
          return (object) new ListFormatter<FinalizedNotification>();
        case 67:
          return (object) new ListFormatter<Map.NodeType>();
        case 68:
          return (object) new ListFormatter<Vector2>();
        case 69:
          return (object) new GeneratedMessagePackResolver.DayPhaseFormatter();
        case 70:
          return (object) new GeneratedMessagePackResolver.EnemyFormatter();
        case 71:
          return (object) new GeneratedMessagePackResolver.EquipmentTypeFormatter();
        case 72:
          return (object) new GeneratedMessagePackResolver.FollowerClothingTypeFormatter();
        case 73:
          return (object) new GeneratedMessagePackResolver.FollowerCommandsFormatter();
        case 74:
          return (object) new GeneratedMessagePackResolver.FollowerCustomisationTypeFormatter();
        case 75:
          return (object) new GeneratedMessagePackResolver.FollowerFactionFormatter();
        case 76:
          return (object) new GeneratedMessagePackResolver.FollowerHatTypeFormatter();
        case 77:
          return (object) new GeneratedMessagePackResolver.FollowerLocationFormatter();
        case 78:
          return (object) new GeneratedMessagePackResolver.FollowerOutfitTypeFormatter();
        case 79:
          return (object) new GeneratedMessagePackResolver.FollowerRoleFormatter();
        case 80 /*0x50*/:
          return (object) new GeneratedMessagePackResolver.FollowerSpecialTypeFormatter();
        case 81:
          return (object) new GeneratedMessagePackResolver.FollowerTaskTypeFormatter();
        case 82:
          return (object) new GeneratedMessagePackResolver.GateTypeFormatter();
        case 83:
          return (object) new GeneratedMessagePackResolver.RelicTypeFormatter();
        case 84:
          return (object) new GeneratedMessagePackResolver.ResurrectionTypeFormatter();
        case 85:
          return (object) new GeneratedMessagePackResolver.SeasonalEventTypeFormatter();
        case 86:
          return (object) new GeneratedMessagePackResolver.SermonCategoryFormatter();
        case 87:
          return (object) new GeneratedMessagePackResolver.ThoughtFormatter();
        case 88:
          return (object) new GeneratedMessagePackResolver.TutorialTopicFormatter();
        case 89:
          return (object) new GeneratedMessagePackResolver.WorkerPriorityFormatter();
        case 90:
          return (object) new GeneratedMessagePackResolver.BluePrint.BluePrintTypeFormatter();
        case 91:
          return (object) new GeneratedMessagePackResolver.CrownAbilities.TYPEFormatter();
        case 92:
          return (object) new GeneratedMessagePackResolver.DataManager.CultLevelFormatter();
        case 93:
          return (object) new GeneratedMessagePackResolver.DataManager.OnboardingPhaseFormatter();
        case 94:
          return (object) new GeneratedMessagePackResolver.DataManager.VariablesFormatter();
        case 95:
          return (object) new GeneratedMessagePackResolver.DoctrineUpgradeSystem.DoctrineTypeFormatter();
        case 96 /*0x60*/:
          return (object) new GeneratedMessagePackResolver.DungeonSandboxManager.ScenarioTypeFormatter();
        case 97:
          return (object) new GeneratedMessagePackResolver.DungeonWorldMapIcon.NodeTypeFormatter();
        case 98:
          return (object) new GeneratedMessagePackResolver.EnemyModifier.ModifierTypeFormatter();
        case 99:
          return (object) new GeneratedMessagePackResolver.FollowerPet.FollowerPetTypeFormatter();
        case 100:
          return (object) new GeneratedMessagePackResolver.FollowerTrait.TraitTypeFormatter();
        case 101:
          return (object) new GeneratedMessagePackResolver.IDAndRelationship.RelationshipStateFormatter();
        case 102:
          return (object) new GeneratedMessagePackResolver.InventoryItem.ITEM_TYPEFormatter();
        case 103:
          return (object) new GeneratedMessagePackResolver.InventoryWeapon.ITEM_TYPEFormatter();
        case 104:
          return (object) new GeneratedMessagePackResolver.MissionManager.MissionTypeFormatter();
        case 105:
          return (object) new GeneratedMessagePackResolver.NotificationFollower.AnimationFormatter();
        case 106:
          return (object) new GeneratedMessagePackResolver.Objectives.CustomQuestTypesFormatter();
        case 107:
          return (object) new GeneratedMessagePackResolver.Objectives.TIMER_TYPEFormatter();
        case 108:
          return (object) new GeneratedMessagePackResolver.Objectives.TYPESFormatter();
        case 109:
          return (object) new GeneratedMessagePackResolver.Objectives_FindChildren.ChildLocationFormatter();
        case 110:
          return (object) new GeneratedMessagePackResolver.Objectives_PlaceStructure.DecorationTypeFormatter();
        case 111:
          return (object) new GeneratedMessagePackResolver.PlayerFleeceManager.FleeceTypeFormatter();
        case 112 /*0x70*/:
          return (object) new GeneratedMessagePackResolver.SeasonsManager.SeasonFormatter();
        case 113:
          return (object) new GeneratedMessagePackResolver.SeasonsManager.WeatherEventFormatter();
        case 114:
          return (object) new GeneratedMessagePackResolver.SermonsAndRituals.SermonRitualTypeFormatter();
        case 115:
          return (object) new GeneratedMessagePackResolver.Shrines.ShrineTypeFormatter();
        case 116:
          return (object) new GeneratedMessagePackResolver.StructureAndTime.IDTypesFormatter();
        case 117:
          return (object) new GeneratedMessagePackResolver.StructureBrain.CategoriesFormatter();
        case 118:
          return (object) new GeneratedMessagePackResolver.StructureBrain.TYPESFormatter();
        case 119:
          return (object) new GeneratedMessagePackResolver.StructureEffectManager.EffectTypeFormatter();
        case 120:
          return (object) new GeneratedMessagePackResolver.StructuresData.PhaseFormatter();
        case 121:
          return (object) new GeneratedMessagePackResolver.TarotCards.CardFormatter();
        case 122:
          return (object) new GeneratedMessagePackResolver.TarotCards.CardCategoryFormatter();
        case 123:
          return (object) new GeneratedMessagePackResolver.UITraitManipulatorMenuController.TypeFormatter();
        case 124:
          return (object) new GeneratedMessagePackResolver.UnlockManager.UnlockTypeFormatter();
        case 125:
          return (object) new GeneratedMessagePackResolver.UpgradeSystem.TypeFormatter();
        case 126:
          return (object) new GeneratedMessagePackResolver.Villager_Info.FactionFormatter();
        case (int) sbyte.MaxValue:
          return (object) new GeneratedMessagePackResolver.WeaponUpgradeSystem.WeaponTypeFormatter();
        case 128 /*0x80*/:
          return (object) new GeneratedMessagePackResolver.WeaponUpgradeSystem.WeaponUpgradeTypeFormatter();
        case 129:
          return (object) new GeneratedMessagePackResolver.WeatherSystemController.WeatherStrengthFormatter();
        case 130:
          return (object) new GeneratedMessagePackResolver.WeatherSystemController.WeatherTypeFormatter();
        case 131:
          return (object) new GeneratedMessagePackResolver.WorshipperInfoManager.OutfitFormatter();
        case 132:
          return (object) new GeneratedMessagePackResolver.Lamb.UI.UpgradeTreeNode.TreeTierFormatter();
        case 133:
          return (object) new GeneratedMessagePackResolver.Lamb.UI.DeathScreen.UIDeathScreenOverlayController.ResultsFormatter();
        case 134:
          return (object) new GeneratedMessagePackResolver.Flockade.FlockadePieceTypeFormatter();
        case 135:
          return (object) new GeneratedMessagePackResolver.Map.NodeTypeFormatter();
        case 136:
          return (object) new GeneratedMessagePackResolver.ObjectivesDataFormatter();
        case 137:
          return (object) new GeneratedMessagePackResolver.ObjectivesDataFinalizedFormatter();
        case 138:
          return (object) new GeneratedMessagePackResolver.Objectives_RoomChallengeFormatter();
        case 139:
          return (object) new GeneratedMessagePackResolver.AlertsFormatter();
        case 140:
          return (object) new GeneratedMessagePackResolver.BluePrintFormatter();
        case 141:
          return (object) new GeneratedMessagePackResolver.BuyEntryFormatter();
        case 142:
          return (object) new GeneratedMessagePackResolver.CrownAbilitiesFormatter();
        case 143:
          return (object) new global::DataManager.DataManagerFormatter();
        case 144 /*0x90*/:
          return (object) new GeneratedMessagePackResolver.DayObjectFormatter();
        case 145:
          return (object) new GeneratedMessagePackResolver.DoctrineAlertsFormatter();
        case 146:
          return (object) new FollowerInfo.FollowerInfoFormatter();
        case 147:
          return (object) new GeneratedMessagePackResolver.FollowerInfoSnapshotFormatter();
        case 148:
          return (object) new GeneratedMessagePackResolver.FollowerInteractionAlertsFormatter();
        case 149:
          return (object) new GeneratedMessagePackResolver.FollowerRoleInfoFormatter();
        case 150:
          return (object) new GeneratedMessagePackResolver.FollowerThoughtsFormatter();
        case 151:
          return (object) new GeneratedMessagePackResolver.IDAndRelationshipFormatter();
        case 152:
          return (object) new GeneratedMessagePackResolver.InventoryFormatter();
        case 153:
          return (object) new GeneratedMessagePackResolver.InventoryItemFormatter();
        case 154:
          return (object) new GeneratedMessagePackResolver.InventoryWeaponFormatter();
        case 155:
          return (object) new GeneratedMessagePackResolver.JellyFishInvestmentFormatter();
        case 156:
          return (object) new GeneratedMessagePackResolver.JellyFishInvestmentDayFormatter();
        case 157:
          return (object) new GeneratedMessagePackResolver.MidasDonationFormatter();
        case 158:
          return (object) new GeneratedMessagePackResolver.Objective_FindRelicFormatter();
        case 159:
          return (object) new GeneratedMessagePackResolver.Objectives_AssignClothingFormatter();
        case 160 /*0xA0*/:
          return (object) new GeneratedMessagePackResolver.Objectives_BedRestFormatter();
        case 161:
          return (object) new GeneratedMessagePackResolver.Objectives_BlizzardOfferingFormatter();
        case 162:
          return (object) new GeneratedMessagePackResolver.Objectives_BuildStructureFormatter();
        case 163:
          return (object) new GeneratedMessagePackResolver.Objectives_BuildWinterDecorationsFormatter();
        case 164:
          return (object) new GeneratedMessagePackResolver.Objectives_CollectItemFormatter();
        case 165:
          return (object) new GeneratedMessagePackResolver.Objectives_CookMealFormatter();
        case 166:
          return (object) new GeneratedMessagePackResolver.Objectives_CraftClothingFormatter();
        case 167:
          return (object) new GeneratedMessagePackResolver.Objectives_CustomFormatter();
        case 168:
          return (object) new GeneratedMessagePackResolver.Objectives_DefeatKnucklebonesFormatter();
        case 169:
          return (object) new GeneratedMessagePackResolver.Objectives_DepositFoodFormatter();
        case 170:
          return (object) new GeneratedMessagePackResolver.Objectives_DrinkFormatter();
        case 171:
          return (object) new GeneratedMessagePackResolver.Objectives_EatMealFormatter();
        case 172:
          return (object) new GeneratedMessagePackResolver.Objectives_FeedAnimalFormatter();
        case 173:
          return (object) new GeneratedMessagePackResolver.Objectives_FindChildrenFormatter();
        case 174:
          return (object) new GeneratedMessagePackResolver.Objectives_FindFollowerFormatter();
        case 175:
          return (object) new GeneratedMessagePackResolver.Objectives_FinishRaceFormatter();
        case 176 /*0xB0*/:
          return (object) new GeneratedMessagePackResolver.Objectives_FlowerBasketsFormatter();
        case 177:
          return (object) new GeneratedMessagePackResolver.Objectives_GetAnimalFormatter();
        case 178:
          return (object) new GeneratedMessagePackResolver.Objectives_GiveItemFormatter();
        case 179:
          return (object) new GeneratedMessagePackResolver.Objectives_KillEnemiesFormatter();
        case 180:
          return (object) new GeneratedMessagePackResolver.Objectives_LegendarySwordReturnFormatter();
        case 181:
          return (object) new GeneratedMessagePackResolver.Objectives_LegendaryWeaponRunFormatter();
        case 182:
          return (object) new GeneratedMessagePackResolver.Objectives_MatingFormatter();
        case 183:
          return (object) new GeneratedMessagePackResolver.Objectives_NoCursesFormatter();
        case 184:
          return (object) new GeneratedMessagePackResolver.Objectives_NoDamageFormatter();
        case 185:
          return (object) new GeneratedMessagePackResolver.Objectives_NoDodgeFormatter();
        case 186:
          return (object) new GeneratedMessagePackResolver.Objectives_NoHealingFormatter();
        case 187:
          return (object) new GeneratedMessagePackResolver.Objectives_PerformRitualFormatter();
        case 188:
          return (object) new GeneratedMessagePackResolver.Objectives_PlaceStructureFormatter();
        case 189:
          return (object) new GeneratedMessagePackResolver.Objectives_RecruitCursedFollowerFormatter();
        case 190:
          return (object) new GeneratedMessagePackResolver.Objectives_RecruitFollowerFormatter();
        case 191:
          return (object) new GeneratedMessagePackResolver.Objectives_RemoveStructureFormatter();
        case 192 /*0xC0*/:
          return (object) new GeneratedMessagePackResolver.Objectives_ShootDummyFormatter();
        case 193:
          return (object) new GeneratedMessagePackResolver.Objectives_ShowFleeceFormatter();
        case 194:
          return (object) new GeneratedMessagePackResolver.Objectives_StoryFormatter();
        case 195:
          return (object) new GeneratedMessagePackResolver.Objectives_TalkToFollowerFormatter();
        case 196:
          return (object) new GeneratedMessagePackResolver.Objectives_UnlockUpgradeFormatter();
        case 197:
          return (object) new GeneratedMessagePackResolver.Objectives_UseRelicFormatter();
        case 198:
          return (object) new GeneratedMessagePackResolver.Objectives_WalkAnimalFormatter();
        case 199:
          return (object) new GeneratedMessagePackResolver.Objectives_WinFlockadeBetFormatter();
        case 200:
          return (object) new GeneratedMessagePackResolver.RitualAlertsFormatter();
        case 201:
          return (object) new GeneratedMessagePackResolver.ShopLocationTrackerFormatter();
        case 202:
          return (object) new GeneratedMessagePackResolver.ShrineUsageInfoFormatter();
        case 203:
          return (object) new GeneratedMessagePackResolver.StoryDataFormatter();
        case 204:
          return (object) new GeneratedMessagePackResolver.StoryDataItemFormatter();
        case 205:
          return (object) new GeneratedMessagePackResolver.StructureAlertsFormatter();
        case 206:
          return (object) new GeneratedMessagePackResolver.StructureAndTimeFormatter();
        case 207:
          return (object) new GeneratedMessagePackResolver.StructureEffectFormatter();
        case 208 /*0xD0*/:
          return (object) new GeneratedMessagePackResolver.StructuresDataFormatter();
        case 209:
          return (object) new GeneratedMessagePackResolver.TarotCardsFormatter();
        case 210:
          return (object) new GeneratedMessagePackResolver.TaskAndTimeFormatter();
        case 211:
          return (object) new GeneratedMessagePackResolver.ThoughtDataFormatter();
        case 212:
          return (object) new GeneratedMessagePackResolver.TraderTrackerFormatter();
        case 213:
          return (object) new GeneratedMessagePackResolver.TraderTrackerItemsFormatter();
        case 214:
          return (object) new GeneratedMessagePackResolver.Villager_InfoFormatter();
        case 215:
          return (object) new GeneratedMessagePackResolver.MetaDataFormatter();
        case 216:
          return (object) new GeneratedMessagePackResolver.DataManager.ClothingVariantFormatter();
        case 217:
          return (object) new GeneratedMessagePackResolver.DataManager.DepositedFollowerFormatter();
        case 218:
          return (object) new GeneratedMessagePackResolver.DataManager.LocationAndLayerFormatter();
        case 219:
          return (object) new GeneratedMessagePackResolver.DataManager.LocationSeedsDataFormatter();
        case 220:
          return (object) new GeneratedMessagePackResolver.DataManager.OfferingFormatter();
        case 221:
          return (object) new GeneratedMessagePackResolver.DataManager.QuestHistoryDataFormatter();
        case 222:
          return (object) new GeneratedMessagePackResolver.DataManager.WoolhavenFlowerPotFormatter();
        case 223:
          return (object) new GeneratedMessagePackResolver.DungeonSandboxManager.ProgressionSnapshotFormatter();
        case 224 /*0xE0*/:
          return (object) new GeneratedMessagePackResolver.DungeonWorldMapIcon.DLCTemporaryMapNodeFormatter();
        case 225:
          return (object) new GeneratedMessagePackResolver.FollowerPet.DLCPetFormatter();
        case 226:
          return (object) new GeneratedMessagePackResolver.Interaction_Kitchen.QueuedMealFormatter();
        case 227:
          return (object) new GeneratedMessagePackResolver.MiniBossController.MiniBossDataFormatter();
        case 228:
          return (object) new GeneratedMessagePackResolver.MissionManager.MissionFormatter();
        case 229:
          return (object) new GeneratedMessagePackResolver.Objective_FindRelic.FinalizedData_FindRelicFormatter();
        case 230:
          return (object) new GeneratedMessagePackResolver.Objectives_AssignClothing.FinalizedData_AssignClothingFormatter();
        case 231:
          return (object) new GeneratedMessagePackResolver.Objectives_BedRest.FinalizedData_BedRestFormatter();
        case 232:
          return (object) new GeneratedMessagePackResolver.Objectives_BlizzardOffering.FinalizedData_BlizzardOfferingFormatter();
        case 233:
          return (object) new GeneratedMessagePackResolver.Objectives_BuildStructure.FinalizedData_BuildStructureFormatter();
        case 234:
          return (object) new GeneratedMessagePackResolver.Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorationsFormatter();
        case 235:
          return (object) new GeneratedMessagePackResolver.Objectives_CollectItem.FinalizedData_CollectItemFormatter();
        case 236:
          return (object) new GeneratedMessagePackResolver.Objectives_CookMeal.FinalizedData_CookMealFormatter();
        case 237:
          return (object) new GeneratedMessagePackResolver.Objectives_CraftClothing.FinalizedData_CraftClothingFormatter();
        case 238:
          return (object) new GeneratedMessagePackResolver.Objectives_Custom.FinalizedData_CustomFormatter();
        case 239:
          return (object) new GeneratedMessagePackResolver.Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebonesFormatter();
        case 240 /*0xF0*/:
          return (object) new GeneratedMessagePackResolver.Objectives_DepositFood.FinalizedData_DepositFoodFormatter();
        case 241:
          return (object) new GeneratedMessagePackResolver.Objectives_Drink.FinalizedData_DrinkFormatter();
        case 242:
          return (object) new GeneratedMessagePackResolver.Objectives_EatMeal.FinalizedData_EatMealFormatter();
        case 243:
          return (object) new GeneratedMessagePackResolver.Objectives_FeedAnimal.FinalizedData_FeedAnimalFormatter();
        case 244:
          return (object) new GeneratedMessagePackResolver.Objectives_FindChildren.FinalizedData_FindChildrenFormatter();
        case 245:
          return (object) new GeneratedMessagePackResolver.Objectives_FindFollower.FinalizedData_FindFollowerFormatter();
        case 246:
          return (object) new GeneratedMessagePackResolver.Objectives_FinishRace.FinalizedData_Objectives_FinishRaceFormatter();
        case 247:
          return (object) new GeneratedMessagePackResolver.Objectives_FlowerBaskets.FinalizedData_FlowerBasketsFormatter();
        case 248:
          return (object) new GeneratedMessagePackResolver.Objectives_GetAnimal.FinalizedData_GetAnimalFormatter();
        case 249:
          return (object) new GeneratedMessagePackResolver.Objectives_GiveItem.FinalizedData_GiveItemFormatter();
        case 250:
          return (object) new GeneratedMessagePackResolver.Objectives_KillEnemies.FinalizedData_KillEnemiesFormatter();
        case 251:
          return (object) new GeneratedMessagePackResolver.Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturnFormatter();
        case 252:
          return (object) new GeneratedMessagePackResolver.Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRunFormatter();
        case 253:
          return (object) new GeneratedMessagePackResolver.Objectives_Mating.FinalizedData_MatingFormatter();
        case 254:
          return (object) new GeneratedMessagePackResolver.Objectives_NoCurses.FinalizedData_NoCursesFormatter();
        case (int) byte.MaxValue:
          return (object) new GeneratedMessagePackResolver.Objectives_NoDamage.FinalizedData_NoDamageFormatter();
        case 256 /*0x0100*/:
          return (object) new GeneratedMessagePackResolver.Objectives_NoDodge.FinalizedData_NoDodgeFormatter();
        case 257:
          return (object) new GeneratedMessagePackResolver.Objectives_NoHealing.FinalizedData_NoHealingFormatter();
        case 258:
          return (object) new GeneratedMessagePackResolver.Objectives_PerformRitual.FinalizedData_PerformRitualFormatter();
        case 259:
          return (object) new GeneratedMessagePackResolver.Objectives_PlaceStructure.FinalizedData_PlaceStructureFormatter();
        case 260:
          return (object) new GeneratedMessagePackResolver.Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollowerFormatter();
        case 261:
          return (object) new GeneratedMessagePackResolver.Objectives_RecruitFollower.FinalizedData_RecruitFollowerFormatter();
        case 262:
          return (object) new GeneratedMessagePackResolver.Objectives_RemoveStructure.FinalizedData_RemoveStructureFormatter();
        case 263:
          return (object) new GeneratedMessagePackResolver.Objectives_ShootDummy.FinalizedData_ShootDummyFormatter();
        case 264:
          return (object) new GeneratedMessagePackResolver.Objectives_ShowFleece.FinalizedData_ShowFleeceFormatter();
        case 265:
          return (object) new GeneratedMessagePackResolver.Objectives_Story.FinalizedDataFormatter();
        case 266:
          return (object) new GeneratedMessagePackResolver.Objectives_TalkToFollower.FinalizedData_TalkToFollowerFormatter();
        case 267:
          return (object) new GeneratedMessagePackResolver.Objectives_UnlockUpgrade.FinalizedData_UnlockUpgradeFormatter();
        case 268:
          return (object) new GeneratedMessagePackResolver.Objectives_UseRelic.FinalizedData_UseRelicFormatter();
        case 269:
          return (object) new GeneratedMessagePackResolver.Objectives_WalkAnimal.FinalizedData_WalkAnimalFormatter();
        case 270:
          return (object) new GeneratedMessagePackResolver.Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBetFormatter();
        case 271:
          return (object) new GeneratedMessagePackResolver.StructuresData.EggDataFormatter();
        case 272:
          return (object) new GeneratedMessagePackResolver.StructuresData.LogisticsSlotFormatter();
        case 273:
          return (object) new GeneratedMessagePackResolver.StructuresData.ResearchObjectFormatter();
        case 274:
          return (object) new GeneratedMessagePackResolver.StructuresData.ClothingStructFormatter();
        case 275:
          return (object) new GeneratedMessagePackResolver.StructuresData.PathDataFormatter();
        case 276:
          return (object) new GeneratedMessagePackResolver.TarotCards.TarotCardFormatter();
        case 277:
          return (object) new GeneratedMessagePackResolver.UpgradeSystem.UpgradeCoolDownFormatter();
        case 278:
          return (object) new GeneratedMessagePackResolver.Lamb.UI.ItemSelector.CategoryFormatter();
        case 279:
          return (object) new GeneratedMessagePackResolver.MMBiomeGeneration.BiomeGenerator.VariableAndConditionFormatter();
        case 280:
          return (object) new GeneratedMessagePackResolver.MMBiomeGeneration.BiomeGenerator.VariableAndCountFormatter();
        case 281:
          return (object) new GeneratedMessagePackResolver.Lamb.UI.FinalizedFaithNotificationFormatter();
        case 282:
          return (object) new GeneratedMessagePackResolver.Lamb.UI.FinalizedFollowerNotificationFormatter();
        case 283:
          return (object) new GeneratedMessagePackResolver.Lamb.UI.FinalizedItemNotificationFormatter();
        case 284:
          return (object) new GeneratedMessagePackResolver.Lamb.UI.FinalizedNotificationSimpleFormatter();
        case 285:
          return (object) new GeneratedMessagePackResolver.Lamb.UI.FinalizedRelationshipNotificationFormatter();
        case 286:
          return (object) new GeneratedMessagePackResolver.src.Alerts.CharacterSkinAlertsFormatter();
        case 287:
          return (object) new GeneratedMessagePackResolver.src.Alerts.ClothingAlertsFormatter();
        case 288:
          return (object) new GeneratedMessagePackResolver.src.Alerts.ClothingAssignAlertsFormatter();
        case 289:
          return (object) new GeneratedMessagePackResolver.src.Alerts.ClothingCustomiseAlertsFormatter();
        case 290:
          return (object) new GeneratedMessagePackResolver.src.Alerts.CurseAlertsFormatter();
        case 291:
          return (object) new GeneratedMessagePackResolver.src.Alerts.FlockadePieceAlertsFormatter();
        case 292:
          return (object) new GeneratedMessagePackResolver.src.Alerts.InventoryAlertsFormatter();
        case 293:
          return (object) new GeneratedMessagePackResolver.src.Alerts.LocationAlertsFormatter();
        case 294:
          return (object) new GeneratedMessagePackResolver.src.Alerts.LoreAlertsFormatter();
        case 295:
          return (object) new GeneratedMessagePackResolver.src.Alerts.PhotoGalleryAlertsFormatter();
        case 296:
          return (object) new GeneratedMessagePackResolver.src.Alerts.RecipeAlertsFormatter();
        case 297:
          return (object) new GeneratedMessagePackResolver.src.Alerts.RelicAlertsFormatter();
        case 298:
          return (object) new GeneratedMessagePackResolver.src.Alerts.RunTarotCardAlertsFormatter();
        case 299:
          return (object) new GeneratedMessagePackResolver.src.Alerts.TarotCardAlertsFormatter();
        case 300:
          return (object) new GeneratedMessagePackResolver.src.Alerts.TraitManipulatorAlertsFormatter();
        case 301:
          return (object) new GeneratedMessagePackResolver.src.Alerts.TutorialAlertsFormatter();
        case 302:
          return (object) new GeneratedMessagePackResolver.src.Alerts.UpgradeAlertsFormatter();
        case 303:
          return (object) new GeneratedMessagePackResolver.src.Alerts.WeaponAlertsFormatter();
        case 304:
          return (object) new GeneratedMessagePackResolver.src.Data.TwitchSettingsFormatter();
        case 305:
          return (object) new StoryObjectiveDataFormatter();
        case 306:
          return (object) new EnemyDataFormatter();
        case 307:
          return (object) new DungeonCompletedFleecesFormatter();
        case 308:
          return (object) new RanchableAnimalFormatter();
        case 309:
          return (object) new FinalizedNotificationPolymorphicFormatter();
        default:
          return (object) null;
      }
    }
  }

  public sealed class AlertsFormatter : IMessagePackFormatter<global::Alerts>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Alerts value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(22);
        resolver.GetFormatterWithVerify<DoctrineAlerts>().Serialize(ref writer, value.Doctrine, options);
        resolver.GetFormatterWithVerify<FollowerInteractionAlerts>().Serialize(ref writer, value.FollowerInteractions, options);
        resolver.GetFormatterWithVerify<RitualAlerts>().Serialize(ref writer, value.Rituals, options);
        resolver.GetFormatterWithVerify<StructureAlerts>().Serialize(ref writer, value.Structures, options);
        resolver.GetFormatterWithVerify<CharacterSkinAlerts>().Serialize(ref writer, value.CharacterSkinAlerts, options);
        resolver.GetFormatterWithVerify<InventoryAlerts>().Serialize(ref writer, value.Inventory, options);
        resolver.GetFormatterWithVerify<WeaponAlerts>().Serialize(ref writer, value.Weapons, options);
        resolver.GetFormatterWithVerify<CurseAlerts>().Serialize(ref writer, value.Curses, options);
        resolver.GetFormatterWithVerify<TarotCardAlerts>().Serialize(ref writer, value.TarotCardAlerts, options);
        resolver.GetFormatterWithVerify<UpgradeAlerts>().Serialize(ref writer, value.Upgrades, options);
        resolver.GetFormatterWithVerify<LocationAlerts>().Serialize(ref writer, value.Locations, options);
        resolver.GetFormatterWithVerify<TutorialAlerts>().Serialize(ref writer, value.Tutorial, options);
        resolver.GetFormatterWithVerify<RecipeAlerts>().Serialize(ref writer, value.Recipes, options);
        resolver.GetFormatterWithVerify<RelicAlerts>().Serialize(ref writer, value.RelicAlerts, options);
        resolver.GetFormatterWithVerify<PhotoGalleryAlerts>().Serialize(ref writer, value.GalleryAlerts, options);
        resolver.GetFormatterWithVerify<ClothingAlerts>().Serialize(ref writer, value.ClothingAlerts, options);
        resolver.GetFormatterWithVerify<ClothingCustomiseAlerts>().Serialize(ref writer, value.ClothingCustomiseAlerts, options);
        resolver.GetFormatterWithVerify<ClothingAssignAlerts>().Serialize(ref writer, value.ClothingAssignAlerts, options);
        resolver.GetFormatterWithVerify<FlockadePieceAlerts>().Serialize(ref writer, value.FlockadePieceAlerts, options);
        resolver.GetFormatterWithVerify<LoreAlerts>().Serialize(ref writer, value.LoreAlerts, options);
        resolver.GetFormatterWithVerify<TraitManipulatorAlerts>().Serialize(ref writer, value.TraitManipulatorAlerts, options);
        resolver.GetFormatterWithVerify<RunTarotCardAlerts>().Serialize(ref writer, value.RunTarotCardAlerts, options);
      }
    }

    public global::Alerts Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Alerts) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Alerts alerts = new global::Alerts();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            alerts.Doctrine = resolver.GetFormatterWithVerify<DoctrineAlerts>().Deserialize(ref reader, options);
            break;
          case 1:
            alerts.FollowerInteractions = resolver.GetFormatterWithVerify<FollowerInteractionAlerts>().Deserialize(ref reader, options);
            break;
          case 2:
            alerts.Rituals = resolver.GetFormatterWithVerify<RitualAlerts>().Deserialize(ref reader, options);
            break;
          case 3:
            alerts.Structures = resolver.GetFormatterWithVerify<StructureAlerts>().Deserialize(ref reader, options);
            break;
          case 4:
            alerts.CharacterSkinAlerts = resolver.GetFormatterWithVerify<CharacterSkinAlerts>().Deserialize(ref reader, options);
            break;
          case 5:
            alerts.Inventory = resolver.GetFormatterWithVerify<InventoryAlerts>().Deserialize(ref reader, options);
            break;
          case 6:
            alerts.Weapons = resolver.GetFormatterWithVerify<WeaponAlerts>().Deserialize(ref reader, options);
            break;
          case 7:
            alerts.Curses = resolver.GetFormatterWithVerify<CurseAlerts>().Deserialize(ref reader, options);
            break;
          case 8:
            alerts.TarotCardAlerts = resolver.GetFormatterWithVerify<TarotCardAlerts>().Deserialize(ref reader, options);
            break;
          case 9:
            alerts.Upgrades = resolver.GetFormatterWithVerify<UpgradeAlerts>().Deserialize(ref reader, options);
            break;
          case 10:
            alerts.Locations = resolver.GetFormatterWithVerify<LocationAlerts>().Deserialize(ref reader, options);
            break;
          case 11:
            alerts.Tutorial = resolver.GetFormatterWithVerify<TutorialAlerts>().Deserialize(ref reader, options);
            break;
          case 12:
            alerts.Recipes = resolver.GetFormatterWithVerify<RecipeAlerts>().Deserialize(ref reader, options);
            break;
          case 13:
            alerts.RelicAlerts = resolver.GetFormatterWithVerify<RelicAlerts>().Deserialize(ref reader, options);
            break;
          case 14:
            alerts.GalleryAlerts = resolver.GetFormatterWithVerify<PhotoGalleryAlerts>().Deserialize(ref reader, options);
            break;
          case 15:
            alerts.ClothingAlerts = resolver.GetFormatterWithVerify<ClothingAlerts>().Deserialize(ref reader, options);
            break;
          case 16 /*0x10*/:
            alerts.ClothingCustomiseAlerts = resolver.GetFormatterWithVerify<ClothingCustomiseAlerts>().Deserialize(ref reader, options);
            break;
          case 17:
            alerts.ClothingAssignAlerts = resolver.GetFormatterWithVerify<ClothingAssignAlerts>().Deserialize(ref reader, options);
            break;
          case 18:
            alerts.FlockadePieceAlerts = resolver.GetFormatterWithVerify<FlockadePieceAlerts>().Deserialize(ref reader, options);
            break;
          case 19:
            alerts.LoreAlerts = resolver.GetFormatterWithVerify<LoreAlerts>().Deserialize(ref reader, options);
            break;
          case 20:
            alerts.TraitManipulatorAlerts = resolver.GetFormatterWithVerify<TraitManipulatorAlerts>().Deserialize(ref reader, options);
            break;
          case 21:
            alerts.RunTarotCardAlerts = resolver.GetFormatterWithVerify<RunTarotCardAlerts>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return alerts;
    }
  }

  public sealed class BluePrintFormatter : IMessagePackFormatter<global::BluePrint>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::BluePrint value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(1);
        resolver.GetFormatterWithVerify<global::BluePrint.BluePrintType>().Serialize(ref writer, value.type, options);
      }
    }

    public global::BluePrint Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::BluePrint) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::BluePrint bluePrint = new global::BluePrint();
      for (int index = 0; index < num; ++index)
      {
        if (index == 0)
          bluePrint.type = resolver.GetFormatterWithVerify<global::BluePrint.BluePrintType>().Deserialize(ref reader, options);
        else
          reader.Skip();
      }
      --reader.Depth;
      return bluePrint;
    }
  }

  public sealed class BuyEntryFormatter : IMessagePackFormatter<BuyEntry>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      BuyEntry value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(11);
        writer.Write(value.Bought);
        writer.Write(value.Decoration);
        writer.Write(value.TarotCard);
        resolver.GetFormatterWithVerify<global::TarotCards.Card>().Serialize(ref writer, value.Card, options);
        resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.decorationToBuy, options);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.itemToBuy, options);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.costType, options);
        writer.Write(value.itemCost);
        writer.Write(value.quantity);
        writer.Write(value.GroupID);
        writer.Write(value.SingleQuantityItem);
      }
    }

    public BuyEntry Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (BuyEntry) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      BuyEntry buyEntry = new BuyEntry();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            buyEntry.Bought = reader.ReadBoolean();
            break;
          case 1:
            buyEntry.Decoration = reader.ReadBoolean();
            break;
          case 2:
            buyEntry.TarotCard = reader.ReadBoolean();
            break;
          case 3:
            buyEntry.Card = resolver.GetFormatterWithVerify<global::TarotCards.Card>().Deserialize(ref reader, options);
            break;
          case 4:
            buyEntry.decorationToBuy = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 5:
            buyEntry.itemToBuy = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 6:
            buyEntry.costType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 7:
            buyEntry.itemCost = reader.ReadInt32();
            break;
          case 8:
            buyEntry.quantity = reader.ReadInt32();
            break;
          case 9:
            buyEntry.GroupID = reader.ReadInt32();
            break;
          case 10:
            buyEntry.SingleQuantityItem = reader.ReadBoolean();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return buyEntry;
    }
  }

  public sealed class CrownAbilitiesFormatter : 
    IMessagePackFormatter<global::CrownAbilities>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::CrownAbilities value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(4);
        resolver.GetFormatterWithVerify<global::CrownAbilities.TYPE>().Serialize(ref writer, value.Type, options);
        writer.Write(value.Unlocked);
        writer.Write(value.UnlockProgress);
        writer.Write(value.Used);
      }
    }

    public global::CrownAbilities Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::CrownAbilities) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::CrownAbilities crownAbilities = new global::CrownAbilities();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            crownAbilities.Type = resolver.GetFormatterWithVerify<global::CrownAbilities.TYPE>().Deserialize(ref reader, options);
            break;
          case 1:
            crownAbilities.Unlocked = reader.ReadBoolean();
            break;
          case 2:
            crownAbilities.UnlockProgress = reader.ReadSingle();
            break;
          case 3:
            crownAbilities.Used = reader.ReadBoolean();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return crownAbilities;
    }
  }

  public sealed class DayObjectFormatter : IMessagePackFormatter<DayObject>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      DayObject value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        writer.WriteArrayHeader(2);
        writer.Write(value.MoonPhase);
        writer.Write(value.TotalDays);
      }
    }

    public DayObject Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (DayObject) null;
      options.Security.DepthStep(ref reader);
      int num = reader.ReadArrayHeader();
      DayObject dayObject = new DayObject();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            dayObject.MoonPhase = reader.ReadInt32();
            break;
          case 1:
            dayObject.TotalDays = reader.ReadInt32();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return dayObject;
    }
  }

  public sealed class DoctrineAlertsFormatter : 
    IMessagePackFormatter<DoctrineAlerts>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      DoctrineAlerts value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(2);
        resolver.GetFormatterWithVerify<List<global::DoctrineUpgradeSystem.DoctrineType>>().Serialize(ref writer, value._alerts, options);
        resolver.GetFormatterWithVerify<List<global::DoctrineUpgradeSystem.DoctrineType>>().Serialize(ref writer, value._singleAlerts, options);
      }
    }

    public DoctrineAlerts Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (DoctrineAlerts) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      DoctrineAlerts doctrineAlerts = new DoctrineAlerts();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            doctrineAlerts._alerts = resolver.GetFormatterWithVerify<List<global::DoctrineUpgradeSystem.DoctrineType>>().Deserialize(ref reader, options);
            break;
          case 1:
            doctrineAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::DoctrineUpgradeSystem.DoctrineType>>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return doctrineAlerts;
    }
  }

  public sealed class FollowerInfoSnapshotFormatter : 
    IMessagePackFormatter<FollowerInfoSnapshot>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerInfoSnapshot value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(19);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Name, options);
        writer.Write(value.SkinCharacter);
        writer.Write(value.SkinVariation);
        writer.Write(value.SkinColour);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.SkinName, options);
        resolver.GetFormatterWithVerify<FollowerHatType>().Serialize(ref writer, value.Hat, options);
        resolver.GetFormatterWithVerify<FollowerOutfitType>().Serialize(ref writer, value.Outfit, options);
        resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.Clothing, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ClothingVariant, options);
        resolver.GetFormatterWithVerify<FollowerCustomisationType>().Serialize(ref writer, value.Customisation, options);
        resolver.GetFormatterWithVerify<FollowerSpecialType>().Serialize(ref writer, value.Special, options);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.Necklace, options);
        writer.Write(value.Illness);
        writer.Write(value.Rest);
        writer.Write(value.Brainwashed);
        writer.Write(value.Dissenter);
        writer.Write(value.CultFaith);
        writer.Write(value.Rotten);
        writer.Write(value.ID);
      }
    }

    public FollowerInfoSnapshot Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (FollowerInfoSnapshot) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      FollowerInfoSnapshot followerInfoSnapshot = new FollowerInfoSnapshot();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            followerInfoSnapshot.Name = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1:
            followerInfoSnapshot.SkinCharacter = reader.ReadInt32();
            break;
          case 2:
            followerInfoSnapshot.SkinVariation = reader.ReadInt32();
            break;
          case 3:
            followerInfoSnapshot.SkinColour = reader.ReadInt32();
            break;
          case 4:
            followerInfoSnapshot.SkinName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 5:
            followerInfoSnapshot.Hat = resolver.GetFormatterWithVerify<FollowerHatType>().Deserialize(ref reader, options);
            break;
          case 6:
            followerInfoSnapshot.Outfit = resolver.GetFormatterWithVerify<FollowerOutfitType>().Deserialize(ref reader, options);
            break;
          case 7:
            followerInfoSnapshot.Clothing = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
            break;
          case 8:
            followerInfoSnapshot.ClothingVariant = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 9:
            followerInfoSnapshot.Customisation = resolver.GetFormatterWithVerify<FollowerCustomisationType>().Deserialize(ref reader, options);
            break;
          case 10:
            followerInfoSnapshot.Special = resolver.GetFormatterWithVerify<FollowerSpecialType>().Deserialize(ref reader, options);
            break;
          case 11:
            followerInfoSnapshot.Necklace = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 12:
            followerInfoSnapshot.Illness = reader.ReadSingle();
            break;
          case 13:
            followerInfoSnapshot.Rest = reader.ReadSingle();
            break;
          case 14:
            followerInfoSnapshot.Brainwashed = reader.ReadBoolean();
            break;
          case 15:
            followerInfoSnapshot.Dissenter = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            followerInfoSnapshot.CultFaith = reader.ReadSingle();
            break;
          case 17:
            followerInfoSnapshot.Rotten = reader.ReadInt32();
            break;
          case 18:
            followerInfoSnapshot.ID = reader.ReadInt32();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return followerInfoSnapshot;
    }
  }

  public sealed class FollowerInteractionAlertsFormatter : 
    IMessagePackFormatter<FollowerInteractionAlerts>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerInteractionAlerts value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(2);
        resolver.GetFormatterWithVerify<List<FollowerCommands>>().Serialize(ref writer, value._alerts, options);
        resolver.GetFormatterWithVerify<List<FollowerCommands>>().Serialize(ref writer, value._singleAlerts, options);
      }
    }

    public FollowerInteractionAlerts Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (FollowerInteractionAlerts) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      FollowerInteractionAlerts interactionAlerts = new FollowerInteractionAlerts();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            interactionAlerts._alerts = resolver.GetFormatterWithVerify<List<FollowerCommands>>().Deserialize(ref reader, options);
            break;
          case 1:
            interactionAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<FollowerCommands>>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return interactionAlerts;
    }
  }

  public sealed class FollowerRoleInfoFormatter : 
    IMessagePackFormatter<FollowerRoleInfo>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerRoleInfo value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
        writer.WriteNil();
      else
        writer.WriteArrayHeader(0);
    }

    public FollowerRoleInfo Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (FollowerRoleInfo) null;
      reader.Skip();
      return new FollowerRoleInfo();
    }
  }

  public sealed class FollowerThoughtsFormatter : 
    IMessagePackFormatter<FollowerThoughts>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerThoughts value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
        writer.WriteNil();
      else
        writer.WriteArrayHeader(0);
    }

    public FollowerThoughts Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (FollowerThoughts) null;
      reader.Skip();
      return new FollowerThoughts();
    }
  }

  public sealed class IDAndRelationshipFormatter : 
    IMessagePackFormatter<global::IDAndRelationship>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::IDAndRelationship value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(3);
        writer.Write(value.ID);
        writer.Write(value.Relationship);
        resolver.GetFormatterWithVerify<global::IDAndRelationship.RelationshipState>().Serialize(ref writer, value.CurrentRelationshipState, options);
      }
    }

    public global::IDAndRelationship Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::IDAndRelationship) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::IDAndRelationship idAndRelationship = new global::IDAndRelationship();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            idAndRelationship.ID = reader.ReadInt32();
            break;
          case 1:
            idAndRelationship.Relationship = reader.ReadInt32();
            break;
          case 2:
            idAndRelationship.CurrentRelationshipState = resolver.GetFormatterWithVerify<global::IDAndRelationship.RelationshipState>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return idAndRelationship;
    }
  }

  public sealed class InventoryFormatter : IMessagePackFormatter<Inventory>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      Inventory value,
      MessagePackSerializerOptions options)
    {
      if ((UnityEngine.Object) value == (UnityEngine.Object) null)
        writer.WriteNil();
      else
        writer.WriteArrayHeader(0);
    }

    public Inventory Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (Inventory) null;
      reader.Skip();
      return new Inventory();
    }
  }

  public sealed class InventoryItemFormatter : 
    IMessagePackFormatter<global::InventoryItem>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::InventoryItem value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        writer.WriteArrayHeader(3);
        writer.Write(value.type);
        writer.Write(value.quantity);
        writer.Write(value.QuantityReserved);
      }
    }

    public global::InventoryItem Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::InventoryItem) null;
      options.Security.DepthStep(ref reader);
      int num = reader.ReadArrayHeader();
      global::InventoryItem inventoryItem = new global::InventoryItem();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            inventoryItem.type = reader.ReadInt32();
            break;
          case 1:
            inventoryItem.quantity = reader.ReadInt32();
            break;
          case 2:
            inventoryItem.QuantityReserved = reader.ReadInt32();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return inventoryItem;
    }
  }

  public sealed class InventoryWeaponFormatter : 
    IMessagePackFormatter<global::InventoryWeapon>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::InventoryWeapon value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(3);
        resolver.GetFormatterWithVerify<global::InventoryWeapon.ITEM_TYPE>().Serialize(ref writer, value.type, options);
        writer.Write(value.quantity);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.name, options);
      }
    }

    public global::InventoryWeapon Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::InventoryWeapon) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::InventoryWeapon.ITEM_TYPE type = global::InventoryWeapon.ITEM_TYPE.SWORD;
      int quantity = 0;
      string str = (string) null;
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            type = resolver.GetFormatterWithVerify<global::InventoryWeapon.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 1:
            quantity = reader.ReadInt32();
            break;
          case 2:
            str = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      global::InventoryWeapon inventoryWeapon = new global::InventoryWeapon(type, quantity);
      if (num > 2)
        inventoryWeapon.name = str;
      --reader.Depth;
      return inventoryWeapon;
    }
  }

  public sealed class JellyFishInvestmentFormatter : 
    IMessagePackFormatter<JellyFishInvestment>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      JellyFishInvestment value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(6);
        writer.Write(value.InitialInvestment);
        writer.Write(value.ActualInvestedAmount);
        writer.Write(value.NewInvestment);
        writer.Write(value.InvestmentDay);
        resolver.GetFormatterWithVerify<List<JellyFishInvestmentDay>>().Serialize(ref writer, value.InvestmentDays, options);
        writer.Write(value.LastDayCheckedInvestment);
      }
    }

    public JellyFishInvestment Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (JellyFishInvestment) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      JellyFishInvestment jellyFishInvestment = new JellyFishInvestment();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            jellyFishInvestment.InitialInvestment = reader.ReadInt32();
            break;
          case 1:
            jellyFishInvestment.ActualInvestedAmount = reader.ReadInt32();
            break;
          case 2:
            jellyFishInvestment.NewInvestment = reader.ReadInt32();
            break;
          case 3:
            jellyFishInvestment.InvestmentDay = reader.ReadInt32();
            break;
          case 4:
            jellyFishInvestment.InvestmentDays = resolver.GetFormatterWithVerify<List<JellyFishInvestmentDay>>().Deserialize(ref reader, options);
            break;
          case 5:
            jellyFishInvestment.LastDayCheckedInvestment = reader.ReadInt32();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return jellyFishInvestment;
    }
  }

  public sealed class JellyFishInvestmentDayFormatter : 
    IMessagePackFormatter<JellyFishInvestmentDay>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      JellyFishInvestmentDay value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        writer.WriteArrayHeader(3);
        writer.Write(value.Day);
        writer.Write(value.InvestmentAmount);
        writer.Write(value.InterestRate);
      }
    }

    public JellyFishInvestmentDay Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (JellyFishInvestmentDay) null;
      options.Security.DepthStep(ref reader);
      int num = reader.ReadArrayHeader();
      JellyFishInvestmentDay fishInvestmentDay = new JellyFishInvestmentDay();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            fishInvestmentDay.Day = reader.ReadInt32();
            break;
          case 1:
            fishInvestmentDay.InvestmentAmount = reader.ReadInt32();
            break;
          case 2:
            fishInvestmentDay.InterestRate = reader.ReadSingle();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return fishInvestmentDay;
    }
  }

  public sealed class MidasDonationFormatter : 
    IMessagePackFormatter<MidasDonation>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      MidasDonation value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        writer.WriteArrayHeader(2);
        writer.Write(value.Day);
        writer.Write(value.InvestmentAmount);
      }
    }

    public MidasDonation Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (MidasDonation) null;
      options.Security.DepthStep(ref reader);
      int num = reader.ReadArrayHeader();
      MidasDonation midasDonation = new MidasDonation();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            midasDonation.Day = reader.ReadInt32();
            break;
          case 1:
            midasDonation.InvestmentAmount = reader.ReadInt32();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return midasDonation;
    }
  }

  public sealed class Objective_FindRelicFormatter : 
    IMessagePackFormatter<global::Objective_FindRelic>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objective_FindRelic value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.TargetLocation, options);
        resolver.GetFormatterWithVerify<RelicType>().Serialize(ref writer, value.RelicType, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objective_FindRelic Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objective_FindRelic) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objective_FindRelic objectiveFindRelic = new global::Objective_FindRelic();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectiveFindRelic.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectiveFindRelic.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectiveFindRelic.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectiveFindRelic.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectiveFindRelic.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectiveFindRelic.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectiveFindRelic.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectiveFindRelic.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectiveFindRelic.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectiveFindRelic.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectiveFindRelic.ID = reader.ReadInt32();
            break;
          case 11:
            objectiveFindRelic.Index = reader.ReadInt32();
            break;
          case 12:
            objectiveFindRelic.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectiveFindRelic.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectiveFindRelic.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectiveFindRelic.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectiveFindRelic.TargetLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 17:
            objectiveFindRelic.RelicType = resolver.GetFormatterWithVerify<RelicType>().Deserialize(ref reader, options);
            break;
          case 25:
            objectiveFindRelic.Follower = reader.ReadInt32();
            break;
          case 26:
            objectiveFindRelic.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectiveFindRelic;
    }
  }

  public sealed class Objectives_AssignClothingFormatter : 
    IMessagePackFormatter<global::Objectives_AssignClothing>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_AssignClothing value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.ClothingType, options);
        writer.Write(value.TargetFollower);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_AssignClothing Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_AssignClothing) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_AssignClothing objectivesAssignClothing = new global::Objectives_AssignClothing();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesAssignClothing.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesAssignClothing.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesAssignClothing.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesAssignClothing.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesAssignClothing.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesAssignClothing.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesAssignClothing.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesAssignClothing.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesAssignClothing.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesAssignClothing.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesAssignClothing.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesAssignClothing.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesAssignClothing.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesAssignClothing.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesAssignClothing.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesAssignClothing.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesAssignClothing.ClothingType = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesAssignClothing.TargetFollower = reader.ReadInt32();
            break;
          case 25:
            objectivesAssignClothing.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesAssignClothing.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesAssignClothing;
    }
  }

  public sealed class Objectives_BedRestFormatter : 
    IMessagePackFormatter<global::Objectives_BedRest>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_BedRest value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.FollowerName, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_BedRest Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_BedRest) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_BedRest objectivesBedRest = new global::Objectives_BedRest();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesBedRest.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesBedRest.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesBedRest.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesBedRest.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesBedRest.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesBedRest.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesBedRest.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesBedRest.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesBedRest.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesBedRest.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesBedRest.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesBedRest.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesBedRest.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesBedRest.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesBedRest.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesBedRest.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesBedRest.FollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesBedRest.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesBedRest.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesBedRest;
    }
  }

  public sealed class Objectives_BlizzardOfferingFormatter : 
    IMessagePackFormatter<global::Objectives_BlizzardOffering>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_BlizzardOffering value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.Target);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.TargetType, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_BlizzardOffering Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_BlizzardOffering) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_BlizzardOffering blizzardOffering = new global::Objectives_BlizzardOffering();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            blizzardOffering.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            blizzardOffering.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            blizzardOffering.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            blizzardOffering.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            blizzardOffering.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            blizzardOffering.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            blizzardOffering.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            blizzardOffering.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            blizzardOffering.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            blizzardOffering.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            blizzardOffering.ID = reader.ReadInt32();
            break;
          case 11:
            blizzardOffering.Index = reader.ReadInt32();
            break;
          case 12:
            blizzardOffering.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            blizzardOffering.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            blizzardOffering.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            blizzardOffering.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            blizzardOffering.Target = reader.ReadInt32();
            break;
          case 17:
            blizzardOffering.TargetType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 25:
            blizzardOffering.Follower = reader.ReadInt32();
            break;
          case 26:
            blizzardOffering.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return blizzardOffering;
    }
  }

  public sealed class Objectives_BuildStructureFormatter : 
    IMessagePackFormatter<global::Objectives_BuildStructure>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_BuildStructure value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.StructureType, options);
        writer.Write(value.Target);
        writer.Write(value.Count);
        writer.Write(value.IncludeAlreadyBuilt);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_BuildStructure Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_BuildStructure) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_BuildStructure objectivesBuildStructure = new global::Objectives_BuildStructure();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesBuildStructure.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesBuildStructure.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesBuildStructure.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesBuildStructure.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesBuildStructure.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesBuildStructure.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesBuildStructure.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesBuildStructure.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesBuildStructure.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesBuildStructure.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesBuildStructure.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesBuildStructure.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesBuildStructure.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesBuildStructure.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesBuildStructure.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesBuildStructure.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesBuildStructure.StructureType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesBuildStructure.Target = reader.ReadInt32();
            break;
          case 18:
            objectivesBuildStructure.Count = reader.ReadInt32();
            break;
          case 19:
            objectivesBuildStructure.IncludeAlreadyBuilt = reader.ReadBoolean();
            break;
          case 25:
            objectivesBuildStructure.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesBuildStructure.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesBuildStructure;
    }
  }

  public sealed class Objectives_BuildWinterDecorationsFormatter : 
    IMessagePackFormatter<global::Objectives_BuildWinterDecorations>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_BuildWinterDecorations value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.Target);
        writer.Write(value.Count);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_BuildWinterDecorations Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_BuildWinterDecorations) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_BuildWinterDecorations winterDecorations = new global::Objectives_BuildWinterDecorations();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            winterDecorations.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            winterDecorations.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            winterDecorations.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            winterDecorations.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            winterDecorations.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            winterDecorations.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            winterDecorations.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            winterDecorations.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            winterDecorations.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            winterDecorations.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            winterDecorations.ID = reader.ReadInt32();
            break;
          case 11:
            winterDecorations.Index = reader.ReadInt32();
            break;
          case 12:
            winterDecorations.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            winterDecorations.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            winterDecorations.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            winterDecorations.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            winterDecorations.Target = reader.ReadInt32();
            break;
          case 17:
            winterDecorations.Count = reader.ReadInt32();
            break;
          case 25:
            winterDecorations.Follower = reader.ReadInt32();
            break;
          case 26:
            winterDecorations.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return winterDecorations;
    }
  }

  public sealed class Objectives_CollectItemFormatter : 
    IMessagePackFormatter<global::Objectives_CollectItem>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_CollectItem value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.ItemType, options);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.TargetLocation, options);
        writer.Write(value.Target);
        writer.Write(value.StartingAmount);
        writer.Write(value.Count);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CustomTerm, options);
        writer.Write(value.countIsTotal);
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_CollectItem Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_CollectItem) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_CollectItem objectivesCollectItem = new global::Objectives_CollectItem();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesCollectItem.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesCollectItem.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesCollectItem.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesCollectItem.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesCollectItem.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesCollectItem.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesCollectItem.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesCollectItem.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesCollectItem.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesCollectItem.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesCollectItem.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesCollectItem.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesCollectItem.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesCollectItem.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesCollectItem.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesCollectItem.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesCollectItem.ItemType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesCollectItem.TargetLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 18:
            objectivesCollectItem.Target = reader.ReadInt32();
            break;
          case 19:
            objectivesCollectItem.StartingAmount = reader.ReadInt32();
            break;
          case 20:
            objectivesCollectItem.Count = reader.ReadInt32();
            break;
          case 21:
            objectivesCollectItem.CustomTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 22:
            objectivesCollectItem.countIsTotal = reader.ReadBoolean();
            break;
          case 25:
            objectivesCollectItem.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesCollectItem.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesCollectItem;
    }
  }

  public sealed class Objectives_CookMealFormatter : 
    IMessagePackFormatter<global::Objectives_CookMeal>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_CookMeal value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.MealType, options);
        writer.Write(value.Count);
        writer.Write(value.StartingAmount);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_CookMeal Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_CookMeal) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_CookMeal objectivesCookMeal = new global::Objectives_CookMeal();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesCookMeal.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesCookMeal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesCookMeal.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesCookMeal.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesCookMeal.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesCookMeal.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesCookMeal.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesCookMeal.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesCookMeal.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesCookMeal.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesCookMeal.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesCookMeal.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesCookMeal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesCookMeal.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesCookMeal.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesCookMeal.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesCookMeal.MealType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesCookMeal.Count = reader.ReadInt32();
            break;
          case 18:
            objectivesCookMeal.StartingAmount = reader.ReadInt32();
            break;
          case 25:
            objectivesCookMeal.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesCookMeal.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesCookMeal;
    }
  }

  public sealed class Objectives_CraftClothingFormatter : 
    IMessagePackFormatter<global::Objectives_CraftClothing>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_CraftClothing value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.ClothingType, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_CraftClothing Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_CraftClothing) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_CraftClothing objectivesCraftClothing = new global::Objectives_CraftClothing();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesCraftClothing.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesCraftClothing.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesCraftClothing.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesCraftClothing.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesCraftClothing.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesCraftClothing.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesCraftClothing.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesCraftClothing.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesCraftClothing.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesCraftClothing.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesCraftClothing.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesCraftClothing.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesCraftClothing.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesCraftClothing.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesCraftClothing.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesCraftClothing.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesCraftClothing.ClothingType = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesCraftClothing.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesCraftClothing.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesCraftClothing;
    }
  }

  public sealed class Objectives_CustomFormatter : 
    IMessagePackFormatter<global::Objectives_Custom>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_Custom value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::Objectives.CustomQuestTypes>().Serialize(ref writer, value.CustomQuestType, options);
        writer.Write(value.TargetFollowerID);
        writer.Write(value.ResultFollowerID);
        resolver.GetFormatterWithVerify<List<global::FollowerTrait.TraitType>>().Serialize(ref writer, value.Traits, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_Custom Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_Custom) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_Custom objectivesCustom = new global::Objectives_Custom();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesCustom.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesCustom.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesCustom.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesCustom.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesCustom.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesCustom.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesCustom.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesCustom.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesCustom.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesCustom.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesCustom.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesCustom.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesCustom.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesCustom.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesCustom.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesCustom.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesCustom.CustomQuestType = resolver.GetFormatterWithVerify<global::Objectives.CustomQuestTypes>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesCustom.TargetFollowerID = reader.ReadInt32();
            break;
          case 18:
            objectivesCustom.ResultFollowerID = reader.ReadInt32();
            break;
          case 19:
            objectivesCustom.Traits = resolver.GetFormatterWithVerify<List<global::FollowerTrait.TraitType>>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesCustom.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesCustom.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesCustom;
    }
  }

  public sealed class Objectives_DefeatKnucklebonesFormatter : 
    IMessagePackFormatter<global::Objectives_DefeatKnucklebones>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_DefeatKnucklebones value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CharacterNameTerm, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_DefeatKnucklebones Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_DefeatKnucklebones) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_DefeatKnucklebones defeatKnucklebones = new global::Objectives_DefeatKnucklebones();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            defeatKnucklebones.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            defeatKnucklebones.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            defeatKnucklebones.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            defeatKnucklebones.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            defeatKnucklebones.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            defeatKnucklebones.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            defeatKnucklebones.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            defeatKnucklebones.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            defeatKnucklebones.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            defeatKnucklebones.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            defeatKnucklebones.ID = reader.ReadInt32();
            break;
          case 11:
            defeatKnucklebones.Index = reader.ReadInt32();
            break;
          case 12:
            defeatKnucklebones.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            defeatKnucklebones.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            defeatKnucklebones.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            defeatKnucklebones.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            defeatKnucklebones.CharacterNameTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 25:
            defeatKnucklebones.Follower = reader.ReadInt32();
            break;
          case 26:
            defeatKnucklebones.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return defeatKnucklebones;
    }
  }

  public sealed class Objectives_DepositFoodFormatter : 
    IMessagePackFormatter<global::Objectives_DepositFood>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_DepositFood value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_DepositFood Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_DepositFood) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_DepositFood objectivesDepositFood = new global::Objectives_DepositFood();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesDepositFood.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesDepositFood.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesDepositFood.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesDepositFood.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesDepositFood.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesDepositFood.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesDepositFood.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesDepositFood.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesDepositFood.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesDepositFood.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesDepositFood.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesDepositFood.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesDepositFood.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesDepositFood.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesDepositFood.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesDepositFood.IsWinterObjective = reader.ReadBoolean();
            break;
          case 25:
            objectivesDepositFood.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesDepositFood.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesDepositFood;
    }
  }

  public sealed class Objectives_DrinkFormatter : 
    IMessagePackFormatter<global::Objectives_Drink>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_Drink value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.DrinkType, options);
        writer.Write(value.TargetFollower);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_Drink Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_Drink) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_Drink objectivesDrink = new global::Objectives_Drink();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesDrink.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesDrink.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesDrink.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesDrink.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesDrink.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesDrink.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesDrink.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesDrink.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesDrink.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesDrink.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesDrink.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesDrink.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesDrink.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesDrink.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesDrink.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesDrink.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesDrink.DrinkType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesDrink.TargetFollower = reader.ReadInt32();
            break;
          case 25:
            objectivesDrink.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesDrink.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesDrink;
    }
  }

  public sealed class Objectives_EatMealFormatter : 
    IMessagePackFormatter<global::Objectives_EatMeal>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_EatMeal value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.MealType, options);
        writer.Write(value.TargetFollower);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_EatMeal Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_EatMeal) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_EatMeal objectivesEatMeal = new global::Objectives_EatMeal();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesEatMeal.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesEatMeal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesEatMeal.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesEatMeal.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesEatMeal.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesEatMeal.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesEatMeal.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesEatMeal.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesEatMeal.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesEatMeal.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesEatMeal.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesEatMeal.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesEatMeal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesEatMeal.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesEatMeal.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesEatMeal.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesEatMeal.MealType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesEatMeal.TargetFollower = reader.ReadInt32();
            break;
          case 25:
            objectivesEatMeal.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesEatMeal.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesEatMeal;
    }
  }

  public sealed class Objectives_FeedAnimalFormatter : 
    IMessagePackFormatter<global::Objectives_FeedAnimal>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_FeedAnimal value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.TargetAnimal);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.Food, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_FeedAnimal Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_FeedAnimal) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_FeedAnimal objectivesFeedAnimal = new global::Objectives_FeedAnimal();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesFeedAnimal.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesFeedAnimal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesFeedAnimal.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesFeedAnimal.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesFeedAnimal.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesFeedAnimal.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesFeedAnimal.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesFeedAnimal.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesFeedAnimal.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesFeedAnimal.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesFeedAnimal.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesFeedAnimal.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesFeedAnimal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesFeedAnimal.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesFeedAnimal.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesFeedAnimal.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesFeedAnimal.TargetAnimal = reader.ReadInt32();
            break;
          case 17:
            objectivesFeedAnimal.Food = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesFeedAnimal.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesFeedAnimal.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesFeedAnimal;
    }
  }

  public sealed class Objectives_FindChildrenFormatter : 
    IMessagePackFormatter<global::Objectives_FindChildren>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_FindChildren value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::Objectives_FindChildren.ChildLocation>().Serialize(ref writer, value.Location, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_FindChildren Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_FindChildren) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_FindChildren objectivesFindChildren = new global::Objectives_FindChildren();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesFindChildren.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesFindChildren.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesFindChildren.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesFindChildren.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesFindChildren.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesFindChildren.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesFindChildren.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesFindChildren.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesFindChildren.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesFindChildren.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesFindChildren.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesFindChildren.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesFindChildren.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesFindChildren.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesFindChildren.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesFindChildren.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesFindChildren.Location = resolver.GetFormatterWithVerify<global::Objectives_FindChildren.ChildLocation>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesFindChildren.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesFindChildren.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesFindChildren;
    }
  }

  public sealed class Objectives_FindFollowerFormatter : 
    IMessagePackFormatter<global::Objectives_FindFollower>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_FindFollower value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.TargetLocation, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.FollowerSkin, options);
        writer.Write(value.FollowerColour);
        writer.Write(value.FollowerVariant);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
        writer.Write(value.ObjectiveVariant);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_FindFollower Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_FindFollower) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_FindFollower objectivesFindFollower = new global::Objectives_FindFollower();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesFindFollower.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesFindFollower.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesFindFollower.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesFindFollower.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesFindFollower.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesFindFollower.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesFindFollower.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesFindFollower.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesFindFollower.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesFindFollower.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesFindFollower.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesFindFollower.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesFindFollower.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesFindFollower.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesFindFollower.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesFindFollower.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesFindFollower.TargetLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesFindFollower.FollowerSkin = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 18:
            objectivesFindFollower.FollowerColour = reader.ReadInt32();
            break;
          case 19:
            objectivesFindFollower.FollowerVariant = reader.ReadInt32();
            break;
          case 20:
            objectivesFindFollower.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 21:
            objectivesFindFollower.ObjectiveVariant = reader.ReadInt32();
            break;
          case 25:
            objectivesFindFollower.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesFindFollower.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesFindFollower;
    }
  }

  public sealed class Objectives_FinishRaceFormatter : 
    IMessagePackFormatter<global::Objectives_FinishRace>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_FinishRace value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.RaceTargetTime);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_FinishRace Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_FinishRace) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_FinishRace objectivesFinishRace = new global::Objectives_FinishRace();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesFinishRace.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesFinishRace.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesFinishRace.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesFinishRace.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesFinishRace.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesFinishRace.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesFinishRace.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesFinishRace.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesFinishRace.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesFinishRace.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesFinishRace.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesFinishRace.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesFinishRace.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesFinishRace.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesFinishRace.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesFinishRace.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesFinishRace.RaceTargetTime = reader.ReadSingle();
            break;
          case 25:
            objectivesFinishRace.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesFinishRace.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesFinishRace;
    }
  }

  public sealed class Objectives_FlowerBasketsFormatter : 
    IMessagePackFormatter<global::Objectives_FlowerBaskets>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_FlowerBaskets value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.FilledPots);
        writer.Write(value.PotsToFill);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_FlowerBaskets Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_FlowerBaskets) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_FlowerBaskets objectivesFlowerBaskets = new global::Objectives_FlowerBaskets();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesFlowerBaskets.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesFlowerBaskets.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesFlowerBaskets.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesFlowerBaskets.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesFlowerBaskets.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesFlowerBaskets.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesFlowerBaskets.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesFlowerBaskets.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesFlowerBaskets.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesFlowerBaskets.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesFlowerBaskets.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesFlowerBaskets.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesFlowerBaskets.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesFlowerBaskets.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesFlowerBaskets.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesFlowerBaskets.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesFlowerBaskets.FilledPots = reader.ReadInt32();
            break;
          case 17:
            objectivesFlowerBaskets.PotsToFill = reader.ReadInt32();
            break;
          case 25:
            objectivesFlowerBaskets.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesFlowerBaskets.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesFlowerBaskets;
    }
  }

  public sealed class Objectives_GetAnimalFormatter : 
    IMessagePackFormatter<global::Objectives_GetAnimal>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_GetAnimal value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.AnimalType, options);
        writer.Write(value.Level);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_GetAnimal Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_GetAnimal) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_GetAnimal objectivesGetAnimal = new global::Objectives_GetAnimal();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesGetAnimal.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesGetAnimal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesGetAnimal.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesGetAnimal.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesGetAnimal.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesGetAnimal.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesGetAnimal.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesGetAnimal.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesGetAnimal.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesGetAnimal.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesGetAnimal.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesGetAnimal.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesGetAnimal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesGetAnimal.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesGetAnimal.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesGetAnimal.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesGetAnimal.AnimalType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesGetAnimal.Level = reader.ReadInt32();
            break;
          case 25:
            objectivesGetAnimal.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesGetAnimal.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesGetAnimal;
    }
  }

  public sealed class Objectives_GiveItemFormatter : 
    IMessagePackFormatter<global::Objectives_GiveItem>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_GiveItem value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.NPCTerm, options);
        writer.Write(value.Quantity);
        writer.Write(value.Target);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.TargetType, options);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.Location, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_GiveItem Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_GiveItem) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_GiveItem objectivesGiveItem = new global::Objectives_GiveItem();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesGiveItem.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesGiveItem.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesGiveItem.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesGiveItem.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesGiveItem.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesGiveItem.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesGiveItem.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesGiveItem.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesGiveItem.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesGiveItem.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesGiveItem.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesGiveItem.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesGiveItem.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesGiveItem.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesGiveItem.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesGiveItem.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesGiveItem.NPCTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesGiveItem.Quantity = reader.ReadInt32();
            break;
          case 18:
            objectivesGiveItem.Target = reader.ReadInt32();
            break;
          case 19:
            objectivesGiveItem.TargetType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 20:
            objectivesGiveItem.Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesGiveItem.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesGiveItem.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesGiveItem;
    }
  }

  public sealed class Objectives_KillEnemiesFormatter : 
    IMessagePackFormatter<global::Objectives_KillEnemies>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_KillEnemies value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<Enemy>().Serialize(ref writer, value.enemyType, options);
        writer.Write(value.enemiesKilledBeforeObjectiveBegan);
        writer.Write(value.enemiesKilledRequired);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_KillEnemies Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_KillEnemies) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_KillEnemies objectivesKillEnemies = new global::Objectives_KillEnemies();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesKillEnemies.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesKillEnemies.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesKillEnemies.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesKillEnemies.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesKillEnemies.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesKillEnemies.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesKillEnemies.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesKillEnemies.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesKillEnemies.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesKillEnemies.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesKillEnemies.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesKillEnemies.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesKillEnemies.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesKillEnemies.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesKillEnemies.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesKillEnemies.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesKillEnemies.enemyType = resolver.GetFormatterWithVerify<Enemy>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesKillEnemies.enemiesKilledBeforeObjectiveBegan = reader.ReadInt32();
            break;
          case 18:
            objectivesKillEnemies.enemiesKilledRequired = reader.ReadInt32();
            break;
          case 25:
            objectivesKillEnemies.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesKillEnemies.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesKillEnemies;
    }
  }

  public sealed class Objectives_LegendarySwordReturnFormatter : 
    IMessagePackFormatter<global::Objectives_LegendarySwordReturn>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_LegendarySwordReturn value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_LegendarySwordReturn Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_LegendarySwordReturn) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_LegendarySwordReturn legendarySwordReturn = new global::Objectives_LegendarySwordReturn();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            legendarySwordReturn.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            legendarySwordReturn.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            legendarySwordReturn.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            legendarySwordReturn.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            legendarySwordReturn.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            legendarySwordReturn.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            legendarySwordReturn.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            legendarySwordReturn.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            legendarySwordReturn.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            legendarySwordReturn.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            legendarySwordReturn.ID = reader.ReadInt32();
            break;
          case 11:
            legendarySwordReturn.Index = reader.ReadInt32();
            break;
          case 12:
            legendarySwordReturn.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            legendarySwordReturn.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            legendarySwordReturn.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            legendarySwordReturn.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            legendarySwordReturn.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 25:
            legendarySwordReturn.Follower = reader.ReadInt32();
            break;
          case 26:
            legendarySwordReturn.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return legendarySwordReturn;
    }
  }

  public sealed class Objectives_LegendaryWeaponRunFormatter : 
    IMessagePackFormatter<global::Objectives_LegendaryWeaponRun>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_LegendaryWeaponRun value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<EquipmentType>().Serialize(ref writer, value.LegendaryWeapon, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_LegendaryWeaponRun Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_LegendaryWeaponRun) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_LegendaryWeaponRun legendaryWeaponRun = new global::Objectives_LegendaryWeaponRun();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            legendaryWeaponRun.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            legendaryWeaponRun.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            legendaryWeaponRun.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            legendaryWeaponRun.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            legendaryWeaponRun.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            legendaryWeaponRun.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            legendaryWeaponRun.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            legendaryWeaponRun.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            legendaryWeaponRun.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            legendaryWeaponRun.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            legendaryWeaponRun.ID = reader.ReadInt32();
            break;
          case 11:
            legendaryWeaponRun.Index = reader.ReadInt32();
            break;
          case 12:
            legendaryWeaponRun.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            legendaryWeaponRun.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            legendaryWeaponRun.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            legendaryWeaponRun.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            legendaryWeaponRun.LegendaryWeapon = resolver.GetFormatterWithVerify<EquipmentType>().Deserialize(ref reader, options);
            break;
          case 25:
            legendaryWeaponRun.Follower = reader.ReadInt32();
            break;
          case 26:
            legendaryWeaponRun.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return legendaryWeaponRun;
    }
  }

  public sealed class Objectives_MatingFormatter : 
    IMessagePackFormatter<global::Objectives_Mating>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_Mating value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.TargetFollower_1);
        writer.Write(value.TargetFollower_2);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_Mating Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_Mating) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_Mating objectivesMating = new global::Objectives_Mating();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesMating.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesMating.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesMating.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesMating.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesMating.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesMating.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesMating.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesMating.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesMating.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesMating.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesMating.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesMating.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesMating.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesMating.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesMating.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesMating.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesMating.TargetFollower_1 = reader.ReadInt32();
            break;
          case 17:
            objectivesMating.TargetFollower_2 = reader.ReadInt32();
            break;
          case 25:
            objectivesMating.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesMating.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesMating;
    }
  }

  public sealed class Objectives_NoCursesFormatter : 
    IMessagePackFormatter<global::Objectives_NoCurses>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_NoCurses value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.RoomsRequired);
        writer.Write(value.RoomsCompleted);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_NoCurses Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_NoCurses) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_NoCurses objectivesNoCurses = new global::Objectives_NoCurses();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesNoCurses.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesNoCurses.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesNoCurses.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesNoCurses.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesNoCurses.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesNoCurses.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesNoCurses.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesNoCurses.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesNoCurses.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesNoCurses.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesNoCurses.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesNoCurses.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesNoCurses.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesNoCurses.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesNoCurses.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesNoCurses.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesNoCurses.RoomsRequired = reader.ReadInt32();
            break;
          case 17:
            objectivesNoCurses.RoomsCompleted = reader.ReadInt32();
            break;
          case 25:
            objectivesNoCurses.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesNoCurses.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesNoCurses;
    }
  }

  public sealed class Objectives_NoDamageFormatter : 
    IMessagePackFormatter<global::Objectives_NoDamage>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_NoDamage value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.RoomsRequired);
        writer.Write(value.RoomsCompleted);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_NoDamage Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_NoDamage) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_NoDamage objectivesNoDamage = new global::Objectives_NoDamage();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesNoDamage.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesNoDamage.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesNoDamage.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesNoDamage.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesNoDamage.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesNoDamage.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesNoDamage.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesNoDamage.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesNoDamage.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesNoDamage.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesNoDamage.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesNoDamage.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesNoDamage.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesNoDamage.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesNoDamage.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesNoDamage.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesNoDamage.RoomsRequired = reader.ReadInt32();
            break;
          case 17:
            objectivesNoDamage.RoomsCompleted = reader.ReadInt32();
            break;
          case 25:
            objectivesNoDamage.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesNoDamage.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesNoDamage;
    }
  }

  public sealed class Objectives_NoDodgeFormatter : 
    IMessagePackFormatter<global::Objectives_NoDodge>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_NoDodge value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.RoomsRequired);
        writer.Write(value.RoomsCompleted);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_NoDodge Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_NoDodge) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_NoDodge objectivesNoDodge = new global::Objectives_NoDodge();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesNoDodge.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesNoDodge.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesNoDodge.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesNoDodge.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesNoDodge.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesNoDodge.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesNoDodge.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesNoDodge.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesNoDodge.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesNoDodge.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesNoDodge.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesNoDodge.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesNoDodge.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesNoDodge.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesNoDodge.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesNoDodge.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesNoDodge.RoomsRequired = reader.ReadInt32();
            break;
          case 17:
            objectivesNoDodge.RoomsCompleted = reader.ReadInt32();
            break;
          case 25:
            objectivesNoDodge.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesNoDodge.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesNoDodge;
    }
  }

  public sealed class Objectives_NoHealingFormatter : 
    IMessagePackFormatter<global::Objectives_NoHealing>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_NoHealing value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.RoomsRequired);
        writer.Write(value.RoomsCompleted);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_NoHealing Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_NoHealing) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_NoHealing objectivesNoHealing = new global::Objectives_NoHealing();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesNoHealing.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesNoHealing.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesNoHealing.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesNoHealing.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesNoHealing.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesNoHealing.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesNoHealing.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesNoHealing.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesNoHealing.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesNoHealing.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesNoHealing.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesNoHealing.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesNoHealing.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesNoHealing.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesNoHealing.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesNoHealing.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesNoHealing.RoomsRequired = reader.ReadInt32();
            break;
          case 17:
            objectivesNoHealing.RoomsCompleted = reader.ReadInt32();
            break;
          case 25:
            objectivesNoHealing.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesNoHealing.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesNoHealing;
    }
  }

  public sealed class Objectives_PerformRitualFormatter : 
    IMessagePackFormatter<global::Objectives_PerformRitual>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_PerformRitual value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Serialize(ref writer, value.Ritual, options);
        writer.Write(value.TargetFollowerID_1);
        writer.Write(value.TargetFollowerID_2);
        writer.Write(value.RequiredFollowers);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_PerformRitual Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_PerformRitual) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_PerformRitual objectivesPerformRitual = new global::Objectives_PerformRitual();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesPerformRitual.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesPerformRitual.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesPerformRitual.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesPerformRitual.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesPerformRitual.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesPerformRitual.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesPerformRitual.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesPerformRitual.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesPerformRitual.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesPerformRitual.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesPerformRitual.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesPerformRitual.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesPerformRitual.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesPerformRitual.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesPerformRitual.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesPerformRitual.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesPerformRitual.Ritual = resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesPerformRitual.TargetFollowerID_1 = reader.ReadInt32();
            break;
          case 18:
            objectivesPerformRitual.TargetFollowerID_2 = reader.ReadInt32();
            break;
          case 19:
            objectivesPerformRitual.RequiredFollowers = reader.ReadInt32();
            break;
          case 25:
            objectivesPerformRitual.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesPerformRitual.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesPerformRitual;
    }
  }

  public sealed class Objectives_PlaceStructureFormatter : 
    IMessagePackFormatter<global::Objectives_PlaceStructure>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_PlaceStructure value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::StructureBrain.Categories>().Serialize(ref writer, value.category, options);
        resolver.GetFormatterWithVerify<global::Objectives_PlaceStructure.DecorationType>().Serialize(ref writer, value.DecoType, options);
        writer.Write(value.Count);
        writer.Write(value.Target);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Term, options);
        writer.Write(value.IncludeAlreadyBuilt);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_PlaceStructure Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_PlaceStructure) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_PlaceStructure objectivesPlaceStructure = new global::Objectives_PlaceStructure();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesPlaceStructure.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesPlaceStructure.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesPlaceStructure.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesPlaceStructure.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesPlaceStructure.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesPlaceStructure.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesPlaceStructure.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesPlaceStructure.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesPlaceStructure.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesPlaceStructure.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesPlaceStructure.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesPlaceStructure.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesPlaceStructure.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesPlaceStructure.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesPlaceStructure.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesPlaceStructure.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesPlaceStructure.category = resolver.GetFormatterWithVerify<global::StructureBrain.Categories>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesPlaceStructure.DecoType = resolver.GetFormatterWithVerify<global::Objectives_PlaceStructure.DecorationType>().Deserialize(ref reader, options);
            break;
          case 18:
            objectivesPlaceStructure.Count = reader.ReadInt32();
            break;
          case 19:
            objectivesPlaceStructure.Target = reader.ReadInt32();
            break;
          case 20:
            objectivesPlaceStructure.Term = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 21:
            objectivesPlaceStructure.IncludeAlreadyBuilt = reader.ReadBoolean();
            break;
          case 25:
            objectivesPlaceStructure.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesPlaceStructure.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesPlaceStructure;
    }
  }

  public sealed class Objectives_RecruitCursedFollowerFormatter : 
    IMessagePackFormatter<global::Objectives_RecruitCursedFollower>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_RecruitCursedFollower value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<Thought>().Serialize(ref writer, value.CursedState, options);
        writer.Write(value.Target);
        writer.Write(value.Count);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.FollowerName, options);
        writer.Write(value.FollowerID);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.FollowerSkin, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_RecruitCursedFollower Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_RecruitCursedFollower) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_RecruitCursedFollower recruitCursedFollower = new global::Objectives_RecruitCursedFollower();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            recruitCursedFollower.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            recruitCursedFollower.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            recruitCursedFollower.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            recruitCursedFollower.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            recruitCursedFollower.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            recruitCursedFollower.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            recruitCursedFollower.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            recruitCursedFollower.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            recruitCursedFollower.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            recruitCursedFollower.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            recruitCursedFollower.ID = reader.ReadInt32();
            break;
          case 11:
            recruitCursedFollower.Index = reader.ReadInt32();
            break;
          case 12:
            recruitCursedFollower.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            recruitCursedFollower.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            recruitCursedFollower.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            recruitCursedFollower.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            recruitCursedFollower.CursedState = resolver.GetFormatterWithVerify<Thought>().Deserialize(ref reader, options);
            break;
          case 17:
            recruitCursedFollower.Target = reader.ReadInt32();
            break;
          case 18:
            recruitCursedFollower.Count = reader.ReadInt32();
            break;
          case 19:
            recruitCursedFollower.FollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 20:
            recruitCursedFollower.FollowerID = reader.ReadInt32();
            break;
          case 21:
            recruitCursedFollower.FollowerSkin = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 25:
            recruitCursedFollower.Follower = reader.ReadInt32();
            break;
          case 26:
            recruitCursedFollower.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return recruitCursedFollower;
    }
  }

  public sealed class Objectives_RecruitFollowerFormatter : 
    IMessagePackFormatter<global::Objectives_RecruitFollower>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_RecruitFollower value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.Count);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_RecruitFollower Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_RecruitFollower) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_RecruitFollower objectivesRecruitFollower = new global::Objectives_RecruitFollower();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesRecruitFollower.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesRecruitFollower.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesRecruitFollower.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesRecruitFollower.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesRecruitFollower.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesRecruitFollower.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesRecruitFollower.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesRecruitFollower.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesRecruitFollower.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesRecruitFollower.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesRecruitFollower.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesRecruitFollower.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesRecruitFollower.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesRecruitFollower.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesRecruitFollower.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesRecruitFollower.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesRecruitFollower.Count = reader.ReadInt32();
            break;
          case 25:
            objectivesRecruitFollower.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesRecruitFollower.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesRecruitFollower;
    }
  }

  public sealed class Objectives_RemoveStructureFormatter : 
    IMessagePackFormatter<global::Objectives_RemoveStructure>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_RemoveStructure value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.StructureType, options);
        writer.Write(value.Target);
        writer.Write(value.Count);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_RemoveStructure Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_RemoveStructure) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_RemoveStructure objectivesRemoveStructure = new global::Objectives_RemoveStructure();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesRemoveStructure.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesRemoveStructure.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesRemoveStructure.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesRemoveStructure.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesRemoveStructure.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesRemoveStructure.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesRemoveStructure.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesRemoveStructure.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesRemoveStructure.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesRemoveStructure.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesRemoveStructure.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesRemoveStructure.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesRemoveStructure.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesRemoveStructure.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesRemoveStructure.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesRemoveStructure.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesRemoveStructure.StructureType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesRemoveStructure.Target = reader.ReadInt32();
            break;
          case 18:
            objectivesRemoveStructure.Count = reader.ReadInt32();
            break;
          case 25:
            objectivesRemoveStructure.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesRemoveStructure.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesRemoveStructure;
    }
  }

  public sealed class Objectives_ShootDummyFormatter : 
    IMessagePackFormatter<global::Objectives_ShootDummy>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_ShootDummy value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_ShootDummy Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_ShootDummy) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_ShootDummy objectivesShootDummy = new global::Objectives_ShootDummy();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesShootDummy.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesShootDummy.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesShootDummy.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesShootDummy.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesShootDummy.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesShootDummy.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesShootDummy.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesShootDummy.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesShootDummy.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesShootDummy.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesShootDummy.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesShootDummy.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesShootDummy.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesShootDummy.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesShootDummy.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesShootDummy.IsWinterObjective = reader.ReadBoolean();
            break;
          case 25:
            objectivesShootDummy.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesShootDummy.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesShootDummy;
    }
  }

  public sealed class Objectives_ShowFleeceFormatter : 
    IMessagePackFormatter<global::Objectives_ShowFleece>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_ShowFleece value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::PlayerFleeceManager.FleeceType>().Serialize(ref writer, value.FleeceType, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_ShowFleece Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_ShowFleece) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_ShowFleece objectivesShowFleece = new global::Objectives_ShowFleece();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesShowFleece.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesShowFleece.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesShowFleece.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesShowFleece.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesShowFleece.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesShowFleece.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesShowFleece.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesShowFleece.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesShowFleece.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesShowFleece.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesShowFleece.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesShowFleece.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesShowFleece.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesShowFleece.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesShowFleece.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesShowFleece.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesShowFleece.FleeceType = resolver.GetFormatterWithVerify<global::PlayerFleeceManager.FleeceType>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesShowFleece.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesShowFleece.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesShowFleece;
    }
  }

  public sealed class Objectives_StoryFormatter : 
    IMessagePackFormatter<global::Objectives_Story>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_Story value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<StoryDataItem>().Serialize(ref writer, value.ParentStoryDataItem, options);
        resolver.GetFormatterWithVerify<StoryDataItem>().Serialize(ref writer, value.StoryDataItem, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_Story Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_Story) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_Story objectivesStory = new global::Objectives_Story();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesStory.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesStory.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesStory.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesStory.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesStory.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesStory.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesStory.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesStory.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesStory.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesStory.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesStory.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesStory.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesStory.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesStory.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesStory.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesStory.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesStory.ParentStoryDataItem = resolver.GetFormatterWithVerify<StoryDataItem>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesStory.StoryDataItem = resolver.GetFormatterWithVerify<StoryDataItem>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesStory.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesStory.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesStory;
    }
  }

  public sealed class Objectives_TalkToFollowerFormatter : 
    IMessagePackFormatter<global::Objectives_TalkToFollower>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_TalkToFollower value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.Done);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ResponseTerm, options);
        writer.Write(value.TargetFollower);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_TalkToFollower Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_TalkToFollower) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_TalkToFollower objectivesTalkToFollower = new global::Objectives_TalkToFollower();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesTalkToFollower.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesTalkToFollower.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesTalkToFollower.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesTalkToFollower.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesTalkToFollower.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesTalkToFollower.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesTalkToFollower.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesTalkToFollower.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesTalkToFollower.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesTalkToFollower.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesTalkToFollower.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesTalkToFollower.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesTalkToFollower.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesTalkToFollower.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesTalkToFollower.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesTalkToFollower.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesTalkToFollower.Done = reader.ReadBoolean();
            break;
          case 17:
            objectivesTalkToFollower.ResponseTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 18:
            objectivesTalkToFollower.TargetFollower = reader.ReadInt32();
            break;
          case 25:
            objectivesTalkToFollower.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesTalkToFollower.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesTalkToFollower;
    }
  }

  public sealed class Objectives_UnlockUpgradeFormatter : 
    IMessagePackFormatter<global::Objectives_UnlockUpgrade>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_UnlockUpgrade value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Serialize(ref writer, value.UnlockType, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_UnlockUpgrade Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_UnlockUpgrade) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_UnlockUpgrade objectivesUnlockUpgrade = new global::Objectives_UnlockUpgrade();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesUnlockUpgrade.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesUnlockUpgrade.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesUnlockUpgrade.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesUnlockUpgrade.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesUnlockUpgrade.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesUnlockUpgrade.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesUnlockUpgrade.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesUnlockUpgrade.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesUnlockUpgrade.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesUnlockUpgrade.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesUnlockUpgrade.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesUnlockUpgrade.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesUnlockUpgrade.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesUnlockUpgrade.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesUnlockUpgrade.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesUnlockUpgrade.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesUnlockUpgrade.UnlockType = resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesUnlockUpgrade.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesUnlockUpgrade.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesUnlockUpgrade;
    }
  }

  public sealed class Objectives_UseRelicFormatter : 
    IMessagePackFormatter<global::Objectives_UseRelic>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_UseRelic value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_UseRelic Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_UseRelic) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_UseRelic objectivesUseRelic = new global::Objectives_UseRelic();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesUseRelic.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesUseRelic.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesUseRelic.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesUseRelic.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesUseRelic.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesUseRelic.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesUseRelic.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesUseRelic.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesUseRelic.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesUseRelic.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesUseRelic.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesUseRelic.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesUseRelic.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesUseRelic.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesUseRelic.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesUseRelic.IsWinterObjective = reader.ReadBoolean();
            break;
          case 25:
            objectivesUseRelic.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesUseRelic.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesUseRelic;
    }
  }

  public sealed class Objectives_WalkAnimalFormatter : 
    IMessagePackFormatter<global::Objectives_WalkAnimal>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_WalkAnimal value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        writer.Write(value.TargetAnimal);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_WalkAnimal Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_WalkAnimal) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_WalkAnimal objectivesWalkAnimal = new global::Objectives_WalkAnimal();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesWalkAnimal.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesWalkAnimal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesWalkAnimal.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesWalkAnimal.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesWalkAnimal.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesWalkAnimal.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesWalkAnimal.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesWalkAnimal.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesWalkAnimal.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesWalkAnimal.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesWalkAnimal.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesWalkAnimal.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesWalkAnimal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesWalkAnimal.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesWalkAnimal.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesWalkAnimal.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesWalkAnimal.TargetAnimal = reader.ReadInt32();
            break;
          case 25:
            objectivesWalkAnimal.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesWalkAnimal.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesWalkAnimal;
    }
  }

  public sealed class Objectives_WinFlockadeBetFormatter : 
    IMessagePackFormatter<global::Objectives_WinFlockadeBet>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Objectives_WinFlockadeBet value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(27);
        resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Serialize(ref writer, value.Type, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
        writer.Write(value.IsComplete);
        writer.Write(value.IsFailed);
        writer.Write(value.FailLocked);
        writer.Write(value.AutoRemoveQuestOnceComplete);
        writer.Write(value.TargetFollowerAllowOldAge);
        writer.Write(value.QuestCooldown);
        writer.Write(value.QuestExpireDuration);
        writer.Write(value.ExpireTimestamp);
        writer.Write(value.ID);
        writer.Write(value.Index);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CompleteTerm, options);
        resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.CompleteTermArguments, options);
        writer.Write(value.IsWinterObjective);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.OpponentTermId, options);
        writer.Write(value.TargetWoolAmount);
        writer.Write(value.Count);
        resolver.GetFormatterWithVerify<global::DataManager.Variables>().Serialize(ref writer, value.WoolCountVariable, options);
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.WriteNil();
        writer.Write(value.Follower);
        resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Serialize(ref writer, value.TimerType, options);
      }
    }

    public global::Objectives_WinFlockadeBet Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Objectives_WinFlockadeBet) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Objectives_WinFlockadeBet objectivesWinFlockadeBet = new global::Objectives_WinFlockadeBet();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            objectivesWinFlockadeBet.Type = resolver.GetFormatterWithVerify<global::Objectives.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            objectivesWinFlockadeBet.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            objectivesWinFlockadeBet.IsComplete = reader.ReadBoolean();
            break;
          case 3:
            objectivesWinFlockadeBet.IsFailed = reader.ReadBoolean();
            break;
          case 4:
            objectivesWinFlockadeBet.FailLocked = reader.ReadBoolean();
            break;
          case 5:
            objectivesWinFlockadeBet.AutoRemoveQuestOnceComplete = reader.ReadBoolean();
            break;
          case 6:
            objectivesWinFlockadeBet.TargetFollowerAllowOldAge = reader.ReadBoolean();
            break;
          case 7:
            objectivesWinFlockadeBet.QuestCooldown = reader.ReadSingle();
            break;
          case 8:
            objectivesWinFlockadeBet.QuestExpireDuration = reader.ReadSingle();
            break;
          case 9:
            objectivesWinFlockadeBet.ExpireTimestamp = reader.ReadSingle();
            break;
          case 10:
            objectivesWinFlockadeBet.ID = reader.ReadInt32();
            break;
          case 11:
            objectivesWinFlockadeBet.Index = reader.ReadInt32();
            break;
          case 12:
            objectivesWinFlockadeBet.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 13:
            objectivesWinFlockadeBet.CompleteTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            objectivesWinFlockadeBet.CompleteTermArguments = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
            break;
          case 15:
            objectivesWinFlockadeBet.IsWinterObjective = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            objectivesWinFlockadeBet.OpponentTermId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 17:
            objectivesWinFlockadeBet.TargetWoolAmount = reader.ReadInt32();
            break;
          case 18:
            objectivesWinFlockadeBet.Count = reader.ReadInt32();
            break;
          case 19:
            objectivesWinFlockadeBet.WoolCountVariable = resolver.GetFormatterWithVerify<global::DataManager.Variables>().Deserialize(ref reader, options);
            break;
          case 25:
            objectivesWinFlockadeBet.Follower = reader.ReadInt32();
            break;
          case 26:
            objectivesWinFlockadeBet.TimerType = resolver.GetFormatterWithVerify<global::Objectives.TIMER_TYPE>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return objectivesWinFlockadeBet;
    }
  }

  public sealed class RitualAlertsFormatter : 
    IMessagePackFormatter<RitualAlerts>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      RitualAlerts value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(2);
        resolver.GetFormatterWithVerify<List<global::UpgradeSystem.Type>>().Serialize(ref writer, value._alerts, options);
        resolver.GetFormatterWithVerify<List<global::UpgradeSystem.Type>>().Serialize(ref writer, value._singleAlerts, options);
      }
    }

    public RitualAlerts Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (RitualAlerts) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      RitualAlerts ritualAlerts = new RitualAlerts();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            ritualAlerts._alerts = resolver.GetFormatterWithVerify<List<global::UpgradeSystem.Type>>().Deserialize(ref reader, options);
            break;
          case 1:
            ritualAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::UpgradeSystem.Type>>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return ritualAlerts;
    }
  }

  public sealed class ShopLocationTrackerFormatter : 
    IMessagePackFormatter<ShopLocationTracker>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      ShopLocationTracker value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(4);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.shopKeeperName, options);
        writer.Write(value.lastDayUsed);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.location, options);
        resolver.GetFormatterWithVerify<List<BuyEntry>>().Serialize(ref writer, value.itemsForSale, options);
      }
    }

    public ShopLocationTracker Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (ShopLocationTracker) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      ShopLocationTracker shopLocationTracker = new ShopLocationTracker();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            shopLocationTracker.shopKeeperName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1:
            shopLocationTracker.lastDayUsed = reader.ReadInt32();
            break;
          case 2:
            shopLocationTracker.location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 3:
            shopLocationTracker.itemsForSale = resolver.GetFormatterWithVerify<List<BuyEntry>>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return shopLocationTracker;
    }
  }

  public sealed class ShrineUsageInfoFormatter : 
    IMessagePackFormatter<ShrineUsageInfo>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      ShrineUsageInfo value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(2);
        resolver.GetFormatterWithVerify<global::Shrines.ShrineType>().Serialize(ref writer, value.TypeOfShrine, options);
        writer.Write(value.useTime);
      }
    }

    public ShrineUsageInfo Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (ShrineUsageInfo) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      ShrineUsageInfo shrineUsageInfo = new ShrineUsageInfo();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            shrineUsageInfo.TypeOfShrine = resolver.GetFormatterWithVerify<global::Shrines.ShrineType>().Deserialize(ref reader, options);
            break;
          case 1:
            shrineUsageInfo.useTime = reader.ReadSingle();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return shrineUsageInfo;
    }
  }

  public sealed class StoryDataFormatter : IMessagePackFormatter<StoryData>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      StoryData value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(1);
        resolver.GetFormatterWithVerify<StoryDataItem>().Serialize(ref writer, value.EntryStoryItem, options);
      }
    }

    public StoryData Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (StoryData) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      StoryData storyData = new StoryData();
      for (int index = 0; index < num; ++index)
      {
        if (index == 0)
          storyData.EntryStoryItem = resolver.GetFormatterWithVerify<StoryDataItem>().Deserialize(ref reader, options);
        else
          reader.Skip();
      }
      --reader.Depth;
      return storyData;
    }
  }

  public sealed class StoryDataItemFormatter : 
    IMessagePackFormatter<StoryDataItem>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      StoryDataItem value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(13);
        writer.Write(value.QuestGiverFollowerID);
        writer.Write(value.TargetFollowerID_1);
        writer.Write(value.TargetFollowerID_2);
        writer.Write(value.DeadFollowerID);
        writer.Write(value.FollowerID);
        writer.Write(value.CachedTargetFollowerID_1);
        writer.Write(value.CachedTargetFollowerID_2);
        writer.Write(value.QuestGiven);
        writer.Write(value.QuestLocked);
        writer.Write(value.QuestDeclined);
        resolver.GetFormatterWithVerify<StoryObjectiveData>().Serialize(ref writer, value.StoryObjectiveData, options);
        resolver.GetFormatterWithVerify<List<StoryDataItem>>().Serialize(ref writer, value.ChildStoryDataItems, options);
        resolver.GetFormatterWithVerify<ObjectivesData>().Serialize(ref writer, value.Objective, options);
      }
    }

    public StoryDataItem Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (StoryDataItem) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      StoryDataItem storyDataItem = new StoryDataItem();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            storyDataItem.QuestGiverFollowerID = reader.ReadInt32();
            break;
          case 1:
            storyDataItem.TargetFollowerID_1 = reader.ReadInt32();
            break;
          case 2:
            storyDataItem.TargetFollowerID_2 = reader.ReadInt32();
            break;
          case 3:
            storyDataItem.DeadFollowerID = reader.ReadInt32();
            break;
          case 4:
            storyDataItem.FollowerID = reader.ReadInt32();
            break;
          case 5:
            storyDataItem.CachedTargetFollowerID_1 = reader.ReadInt32();
            break;
          case 6:
            storyDataItem.CachedTargetFollowerID_2 = reader.ReadInt32();
            break;
          case 7:
            storyDataItem.QuestGiven = reader.ReadBoolean();
            break;
          case 8:
            storyDataItem.QuestLocked = reader.ReadBoolean();
            break;
          case 9:
            storyDataItem.QuestDeclined = reader.ReadBoolean();
            break;
          case 10:
            storyDataItem.StoryObjectiveData = resolver.GetFormatterWithVerify<StoryObjectiveData>().Deserialize(ref reader, options);
            break;
          case 11:
            storyDataItem.ChildStoryDataItems = resolver.GetFormatterWithVerify<List<StoryDataItem>>().Deserialize(ref reader, options);
            break;
          case 12:
            storyDataItem.Objective = resolver.GetFormatterWithVerify<ObjectivesData>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return storyDataItem;
    }
  }

  public sealed class StructureAlertsFormatter : 
    IMessagePackFormatter<StructureAlerts>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      StructureAlerts value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(2);
        resolver.GetFormatterWithVerify<List<global::StructureBrain.TYPES>>().Serialize(ref writer, value._alerts, options);
        resolver.GetFormatterWithVerify<List<global::StructureBrain.TYPES>>().Serialize(ref writer, value._singleAlerts, options);
      }
    }

    public StructureAlerts Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (StructureAlerts) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      StructureAlerts structureAlerts = new StructureAlerts();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            structureAlerts._alerts = resolver.GetFormatterWithVerify<List<global::StructureBrain.TYPES>>().Deserialize(ref reader, options);
            break;
          case 1:
            structureAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::StructureBrain.TYPES>>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return structureAlerts;
    }
  }

  public sealed class StructureAndTimeFormatter : 
    IMessagePackFormatter<global::StructureAndTime>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::StructureAndTime value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(3);
        writer.Write(value.StructureID);
        writer.Write(value.TimeReacted);
        resolver.GetFormatterWithVerify<global::StructureAndTime.IDTypes>().Serialize(ref writer, value.IDType, options);
      }
    }

    public global::StructureAndTime Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::StructureAndTime) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::StructureAndTime structureAndTime = new global::StructureAndTime();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            structureAndTime.StructureID = reader.ReadInt32();
            break;
          case 1:
            structureAndTime.TimeReacted = reader.ReadSingle();
            break;
          case 2:
            structureAndTime.IDType = resolver.GetFormatterWithVerify<global::StructureAndTime.IDTypes>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return structureAndTime;
    }
  }

  public sealed class StructureEffectFormatter : 
    IMessagePackFormatter<StructureEffect>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      StructureEffect value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(6);
        writer.Write(value.TimeStarted);
        writer.Write(value.DurationInGameMinutes);
        writer.Write(value.CoolDownInGameMinutes);
        writer.Write(value.StructureID);
        writer.Write(value.CoolingDown);
        resolver.GetFormatterWithVerify<global::StructureEffectManager.EffectType>().Serialize(ref writer, value.Type, options);
      }
    }

    public StructureEffect Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (StructureEffect) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      StructureEffect structureEffect = new StructureEffect();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            structureEffect.TimeStarted = reader.ReadSingle();
            break;
          case 1:
            structureEffect.DurationInGameMinutes = reader.ReadSingle();
            break;
          case 2:
            structureEffect.CoolDownInGameMinutes = reader.ReadSingle();
            break;
          case 3:
            structureEffect.StructureID = reader.ReadInt32();
            break;
          case 4:
            structureEffect.CoolingDown = reader.ReadBoolean();
            break;
          case 5:
            structureEffect.Type = resolver.GetFormatterWithVerify<global::StructureEffectManager.EffectType>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return structureEffect;
    }
  }

  public sealed class StructuresDataFormatter : 
    IMessagePackFormatter<global::StructuresData>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::StructuresData value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(125);
        resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.Type, options);
        writer.Write(value.VariantIndex);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.PrefabPath, options);
        writer.Write(value.RemoveOnDie);
        writer.Write(value.ProgressTarget);
        writer.Write(value.WorkIsRequiredForProgress);
        writer.Write(value.IsUpgrade);
        writer.Write(value.IsUpgradeDestroyPrevious);
        writer.Write(value.IgnoreGrid);
        writer.Write(value.IsBuildingProject);
        writer.Write(value.IsCollapsed);
        writer.Write(value.IsAflame);
        writer.Write(value.IsSnowedUnder);
        writer.Write(value.AflameCollapseTarget);
        resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.UpgradeFromType, options);
        resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.RequiresType, options);
        writer.Write(value.TILE_WIDTH);
        writer.Write(value.TILE_HEIGHT);
        writer.Write(value.CanBeMoved);
        writer.Write(value.CanBeRecycled);
        writer.Write(value.IsObstruction);
        writer.Write(value.DoesNotOccupyGrid);
        writer.Write(value.isDeletable);
        resolver.GetFormatterWithVerify<Vector2Int>().Serialize(ref writer, value.LootCountToDropRange, options);
        resolver.GetFormatterWithVerify<Vector2Int>().Serialize(ref writer, value.CropLootCountToDropRange, options);
        resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.MultipleLootToDrop, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.MultipleLootToDropChance, options);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.LootToDrop, options);
        writer.Write(value.LootCountToDrop);
        writer.Write(value.ID);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.Location, options);
        writer.Write(value.DontLoadMe);
        writer.Write(value.Destroyed);
        writer.Write(value.GridX);
        writer.Write(value.GridY);
        resolver.GetFormatterWithVerify<Vector2Int>().Serialize(ref writer, value.Bounds, options);
        resolver.GetFormatterWithVerify<List<global::InventoryItem>>().Serialize(ref writer, value.Inventory, options);
        writer.Write(value.Progress);
        writer.Write(value.PowerRequirement);
        resolver.GetFormatterWithVerify<Vector3>().Serialize(ref writer, value.Position, options);
        resolver.GetFormatterWithVerify<Vector3>().Serialize(ref writer, value.Offset, options);
        writer.Write(value.OffsetMax);
        writer.Write(value.Repaired);
        resolver.GetFormatterWithVerify<Vector2Int>().Serialize(ref writer, value.GridTilePosition, options);
        resolver.GetFormatterWithVerify<Vector3Int>().Serialize(ref writer, value.PlacementRegionPosition, options);
        writer.Write(value.Age);
        writer.Write(value.Exhausted);
        writer.Write(value.UpgradeLevel);
        writer.Write(value.ClaimedByPlayer);
        writer.Write(value.AvailableSlots);
        resolver.GetFormatterWithVerify<List<global::StructuresData.PathData>>().Serialize(ref writer, value.pathData, options);
        writer.Write(value.Direction);
        writer.Write(value.Rotation);
        resolver.GetFormatterWithVerify<global::Villager_Info>().Serialize(ref writer, value.v_i, options);
        writer.Write(value.SoulCount);
        writer.Write(value.Level);
        resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.ToBuildType, options);
        resolver.GetFormatterWithVerify<global::StructuresData.Phase>().Serialize(ref writer, value.CurrentPhase, options);
        writer.Write(value.Purchased);
        writer.Write(value.FollowerID);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.MultipleFollowerIDs, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.FollowersClaimedSlots, options);
        writer.Write(value.BedpanCount);
        writer.Write(value.HasFood);
        writer.Write(value.FollowerImprisonedTimestamp);
        writer.Write(value.FollowerImprisonedFaith);
        writer.Write(value.GivenGift);
        writer.Write(value.Dir);
        writer.Write(value.BodyWrapped);
        writer.Write(value.BeenInMorgueAlready);
        writer.Write(value.Prioritised);
        writer.Write(value.PrioritisedAsBuildingObstruction);
        writer.Write(value.WeedsAndRubblePlaced);
        resolver.GetFormatterWithVerify<List<global::StructuresData.Ranchable_Animal>>().Serialize(ref writer, value.Animals, options);
        resolver.GetFormatterWithVerify<DayPhase>().Serialize(ref writer, value.TargetPhase, options);
        resolver.GetFormatterWithVerify<GateType>().Serialize(ref writer, value.GateType, options);
        writer.Write(value.CanBecomeRotten);
        writer.Write(value.Rotten);
        writer.Write(value.Burned);
        writer.Write(value.Eaten);
        writer.Write(value.GatheringEndPhase);
        writer.Write(value.IsSapling);
        writer.Write(value.GrowthStage);
        writer.Write(value.CanRegrow);
        writer.Write(value.BenefitedFromFertilizer);
        writer.Write(value.RemainingHarvests);
        writer.Write(value.Withered);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Animation, options);
        writer.Write(value.StartingScale);
        writer.Write(value.Picked);
        writer.Write(value.Watered);
        writer.Write(value.WateredCount);
        writer.Write(value.HasBird);
        writer.Write(value.TotalPoops);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.SignPostItem, options);
        writer.Write(value.GivenHealth);
        resolver.GetFormatterWithVerify<global::StructuresData.EggData>().Serialize(ref writer, value.EggInfo, options);
        writer.Write(value.HasEgg);
        writer.Write(value.EggReady);
        writer.Write(value.MatingFailed);
        writer.Write(value.WeedType);
        writer.Write(value.LastPrayTime);
        writer.Write(value.Fuel);
        writer.Write(value.MaxFuel);
        writer.Write(value.FullyFueled);
        writer.Write(value.FuelDepletionDayTimestamp);
        writer.Write(value.onlyDepleteWhenFullyFueled);
        resolver.GetFormatterWithVerify<DayPhase>().Serialize(ref writer, value.PhaseAddedFuel, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.QueuedRefineryVariants, options);
        resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.QueuedResources, options);
        resolver.GetFormatterWithVerify<List<global::StructuresData.ClothingStruct>>().Serialize(ref writer, value.QueuedClothings, options);
        resolver.GetFormatterWithVerify<List<global::StructuresData.ClothingStruct>>().Serialize(ref writer, value.AllClothing, options);
        resolver.GetFormatterWithVerify<List<global::StructuresData.ClothingStruct>>().Serialize(ref writer, value.ReservedClothing, options);
        resolver.GetFormatterWithVerify<global::StructuresData.ClothingStruct>().Serialize(ref writer, value.CurrentTailoringClothes, options);
        resolver.GetFormatterWithVerify<List<global::Interaction_Kitchen.QueuedMeal>>().Serialize(ref writer, value.QueuedMeals, options);
        resolver.GetFormatterWithVerify<global::Interaction_Kitchen.QueuedMeal>().Serialize(ref writer, value.CurrentCookingMeal, options);
        resolver.GetFormatterWithVerify<Dictionary<int, int>>().Serialize(ref writer, value.ReservedFollowers, options);
        writer.Write(value.WeaponUpgradePointProgress);
        writer.Write(value.WeaponUpgradePointDuration);
        resolver.GetFormatterWithVerify<global::WeaponUpgradeSystem.WeaponType>().Serialize(ref writer, value.CurrentUnlockingWeaponType, options);
        resolver.GetFormatterWithVerify<global::WeaponUpgradeSystem.WeaponUpgradeType>().Serialize(ref writer, value.CurrentUnlockingUpgradeType, options);
        writer.Write(value.DefrostedCrop);
        writer.WriteNil();
        writer.WriteNil();
        resolver.GetFormatterWithVerify<List<global::StructuresData.LogisticsSlot>>().Serialize(ref writer, value.LogisticSlots, options);
      }
    }

    public global::StructuresData Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::StructuresData) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::StructuresData structuresData = new global::StructuresData();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            structuresData.Type = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 1:
            structuresData.VariantIndex = reader.ReadInt32();
            break;
          case 2:
            structuresData.PrefabPath = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 3:
            structuresData.RemoveOnDie = reader.ReadBoolean();
            break;
          case 4:
            structuresData.ProgressTarget = reader.ReadSingle();
            break;
          case 5:
            structuresData.WorkIsRequiredForProgress = reader.ReadBoolean();
            break;
          case 6:
            structuresData.IsUpgrade = reader.ReadBoolean();
            break;
          case 7:
            structuresData.IsUpgradeDestroyPrevious = reader.ReadBoolean();
            break;
          case 8:
            structuresData.IgnoreGrid = reader.ReadBoolean();
            break;
          case 9:
            structuresData.IsBuildingProject = reader.ReadBoolean();
            break;
          case 10:
            structuresData.IsCollapsed = reader.ReadBoolean();
            break;
          case 11:
            structuresData.IsAflame = reader.ReadBoolean();
            break;
          case 12:
            structuresData.IsSnowedUnder = reader.ReadBoolean();
            break;
          case 13:
            structuresData.AflameCollapseTarget = reader.ReadSingle();
            break;
          case 14:
            structuresData.UpgradeFromType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 15:
            structuresData.RequiresType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 16 /*0x10*/:
            structuresData.TILE_WIDTH = reader.ReadInt32();
            break;
          case 17:
            structuresData.TILE_HEIGHT = reader.ReadInt32();
            break;
          case 18:
            structuresData.CanBeMoved = reader.ReadBoolean();
            break;
          case 19:
            structuresData.CanBeRecycled = reader.ReadBoolean();
            break;
          case 20:
            structuresData.IsObstruction = reader.ReadBoolean();
            break;
          case 21:
            structuresData.DoesNotOccupyGrid = reader.ReadBoolean();
            break;
          case 22:
            structuresData.isDeletable = reader.ReadBoolean();
            break;
          case 23:
            structuresData.LootCountToDropRange = resolver.GetFormatterWithVerify<Vector2Int>().Deserialize(ref reader, options);
            break;
          case 24:
            structuresData.CropLootCountToDropRange = resolver.GetFormatterWithVerify<Vector2Int>().Deserialize(ref reader, options);
            break;
          case 25:
            structuresData.MultipleLootToDrop = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          case 26:
            structuresData.MultipleLootToDropChance = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 27:
            structuresData.LootToDrop = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 28:
            structuresData.LootCountToDrop = reader.ReadInt32();
            break;
          case 29:
            structuresData.ID = reader.ReadInt32();
            break;
          case 30:
            structuresData.Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 31 /*0x1F*/:
            structuresData.DontLoadMe = reader.ReadBoolean();
            break;
          case 32 /*0x20*/:
            structuresData.Destroyed = reader.ReadBoolean();
            break;
          case 33:
            structuresData.GridX = reader.ReadInt32();
            break;
          case 34:
            structuresData.GridY = reader.ReadInt32();
            break;
          case 35:
            structuresData.Bounds = resolver.GetFormatterWithVerify<Vector2Int>().Deserialize(ref reader, options);
            break;
          case 36:
            structuresData.Inventory = resolver.GetFormatterWithVerify<List<global::InventoryItem>>().Deserialize(ref reader, options);
            break;
          case 37:
            structuresData.Progress = reader.ReadSingle();
            break;
          case 38:
            structuresData.PowerRequirement = reader.ReadSingle();
            break;
          case 39:
            structuresData.Position = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
            break;
          case 40:
            structuresData.Offset = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
            break;
          case 41:
            structuresData.OffsetMax = reader.ReadSingle();
            break;
          case 42:
            structuresData.Repaired = reader.ReadBoolean();
            break;
          case 43:
            structuresData.GridTilePosition = resolver.GetFormatterWithVerify<Vector2Int>().Deserialize(ref reader, options);
            break;
          case 44:
            structuresData.PlacementRegionPosition = resolver.GetFormatterWithVerify<Vector3Int>().Deserialize(ref reader, options);
            break;
          case 45:
            structuresData.Age = reader.ReadInt32();
            break;
          case 46:
            structuresData.Exhausted = reader.ReadBoolean();
            break;
          case 47:
            structuresData.UpgradeLevel = reader.ReadInt32();
            break;
          case 48 /*0x30*/:
            structuresData.ClaimedByPlayer = reader.ReadBoolean();
            break;
          case 49:
            structuresData.AvailableSlots = reader.ReadInt32();
            break;
          case 50:
            structuresData.pathData = resolver.GetFormatterWithVerify<List<global::StructuresData.PathData>>().Deserialize(ref reader, options);
            break;
          case 51:
            structuresData.Direction = reader.ReadInt32();
            break;
          case 52:
            structuresData.Rotation = reader.ReadInt32();
            break;
          case 53:
            structuresData.v_i = resolver.GetFormatterWithVerify<global::Villager_Info>().Deserialize(ref reader, options);
            break;
          case 54:
            structuresData.SoulCount = reader.ReadInt32();
            break;
          case 55:
            structuresData.Level = reader.ReadInt32();
            break;
          case 56:
            structuresData.ToBuildType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 57:
            structuresData.CurrentPhase = resolver.GetFormatterWithVerify<global::StructuresData.Phase>().Deserialize(ref reader, options);
            break;
          case 58:
            structuresData.Purchased = reader.ReadBoolean();
            break;
          case 59:
            structuresData.FollowerID = reader.ReadInt32();
            break;
          case 60:
            structuresData.MultipleFollowerIDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 61:
            structuresData.FollowersClaimedSlots = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 62:
            structuresData.BedpanCount = reader.ReadInt32();
            break;
          case 63 /*0x3F*/:
            structuresData.HasFood = reader.ReadBoolean();
            break;
          case 64 /*0x40*/:
            structuresData.FollowerImprisonedTimestamp = reader.ReadSingle();
            break;
          case 65:
            structuresData.FollowerImprisonedFaith = reader.ReadSingle();
            break;
          case 66:
            structuresData.GivenGift = reader.ReadBoolean();
            break;
          case 67:
            structuresData.Dir = reader.ReadInt32();
            break;
          case 68:
            structuresData.BodyWrapped = reader.ReadBoolean();
            break;
          case 69:
            structuresData.BeenInMorgueAlready = reader.ReadBoolean();
            break;
          case 70:
            structuresData.Prioritised = reader.ReadBoolean();
            break;
          case 71:
            structuresData.PrioritisedAsBuildingObstruction = reader.ReadBoolean();
            break;
          case 72:
            structuresData.WeedsAndRubblePlaced = reader.ReadBoolean();
            break;
          case 73:
            structuresData.Animals = resolver.GetFormatterWithVerify<List<global::StructuresData.Ranchable_Animal>>().Deserialize(ref reader, options);
            break;
          case 74:
            structuresData.TargetPhase = resolver.GetFormatterWithVerify<DayPhase>().Deserialize(ref reader, options);
            break;
          case 75:
            structuresData.GateType = resolver.GetFormatterWithVerify<GateType>().Deserialize(ref reader, options);
            break;
          case 76:
            structuresData.CanBecomeRotten = reader.ReadBoolean();
            break;
          case 77:
            structuresData.Rotten = reader.ReadBoolean();
            break;
          case 78:
            structuresData.Burned = reader.ReadBoolean();
            break;
          case 79:
            structuresData.Eaten = reader.ReadBoolean();
            break;
          case 80 /*0x50*/:
            structuresData.GatheringEndPhase = reader.ReadInt32();
            break;
          case 81:
            structuresData.IsSapling = reader.ReadBoolean();
            break;
          case 82:
            structuresData.GrowthStage = reader.ReadSingle();
            break;
          case 83:
            structuresData.CanRegrow = reader.ReadBoolean();
            break;
          case 84:
            structuresData.BenefitedFromFertilizer = reader.ReadBoolean();
            break;
          case 85:
            structuresData.RemainingHarvests = reader.ReadInt32();
            break;
          case 86:
            structuresData.Withered = reader.ReadBoolean();
            break;
          case 87:
            structuresData.Animation = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 88:
            structuresData.StartingScale = reader.ReadSingle();
            break;
          case 89:
            structuresData.Picked = reader.ReadBoolean();
            break;
          case 90:
            structuresData.Watered = reader.ReadBoolean();
            break;
          case 91:
            structuresData.WateredCount = reader.ReadInt32();
            break;
          case 92:
            structuresData.HasBird = reader.ReadBoolean();
            break;
          case 93:
            structuresData.TotalPoops = reader.ReadInt32();
            break;
          case 94:
            structuresData.SignPostItem = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 95:
            structuresData.GivenHealth = reader.ReadBoolean();
            break;
          case 96 /*0x60*/:
            structuresData.EggInfo = resolver.GetFormatterWithVerify<global::StructuresData.EggData>().Deserialize(ref reader, options);
            break;
          case 97:
            structuresData.HasEgg = reader.ReadBoolean();
            break;
          case 98:
            structuresData.EggReady = reader.ReadBoolean();
            break;
          case 99:
            structuresData.MatingFailed = reader.ReadBoolean();
            break;
          case 100:
            structuresData.WeedType = reader.ReadInt32();
            break;
          case 101:
            structuresData.LastPrayTime = reader.ReadSingle();
            break;
          case 102:
            structuresData.Fuel = reader.ReadInt32();
            break;
          case 103:
            structuresData.MaxFuel = reader.ReadInt32();
            break;
          case 104:
            structuresData.FullyFueled = reader.ReadBoolean();
            break;
          case 105:
            structuresData.FuelDepletionDayTimestamp = reader.ReadInt32();
            break;
          case 106:
            structuresData.onlyDepleteWhenFullyFueled = reader.ReadBoolean();
            break;
          case 107:
            structuresData.PhaseAddedFuel = resolver.GetFormatterWithVerify<DayPhase>().Deserialize(ref reader, options);
            break;
          case 108:
            structuresData.QueuedRefineryVariants = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 109:
            structuresData.QueuedResources = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          case 110:
            structuresData.QueuedClothings = resolver.GetFormatterWithVerify<List<global::StructuresData.ClothingStruct>>().Deserialize(ref reader, options);
            break;
          case 111:
            structuresData.AllClothing = resolver.GetFormatterWithVerify<List<global::StructuresData.ClothingStruct>>().Deserialize(ref reader, options);
            break;
          case 112 /*0x70*/:
            structuresData.ReservedClothing = resolver.GetFormatterWithVerify<List<global::StructuresData.ClothingStruct>>().Deserialize(ref reader, options);
            break;
          case 113:
            structuresData.CurrentTailoringClothes = resolver.GetFormatterWithVerify<global::StructuresData.ClothingStruct>().Deserialize(ref reader, options);
            break;
          case 114:
            structuresData.QueuedMeals = resolver.GetFormatterWithVerify<List<global::Interaction_Kitchen.QueuedMeal>>().Deserialize(ref reader, options);
            break;
          case 115:
            structuresData.CurrentCookingMeal = resolver.GetFormatterWithVerify<global::Interaction_Kitchen.QueuedMeal>().Deserialize(ref reader, options);
            break;
          case 116:
            structuresData.ReservedFollowers = resolver.GetFormatterWithVerify<Dictionary<int, int>>().Deserialize(ref reader, options);
            break;
          case 117:
            structuresData.WeaponUpgradePointProgress = reader.ReadSingle();
            break;
          case 118:
            structuresData.WeaponUpgradePointDuration = reader.ReadSingle();
            break;
          case 119:
            structuresData.CurrentUnlockingWeaponType = resolver.GetFormatterWithVerify<global::WeaponUpgradeSystem.WeaponType>().Deserialize(ref reader, options);
            break;
          case 120:
            structuresData.CurrentUnlockingUpgradeType = resolver.GetFormatterWithVerify<global::WeaponUpgradeSystem.WeaponUpgradeType>().Deserialize(ref reader, options);
            break;
          case 121:
            structuresData.DefrostedCrop = reader.ReadBoolean();
            break;
          case 124:
            structuresData.LogisticSlots = resolver.GetFormatterWithVerify<List<global::StructuresData.LogisticsSlot>>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return structuresData;
    }
  }

  public sealed class TarotCardsFormatter : IMessagePackFormatter<global::TarotCards>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::TarotCards value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(5);
        resolver.GetFormatterWithVerify<global::TarotCards.Card>().Serialize(ref writer, value.Type, options);
        writer.Write(value.Unlocked);
        writer.Write(value.UnlockProgress);
        writer.Write(value.Used);
        resolver.GetFormatterWithVerify<global::TarotCards.CardCategory>().Serialize(ref writer, value.cardCategory, options);
      }
    }

    public global::TarotCards Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::TarotCards) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::TarotCards tarotCards = new global::TarotCards();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            tarotCards.Type = resolver.GetFormatterWithVerify<global::TarotCards.Card>().Deserialize(ref reader, options);
            break;
          case 1:
            tarotCards.Unlocked = reader.ReadBoolean();
            break;
          case 2:
            tarotCards.UnlockProgress = reader.ReadSingle();
            break;
          case 3:
            tarotCards.Used = reader.ReadBoolean();
            break;
          case 4:
            tarotCards.cardCategory = resolver.GetFormatterWithVerify<global::TarotCards.CardCategory>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return tarotCards;
    }
  }

  public sealed class TaskAndTimeFormatter : 
    IMessagePackFormatter<TaskAndTime>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      TaskAndTime value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(2);
        resolver.GetFormatterWithVerify<FollowerTaskType>().Serialize(ref writer, value.Task, options);
        writer.Write(value.Time);
      }
    }

    public TaskAndTime Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (TaskAndTime) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      TaskAndTime taskAndTime = new TaskAndTime();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            taskAndTime.Task = resolver.GetFormatterWithVerify<FollowerTaskType>().Deserialize(ref reader, options);
            break;
          case 1:
            taskAndTime.Time = reader.ReadSingle();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return taskAndTime;
    }
  }

  public sealed class ThoughtDataFormatter : 
    IMessagePackFormatter<ThoughtData>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      ThoughtData value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(15);
        resolver.GetFormatterWithVerify<Thought>().Serialize(ref writer, value.ThoughtType, options);
        resolver.GetFormatterWithVerify<Thought>().Serialize(ref writer, value.ThoughtGroup, options);
        writer.Write(value.Modifier);
        writer.Write(value.StartingModifier);
        writer.Write(value.Duration);
        writer.Write(value.Quantity);
        writer.Write(value.Stacking);
        writer.Write(value.StackModifier);
        writer.Write(value.TotalCountDisplay);
        writer.Write(value.ReduceOverTime);
        resolver.GetFormatterWithVerify<List<float>>().Serialize(ref writer, value.CoolDowns, options);
        resolver.GetFormatterWithVerify<List<float>>().Serialize(ref writer, value.TimeStarted, options);
        writer.Write(value.FollowerID);
        writer.Write(value.Warmth);
        writer.Write(value.TrackThought);
      }
    }

    public ThoughtData Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (ThoughtData) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num1 = reader.ReadArrayHeader();
      Thought thought = Thought.None;
      Thought thoughtGroup = Thought.None;
      float num2 = 0.0f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      int num5 = 0;
      int num6 = 0;
      int num7 = 0;
      int num8 = 0;
      bool flag1 = false;
      List<float> floatList1 = (List<float>) null;
      List<float> floatList2 = (List<float>) null;
      int num9 = 0;
      float num10 = 0.0f;
      bool flag2 = false;
      for (int index = 0; index < num1; ++index)
      {
        switch (index)
        {
          case 0:
            thought = resolver.GetFormatterWithVerify<Thought>().Deserialize(ref reader, options);
            break;
          case 1:
            thoughtGroup = resolver.GetFormatterWithVerify<Thought>().Deserialize(ref reader, options);
            break;
          case 2:
            num2 = reader.ReadSingle();
            break;
          case 3:
            num3 = reader.ReadSingle();
            break;
          case 4:
            num4 = reader.ReadSingle();
            break;
          case 5:
            num5 = reader.ReadInt32();
            break;
          case 6:
            num6 = reader.ReadInt32();
            break;
          case 7:
            num7 = reader.ReadInt32();
            break;
          case 8:
            num8 = reader.ReadInt32();
            break;
          case 9:
            flag1 = reader.ReadBoolean();
            break;
          case 10:
            floatList1 = resolver.GetFormatterWithVerify<List<float>>().Deserialize(ref reader, options);
            break;
          case 11:
            floatList2 = resolver.GetFormatterWithVerify<List<float>>().Deserialize(ref reader, options);
            break;
          case 12:
            num9 = reader.ReadInt32();
            break;
          case 13:
            num10 = reader.ReadSingle();
            break;
          case 14:
            flag2 = reader.ReadBoolean();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      ThoughtData thoughtData = new ThoughtData(thought, thoughtGroup);
      if (num1 > 2)
      {
        thoughtData.Modifier = num2;
        if (num1 > 3)
        {
          thoughtData.StartingModifier = num3;
          if (num1 > 4)
          {
            thoughtData.Duration = num4;
            if (num1 > 5)
            {
              thoughtData.Quantity = num5;
              if (num1 > 6)
              {
                thoughtData.Stacking = num6;
                if (num1 > 7)
                {
                  thoughtData.StackModifier = num7;
                  if (num1 > 8)
                  {
                    thoughtData.TotalCountDisplay = num8;
                    if (num1 > 9)
                    {
                      thoughtData.ReduceOverTime = flag1;
                      if (num1 > 10)
                      {
                        thoughtData.CoolDowns = floatList1;
                        if (num1 > 11)
                        {
                          thoughtData.TimeStarted = floatList2;
                          if (num1 > 12)
                          {
                            thoughtData.FollowerID = num9;
                            if (num1 > 13)
                            {
                              thoughtData.Warmth = num10;
                              if (num1 > 14)
                                thoughtData.TrackThought = flag2;
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      --reader.Depth;
      return thoughtData;
    }
  }

  public sealed class TraderTrackerFormatter : 
    IMessagePackFormatter<TraderTracker>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      TraderTracker value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(5);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.location, options);
        resolver.GetFormatterWithVerify<List<TraderTrackerItems>>().Serialize(ref writer, value.itemsToTrade, options);
        resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.itemsForSale, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.traderName, options);
        resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.blackList, options);
      }
    }

    public TraderTracker Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (TraderTracker) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      TraderTracker traderTracker = new TraderTracker();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            traderTracker.location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 1:
            traderTracker.itemsToTrade = resolver.GetFormatterWithVerify<List<TraderTrackerItems>>().Deserialize(ref reader, options);
            break;
          case 2:
            traderTracker.itemsForSale = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          case 3:
            traderTracker.traderName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 4:
            traderTracker.blackList = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return traderTracker;
    }
  }

  public sealed class TraderTrackerItemsFormatter : 
    IMessagePackFormatter<TraderTrackerItems>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      TraderTrackerItems value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(7);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.itemForTrade, options);
        writer.Write(value.SellPrice);
        writer.Write(value.BuyPrice);
        writer.Write(value.BuyOffsetPercent);
        writer.Write(value.BuyOffset);
        writer.Write(value.SellOffset);
        writer.Write(value.LastDayChecked);
      }
    }

    public TraderTrackerItems Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (TraderTrackerItems) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      TraderTrackerItems traderTrackerItems = new TraderTrackerItems();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            traderTrackerItems.itemForTrade = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 1:
            traderTrackerItems.SellPrice = reader.ReadInt32();
            break;
          case 2:
            traderTrackerItems.BuyPrice = reader.ReadInt32();
            break;
          case 3:
            traderTrackerItems.BuyOffsetPercent = reader.ReadInt32();
            break;
          case 4:
            traderTrackerItems.BuyOffset = reader.ReadInt32();
            break;
          case 5:
            traderTrackerItems.SellOffset = reader.ReadInt32();
            break;
          case 6:
            traderTrackerItems.LastDayChecked = reader.ReadInt32();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return traderTrackerItems;
    }
  }

  public sealed class Villager_InfoFormatter : 
    IMessagePackFormatter<global::Villager_Info>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      global::Villager_Info value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(36);
        writer.Write(value.ID);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Name, options);
        writer.Write(value.SkinVariation);
        writer.Write(value.SkinColour);
        writer.Write(value.Age);
        writer.Write(value.color_r);
        writer.Write(value.color_g);
        writer.Write(value.color_b);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.SkinName, options);
        resolver.GetFormatterWithVerify<global::Villager_Info.Faction>().Serialize(ref writer, value.MyFaction, options);
        resolver.GetFormatterWithVerify<global::WorshipperInfoManager.Outfit>().Serialize(ref writer, value.Outfit, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.WorkPlace, options);
        writer.Write(value.WorkPlaceSlot);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Dwelling, options);
        writer.Write(value.DwellingSlot);
        writer.Write(value.DwellingClaimed);
        writer.Write(value.HP);
        writer.Write(value.TotalHP);
        writer.Write(value.SleptOutside);
        resolver.GetFormatterWithVerify<Vector3>().Serialize(ref writer, value.SleptOutsidePosition, options);
        writer.Write(value.FearLove);
        writer.Write(value.Hunger);
        writer.Write(value._Starve);
        writer.Write(value.Sleep);
        writer.Write(value.DevotionGiven);
        writer.Write(value.Devotion);
        writer.Write(value.Level);
        writer.Write(value.isDissenter);
        writer.Write(value.Complaint_House);
        writer.Write(value.Complaint_Food);
        writer.Write(value.GuaranteedGoodInteractionsUntil);
        writer.Write(value.FastingUntil);
        writer.Write(value.IncreasedDevotionOutputUntil);
        writer.Write(value.BrainwashedUntil);
        writer.Write(value.MotivatedUntil);
        resolver.GetFormatterWithVerify<List<global::IDAndRelationship>>().Serialize(ref writer, value.Relationships, options);
      }
    }

    public global::Villager_Info Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (global::Villager_Info) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      global::Villager_Info villagerInfo = new global::Villager_Info();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            villagerInfo.ID = reader.ReadInt32();
            break;
          case 1:
            villagerInfo.Name = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            villagerInfo.SkinVariation = reader.ReadInt32();
            break;
          case 3:
            villagerInfo.SkinColour = reader.ReadInt32();
            break;
          case 4:
            villagerInfo.Age = reader.ReadInt32();
            break;
          case 5:
            villagerInfo.color_r = reader.ReadSingle();
            break;
          case 6:
            villagerInfo.color_g = reader.ReadSingle();
            break;
          case 7:
            villagerInfo.color_b = reader.ReadSingle();
            break;
          case 8:
            villagerInfo.SkinName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 9:
            villagerInfo.MyFaction = resolver.GetFormatterWithVerify<global::Villager_Info.Faction>().Deserialize(ref reader, options);
            break;
          case 10:
            villagerInfo.Outfit = resolver.GetFormatterWithVerify<global::WorshipperInfoManager.Outfit>().Deserialize(ref reader, options);
            break;
          case 11:
            villagerInfo.WorkPlace = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 12:
            villagerInfo.WorkPlaceSlot = reader.ReadInt32();
            break;
          case 13:
            villagerInfo.Dwelling = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 14:
            villagerInfo.DwellingSlot = reader.ReadInt32();
            break;
          case 15:
            villagerInfo.DwellingClaimed = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            villagerInfo.HP = reader.ReadSingle();
            break;
          case 17:
            villagerInfo.TotalHP = reader.ReadSingle();
            break;
          case 18:
            villagerInfo.SleptOutside = reader.ReadBoolean();
            break;
          case 19:
            villagerInfo.SleptOutsidePosition = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
            break;
          case 20:
            villagerInfo.FearLove = reader.ReadSingle();
            break;
          case 21:
            villagerInfo.Hunger = reader.ReadSingle();
            break;
          case 22:
            villagerInfo._Starve = reader.ReadSingle();
            break;
          case 23:
            villagerInfo.Sleep = reader.ReadSingle();
            break;
          case 24:
            villagerInfo.DevotionGiven = reader.ReadInt32();
            break;
          case 25:
            villagerInfo.Devotion = reader.ReadSingle();
            break;
          case 26:
            villagerInfo.Level = reader.ReadInt32();
            break;
          case 27:
            villagerInfo.isDissenter = reader.ReadBoolean();
            break;
          case 28:
            villagerInfo.Complaint_House = reader.ReadBoolean();
            break;
          case 29:
            villagerInfo.Complaint_Food = reader.ReadBoolean();
            break;
          case 30:
            villagerInfo.GuaranteedGoodInteractionsUntil = reader.ReadInt32();
            break;
          case 31 /*0x1F*/:
            villagerInfo.FastingUntil = reader.ReadInt32();
            break;
          case 32 /*0x20*/:
            villagerInfo.IncreasedDevotionOutputUntil = reader.ReadInt32();
            break;
          case 33:
            villagerInfo.BrainwashedUntil = reader.ReadInt32();
            break;
          case 34:
            villagerInfo.MotivatedUntil = reader.ReadInt32();
            break;
          case 35:
            villagerInfo.Relationships = resolver.GetFormatterWithVerify<List<global::IDAndRelationship>>().Deserialize(ref reader, options);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return villagerInfo;
    }
  }

  public sealed class MetaDataFormatter : IMessagePackFormatter<MetaData>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      MetaData value,
      MessagePackSerializerOptions options)
    {
      IFormatterResolver resolver = options.Resolver;
      writer.WriteArrayHeader(31 /*0x1F*/);
      resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CultName, options);
      writer.Write(value.FollowerCount);
      writer.Write(value.StructureCount);
      writer.Write(value.DeathCount);
      writer.Write(value.Day);
      writer.Write(value.Difficulty);
      writer.Write(value.PlayTime);
      writer.Write(value.Dungeon1Completed);
      writer.Write(value.Dungeon1NGPCompleted);
      writer.Write(value.Dungeon2Completed);
      writer.Write(value.Dungeon2NGPCompleted);
      writer.Write(value.Dungeon3Completed);
      writer.Write(value.Dungeon3NGPCompleted);
      writer.Write(value.Dungeon4Completed);
      writer.Write(value.Dungeon4NGPCompleted);
      writer.Write(value.GameBeaten);
      writer.Write(value.SandboxBeaten);
      writer.Write(value.DeathCatRecruited);
      writer.Write(value.WolfBeaten);
      writer.Write(value.YngyaBeaten);
      writer.Write(value.ExecutionerBeaten);
      writer.Write(value.RottingFollowerCount);
      writer.Write(value.LambGhostsCount);
      writer.Write(value.WinterCount);
      writer.Write(value.PercentageCompleted);
      writer.Write(value.DLCPercentageCompleted);
      writer.Write(value.Permadeath);
      writer.Write(value.QuickStart);
      writer.Write(value.Penitence);
      resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Version, options);
      writer.Write(value.ActivatedMajorDLC);
    }

    public MetaData Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        throw new InvalidOperationException("typecode is null, struct not supported");
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      MetaData metaData = new MetaData();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            metaData.CultName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1:
            metaData.FollowerCount = reader.ReadInt32();
            break;
          case 2:
            metaData.StructureCount = reader.ReadInt32();
            break;
          case 3:
            metaData.DeathCount = reader.ReadInt32();
            break;
          case 4:
            metaData.Day = reader.ReadInt32();
            break;
          case 5:
            metaData.Difficulty = reader.ReadInt32();
            break;
          case 6:
            metaData.PlayTime = reader.ReadSingle();
            break;
          case 7:
            metaData.Dungeon1Completed = reader.ReadBoolean();
            break;
          case 8:
            metaData.Dungeon1NGPCompleted = reader.ReadBoolean();
            break;
          case 9:
            metaData.Dungeon2Completed = reader.ReadBoolean();
            break;
          case 10:
            metaData.Dungeon2NGPCompleted = reader.ReadBoolean();
            break;
          case 11:
            metaData.Dungeon3Completed = reader.ReadBoolean();
            break;
          case 12:
            metaData.Dungeon3NGPCompleted = reader.ReadBoolean();
            break;
          case 13:
            metaData.Dungeon4Completed = reader.ReadBoolean();
            break;
          case 14:
            metaData.Dungeon4NGPCompleted = reader.ReadBoolean();
            break;
          case 15:
            metaData.GameBeaten = reader.ReadBoolean();
            break;
          case 16 /*0x10*/:
            metaData.SandboxBeaten = reader.ReadBoolean();
            break;
          case 17:
            metaData.DeathCatRecruited = reader.ReadBoolean();
            break;
          case 18:
            metaData.WolfBeaten = reader.ReadBoolean();
            break;
          case 19:
            metaData.YngyaBeaten = reader.ReadBoolean();
            break;
          case 20:
            metaData.ExecutionerBeaten = reader.ReadBoolean();
            break;
          case 21:
            metaData.RottingFollowerCount = reader.ReadInt32();
            break;
          case 22:
            metaData.LambGhostsCount = reader.ReadInt32();
            break;
          case 23:
            metaData.WinterCount = reader.ReadInt32();
            break;
          case 24:
            metaData.PercentageCompleted = reader.ReadInt32();
            break;
          case 25:
            metaData.DLCPercentageCompleted = reader.ReadInt32();
            break;
          case 26:
            metaData.Permadeath = reader.ReadBoolean();
            break;
          case 27:
            metaData.QuickStart = reader.ReadBoolean();
            break;
          case 28:
            metaData.Penitence = reader.ReadBoolean();
            break;
          case 29:
            metaData.Version = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 30:
            metaData.ActivatedMajorDLC = reader.ReadBoolean();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return metaData;
    }
  }

  public class DataManager
  {
    public sealed class ClothingVariantFormatter : 
      IMessagePackFormatter<global::DataManager.ClothingVariant>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.ClothingVariant value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(3);
          resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.ClothingType, options);
          writer.Write(value.Colour);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Variant, options);
        }
      }

      public global::DataManager.ClothingVariant Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::DataManager.ClothingVariant) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::DataManager.ClothingVariant clothingVariant = new global::DataManager.ClothingVariant();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              clothingVariant.ClothingType = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
              break;
            case 1:
              clothingVariant.Colour = reader.ReadInt32();
              break;
            case 2:
              clothingVariant.Variant = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return clothingVariant;
      }
    }

    public sealed class DepositedFollowerFormatter : 
      IMessagePackFormatter<global::DataManager.DepositedFollower>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.DepositedFollower value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(3);
          resolver.GetFormatterWithVerify<FollowerInfo>().Serialize(ref writer, value.FollowerInfo, options);
          writer.Write(value.DepositedDay);
          writer.Write(value.Hatched);
        }
      }

      public global::DataManager.DepositedFollower Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::DataManager.DepositedFollower) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::DataManager.DepositedFollower depositedFollower = new global::DataManager.DepositedFollower();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              depositedFollower.FollowerInfo = resolver.GetFormatterWithVerify<FollowerInfo>().Deserialize(ref reader, options);
              break;
            case 1:
              depositedFollower.DepositedDay = reader.ReadInt32();
              break;
            case 2:
              depositedFollower.Hatched = reader.ReadBoolean();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return depositedFollower;
      }
    }

    public sealed class LocationAndLayerFormatter : 
      IMessagePackFormatter<global::DataManager.LocationAndLayer>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.LocationAndLayer value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(2);
          resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.Location, options);
          writer.Write(value.Layer);
        }
      }

      public global::DataManager.LocationAndLayer Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::DataManager.LocationAndLayer) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        FollowerLocation Location = FollowerLocation.Church;
        int Layer = 0;
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
              break;
            case 1:
              Layer = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        global::DataManager.LocationAndLayer locationAndLayer = new global::DataManager.LocationAndLayer(Location, Layer);
        --reader.Depth;
        return locationAndLayer;
      }
    }

    public sealed class LocationSeedsDataFormatter : 
      IMessagePackFormatter<global::DataManager.LocationSeedsData>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.LocationSeedsData value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(2);
          resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.Location, options);
          writer.Write(value.Seed);
        }
      }

      public global::DataManager.LocationSeedsData Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::DataManager.LocationSeedsData) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::DataManager.LocationSeedsData locationSeedsData = new global::DataManager.LocationSeedsData();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              locationSeedsData.Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
              break;
            case 1:
              locationSeedsData.Seed = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return locationSeedsData;
      }
    }

    public sealed class OfferingFormatter : 
      IMessagePackFormatter<global::DataManager.Offering>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.Offering value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(2);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.Type, options);
          writer.Write(value.Quantity);
        }
      }

      public global::DataManager.Offering Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::DataManager.Offering) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::DataManager.Offering offering = new global::DataManager.Offering();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              offering.Type = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 1:
              offering.Quantity = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return offering;
      }
    }

    public sealed class QuestHistoryDataFormatter : 
      IMessagePackFormatter<global::DataManager.QuestHistoryData>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.QuestHistoryData value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          writer.WriteArrayHeader(4);
          writer.Write(value.QuestIndex);
          writer.Write(value.QuestTimestamp);
          writer.Write(value.QuestCooldownDuration);
          writer.Write(value.IsStory);
        }
      }

      public global::DataManager.QuestHistoryData Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::DataManager.QuestHistoryData) null;
        options.Security.DepthStep(ref reader);
        int num = reader.ReadArrayHeader();
        global::DataManager.QuestHistoryData questHistoryData = new global::DataManager.QuestHistoryData();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              questHistoryData.QuestIndex = reader.ReadInt32();
              break;
            case 1:
              questHistoryData.QuestTimestamp = reader.ReadSingle();
              break;
            case 2:
              questHistoryData.QuestCooldownDuration = reader.ReadSingle();
              break;
            case 3:
              questHistoryData.IsStory = reader.ReadBoolean();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return questHistoryData;
      }
    }

    public sealed class WoolhavenFlowerPotFormatter : 
      IMessagePackFormatter<global::DataManager.WoolhavenFlowerPot>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.WoolhavenFlowerPot value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          writer.WriteArrayHeader(2);
          writer.Write(value.ID);
          writer.Write(value.FlowersAdded);
        }
      }

      public global::DataManager.WoolhavenFlowerPot Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::DataManager.WoolhavenFlowerPot) null;
        options.Security.DepthStep(ref reader);
        int num = reader.ReadArrayHeader();
        global::DataManager.WoolhavenFlowerPot woolhavenFlowerPot = new global::DataManager.WoolhavenFlowerPot();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              woolhavenFlowerPot.ID = reader.ReadInt32();
              break;
            case 1:
              woolhavenFlowerPot.FlowersAdded = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return woolhavenFlowerPot;
      }
    }

    public sealed class CultLevelFormatter : 
      IMessagePackFormatter<global::DataManager.CultLevel>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.CultLevel value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::DataManager.CultLevel Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::DataManager.CultLevel) reader.ReadInt32();
      }
    }

    public sealed class OnboardingPhaseFormatter : 
      IMessagePackFormatter<global::DataManager.OnboardingPhase>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.OnboardingPhase value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::DataManager.OnboardingPhase Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::DataManager.OnboardingPhase) reader.ReadInt32();
      }
    }

    public sealed class VariablesFormatter : 
      IMessagePackFormatter<global::DataManager.Variables>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DataManager.Variables value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::DataManager.Variables Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::DataManager.Variables) reader.ReadInt32();
      }
    }
  }

  public class DungeonSandboxManager
  {
    public sealed class ProgressionSnapshotFormatter : 
      IMessagePackFormatter<global::DungeonSandboxManager.ProgressionSnapshot>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DungeonSandboxManager.ProgressionSnapshot value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<global::DungeonSandboxManager.ScenarioType>().Serialize(ref writer, value.ScenarioType, options);
          resolver.GetFormatterWithVerify<global::PlayerFleeceManager.FleeceType>().Serialize(ref writer, value.FleeceType, options);
          writer.Write(value.CompletedRuns);
          writer.Write(value.CompletionSeen);
        }
      }

      public global::DungeonSandboxManager.ProgressionSnapshot Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::DungeonSandboxManager.ProgressionSnapshot) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::DungeonSandboxManager.ProgressionSnapshot progressionSnapshot = new global::DungeonSandboxManager.ProgressionSnapshot();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              progressionSnapshot.ScenarioType = resolver.GetFormatterWithVerify<global::DungeonSandboxManager.ScenarioType>().Deserialize(ref reader, options);
              break;
            case 1:
              progressionSnapshot.FleeceType = resolver.GetFormatterWithVerify<global::PlayerFleeceManager.FleeceType>().Deserialize(ref reader, options);
              break;
            case 2:
              progressionSnapshot.CompletedRuns = reader.ReadInt32();
              break;
            case 3:
              progressionSnapshot.CompletionSeen = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return progressionSnapshot;
      }
    }

    public sealed class ScenarioTypeFormatter : 
      IMessagePackFormatter<global::DungeonSandboxManager.ScenarioType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DungeonSandboxManager.ScenarioType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::DungeonSandboxManager.ScenarioType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::DungeonSandboxManager.ScenarioType) reader.ReadInt32();
      }
    }
  }

  public class DungeonWorldMapIcon
  {
    public sealed class DLCTemporaryMapNodeFormatter : 
      IMessagePackFormatter<global::DungeonWorldMapIcon.DLCTemporaryMapNode>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DungeonWorldMapIcon.DLCTemporaryMapNode value,
        MessagePackSerializerOptions options)
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(4);
        writer.Write(value.NodeID);
        writer.Write(value.ParentNodeID);
        writer.Write(value.PrefabIndex);
        resolver.GetFormatterWithVerify<Vector3>().Serialize(ref writer, value.Position, options);
      }

      public global::DungeonWorldMapIcon.DLCTemporaryMapNode Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          throw new InvalidOperationException("typecode is null, struct not supported");
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::DungeonWorldMapIcon.DLCTemporaryMapNode temporaryMapNode = new global::DungeonWorldMapIcon.DLCTemporaryMapNode();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              temporaryMapNode.NodeID = reader.ReadInt32();
              break;
            case 1:
              temporaryMapNode.ParentNodeID = reader.ReadInt32();
              break;
            case 2:
              temporaryMapNode.PrefabIndex = reader.ReadInt32();
              break;
            case 3:
              temporaryMapNode.Position = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return temporaryMapNode;
      }
    }

    public sealed class NodeTypeFormatter : 
      IMessagePackFormatter<global::DungeonWorldMapIcon.NodeType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DungeonWorldMapIcon.NodeType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::DungeonWorldMapIcon.NodeType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::DungeonWorldMapIcon.NodeType) reader.ReadInt32();
      }
    }
  }

  public class FollowerPet
  {
    public sealed class DLCPetFormatter : 
      IMessagePackFormatter<global::FollowerPet.DLCPet>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::FollowerPet.DLCPet value,
        MessagePackSerializerOptions options)
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(6);
        resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.ItemType, options);
        resolver.GetFormatterWithVerify<global::FollowerPet.FollowerPetType>().Serialize(ref writer, value.PetType, options);
        writer.Write(value.Horns);
        writer.Write(value.Ears);
        writer.Write(value.Head);
        writer.Write(value.Colour);
      }

      public global::FollowerPet.DLCPet Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          throw new InvalidOperationException("typecode is null, struct not supported");
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::FollowerPet.DLCPet dlcPet = new global::FollowerPet.DLCPet();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dlcPet.ItemType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 1:
              dlcPet.PetType = resolver.GetFormatterWithVerify<global::FollowerPet.FollowerPetType>().Deserialize(ref reader, options);
              break;
            case 2:
              dlcPet.Horns = reader.ReadInt32();
              break;
            case 3:
              dlcPet.Ears = reader.ReadInt32();
              break;
            case 4:
              dlcPet.Head = reader.ReadInt32();
              break;
            case 5:
              dlcPet.Colour = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dlcPet;
      }
    }

    public sealed class FollowerPetTypeFormatter : 
      IMessagePackFormatter<global::FollowerPet.FollowerPetType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::FollowerPet.FollowerPetType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::FollowerPet.FollowerPetType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::FollowerPet.FollowerPetType) reader.ReadInt32();
      }
    }
  }

  public class Interaction_Kitchen
  {
    public sealed class QueuedMealFormatter : 
      IMessagePackFormatter<global::Interaction_Kitchen.QueuedMeal>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Interaction_Kitchen.QueuedMeal value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.MealType, options);
          resolver.GetFormatterWithVerify<List<global::InventoryItem>>().Serialize(ref writer, value.Ingredients, options);
          writer.Write(value.CookingDuration);
          writer.Write(value.CookedTime);
        }
      }

      public global::Interaction_Kitchen.QueuedMeal Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Interaction_Kitchen.QueuedMeal) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Interaction_Kitchen.QueuedMeal queuedMeal = new global::Interaction_Kitchen.QueuedMeal();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              queuedMeal.MealType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 1:
              queuedMeal.Ingredients = resolver.GetFormatterWithVerify<List<global::InventoryItem>>().Deserialize(ref reader, options);
              break;
            case 2:
              queuedMeal.CookingDuration = reader.ReadSingle();
              break;
            case 3:
              queuedMeal.CookedTime = reader.ReadSingle();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return queuedMeal;
      }
    }
  }

  public class MiniBossController
  {
    public sealed class MiniBossDataFormatter : 
      IMessagePackFormatter<global::MiniBossController.MiniBossData>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::MiniBossController.MiniBossData value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(2);
          resolver.GetFormatterWithVerify<Enemy>().Serialize(ref writer, value.EnemyType, options);
          resolver.GetFormatterWithVerify<List<global::EnemyModifier.ModifierType>>().Serialize(ref writer, value.EncounteredModifiers, options);
        }
      }

      public global::MiniBossController.MiniBossData Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::MiniBossController.MiniBossData) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::MiniBossController.MiniBossData miniBossData = new global::MiniBossController.MiniBossData();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              miniBossData.EnemyType = resolver.GetFormatterWithVerify<Enemy>().Deserialize(ref reader, options);
              break;
            case 1:
              miniBossData.EncounteredModifiers = resolver.GetFormatterWithVerify<List<global::EnemyModifier.ModifierType>>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return miniBossData;
      }
    }
  }

  public class MissionManager
  {
    public sealed class MissionFormatter : 
      IMessagePackFormatter<global::MissionManager.Mission>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::MissionManager.Mission value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(8);
          resolver.GetFormatterWithVerify<global::MissionManager.MissionType>().Serialize(ref writer, value.MissionType, options);
          writer.Write(value.Difficulty);
          writer.Write(value.ID);
          writer.Write(value.ExpiryTimestamp);
          resolver.GetFormatterWithVerify<BuyEntry[]>().Serialize(ref writer, value.Rewards, options);
          writer.Write(value.GoldenMission);
          resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.Decoration, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.FollowerSkin, options);
        }
      }

      public global::MissionManager.Mission Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::MissionManager.Mission) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::MissionManager.Mission mission = new global::MissionManager.Mission();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              mission.MissionType = resolver.GetFormatterWithVerify<global::MissionManager.MissionType>().Deserialize(ref reader, options);
              break;
            case 1:
              mission.Difficulty = reader.ReadInt32();
              break;
            case 2:
              mission.ID = reader.ReadInt32();
              break;
            case 3:
              mission.ExpiryTimestamp = reader.ReadSingle();
              break;
            case 4:
              mission.Rewards = resolver.GetFormatterWithVerify<BuyEntry[]>().Deserialize(ref reader, options);
              break;
            case 5:
              mission.GoldenMission = reader.ReadBoolean();
              break;
            case 6:
              mission.Decoration = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
              break;
            case 7:
              mission.FollowerSkin = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return mission;
      }
    }

    public sealed class MissionTypeFormatter : 
      IMessagePackFormatter<global::MissionManager.MissionType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::MissionManager.MissionType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::MissionManager.MissionType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::MissionManager.MissionType) reader.ReadInt32();
      }
    }
  }

  public class Objective_FindRelic
  {
    public sealed class FinalizedData_FindRelicFormatter : 
      IMessagePackFormatter<global::Objective_FindRelic.FinalizedData_FindRelic>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objective_FindRelic.FinalizedData_FindRelic value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.TargetLocation, options);
          resolver.GetFormatterWithVerify<RelicType>().Serialize(ref writer, value.RelicType, options);
        }
      }

      public global::Objective_FindRelic.FinalizedData_FindRelic Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objective_FindRelic.FinalizedData_FindRelic) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objective_FindRelic.FinalizedData_FindRelic finalizedDataFindRelic = new global::Objective_FindRelic.FinalizedData_FindRelic();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataFindRelic.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataFindRelic.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataFindRelic.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataFindRelic.TargetLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataFindRelic.RelicType = resolver.GetFormatterWithVerify<RelicType>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataFindRelic;
      }
    }
  }

  public class Objectives_AssignClothing
  {
    public sealed class FinalizedData_AssignClothingFormatter : 
      IMessagePackFormatter<global::Objectives_AssignClothing.FinalizedData_AssignClothing>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_AssignClothing.FinalizedData_AssignClothing value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.ClothingType, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
        }
      }

      public global::Objectives_AssignClothing.FinalizedData_AssignClothing Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_AssignClothing.FinalizedData_AssignClothing) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_AssignClothing.FinalizedData_AssignClothing dataAssignClothing = new global::Objectives_AssignClothing.FinalizedData_AssignClothing();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataAssignClothing.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataAssignClothing.Index = reader.ReadInt32();
              break;
            case 2:
              dataAssignClothing.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataAssignClothing.ClothingType = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
              break;
            case 4:
              dataAssignClothing.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataAssignClothing;
      }
    }
  }

  public class Objectives_BedRest
  {
    public sealed class FinalizedData_BedRestFormatter : 
      IMessagePackFormatter<global::Objectives_BedRest.FinalizedData_BedRest>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_BedRest.FinalizedData_BedRest value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.FollowerName, options);
        }
      }

      public global::Objectives_BedRest.FinalizedData_BedRest Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_BedRest.FinalizedData_BedRest) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_BedRest.FinalizedData_BedRest finalizedDataBedRest = new global::Objectives_BedRest.FinalizedData_BedRest();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataBedRest.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataBedRest.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataBedRest.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataBedRest.FollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataBedRest;
      }
    }
  }

  public class Objectives_BlizzardOffering
  {
    public sealed class FinalizedData_BlizzardOfferingFormatter : 
      IMessagePackFormatter<global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(6);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.Count);
          writer.Write(value.Target);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.TargetType, options);
        }
      }

      public global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering blizzardOffering = new global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              blizzardOffering.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              blizzardOffering.Index = reader.ReadInt32();
              break;
            case 2:
              blizzardOffering.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              blizzardOffering.Count = reader.ReadInt32();
              break;
            case 4:
              blizzardOffering.Target = reader.ReadInt32();
              break;
            case 5:
              blizzardOffering.TargetType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return blizzardOffering;
      }
    }
  }

  public class Objectives_BuildStructure
  {
    public sealed class FinalizedData_BuildStructureFormatter : 
      IMessagePackFormatter<global::Objectives_BuildStructure.FinalizedData_BuildStructure>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_BuildStructure.FinalizedData_BuildStructure value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(6);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.StructureType, options);
          writer.Write(value.Target);
          writer.Write(value.Count);
        }
      }

      public global::Objectives_BuildStructure.FinalizedData_BuildStructure Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_BuildStructure.FinalizedData_BuildStructure) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_BuildStructure.FinalizedData_BuildStructure dataBuildStructure = new global::Objectives_BuildStructure.FinalizedData_BuildStructure();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataBuildStructure.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataBuildStructure.Index = reader.ReadInt32();
              break;
            case 2:
              dataBuildStructure.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataBuildStructure.StructureType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
              break;
            case 4:
              dataBuildStructure.Target = reader.ReadInt32();
              break;
            case 5:
              dataBuildStructure.Count = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataBuildStructure;
      }
    }
  }

  public class Objectives_BuildWinterDecorations
  {
    public sealed class FinalizedData_BuildWinterDecorationsFormatter : 
      IMessagePackFormatter<global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.Target);
          writer.Write(value.Count);
        }
      }

      public global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations winterDecorations = new global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              winterDecorations.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              winterDecorations.Index = reader.ReadInt32();
              break;
            case 2:
              winterDecorations.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              winterDecorations.Target = reader.ReadInt32();
              break;
            case 4:
              winterDecorations.Count = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return winterDecorations;
      }
    }
  }

  public class Objectives_CollectItem
  {
    public sealed class FinalizedData_CollectItemFormatter : 
      IMessagePackFormatter<global::Objectives_CollectItem.FinalizedData_CollectItem>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_CollectItem.FinalizedData_CollectItem value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(9);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.ItemType, options);
          resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.TargetLocation, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LocKey, options);
          writer.Write(value.Target);
          writer.Write(value.Count);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CustomTerm, options);
        }
      }

      public global::Objectives_CollectItem.FinalizedData_CollectItem Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_CollectItem.FinalizedData_CollectItem) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_CollectItem.FinalizedData_CollectItem finalizedDataCollectItem = new global::Objectives_CollectItem.FinalizedData_CollectItem();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataCollectItem.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataCollectItem.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataCollectItem.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataCollectItem.ItemType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataCollectItem.TargetLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
              break;
            case 5:
              finalizedDataCollectItem.LocKey = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 6:
              finalizedDataCollectItem.Target = reader.ReadInt32();
              break;
            case 7:
              finalizedDataCollectItem.Count = reader.ReadInt32();
              break;
            case 8:
              finalizedDataCollectItem.CustomTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataCollectItem;
      }
    }
  }

  public class Objectives_CookMeal
  {
    public sealed class FinalizedData_CookMealFormatter : 
      IMessagePackFormatter<global::Objectives_CookMeal.FinalizedData_CookMeal>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_CookMeal.FinalizedData_CookMeal value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(6);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.MealType, options);
          writer.Write(value.Target);
          writer.Write(value.Count);
        }
      }

      public global::Objectives_CookMeal.FinalizedData_CookMeal Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_CookMeal.FinalizedData_CookMeal) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_CookMeal.FinalizedData_CookMeal finalizedDataCookMeal = new global::Objectives_CookMeal.FinalizedData_CookMeal();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataCookMeal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataCookMeal.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataCookMeal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataCookMeal.MealType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataCookMeal.Target = reader.ReadInt32();
              break;
            case 5:
              finalizedDataCookMeal.Count = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataCookMeal;
      }
    }
  }

  public class Objectives_CraftClothing
  {
    public sealed class FinalizedData_CraftClothingFormatter : 
      IMessagePackFormatter<global::Objectives_CraftClothing.FinalizedData_CraftClothing>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_CraftClothing.FinalizedData_CraftClothing value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.ClothingType, options);
        }
      }

      public global::Objectives_CraftClothing.FinalizedData_CraftClothing Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_CraftClothing.FinalizedData_CraftClothing) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_CraftClothing.FinalizedData_CraftClothing dataCraftClothing = new global::Objectives_CraftClothing.FinalizedData_CraftClothing();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataCraftClothing.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataCraftClothing.Index = reader.ReadInt32();
              break;
            case 2:
              dataCraftClothing.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataCraftClothing.ClothingType = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataCraftClothing;
      }
    }
  }

  public class Objectives_Custom
  {
    public sealed class FinalizedData_CustomFormatter : 
      IMessagePackFormatter<global::Objectives_Custom.FinalizedData_Custom>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_Custom.FinalizedData_Custom value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(6);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::Objectives.CustomQuestTypes>().Serialize(ref writer, value.CustomQuestType, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
          resolver.GetFormatterWithVerify<List<global::FollowerTrait.TraitType>>().Serialize(ref writer, value.Traits, options);
        }
      }

      public global::Objectives_Custom.FinalizedData_Custom Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_Custom.FinalizedData_Custom) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_Custom.FinalizedData_Custom finalizedDataCustom = new global::Objectives_Custom.FinalizedData_Custom();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataCustom.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataCustom.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataCustom.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataCustom.CustomQuestType = resolver.GetFormatterWithVerify<global::Objectives.CustomQuestTypes>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataCustom.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 5:
              finalizedDataCustom.Traits = resolver.GetFormatterWithVerify<List<global::FollowerTrait.TraitType>>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataCustom;
      }
    }
  }

  public class Objectives_DefeatKnucklebones
  {
    public sealed class FinalizedData_DefeatKnucklebonesFormatter : 
      IMessagePackFormatter<global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CharacterNameTerm, options);
        }
      }

      public global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones defeatKnucklebones = new global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              defeatKnucklebones.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              defeatKnucklebones.Index = reader.ReadInt32();
              break;
            case 2:
              defeatKnucklebones.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              defeatKnucklebones.CharacterNameTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return defeatKnucklebones;
      }
    }
  }

  public class Objectives_DepositFood
  {
    public sealed class FinalizedData_DepositFoodFormatter : 
      IMessagePackFormatter<global::Objectives_DepositFood.FinalizedData_DepositFood>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_DepositFood.FinalizedData_DepositFood value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(3);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        }
      }

      public global::Objectives_DepositFood.FinalizedData_DepositFood Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_DepositFood.FinalizedData_DepositFood) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_DepositFood.FinalizedData_DepositFood finalizedDataDepositFood = new global::Objectives_DepositFood.FinalizedData_DepositFood();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataDepositFood.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataDepositFood.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataDepositFood.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataDepositFood;
      }
    }
  }

  public class Objectives_Drink
  {
    public sealed class FinalizedData_DrinkFormatter : 
      IMessagePackFormatter<global::Objectives_Drink.FinalizedData_Drink>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_Drink.FinalizedData_Drink value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.DrinkType, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
        }
      }

      public global::Objectives_Drink.FinalizedData_Drink Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_Drink.FinalizedData_Drink) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_Drink.FinalizedData_Drink finalizedDataDrink = new global::Objectives_Drink.FinalizedData_Drink();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataDrink.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataDrink.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataDrink.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataDrink.DrinkType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataDrink.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataDrink;
      }
    }
  }

  public class Objectives_EatMeal
  {
    public sealed class FinalizedData_EatMealFormatter : 
      IMessagePackFormatter<global::Objectives_EatMeal.FinalizedData_EatMeal>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_EatMeal.FinalizedData_EatMeal value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.MealType, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
        }
      }

      public global::Objectives_EatMeal.FinalizedData_EatMeal Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_EatMeal.FinalizedData_EatMeal) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_EatMeal.FinalizedData_EatMeal finalizedDataEatMeal = new global::Objectives_EatMeal.FinalizedData_EatMeal();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataEatMeal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataEatMeal.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataEatMeal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataEatMeal.MealType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataEatMeal.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataEatMeal;
      }
    }
  }

  public class Objectives_FeedAnimal
  {
    public sealed class FinalizedData_FeedAnimalFormatter : 
      IMessagePackFormatter<global::Objectives_FeedAnimal.FinalizedData_FeedAnimal>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_FeedAnimal.FinalizedData_FeedAnimal value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetAnimalName, options);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.Food, options);
        }
      }

      public global::Objectives_FeedAnimal.FinalizedData_FeedAnimal Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_FeedAnimal.FinalizedData_FeedAnimal) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_FeedAnimal.FinalizedData_FeedAnimal finalizedDataFeedAnimal = new global::Objectives_FeedAnimal.FinalizedData_FeedAnimal();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataFeedAnimal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataFeedAnimal.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataFeedAnimal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataFeedAnimal.TargetAnimalName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataFeedAnimal.Food = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataFeedAnimal;
      }
    }
  }

  public class Objectives_FindChildren
  {
    public sealed class FinalizedData_FindChildrenFormatter : 
      IMessagePackFormatter<global::Objectives_FindChildren.FinalizedData_FindChildren>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_FindChildren.FinalizedData_FindChildren value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::Objectives_FindChildren.ChildLocation>().Serialize(ref writer, value.Location, options);
        }
      }

      public global::Objectives_FindChildren.FinalizedData_FindChildren Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_FindChildren.FinalizedData_FindChildren) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_FindChildren.FinalizedData_FindChildren dataFindChildren = new global::Objectives_FindChildren.FinalizedData_FindChildren();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataFindChildren.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataFindChildren.Index = reader.ReadInt32();
              break;
            case 2:
              dataFindChildren.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataFindChildren.Location = resolver.GetFormatterWithVerify<global::Objectives_FindChildren.ChildLocation>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataFindChildren;
      }
    }

    public sealed class ChildLocationFormatter : 
      IMessagePackFormatter<global::Objectives_FindChildren.ChildLocation>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_FindChildren.ChildLocation value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::Objectives_FindChildren.ChildLocation Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::Objectives_FindChildren.ChildLocation) reader.ReadInt32();
      }
    }
  }

  public class Objectives_FindFollower
  {
    public sealed class FinalizedData_FindFollowerFormatter : 
      IMessagePackFormatter<global::Objectives_FindFollower.FinalizedData_FindFollower>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_FindFollower.FinalizedData_FindFollower value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.TargetLocation, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
        }
      }

      public global::Objectives_FindFollower.FinalizedData_FindFollower Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_FindFollower.FinalizedData_FindFollower) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_FindFollower.FinalizedData_FindFollower dataFindFollower = new global::Objectives_FindFollower.FinalizedData_FindFollower();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataFindFollower.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataFindFollower.Index = reader.ReadInt32();
              break;
            case 2:
              dataFindFollower.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataFindFollower.TargetLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
              break;
            case 4:
              dataFindFollower.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataFindFollower;
      }
    }
  }

  public class Objectives_FinishRace
  {
    public sealed class FinalizedData_Objectives_FinishRaceFormatter : 
      IMessagePackFormatter<global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.RaceTime);
          writer.Write(value.RaceTargetTime);
        }
      }

      public global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace objectivesFinishRace = new global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              objectivesFinishRace.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              objectivesFinishRace.Index = reader.ReadInt32();
              break;
            case 2:
              objectivesFinishRace.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              objectivesFinishRace.RaceTime = reader.ReadSingle();
              break;
            case 4:
              objectivesFinishRace.RaceTargetTime = reader.ReadSingle();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return objectivesFinishRace;
      }
    }
  }

  public class Objectives_FlowerBaskets
  {
    public sealed class FinalizedData_FlowerBasketsFormatter : 
      IMessagePackFormatter<global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.FilledPots);
          writer.Write(value.PotsToFill);
        }
      }

      public global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets dataFlowerBaskets = new global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataFlowerBaskets.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataFlowerBaskets.Index = reader.ReadInt32();
              break;
            case 2:
              dataFlowerBaskets.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataFlowerBaskets.FilledPots = reader.ReadInt32();
              break;
            case 4:
              dataFlowerBaskets.PotsToFill = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataFlowerBaskets;
      }
    }
  }

  public class Objectives_GetAnimal
  {
    public sealed class FinalizedData_GetAnimalFormatter : 
      IMessagePackFormatter<global::Objectives_GetAnimal.FinalizedData_GetAnimal>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_GetAnimal.FinalizedData_GetAnimal value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.AnimalType, options);
          writer.Write(value.Level);
        }
      }

      public global::Objectives_GetAnimal.FinalizedData_GetAnimal Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_GetAnimal.FinalizedData_GetAnimal) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_GetAnimal.FinalizedData_GetAnimal finalizedDataGetAnimal = new global::Objectives_GetAnimal.FinalizedData_GetAnimal();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataGetAnimal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataGetAnimal.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataGetAnimal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataGetAnimal.AnimalType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataGetAnimal.Level = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataGetAnimal;
      }
    }
  }

  public class Objectives_GiveItem
  {
    public sealed class FinalizedData_GiveItemFormatter : 
      IMessagePackFormatter<global::Objectives_GiveItem.FinalizedData_GiveItem>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_GiveItem.FinalizedData_GiveItem value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(8);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.NPCTerm, options);
          writer.Write(value.Count);
          writer.Write(value.Target);
          resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.TargetType, options);
          resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.Location, options);
        }
      }

      public global::Objectives_GiveItem.FinalizedData_GiveItem Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_GiveItem.FinalizedData_GiveItem) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_GiveItem.FinalizedData_GiveItem finalizedDataGiveItem = new global::Objectives_GiveItem.FinalizedData_GiveItem();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataGiveItem.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataGiveItem.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataGiveItem.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataGiveItem.NPCTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataGiveItem.Count = reader.ReadInt32();
              break;
            case 5:
              finalizedDataGiveItem.Target = reader.ReadInt32();
              break;
            case 6:
              finalizedDataGiveItem.TargetType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 7:
              finalizedDataGiveItem.Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataGiveItem;
      }
    }
  }

  public class Objectives_KillEnemies
  {
    public sealed class FinalizedData_KillEnemiesFormatter : 
      IMessagePackFormatter<global::Objectives_KillEnemies.FinalizedData_KillEnemies>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_KillEnemies.FinalizedData_KillEnemies value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<Enemy>().Serialize(ref writer, value.EnemyType, options);
          writer.Write(value.EnemiesKilledRequired);
        }
      }

      public global::Objectives_KillEnemies.FinalizedData_KillEnemies Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_KillEnemies.FinalizedData_KillEnemies) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_KillEnemies.FinalizedData_KillEnemies finalizedDataKillEnemies = new global::Objectives_KillEnemies.FinalizedData_KillEnemies();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataKillEnemies.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataKillEnemies.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataKillEnemies.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataKillEnemies.EnemyType = resolver.GetFormatterWithVerify<Enemy>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataKillEnemies.EnemiesKilledRequired = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataKillEnemies;
      }
    }
  }

  public class Objectives_LegendarySwordReturn
  {
    public sealed class FinalizedData_LegendarySwordReturnFormatter : 
      IMessagePackFormatter<global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
        }
      }

      public global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn legendarySwordReturn = new global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              legendarySwordReturn.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              legendarySwordReturn.Index = reader.ReadInt32();
              break;
            case 2:
              legendarySwordReturn.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              legendarySwordReturn.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return legendarySwordReturn;
      }
    }
  }

  public class Objectives_LegendaryWeaponRun
  {
    public sealed class FinalizedData_LegendaryWeaponRunFormatter : 
      IMessagePackFormatter<global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<EquipmentType>().Serialize(ref writer, value.LegendaryWeapon, options);
        }
      }

      public global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun legendaryWeaponRun = new global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              legendaryWeaponRun.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              legendaryWeaponRun.Index = reader.ReadInt32();
              break;
            case 2:
              legendaryWeaponRun.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              legendaryWeaponRun.LegendaryWeapon = resolver.GetFormatterWithVerify<EquipmentType>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return legendaryWeaponRun;
      }
    }
  }

  public class Objectives_Mating
  {
    public sealed class FinalizedData_MatingFormatter : 
      IMessagePackFormatter<global::Objectives_Mating.FinalizedData_Mating>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_Mating.FinalizedData_Mating value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName_1, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName_2, options);
        }
      }

      public global::Objectives_Mating.FinalizedData_Mating Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_Mating.FinalizedData_Mating) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_Mating.FinalizedData_Mating finalizedDataMating = new global::Objectives_Mating.FinalizedData_Mating();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataMating.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataMating.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataMating.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataMating.TargetFollowerName_1 = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 4:
              finalizedDataMating.TargetFollowerName_2 = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataMating;
      }
    }
  }

  public class Objectives_NoCurses
  {
    public sealed class FinalizedData_NoCursesFormatter : 
      IMessagePackFormatter<global::Objectives_NoCurses.FinalizedData_NoCurses>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_NoCurses.FinalizedData_NoCurses value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.RoomsRequired);
          writer.Write(value.RoomsCompleted);
        }
      }

      public global::Objectives_NoCurses.FinalizedData_NoCurses Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_NoCurses.FinalizedData_NoCurses) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_NoCurses.FinalizedData_NoCurses finalizedDataNoCurses = new global::Objectives_NoCurses.FinalizedData_NoCurses();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataNoCurses.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataNoCurses.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataNoCurses.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataNoCurses.RoomsRequired = reader.ReadInt32();
              break;
            case 4:
              finalizedDataNoCurses.RoomsCompleted = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataNoCurses;
      }
    }
  }

  public class Objectives_NoDamage
  {
    public sealed class FinalizedData_NoDamageFormatter : 
      IMessagePackFormatter<global::Objectives_NoDamage.FinalizedData_NoDamage>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_NoDamage.FinalizedData_NoDamage value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.RoomsRequired);
          writer.Write(value.RoomsCompleted);
        }
      }

      public global::Objectives_NoDamage.FinalizedData_NoDamage Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_NoDamage.FinalizedData_NoDamage) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_NoDamage.FinalizedData_NoDamage finalizedDataNoDamage = new global::Objectives_NoDamage.FinalizedData_NoDamage();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataNoDamage.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataNoDamage.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataNoDamage.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataNoDamage.RoomsRequired = reader.ReadInt32();
              break;
            case 4:
              finalizedDataNoDamage.RoomsCompleted = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataNoDamage;
      }
    }
  }

  public class Objectives_NoDodge
  {
    public sealed class FinalizedData_NoDodgeFormatter : 
      IMessagePackFormatter<global::Objectives_NoDodge.FinalizedData_NoDodge>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_NoDodge.FinalizedData_NoDodge value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.RoomsRequired);
          writer.Write(value.RoomsCompleted);
        }
      }

      public global::Objectives_NoDodge.FinalizedData_NoDodge Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_NoDodge.FinalizedData_NoDodge) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_NoDodge.FinalizedData_NoDodge finalizedDataNoDodge = new global::Objectives_NoDodge.FinalizedData_NoDodge();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataNoDodge.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataNoDodge.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataNoDodge.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataNoDodge.RoomsRequired = reader.ReadInt32();
              break;
            case 4:
              finalizedDataNoDodge.RoomsCompleted = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataNoDodge;
      }
    }
  }

  public class Objectives_NoHealing
  {
    public sealed class FinalizedData_NoHealingFormatter : 
      IMessagePackFormatter<global::Objectives_NoHealing.FinalizedData_NoHealing>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_NoHealing.FinalizedData_NoHealing value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(3);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        }
      }

      public global::Objectives_NoHealing.FinalizedData_NoHealing Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_NoHealing.FinalizedData_NoHealing) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_NoHealing.FinalizedData_NoHealing finalizedDataNoHealing = new global::Objectives_NoHealing.FinalizedData_NoHealing();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataNoHealing.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataNoHealing.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataNoHealing.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataNoHealing;
      }
    }
  }

  public class Objectives_PerformRitual
  {
    public sealed class FinalizedData_PerformRitualFormatter : 
      IMessagePackFormatter<global::Objectives_PerformRitual.FinalizedData_PerformRitual>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_PerformRitual.FinalizedData_PerformRitual value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(6);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Serialize(ref writer, value.Ritual, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName_1, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName_2, options);
        }
      }

      public global::Objectives_PerformRitual.FinalizedData_PerformRitual Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_PerformRitual.FinalizedData_PerformRitual) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_PerformRitual.FinalizedData_PerformRitual dataPerformRitual = new global::Objectives_PerformRitual.FinalizedData_PerformRitual();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataPerformRitual.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataPerformRitual.Index = reader.ReadInt32();
              break;
            case 2:
              dataPerformRitual.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataPerformRitual.Ritual = resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Deserialize(ref reader, options);
              break;
            case 4:
              dataPerformRitual.TargetFollowerName_1 = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 5:
              dataPerformRitual.TargetFollowerName_2 = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataPerformRitual;
      }
    }
  }

  public class Objectives_PlaceStructure
  {
    public sealed class FinalizedData_PlaceStructureFormatter : 
      IMessagePackFormatter<global::Objectives_PlaceStructure.FinalizedData_PlaceStructure>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_PlaceStructure.FinalizedData_PlaceStructure value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(6);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::Objectives_PlaceStructure.DecorationType>().Serialize(ref writer, value.DecoType, options);
          writer.Write(value.Target);
          writer.Write(value.Count);
        }
      }

      public global::Objectives_PlaceStructure.FinalizedData_PlaceStructure Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_PlaceStructure.FinalizedData_PlaceStructure) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_PlaceStructure.FinalizedData_PlaceStructure dataPlaceStructure = new global::Objectives_PlaceStructure.FinalizedData_PlaceStructure();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataPlaceStructure.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataPlaceStructure.Index = reader.ReadInt32();
              break;
            case 2:
              dataPlaceStructure.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataPlaceStructure.DecoType = resolver.GetFormatterWithVerify<global::Objectives_PlaceStructure.DecorationType>().Deserialize(ref reader, options);
              break;
            case 4:
              dataPlaceStructure.Target = reader.ReadInt32();
              break;
            case 5:
              dataPlaceStructure.Count = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataPlaceStructure;
      }
    }

    public sealed class DecorationTypeFormatter : 
      IMessagePackFormatter<global::Objectives_PlaceStructure.DecorationType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_PlaceStructure.DecorationType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::Objectives_PlaceStructure.DecorationType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::Objectives_PlaceStructure.DecorationType) reader.ReadInt32();
      }
    }
  }

  public class Objectives_RecruitCursedFollower
  {
    public sealed class FinalizedData_RecruitCursedFollowerFormatter : 
      IMessagePackFormatter<global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(8);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<Thought>().Serialize(ref writer, value.CursedState, options);
          writer.Write(value.Target);
          writer.Write(value.Count);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.FollowerName, options);
          writer.Write(value.FollowerID);
        }
      }

      public global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower recruitCursedFollower = new global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              recruitCursedFollower.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              recruitCursedFollower.Index = reader.ReadInt32();
              break;
            case 2:
              recruitCursedFollower.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              recruitCursedFollower.CursedState = resolver.GetFormatterWithVerify<Thought>().Deserialize(ref reader, options);
              break;
            case 4:
              recruitCursedFollower.Target = reader.ReadInt32();
              break;
            case 5:
              recruitCursedFollower.Count = reader.ReadInt32();
              break;
            case 6:
              recruitCursedFollower.FollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 7:
              recruitCursedFollower.FollowerID = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return recruitCursedFollower;
      }
    }
  }

  public class Objectives_RecruitFollower
  {
    public sealed class FinalizedData_RecruitFollowerFormatter : 
      IMessagePackFormatter<global::Objectives_RecruitFollower.FinalizedData_RecruitFollower>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_RecruitFollower.FinalizedData_RecruitFollower value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.Count);
          writer.Write(value.FollowerCount);
        }
      }

      public global::Objectives_RecruitFollower.FinalizedData_RecruitFollower Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_RecruitFollower.FinalizedData_RecruitFollower) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_RecruitFollower.FinalizedData_RecruitFollower dataRecruitFollower = new global::Objectives_RecruitFollower.FinalizedData_RecruitFollower();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataRecruitFollower.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataRecruitFollower.Index = reader.ReadInt32();
              break;
            case 2:
              dataRecruitFollower.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataRecruitFollower.Count = reader.ReadInt32();
              break;
            case 4:
              dataRecruitFollower.FollowerCount = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataRecruitFollower;
      }
    }
  }

  public class Objectives_RemoveStructure
  {
    public sealed class FinalizedData_RemoveStructureFormatter : 
      IMessagePackFormatter<global::Objectives_RemoveStructure.FinalizedData_RemoveStructure>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_RemoveStructure.FinalizedData_RemoveStructure value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(6);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.StructureType, options);
          writer.Write(value.Target);
          writer.Write(value.Count);
        }
      }

      public global::Objectives_RemoveStructure.FinalizedData_RemoveStructure Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_RemoveStructure.FinalizedData_RemoveStructure) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_RemoveStructure.FinalizedData_RemoveStructure dataRemoveStructure = new global::Objectives_RemoveStructure.FinalizedData_RemoveStructure();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataRemoveStructure.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataRemoveStructure.Index = reader.ReadInt32();
              break;
            case 2:
              dataRemoveStructure.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataRemoveStructure.StructureType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
              break;
            case 4:
              dataRemoveStructure.Target = reader.ReadInt32();
              break;
            case 5:
              dataRemoveStructure.Count = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataRemoveStructure;
      }
    }
  }

  public class Objectives_ShootDummy
  {
    public sealed class FinalizedData_ShootDummyFormatter : 
      IMessagePackFormatter<global::Objectives_ShootDummy.FinalizedData_ShootDummy>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_ShootDummy.FinalizedData_ShootDummy value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.Target);
        }
      }

      public global::Objectives_ShootDummy.FinalizedData_ShootDummy Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_ShootDummy.FinalizedData_ShootDummy) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_ShootDummy.FinalizedData_ShootDummy finalizedDataShootDummy = new global::Objectives_ShootDummy.FinalizedData_ShootDummy();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataShootDummy.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataShootDummy.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataShootDummy.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataShootDummy.Target = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataShootDummy;
      }
    }
  }

  public class Objectives_ShowFleece
  {
    public sealed class FinalizedData_ShowFleeceFormatter : 
      IMessagePackFormatter<global::Objectives_ShowFleece.FinalizedData_ShowFleece>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_ShowFleece.FinalizedData_ShowFleece value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::PlayerFleeceManager.FleeceType>().Serialize(ref writer, value.FleeceType, options);
        }
      }

      public global::Objectives_ShowFleece.FinalizedData_ShowFleece Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_ShowFleece.FinalizedData_ShowFleece) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_ShowFleece.FinalizedData_ShowFleece finalizedDataShowFleece = new global::Objectives_ShowFleece.FinalizedData_ShowFleece();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataShowFleece.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataShowFleece.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataShowFleece.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataShowFleece.FleeceType = resolver.GetFormatterWithVerify<global::PlayerFleeceManager.FleeceType>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataShowFleece;
      }
    }
  }

  public class Objectives_Story
  {
    public sealed class FinalizedDataFormatter : 
      IMessagePackFormatter<global::Objectives_Story.FinalizedData>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_Story.FinalizedData value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(3);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
        }
      }

      public global::Objectives_Story.FinalizedData Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_Story.FinalizedData) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_Story.FinalizedData finalizedData = new global::Objectives_Story.FinalizedData();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedData.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedData.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedData.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedData;
      }
    }
  }

  public class Objectives_TalkToFollower
  {
    public sealed class FinalizedData_TalkToFollowerFormatter : 
      IMessagePackFormatter<global::Objectives_TalkToFollower.FinalizedData_TalkToFollower>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_TalkToFollower.FinalizedData_TalkToFollower value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LocKey, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetFollowerName, options);
        }
      }

      public global::Objectives_TalkToFollower.FinalizedData_TalkToFollower Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_TalkToFollower.FinalizedData_TalkToFollower) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_TalkToFollower.FinalizedData_TalkToFollower dataTalkToFollower = new global::Objectives_TalkToFollower.FinalizedData_TalkToFollower();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataTalkToFollower.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataTalkToFollower.Index = reader.ReadInt32();
              break;
            case 2:
              dataTalkToFollower.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataTalkToFollower.LocKey = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 4:
              dataTalkToFollower.TargetFollowerName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataTalkToFollower;
      }
    }
  }

  public class Objectives_UnlockUpgrade
  {
    public sealed class FinalizedData_UnlockUpgradeFormatter : 
      IMessagePackFormatter<global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Serialize(ref writer, value.UnlockType, options);
        }
      }

      public global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade dataUnlockUpgrade = new global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              dataUnlockUpgrade.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              dataUnlockUpgrade.Index = reader.ReadInt32();
              break;
            case 2:
              dataUnlockUpgrade.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              dataUnlockUpgrade.UnlockType = resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return dataUnlockUpgrade;
      }
    }
  }

  public class Objectives_UseRelic
  {
    public sealed class FinalizedData_UseRelicFormatter : 
      IMessagePackFormatter<global::Objectives_UseRelic.FinalizedData_UseRelic>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_UseRelic.FinalizedData_UseRelic value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          writer.Write(value.Target);
        }
      }

      public global::Objectives_UseRelic.FinalizedData_UseRelic Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_UseRelic.FinalizedData_UseRelic) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_UseRelic.FinalizedData_UseRelic finalizedDataUseRelic = new global::Objectives_UseRelic.FinalizedData_UseRelic();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataUseRelic.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataUseRelic.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataUseRelic.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataUseRelic.Target = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataUseRelic;
      }
    }
  }

  public class Objectives_WalkAnimal
  {
    public sealed class FinalizedData_WalkAnimalFormatter : 
      IMessagePackFormatter<global::Objectives_WalkAnimal.FinalizedData_WalkAnimal>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_WalkAnimal.FinalizedData_WalkAnimal value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(4);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TargetAnimalName, options);
        }
      }

      public global::Objectives_WalkAnimal.FinalizedData_WalkAnimal Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_WalkAnimal.FinalizedData_WalkAnimal) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::Objectives_WalkAnimal.FinalizedData_WalkAnimal finalizedDataWalkAnimal = new global::Objectives_WalkAnimal.FinalizedData_WalkAnimal();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              finalizedDataWalkAnimal.GroupId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              finalizedDataWalkAnimal.Index = reader.ReadInt32();
              break;
            case 2:
              finalizedDataWalkAnimal.UniqueGroupID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              finalizedDataWalkAnimal.TargetAnimalName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return finalizedDataWalkAnimal;
      }
    }
  }

  public class Objectives_WinFlockadeBet
  {
    public sealed class FinalizedData_WinFlockadeBetFormatter : 
      IMessagePackFormatter<global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(5);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GroupId, options);
          writer.Write(value.Index);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.UniqueGroupID, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.OpponentTermId, options);
          writer.Write(value.WoolAmount);
        }
      }

      public global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num1 = reader.ReadArrayHeader();
        string str1 = (string) null;
        int num2 = 0;
        string opponentTermId = (string) null;
        int woolAmount = 0;
        string str2 = (string) null;
        for (int index = 0; index < num1; ++index)
        {
          switch (index)
          {
            case 0:
              opponentTermId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 1:
              woolAmount = reader.ReadInt32();
              break;
            case 2:
              str2 = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 3:
              str1 = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 4:
              num2 = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet dataWinFlockadeBet = new global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet(opponentTermId, woolAmount);
        if (num1 > 2)
        {
          dataWinFlockadeBet.UniqueGroupID = str2;
          if (num1 > 3)
          {
            dataWinFlockadeBet.OpponentTermId = str1;
            if (num1 > 4)
              dataWinFlockadeBet.WoolAmount = num2;
          }
        }
        --reader.Depth;
        return dataWinFlockadeBet;
      }
    }
  }

  public class StructuresData
  {
    public sealed class EggDataFormatter : 
      IMessagePackFormatter<global::StructuresData.EggData>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructuresData.EggData value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(16 /*0x10*/);
          writer.Write(value.EggSeed);
          writer.Write(value.Parent_1_ID);
          writer.Write(value.Parent_2_ID);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Parent_1_SkinName, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Parent_2_SkinName, options);
          writer.Write(value.Parent_1_SkinColor);
          writer.Write(value.Parent_2_SkinColor);
          writer.Write(value.Parent_1_SkinVariant);
          writer.Write(value.Parent_2_SkinVariant);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Parent1Name, options);
          resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Parent2Name, options);
          resolver.GetFormatterWithVerify<List<global::FollowerTrait.TraitType>>().Serialize(ref writer, value.Traits, options);
          writer.Write(value.Golden);
          writer.Write(value.Rotting);
          writer.Write(value.RottingUnique);
          resolver.GetFormatterWithVerify<FollowerSpecialType>().Serialize(ref writer, value.Special, options);
        }
      }

      public global::StructuresData.EggData Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::StructuresData.EggData) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::StructuresData.EggData eggData = new global::StructuresData.EggData();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              eggData.EggSeed = reader.ReadInt32();
              break;
            case 1:
              eggData.Parent_1_ID = reader.ReadInt32();
              break;
            case 2:
              eggData.Parent_2_ID = reader.ReadInt32();
              break;
            case 3:
              eggData.Parent_1_SkinName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 4:
              eggData.Parent_2_SkinName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 5:
              eggData.Parent_1_SkinColor = reader.ReadInt32();
              break;
            case 6:
              eggData.Parent_2_SkinColor = reader.ReadInt32();
              break;
            case 7:
              eggData.Parent_1_SkinVariant = reader.ReadInt32();
              break;
            case 8:
              eggData.Parent_2_SkinVariant = reader.ReadInt32();
              break;
            case 9:
              eggData.Parent1Name = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 10:
              eggData.Parent2Name = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            case 11:
              eggData.Traits = resolver.GetFormatterWithVerify<List<global::FollowerTrait.TraitType>>().Deserialize(ref reader, options);
              break;
            case 12:
              eggData.Golden = reader.ReadBoolean();
              break;
            case 13:
              eggData.Rotting = reader.ReadBoolean();
              break;
            case 14:
              eggData.RottingUnique = reader.ReadBoolean();
              break;
            case 15:
              eggData.Special = resolver.GetFormatterWithVerify<FollowerSpecialType>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return eggData;
      }
    }

    public sealed class LogisticsSlotFormatter : 
      IMessagePackFormatter<global::StructuresData.LogisticsSlot>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructuresData.LogisticsSlot value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(2);
          resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.RootStructureType, options);
          resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.TargetStructureType, options);
        }
      }

      public global::StructuresData.LogisticsSlot Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::StructuresData.LogisticsSlot) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::StructuresData.LogisticsSlot logisticsSlot = new global::StructuresData.LogisticsSlot();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              logisticsSlot.RootStructureType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
              break;
            case 1:
              logisticsSlot.TargetStructureType = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return logisticsSlot;
      }
    }

    public sealed class ResearchObjectFormatter : 
      IMessagePackFormatter<global::StructuresData.ResearchObject>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructuresData.ResearchObject value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(2);
          resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Serialize(ref writer, value.Type, options);
          writer.Write(value.Progress);
        }
      }

      public global::StructuresData.ResearchObject Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::StructuresData.ResearchObject) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num1 = reader.ReadArrayHeader();
        global::StructureBrain.TYPES Type = global::StructureBrain.TYPES.NONE;
        float num2 = 0.0f;
        for (int index = 0; index < num1; ++index)
        {
          switch (index)
          {
            case 0:
              Type = resolver.GetFormatterWithVerify<global::StructureBrain.TYPES>().Deserialize(ref reader, options);
              break;
            case 1:
              num2 = reader.ReadSingle();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        global::StructuresData.ResearchObject researchObject = new global::StructuresData.ResearchObject(Type);
        if (num1 > 1)
          researchObject.Progress = num2;
        --reader.Depth;
        return researchObject;
      }
    }

    public sealed class ClothingStructFormatter : 
      IMessagePackFormatter<global::StructuresData.ClothingStruct>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructuresData.ClothingStruct value,
        MessagePackSerializerOptions options)
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(2);
        resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.ClothingType, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Variant, options);
      }

      public global::StructuresData.ClothingStruct Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          throw new InvalidOperationException("typecode is null, struct not supported");
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        FollowerClothingType clothingType = FollowerClothingType.None;
        string variant = (string) null;
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              clothingType = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
              break;
            case 1:
              variant = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        global::StructuresData.ClothingStruct clothingStruct = new global::StructuresData.ClothingStruct(clothingType, variant);
        --reader.Depth;
        return clothingStruct;
      }
    }

    public sealed class PathDataFormatter : 
      IMessagePackFormatter<global::StructuresData.PathData>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructuresData.PathData value,
        MessagePackSerializerOptions options)
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(3);
        resolver.GetFormatterWithVerify<Vector2Int>().Serialize(ref writer, value.TilePosition, options);
        resolver.GetFormatterWithVerify<Vector3>().Serialize(ref writer, value.WorldPosition, options);
        writer.Write(value.PathID);
      }

      public global::StructuresData.PathData Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          throw new InvalidOperationException("typecode is null, struct not supported");
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::StructuresData.PathData pathData = new global::StructuresData.PathData();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              pathData.TilePosition = resolver.GetFormatterWithVerify<Vector2Int>().Deserialize(ref reader, options);
              break;
            case 1:
              pathData.WorldPosition = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
              break;
            case 2:
              pathData.PathID = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return pathData;
      }
    }

    public sealed class PhaseFormatter : 
      IMessagePackFormatter<global::StructuresData.Phase>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructuresData.Phase value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::StructuresData.Phase Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::StructuresData.Phase) reader.ReadInt32();
      }
    }
  }

  public class TarotCards
  {
    public sealed class TarotCardFormatter : 
      IMessagePackFormatter<global::TarotCards.TarotCard>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::TarotCards.TarotCard value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(2);
          resolver.GetFormatterWithVerify<global::TarotCards.Card>().Serialize(ref writer, value.CardType, options);
          writer.Write(value.UpgradeIndex);
        }
      }

      public global::TarotCards.TarotCard Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::TarotCards.TarotCard) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::TarotCards.Card type = global::TarotCards.Card.Hearts1;
        int upgrade = 0;
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              type = resolver.GetFormatterWithVerify<global::TarotCards.Card>().Deserialize(ref reader, options);
              break;
            case 1:
              upgrade = reader.ReadInt32();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        global::TarotCards.TarotCard tarotCard = new global::TarotCards.TarotCard(type, upgrade);
        --reader.Depth;
        return tarotCard;
      }
    }

    public sealed class CardFormatter : IMessagePackFormatter<global::TarotCards.Card>, IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::TarotCards.Card value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::TarotCards.Card Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::TarotCards.Card) reader.ReadInt32();
      }
    }

    public sealed class CardCategoryFormatter : 
      IMessagePackFormatter<global::TarotCards.CardCategory>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::TarotCards.CardCategory value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::TarotCards.CardCategory Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::TarotCards.CardCategory) reader.ReadInt32();
      }
    }
  }

  public class UpgradeSystem
  {
    public sealed class UpgradeCoolDownFormatter : 
      IMessagePackFormatter<global::UpgradeSystem.UpgradeCoolDown>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::UpgradeSystem.UpgradeCoolDown value,
        MessagePackSerializerOptions options)
      {
        if (value == null)
        {
          writer.WriteNil();
        }
        else
        {
          IFormatterResolver resolver = options.Resolver;
          writer.WriteArrayHeader(3);
          resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Serialize(ref writer, value.Type, options);
          writer.Write(value.TotalElapsedGameTime);
          writer.Write(value.Duration);
        }
      }

      public global::UpgradeSystem.UpgradeCoolDown Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        if (reader.TryReadNil())
          return (global::UpgradeSystem.UpgradeCoolDown) null;
        options.Security.DepthStep(ref reader);
        IFormatterResolver resolver = options.Resolver;
        int num = reader.ReadArrayHeader();
        global::UpgradeSystem.UpgradeCoolDown upgradeCoolDown = new global::UpgradeSystem.UpgradeCoolDown();
        for (int index = 0; index < num; ++index)
        {
          switch (index)
          {
            case 0:
              upgradeCoolDown.Type = resolver.GetFormatterWithVerify<global::UpgradeSystem.Type>().Deserialize(ref reader, options);
              break;
            case 1:
              upgradeCoolDown.TotalElapsedGameTime = reader.ReadSingle();
              break;
            case 2:
              upgradeCoolDown.Duration = reader.ReadSingle();
              break;
            default:
              reader.Skip();
              break;
          }
        }
        --reader.Depth;
        return upgradeCoolDown;
      }
    }

    public sealed class TypeFormatter : 
      IMessagePackFormatter<global::UpgradeSystem.Type>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::UpgradeSystem.Type value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::UpgradeSystem.Type Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::UpgradeSystem.Type) reader.ReadInt32();
      }
    }
  }

  public class Lamb
  {
    public class UI
    {
      public class ItemSelector
      {
        public sealed class CategoryFormatter : 
          IMessagePackFormatter<Lamb.UI.ItemSelector.Category>,
          IMessagePackFormatter
        {
          public void Serialize(
            ref MessagePackWriter writer,
            Lamb.UI.ItemSelector.Category value,
            MessagePackSerializerOptions options)
          {
            if (value == null)
            {
              writer.WriteNil();
            }
            else
            {
              IFormatterResolver resolver = options.Resolver;
              writer.WriteArrayHeader(3);
              resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Key, options);
              resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.TrackedItems, options);
              resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.MostRecentItem, options);
            }
          }

          public Lamb.UI.ItemSelector.Category Deserialize(
            ref MessagePackReader reader,
            MessagePackSerializerOptions options)
          {
            if (reader.TryReadNil())
              return (Lamb.UI.ItemSelector.Category) null;
            options.Security.DepthStep(ref reader);
            IFormatterResolver resolver = options.Resolver;
            int num = reader.ReadArrayHeader();
            Lamb.UI.ItemSelector.Category category = new Lamb.UI.ItemSelector.Category();
            for (int index = 0; index < num; ++index)
            {
              switch (index)
              {
                case 0:
                  category.Key = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
                  break;
                case 1:
                  category.TrackedItems = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
                  break;
                case 2:
                  category.MostRecentItem = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
                  break;
                default:
                  reader.Skip();
                  break;
              }
            }
            --reader.Depth;
            return category;
          }
        }
      }

      public sealed class FinalizedFaithNotificationFormatter : 
        IMessagePackFormatter<FinalizedFaithNotification>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          FinalizedFaithNotification value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(5);
            resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LocKey, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.LocalisedParameters, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.NonLocalisedParameters, options);
            writer.Write(value.FaithDelta);
            resolver.GetFormatterWithVerify<FollowerInfoSnapshot>().Serialize(ref writer, value.followerInfoSnapshot, options);
          }
        }

        public FinalizedFaithNotification Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (FinalizedFaithNotification) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          FinalizedFaithNotification faithNotification = new FinalizedFaithNotification();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                faithNotification.LocKey = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
                break;
              case 1:
                faithNotification.LocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 2:
                faithNotification.NonLocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 3:
                faithNotification.FaithDelta = reader.ReadSingle();
                break;
              case 4:
                faithNotification.followerInfoSnapshot = resolver.GetFormatterWithVerify<FollowerInfoSnapshot>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return faithNotification;
        }
      }

      public sealed class FinalizedFollowerNotificationFormatter : 
        IMessagePackFormatter<FinalizedFollowerNotification>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          FinalizedFollowerNotification value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(5);
            resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LocKey, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.LocalisedParameters, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.NonLocalisedParameters, options);
            resolver.GetFormatterWithVerify<FollowerInfoSnapshot>().Serialize(ref writer, value.followerInfoSnapshot, options);
            resolver.GetFormatterWithVerify<global::NotificationFollower.Animation>().Serialize(ref writer, value.Animation, options);
          }
        }

        public FinalizedFollowerNotification Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (FinalizedFollowerNotification) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          FinalizedFollowerNotification followerNotification = new FinalizedFollowerNotification();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                followerNotification.LocKey = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
                break;
              case 1:
                followerNotification.LocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 2:
                followerNotification.NonLocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 3:
                followerNotification.followerInfoSnapshot = resolver.GetFormatterWithVerify<FollowerInfoSnapshot>().Deserialize(ref reader, options);
                break;
              case 4:
                followerNotification.Animation = resolver.GetFormatterWithVerify<global::NotificationFollower.Animation>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return followerNotification;
        }
      }

      public sealed class FinalizedItemNotificationFormatter : 
        IMessagePackFormatter<FinalizedItemNotification>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          FinalizedItemNotification value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(5);
            resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LocKey, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.LocalisedParameters, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.NonLocalisedParameters, options);
            resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.ItemType, options);
            writer.Write(value.ItemDelta);
          }
        }

        public FinalizedItemNotification Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (FinalizedItemNotification) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          FinalizedItemNotification itemNotification = new FinalizedItemNotification();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                itemNotification.LocKey = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
                break;
              case 1:
                itemNotification.LocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 2:
                itemNotification.NonLocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 3:
                itemNotification.ItemType = resolver.GetFormatterWithVerify<global::InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
                break;
              case 4:
                itemNotification.ItemDelta = reader.ReadInt32();
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return itemNotification;
        }
      }

      public sealed class FinalizedNotificationSimpleFormatter : 
        IMessagePackFormatter<FinalizedNotificationSimple>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          FinalizedNotificationSimple value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(3);
            resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LocKey, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.LocalisedParameters, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.NonLocalisedParameters, options);
          }
        }

        public FinalizedNotificationSimple Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (FinalizedNotificationSimple) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          FinalizedNotificationSimple notificationSimple = new FinalizedNotificationSimple();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                notificationSimple.LocKey = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
                break;
              case 1:
                notificationSimple.LocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 2:
                notificationSimple.NonLocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return notificationSimple;
        }
      }

      public sealed class FinalizedRelationshipNotificationFormatter : 
        IMessagePackFormatter<FinalizedRelationshipNotification>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          FinalizedRelationshipNotification value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(7);
            resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LocKey, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.LocalisedParameters, options);
            resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.NonLocalisedParameters, options);
            resolver.GetFormatterWithVerify<FollowerInfoSnapshot>().Serialize(ref writer, value.followerInfoSnapshotA, options);
            resolver.GetFormatterWithVerify<FollowerInfoSnapshot>().Serialize(ref writer, value.followerInfoSnapshotB, options);
            resolver.GetFormatterWithVerify<global::NotificationFollower.Animation>().Serialize(ref writer, value.FollowerAnimationA, options);
            resolver.GetFormatterWithVerify<global::NotificationFollower.Animation>().Serialize(ref writer, value.FollowerAnimationB, options);
          }
        }

        public FinalizedRelationshipNotification Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (FinalizedRelationshipNotification) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          FinalizedRelationshipNotification relationshipNotification = new FinalizedRelationshipNotification();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                relationshipNotification.LocKey = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
                break;
              case 1:
                relationshipNotification.LocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 2:
                relationshipNotification.NonLocalisedParameters = resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
                break;
              case 3:
                relationshipNotification.followerInfoSnapshotA = resolver.GetFormatterWithVerify<FollowerInfoSnapshot>().Deserialize(ref reader, options);
                break;
              case 4:
                relationshipNotification.followerInfoSnapshotB = resolver.GetFormatterWithVerify<FollowerInfoSnapshot>().Deserialize(ref reader, options);
                break;
              case 5:
                relationshipNotification.FollowerAnimationA = resolver.GetFormatterWithVerify<global::NotificationFollower.Animation>().Deserialize(ref reader, options);
                break;
              case 6:
                relationshipNotification.FollowerAnimationB = resolver.GetFormatterWithVerify<global::NotificationFollower.Animation>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return relationshipNotification;
        }
      }

      public class UpgradeTreeNode
      {
        public sealed class TreeTierFormatter : 
          IMessagePackFormatter<Lamb.UI.UpgradeTreeNode.TreeTier>,
          IMessagePackFormatter
        {
          public void Serialize(
            ref MessagePackWriter writer,
            Lamb.UI.UpgradeTreeNode.TreeTier value,
            MessagePackSerializerOptions options)
          {
            writer.Write((int) value);
          }

          public Lamb.UI.UpgradeTreeNode.TreeTier Deserialize(
            ref MessagePackReader reader,
            MessagePackSerializerOptions options)
          {
            return (Lamb.UI.UpgradeTreeNode.TreeTier) reader.ReadInt32();
          }
        }
      }

      public class DeathScreen
      {
        public class UIDeathScreenOverlayController
        {
          public sealed class ResultsFormatter : 
            IMessagePackFormatter<Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results>,
            IMessagePackFormatter
          {
            public void Serialize(
              ref MessagePackWriter writer,
              Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results value,
              MessagePackSerializerOptions options)
            {
              writer.Write((int) value);
            }

            public Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results Deserialize(
              ref MessagePackReader reader,
              MessagePackSerializerOptions options)
            {
              return (Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results) reader.ReadInt32();
            }
          }
        }
      }
    }
  }

  public class MMBiomeGeneration
  {
    public class BiomeGenerator
    {
      public sealed class VariableAndConditionFormatter : 
        IMessagePackFormatter<MMBiomeGeneration.BiomeGenerator.VariableAndCondition>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          MMBiomeGeneration.BiomeGenerator.VariableAndCondition value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<global::DataManager.Variables>().Serialize(ref writer, value.Variable, options);
            writer.Write(value.Condition);
          }
        }

        public MMBiomeGeneration.BiomeGenerator.VariableAndCondition Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (MMBiomeGeneration.BiomeGenerator.VariableAndCondition) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          MMBiomeGeneration.BiomeGenerator.VariableAndCondition variableAndCondition = new MMBiomeGeneration.BiomeGenerator.VariableAndCondition();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                variableAndCondition.Variable = resolver.GetFormatterWithVerify<global::DataManager.Variables>().Deserialize(ref reader, options);
                break;
              case 1:
                variableAndCondition.Condition = reader.ReadBoolean();
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return variableAndCondition;
        }
      }

      public sealed class VariableAndCountFormatter : 
        IMessagePackFormatter<MMBiomeGeneration.BiomeGenerator.VariableAndCount>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          MMBiomeGeneration.BiomeGenerator.VariableAndCount value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<global::DataManager.Variables>().Serialize(ref writer, value.Variable, options);
            writer.Write(value.Count);
          }
        }

        public MMBiomeGeneration.BiomeGenerator.VariableAndCount Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (MMBiomeGeneration.BiomeGenerator.VariableAndCount) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          MMBiomeGeneration.BiomeGenerator.VariableAndCount variableAndCount = new MMBiomeGeneration.BiomeGenerator.VariableAndCount();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                variableAndCount.Variable = resolver.GetFormatterWithVerify<global::DataManager.Variables>().Deserialize(ref reader, options);
                break;
              case 1:
                variableAndCount.Count = reader.ReadInt32();
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return variableAndCount;
        }
      }
    }
  }

  public class src
  {
    public class Alerts
    {
      public sealed class CharacterSkinAlertsFormatter : 
        IMessagePackFormatter<CharacterSkinAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          CharacterSkinAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public CharacterSkinAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (CharacterSkinAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          CharacterSkinAlerts characterSkinAlerts = new CharacterSkinAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                characterSkinAlerts._alerts = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
                break;
              case 1:
                characterSkinAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return characterSkinAlerts;
        }
      }

      public sealed class ClothingAlertsFormatter : 
        IMessagePackFormatter<ClothingAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          ClothingAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public ClothingAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (ClothingAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          ClothingAlerts clothingAlerts = new ClothingAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                clothingAlerts._alerts = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
                break;
              case 1:
                clothingAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return clothingAlerts;
        }
      }

      public sealed class ClothingAssignAlertsFormatter : 
        IMessagePackFormatter<ClothingAssignAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          ClothingAssignAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public ClothingAssignAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (ClothingAssignAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          ClothingAssignAlerts clothingAssignAlerts = new ClothingAssignAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                clothingAssignAlerts._alerts = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
                break;
              case 1:
                clothingAssignAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return clothingAssignAlerts;
        }
      }

      public sealed class ClothingCustomiseAlertsFormatter : 
        IMessagePackFormatter<ClothingCustomiseAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          ClothingCustomiseAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public ClothingCustomiseAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (ClothingCustomiseAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          ClothingCustomiseAlerts clothingCustomiseAlerts = new ClothingCustomiseAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                clothingCustomiseAlerts._alerts = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
                break;
              case 1:
                clothingCustomiseAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return clothingCustomiseAlerts;
        }
      }

      public sealed class CurseAlertsFormatter : 
        IMessagePackFormatter<CurseAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          CurseAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public CurseAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (CurseAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          CurseAlerts curseAlerts = new CurseAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                curseAlerts._alerts = resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Deserialize(ref reader, options);
                break;
              case 1:
                curseAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return curseAlerts;
        }
      }

      public sealed class FlockadePieceAlertsFormatter : 
        IMessagePackFormatter<FlockadePieceAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          FlockadePieceAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<FlockadePieceType>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<FlockadePieceType>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public FlockadePieceAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (FlockadePieceAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          FlockadePieceAlerts flockadePieceAlerts = new FlockadePieceAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                flockadePieceAlerts._alerts = resolver.GetFormatterWithVerify<List<FlockadePieceType>>().Deserialize(ref reader, options);
                break;
              case 1:
                flockadePieceAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<FlockadePieceType>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return flockadePieceAlerts;
        }
      }

      public sealed class InventoryAlertsFormatter : 
        IMessagePackFormatter<InventoryAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          InventoryAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public InventoryAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (InventoryAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          InventoryAlerts inventoryAlerts = new InventoryAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                inventoryAlerts._alerts = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
                break;
              case 1:
                inventoryAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return inventoryAlerts;
        }
      }

      public sealed class LocationAlertsFormatter : 
        IMessagePackFormatter<LocationAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          LocationAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public LocationAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (LocationAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          LocationAlerts locationAlerts = new LocationAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                locationAlerts._alerts = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
                break;
              case 1:
                locationAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return locationAlerts;
        }
      }

      public sealed class LoreAlertsFormatter : 
        IMessagePackFormatter<LoreAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          LoreAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public LoreAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (LoreAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          LoreAlerts loreAlerts = new LoreAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                loreAlerts._alerts = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
                break;
              case 1:
                loreAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return loreAlerts;
        }
      }

      public sealed class PhotoGalleryAlertsFormatter : 
        IMessagePackFormatter<PhotoGalleryAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          PhotoGalleryAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public PhotoGalleryAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (PhotoGalleryAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          PhotoGalleryAlerts photoGalleryAlerts = new PhotoGalleryAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                photoGalleryAlerts._alerts = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
                break;
              case 1:
                photoGalleryAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return photoGalleryAlerts;
        }
      }

      public sealed class RecipeAlertsFormatter : 
        IMessagePackFormatter<RecipeAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          RecipeAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public RecipeAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (RecipeAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          RecipeAlerts recipeAlerts = new RecipeAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                recipeAlerts._alerts = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
                break;
              case 1:
                recipeAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return recipeAlerts;
        }
      }

      public sealed class RelicAlertsFormatter : 
        IMessagePackFormatter<RelicAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          RelicAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<RelicType>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<RelicType>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public RelicAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (RelicAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          RelicAlerts relicAlerts = new RelicAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                relicAlerts._alerts = resolver.GetFormatterWithVerify<List<RelicType>>().Deserialize(ref reader, options);
                break;
              case 1:
                relicAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<RelicType>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return relicAlerts;
        }
      }

      public sealed class RunTarotCardAlertsFormatter : 
        IMessagePackFormatter<RunTarotCardAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          RunTarotCardAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public RunTarotCardAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (RunTarotCardAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          RunTarotCardAlerts runTarotCardAlerts = new RunTarotCardAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                runTarotCardAlerts._alerts = resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Deserialize(ref reader, options);
                break;
              case 1:
                runTarotCardAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return runTarotCardAlerts;
        }
      }

      public sealed class TarotCardAlertsFormatter : 
        IMessagePackFormatter<TarotCardAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          TarotCardAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public TarotCardAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (TarotCardAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          TarotCardAlerts tarotCardAlerts = new TarotCardAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                tarotCardAlerts._alerts = resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Deserialize(ref reader, options);
                break;
              case 1:
                tarotCardAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::TarotCards.Card>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return tarotCardAlerts;
        }
      }

      public sealed class TraitManipulatorAlertsFormatter : 
        IMessagePackFormatter<TraitManipulatorAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          TraitManipulatorAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<global::UITraitManipulatorMenuController.Type>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<global::UITraitManipulatorMenuController.Type>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public TraitManipulatorAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (TraitManipulatorAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          TraitManipulatorAlerts manipulatorAlerts = new TraitManipulatorAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                manipulatorAlerts._alerts = resolver.GetFormatterWithVerify<List<global::UITraitManipulatorMenuController.Type>>().Deserialize(ref reader, options);
                break;
              case 1:
                manipulatorAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::UITraitManipulatorMenuController.Type>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return manipulatorAlerts;
        }
      }

      public sealed class TutorialAlertsFormatter : 
        IMessagePackFormatter<TutorialAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          TutorialAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<TutorialTopic>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<TutorialTopic>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public TutorialAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (TutorialAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          TutorialAlerts tutorialAlerts = new TutorialAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                tutorialAlerts._alerts = resolver.GetFormatterWithVerify<List<TutorialTopic>>().Deserialize(ref reader, options);
                break;
              case 1:
                tutorialAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<TutorialTopic>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return tutorialAlerts;
        }
      }

      public sealed class UpgradeAlertsFormatter : 
        IMessagePackFormatter<UpgradeAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          UpgradeAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<global::UpgradeSystem.Type>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<global::UpgradeSystem.Type>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public UpgradeAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (UpgradeAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          UpgradeAlerts upgradeAlerts = new UpgradeAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                upgradeAlerts._alerts = resolver.GetFormatterWithVerify<List<global::UpgradeSystem.Type>>().Deserialize(ref reader, options);
                break;
              case 1:
                upgradeAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<global::UpgradeSystem.Type>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return upgradeAlerts;
        }
      }

      public sealed class WeaponAlertsFormatter : 
        IMessagePackFormatter<WeaponAlerts>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          WeaponAlerts value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            IFormatterResolver resolver = options.Resolver;
            writer.WriteArrayHeader(2);
            resolver.GetFormatterWithVerify<List<EquipmentType>>().Serialize(ref writer, value._alerts, options);
            resolver.GetFormatterWithVerify<List<EquipmentType>>().Serialize(ref writer, value._singleAlerts, options);
          }
        }

        public WeaponAlerts Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (WeaponAlerts) null;
          options.Security.DepthStep(ref reader);
          IFormatterResolver resolver = options.Resolver;
          int num = reader.ReadArrayHeader();
          WeaponAlerts weaponAlerts = new WeaponAlerts();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 0:
                weaponAlerts._alerts = resolver.GetFormatterWithVerify<List<EquipmentType>>().Deserialize(ref reader, options);
                break;
              case 1:
                weaponAlerts._singleAlerts = resolver.GetFormatterWithVerify<List<EquipmentType>>().Deserialize(ref reader, options);
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return weaponAlerts;
        }
      }
    }

    public class Data
    {
      public sealed class TwitchSettingsFormatter : 
        IMessagePackFormatter<TwitchSettings>,
        IMessagePackFormatter
      {
        public void Serialize(
          ref MessagePackWriter writer,
          TwitchSettings value,
          MessagePackSerializerOptions options)
        {
          if (value == null)
          {
            writer.WriteNil();
          }
          else
          {
            writer.WriteArrayHeader(6);
            writer.WriteNil();
            writer.Write(value.HelpHinderEnabled);
            writer.Write(value.HelpHinderFrequency);
            writer.Write(value.TotemEnabled);
            writer.Write(value.FollowerNamesEnabled);
            writer.Write(value.TwitchMessagesEnabled);
          }
        }

        public TwitchSettings Deserialize(
          ref MessagePackReader reader,
          MessagePackSerializerOptions options)
        {
          if (reader.TryReadNil())
            return (TwitchSettings) null;
          options.Security.DepthStep(ref reader);
          int num = reader.ReadArrayHeader();
          TwitchSettings twitchSettings = new TwitchSettings();
          for (int index = 0; index < num; ++index)
          {
            switch (index)
            {
              case 1:
                twitchSettings.HelpHinderEnabled = reader.ReadBoolean();
                break;
              case 2:
                twitchSettings.HelpHinderFrequency = reader.ReadSingle();
                break;
              case 3:
                twitchSettings.TotemEnabled = reader.ReadBoolean();
                break;
              case 4:
                twitchSettings.FollowerNamesEnabled = reader.ReadBoolean();
                break;
              case 5:
                twitchSettings.TwitchMessagesEnabled = reader.ReadBoolean();
                break;
              default:
                reader.Skip();
                break;
            }
          }
          --reader.Depth;
          return twitchSettings;
        }
      }
    }
  }

  public sealed class DayPhaseFormatter : IMessagePackFormatter<DayPhase>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      DayPhase value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public DayPhase Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return (DayPhase) reader.ReadInt32();
    }
  }

  public sealed class EnemyFormatter : IMessagePackFormatter<Enemy>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      Enemy value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public Enemy Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return (Enemy) reader.ReadInt32();
    }
  }

  public sealed class EquipmentTypeFormatter : 
    IMessagePackFormatter<EquipmentType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      EquipmentType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public EquipmentType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (EquipmentType) reader.ReadInt32();
    }
  }

  public sealed class FollowerClothingTypeFormatter : 
    IMessagePackFormatter<FollowerClothingType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerClothingType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerClothingType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerClothingType) reader.ReadInt32();
    }
  }

  public sealed class FollowerCommandsFormatter : 
    IMessagePackFormatter<FollowerCommands>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerCommands value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerCommands Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerCommands) reader.ReadInt32();
    }
  }

  public sealed class FollowerCustomisationTypeFormatter : 
    IMessagePackFormatter<FollowerCustomisationType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerCustomisationType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerCustomisationType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerCustomisationType) reader.ReadInt32();
    }
  }

  public sealed class FollowerFactionFormatter : 
    IMessagePackFormatter<FollowerFaction>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerFaction value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerFaction Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerFaction) reader.ReadInt32();
    }
  }

  public sealed class FollowerHatTypeFormatter : 
    IMessagePackFormatter<FollowerHatType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerHatType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerHatType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerHatType) reader.ReadInt32();
    }
  }

  public sealed class FollowerLocationFormatter : 
    IMessagePackFormatter<FollowerLocation>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerLocation value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerLocation Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerLocation) reader.ReadInt32();
    }
  }

  public sealed class FollowerOutfitTypeFormatter : 
    IMessagePackFormatter<FollowerOutfitType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerOutfitType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerOutfitType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerOutfitType) reader.ReadInt32();
    }
  }

  public sealed class FollowerRoleFormatter : 
    IMessagePackFormatter<FollowerRole>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerRole value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerRole Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerRole) reader.ReadInt32();
    }
  }

  public sealed class FollowerSpecialTypeFormatter : 
    IMessagePackFormatter<FollowerSpecialType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerSpecialType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerSpecialType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerSpecialType) reader.ReadInt32();
    }
  }

  public sealed class FollowerTaskTypeFormatter : 
    IMessagePackFormatter<FollowerTaskType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerTaskType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public FollowerTaskType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (FollowerTaskType) reader.ReadInt32();
    }
  }

  public sealed class GateTypeFormatter : IMessagePackFormatter<GateType>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      GateType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public GateType Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return (GateType) reader.ReadInt32();
    }
  }

  public sealed class RelicTypeFormatter : IMessagePackFormatter<RelicType>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      RelicType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public RelicType Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return (RelicType) reader.ReadInt32();
    }
  }

  public sealed class ResurrectionTypeFormatter : 
    IMessagePackFormatter<ResurrectionType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      ResurrectionType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public ResurrectionType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (ResurrectionType) reader.ReadInt32();
    }
  }

  public sealed class SeasonalEventTypeFormatter : 
    IMessagePackFormatter<SeasonalEventType>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      SeasonalEventType value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public SeasonalEventType Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (SeasonalEventType) reader.ReadInt32();
    }
  }

  public sealed class SermonCategoryFormatter : 
    IMessagePackFormatter<SermonCategory>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      SermonCategory value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public SermonCategory Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (SermonCategory) reader.ReadInt32();
    }
  }

  public sealed class ThoughtFormatter : IMessagePackFormatter<Thought>, IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      Thought value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public Thought Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return (Thought) reader.ReadInt32();
    }
  }

  public sealed class TutorialTopicFormatter : 
    IMessagePackFormatter<TutorialTopic>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      TutorialTopic value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public TutorialTopic Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (TutorialTopic) reader.ReadInt32();
    }
  }

  public sealed class WorkerPriorityFormatter : 
    IMessagePackFormatter<WorkerPriority>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      WorkerPriority value,
      MessagePackSerializerOptions options)
    {
      writer.Write((int) value);
    }

    public WorkerPriority Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      return (WorkerPriority) reader.ReadInt32();
    }
  }

  public class BluePrint
  {
    public sealed class BluePrintTypeFormatter : 
      IMessagePackFormatter<global::BluePrint.BluePrintType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::BluePrint.BluePrintType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::BluePrint.BluePrintType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::BluePrint.BluePrintType) reader.ReadInt32();
      }
    }
  }

  public class CrownAbilities
  {
    public sealed class TYPEFormatter : 
      IMessagePackFormatter<global::CrownAbilities.TYPE>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::CrownAbilities.TYPE value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::CrownAbilities.TYPE Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::CrownAbilities.TYPE) reader.ReadInt32();
      }
    }
  }

  public class DoctrineUpgradeSystem
  {
    public sealed class DoctrineTypeFormatter : 
      IMessagePackFormatter<global::DoctrineUpgradeSystem.DoctrineType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::DoctrineUpgradeSystem.DoctrineType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::DoctrineUpgradeSystem.DoctrineType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::DoctrineUpgradeSystem.DoctrineType) reader.ReadInt32();
      }
    }
  }

  public class EnemyModifier
  {
    public sealed class ModifierTypeFormatter : 
      IMessagePackFormatter<global::EnemyModifier.ModifierType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::EnemyModifier.ModifierType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::EnemyModifier.ModifierType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::EnemyModifier.ModifierType) reader.ReadInt32();
      }
    }
  }

  public class FollowerTrait
  {
    public sealed class TraitTypeFormatter : 
      IMessagePackFormatter<global::FollowerTrait.TraitType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::FollowerTrait.TraitType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::FollowerTrait.TraitType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::FollowerTrait.TraitType) reader.ReadInt32();
      }
    }
  }

  public class IDAndRelationship
  {
    public sealed class RelationshipStateFormatter : 
      IMessagePackFormatter<global::IDAndRelationship.RelationshipState>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::IDAndRelationship.RelationshipState value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::IDAndRelationship.RelationshipState Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::IDAndRelationship.RelationshipState) reader.ReadInt32();
      }
    }
  }

  public class InventoryItem
  {
    public sealed class ITEM_TYPEFormatter : 
      IMessagePackFormatter<global::InventoryItem.ITEM_TYPE>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::InventoryItem.ITEM_TYPE value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::InventoryItem.ITEM_TYPE Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::InventoryItem.ITEM_TYPE) reader.ReadInt32();
      }
    }
  }

  public class InventoryWeapon
  {
    public sealed class ITEM_TYPEFormatter : 
      IMessagePackFormatter<global::InventoryWeapon.ITEM_TYPE>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::InventoryWeapon.ITEM_TYPE value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::InventoryWeapon.ITEM_TYPE Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::InventoryWeapon.ITEM_TYPE) reader.ReadInt32();
      }
    }
  }

  public class NotificationFollower
  {
    public sealed class AnimationFormatter : 
      IMessagePackFormatter<global::NotificationFollower.Animation>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::NotificationFollower.Animation value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::NotificationFollower.Animation Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::NotificationFollower.Animation) reader.ReadInt32();
      }
    }
  }

  public class Objectives
  {
    public sealed class CustomQuestTypesFormatter : 
      IMessagePackFormatter<global::Objectives.CustomQuestTypes>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives.CustomQuestTypes value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::Objectives.CustomQuestTypes Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::Objectives.CustomQuestTypes) reader.ReadInt32();
      }
    }

    public sealed class TIMER_TYPEFormatter : 
      IMessagePackFormatter<global::Objectives.TIMER_TYPE>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives.TIMER_TYPE value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::Objectives.TIMER_TYPE Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::Objectives.TIMER_TYPE) reader.ReadInt32();
      }
    }

    public sealed class TYPESFormatter : 
      IMessagePackFormatter<global::Objectives.TYPES>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Objectives.TYPES value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::Objectives.TYPES Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::Objectives.TYPES) reader.ReadInt32();
      }
    }
  }

  public class PlayerFleeceManager
  {
    public sealed class FleeceTypeFormatter : 
      IMessagePackFormatter<global::PlayerFleeceManager.FleeceType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::PlayerFleeceManager.FleeceType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::PlayerFleeceManager.FleeceType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::PlayerFleeceManager.FleeceType) reader.ReadInt32();
      }
    }
  }

  public class SeasonsManager
  {
    public sealed class SeasonFormatter : 
      IMessagePackFormatter<global::SeasonsManager.Season>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::SeasonsManager.Season value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::SeasonsManager.Season Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::SeasonsManager.Season) reader.ReadInt32();
      }
    }

    public sealed class WeatherEventFormatter : 
      IMessagePackFormatter<global::SeasonsManager.WeatherEvent>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::SeasonsManager.WeatherEvent value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::SeasonsManager.WeatherEvent Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::SeasonsManager.WeatherEvent) reader.ReadInt32();
      }
    }
  }

  public class SermonsAndRituals
  {
    public sealed class SermonRitualTypeFormatter : 
      IMessagePackFormatter<global::SermonsAndRituals.SermonRitualType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::SermonsAndRituals.SermonRitualType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::SermonsAndRituals.SermonRitualType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::SermonsAndRituals.SermonRitualType) reader.ReadInt32();
      }
    }
  }

  public class Shrines
  {
    public sealed class ShrineTypeFormatter : 
      IMessagePackFormatter<global::Shrines.ShrineType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Shrines.ShrineType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::Shrines.ShrineType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::Shrines.ShrineType) reader.ReadInt32();
      }
    }
  }

  public class StructureAndTime
  {
    public sealed class IDTypesFormatter : 
      IMessagePackFormatter<global::StructureAndTime.IDTypes>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructureAndTime.IDTypes value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::StructureAndTime.IDTypes Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::StructureAndTime.IDTypes) reader.ReadInt32();
      }
    }
  }

  public class StructureBrain
  {
    public sealed class CategoriesFormatter : 
      IMessagePackFormatter<global::StructureBrain.Categories>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructureBrain.Categories value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::StructureBrain.Categories Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::StructureBrain.Categories) reader.ReadInt32();
      }
    }

    public sealed class TYPESFormatter : 
      IMessagePackFormatter<global::StructureBrain.TYPES>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructureBrain.TYPES value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::StructureBrain.TYPES Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::StructureBrain.TYPES) reader.ReadInt32();
      }
    }
  }

  public class StructureEffectManager
  {
    public sealed class EffectTypeFormatter : 
      IMessagePackFormatter<global::StructureEffectManager.EffectType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::StructureEffectManager.EffectType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::StructureEffectManager.EffectType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::StructureEffectManager.EffectType) reader.ReadInt32();
      }
    }
  }

  public class UITraitManipulatorMenuController
  {
    public sealed class TypeFormatter : 
      IMessagePackFormatter<global::UITraitManipulatorMenuController.Type>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::UITraitManipulatorMenuController.Type value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::UITraitManipulatorMenuController.Type Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::UITraitManipulatorMenuController.Type) reader.ReadInt32();
      }
    }
  }

  public class UnlockManager
  {
    public sealed class UnlockTypeFormatter : 
      IMessagePackFormatter<global::UnlockManager.UnlockType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::UnlockManager.UnlockType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::UnlockManager.UnlockType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::UnlockManager.UnlockType) reader.ReadInt32();
      }
    }
  }

  public class Villager_Info
  {
    public sealed class FactionFormatter : 
      IMessagePackFormatter<global::Villager_Info.Faction>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::Villager_Info.Faction value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::Villager_Info.Faction Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::Villager_Info.Faction) reader.ReadInt32();
      }
    }
  }

  public class WeaponUpgradeSystem
  {
    public sealed class WeaponTypeFormatter : 
      IMessagePackFormatter<global::WeaponUpgradeSystem.WeaponType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::WeaponUpgradeSystem.WeaponType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::WeaponUpgradeSystem.WeaponType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::WeaponUpgradeSystem.WeaponType) reader.ReadInt32();
      }
    }

    public sealed class WeaponUpgradeTypeFormatter : 
      IMessagePackFormatter<global::WeaponUpgradeSystem.WeaponUpgradeType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::WeaponUpgradeSystem.WeaponUpgradeType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::WeaponUpgradeSystem.WeaponUpgradeType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::WeaponUpgradeSystem.WeaponUpgradeType) reader.ReadInt32();
      }
    }
  }

  public class WeatherSystemController
  {
    public sealed class WeatherStrengthFormatter : 
      IMessagePackFormatter<global::WeatherSystemController.WeatherStrength>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::WeatherSystemController.WeatherStrength value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::WeatherSystemController.WeatherStrength Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::WeatherSystemController.WeatherStrength) reader.ReadInt32();
      }
    }

    public sealed class WeatherTypeFormatter : 
      IMessagePackFormatter<global::WeatherSystemController.WeatherType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::WeatherSystemController.WeatherType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::WeatherSystemController.WeatherType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::WeatherSystemController.WeatherType) reader.ReadInt32();
      }
    }
  }

  public class WorshipperInfoManager
  {
    public sealed class OutfitFormatter : 
      IMessagePackFormatter<global::WorshipperInfoManager.Outfit>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        global::WorshipperInfoManager.Outfit value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public global::WorshipperInfoManager.Outfit Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (global::WorshipperInfoManager.Outfit) reader.ReadInt32();
      }
    }
  }

  public class Flockade
  {
    public sealed class FlockadePieceTypeFormatter : 
      IMessagePackFormatter<FlockadePieceType>,
      IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        FlockadePieceType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public FlockadePieceType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (FlockadePieceType) reader.ReadInt32();
      }
    }
  }

  public class Map
  {
    public sealed class NodeTypeFormatter : IMessagePackFormatter<Map.NodeType>, IMessagePackFormatter
    {
      public void Serialize(
        ref MessagePackWriter writer,
        Map.NodeType value,
        MessagePackSerializerOptions options)
      {
        writer.Write((int) value);
      }

      public Map.NodeType Deserialize(
        ref MessagePackReader reader,
        MessagePackSerializerOptions options)
      {
        return (Map.NodeType) reader.ReadInt32();
      }
    }
  }

  public sealed class ObjectivesDataFormatter : 
    IMessagePackFormatter<ObjectivesData>,
    IMessagePackFormatter
  {
    public Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap;
    public Dictionary<int, int> keyToJumpMap;

    public ObjectivesDataFormatter()
    {
      this.typeToKeyAndJumpMap = new Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>(43, RuntimeTypeHandleEqualityComparer.Default)
      {
        {
          typeof (global::Objective_FindRelic).TypeHandle,
          new KeyValuePair<int, int>(0, 0)
        },
        {
          typeof (global::Objectives_AssignClothing).TypeHandle,
          new KeyValuePair<int, int>(1, 1)
        },
        {
          typeof (global::Objectives_BedRest).TypeHandle,
          new KeyValuePair<int, int>(2, 2)
        },
        {
          typeof (global::Objectives_BlizzardOffering).TypeHandle,
          new KeyValuePair<int, int>(3, 3)
        },
        {
          typeof (global::Objectives_BuildStructure).TypeHandle,
          new KeyValuePair<int, int>(4, 4)
        },
        {
          typeof (global::Objectives_CollectItem).TypeHandle,
          new KeyValuePair<int, int>(5, 5)
        },
        {
          typeof (global::Objectives_CookMeal).TypeHandle,
          new KeyValuePair<int, int>(6, 6)
        },
        {
          typeof (global::Objectives_CraftClothing).TypeHandle,
          new KeyValuePair<int, int>(7, 7)
        },
        {
          typeof (global::Objectives_Custom).TypeHandle,
          new KeyValuePair<int, int>(8, 8)
        },
        {
          typeof (global::Objectives_DefeatKnucklebones).TypeHandle,
          new KeyValuePair<int, int>(9, 9)
        },
        {
          typeof (global::Objectives_DepositFood).TypeHandle,
          new KeyValuePair<int, int>(10, 10)
        },
        {
          typeof (global::Objectives_Drink).TypeHandle,
          new KeyValuePair<int, int>(11, 11)
        },
        {
          typeof (global::Objectives_EatMeal).TypeHandle,
          new KeyValuePair<int, int>(12, 12)
        },
        {
          typeof (global::Objectives_FindChildren).TypeHandle,
          new KeyValuePair<int, int>(13, 13)
        },
        {
          typeof (global::Objectives_FindFollower).TypeHandle,
          new KeyValuePair<int, int>(14, 14)
        },
        {
          typeof (global::Objectives_FinishRace).TypeHandle,
          new KeyValuePair<int, int>(15, 15)
        },
        {
          typeof (global::Objectives_FlowerBaskets).TypeHandle,
          new KeyValuePair<int, int>(16 /*0x10*/, 16 /*0x10*/)
        },
        {
          typeof (global::Objectives_GetAnimal).TypeHandle,
          new KeyValuePair<int, int>(17, 17)
        },
        {
          typeof (global::Objectives_GiveItem).TypeHandle,
          new KeyValuePair<int, int>(18, 18)
        },
        {
          typeof (global::Objectives_KillEnemies).TypeHandle,
          new KeyValuePair<int, int>(19, 19)
        },
        {
          typeof (global::Objectives_LegendaryWeaponRun).TypeHandle,
          new KeyValuePair<int, int>(20, 20)
        },
        {
          typeof (global::Objectives_Mating).TypeHandle,
          new KeyValuePair<int, int>(21, 21)
        },
        {
          typeof (global::Objectives_PerformRitual).TypeHandle,
          new KeyValuePair<int, int>(22, 22)
        },
        {
          typeof (global::Objectives_PlaceStructure).TypeHandle,
          new KeyValuePair<int, int>(23, 23)
        },
        {
          typeof (global::Objectives_RecruitCursedFollower).TypeHandle,
          new KeyValuePair<int, int>(24, 24)
        },
        {
          typeof (global::Objectives_RecruitFollower).TypeHandle,
          new KeyValuePair<int, int>(25, 25)
        },
        {
          typeof (global::Objectives_RemoveStructure).TypeHandle,
          new KeyValuePair<int, int>(26, 26)
        },
        {
          typeof (global::Objectives_ShootDummy).TypeHandle,
          new KeyValuePair<int, int>(27, 27)
        },
        {
          typeof (global::Objectives_ShowFleece).TypeHandle,
          new KeyValuePair<int, int>(28, 28)
        },
        {
          typeof (global::Objectives_Story).TypeHandle,
          new KeyValuePair<int, int>(29, 29)
        },
        {
          typeof (global::Objectives_TalkToFollower).TypeHandle,
          new KeyValuePair<int, int>(30, 30)
        },
        {
          typeof (global::Objectives_UnlockUpgrade).TypeHandle,
          new KeyValuePair<int, int>(31 /*0x1F*/, 31 /*0x1F*/)
        },
        {
          typeof (global::Objectives_UseRelic).TypeHandle,
          new KeyValuePair<int, int>(32 /*0x20*/, 32 /*0x20*/)
        },
        {
          typeof (global::Objectives_WinFlockadeBet).TypeHandle,
          new KeyValuePair<int, int>(33, 33)
        },
        {
          typeof (Objectives_RoomChallenge).TypeHandle,
          new KeyValuePair<int, int>(34, 34)
        },
        {
          typeof (global::Objectives_NoDodge).TypeHandle,
          new KeyValuePair<int, int>(35, 35)
        },
        {
          typeof (global::Objectives_NoDamage).TypeHandle,
          new KeyValuePair<int, int>(36, 36)
        },
        {
          typeof (global::Objectives_NoCurses).TypeHandle,
          new KeyValuePair<int, int>(37, 37)
        },
        {
          typeof (global::Objectives_NoHealing).TypeHandle,
          new KeyValuePair<int, int>(38, 38)
        },
        {
          typeof (global::Objectives_BuildWinterDecorations).TypeHandle,
          new KeyValuePair<int, int>(39, 39)
        },
        {
          typeof (global::Objectives_FeedAnimal).TypeHandle,
          new KeyValuePair<int, int>(40, 40)
        },
        {
          typeof (global::Objectives_WalkAnimal).TypeHandle,
          new KeyValuePair<int, int>(41, 41)
        },
        {
          typeof (global::Objectives_LegendarySwordReturn).TypeHandle,
          new KeyValuePair<int, int>(42, 42)
        }
      };
      this.keyToJumpMap = new Dictionary<int, int>(43)
      {
        {
          0,
          0
        },
        {
          1,
          1
        },
        {
          2,
          2
        },
        {
          3,
          3
        },
        {
          4,
          4
        },
        {
          5,
          5
        },
        {
          6,
          6
        },
        {
          7,
          7
        },
        {
          8,
          8
        },
        {
          9,
          9
        },
        {
          10,
          10
        },
        {
          11,
          11
        },
        {
          12,
          12
        },
        {
          13,
          13
        },
        {
          14,
          14
        },
        {
          15,
          15
        },
        {
          16 /*0x10*/,
          16 /*0x10*/
        },
        {
          17,
          17
        },
        {
          18,
          18
        },
        {
          19,
          19
        },
        {
          20,
          20
        },
        {
          21,
          21
        },
        {
          22,
          22
        },
        {
          23,
          23
        },
        {
          24,
          24
        },
        {
          25,
          25
        },
        {
          26,
          26
        },
        {
          27,
          27
        },
        {
          28,
          28
        },
        {
          29,
          29
        },
        {
          30,
          30
        },
        {
          31 /*0x1F*/,
          31 /*0x1F*/
        },
        {
          32 /*0x20*/,
          32 /*0x20*/
        },
        {
          33,
          33
        },
        {
          34,
          34
        },
        {
          35,
          35
        },
        {
          36,
          36
        },
        {
          37,
          37
        },
        {
          38,
          38
        },
        {
          39,
          39
        },
        {
          40,
          40
        },
        {
          41,
          41
        },
        {
          42,
          42
        }
      };
    }

    public void Serialize(
      ref MessagePackWriter writer,
      ObjectivesData value,
      MessagePackSerializerOptions options)
    {
      KeyValuePair<int, int> keyValuePair;
      if (value != null && this.typeToKeyAndJumpMap.TryGetValue(value.GetType().TypeHandle, out keyValuePair))
      {
        writer.WriteArrayHeader(2);
        writer.WriteInt32(keyValuePair.Key);
        switch (keyValuePair.Value)
        {
          case 0:
            options.Resolver.GetFormatterWithVerify<global::Objective_FindRelic>().Serialize(ref writer, (global::Objective_FindRelic) value, options);
            break;
          case 1:
            options.Resolver.GetFormatterWithVerify<global::Objectives_AssignClothing>().Serialize(ref writer, (global::Objectives_AssignClothing) value, options);
            break;
          case 2:
            options.Resolver.GetFormatterWithVerify<global::Objectives_BedRest>().Serialize(ref writer, (global::Objectives_BedRest) value, options);
            break;
          case 3:
            options.Resolver.GetFormatterWithVerify<global::Objectives_BlizzardOffering>().Serialize(ref writer, (global::Objectives_BlizzardOffering) value, options);
            break;
          case 4:
            options.Resolver.GetFormatterWithVerify<global::Objectives_BuildStructure>().Serialize(ref writer, (global::Objectives_BuildStructure) value, options);
            break;
          case 5:
            options.Resolver.GetFormatterWithVerify<global::Objectives_CollectItem>().Serialize(ref writer, (global::Objectives_CollectItem) value, options);
            break;
          case 6:
            options.Resolver.GetFormatterWithVerify<global::Objectives_CookMeal>().Serialize(ref writer, (global::Objectives_CookMeal) value, options);
            break;
          case 7:
            options.Resolver.GetFormatterWithVerify<global::Objectives_CraftClothing>().Serialize(ref writer, (global::Objectives_CraftClothing) value, options);
            break;
          case 8:
            options.Resolver.GetFormatterWithVerify<global::Objectives_Custom>().Serialize(ref writer, (global::Objectives_Custom) value, options);
            break;
          case 9:
            options.Resolver.GetFormatterWithVerify<global::Objectives_DefeatKnucklebones>().Serialize(ref writer, (global::Objectives_DefeatKnucklebones) value, options);
            break;
          case 10:
            options.Resolver.GetFormatterWithVerify<global::Objectives_DepositFood>().Serialize(ref writer, (global::Objectives_DepositFood) value, options);
            break;
          case 11:
            options.Resolver.GetFormatterWithVerify<global::Objectives_Drink>().Serialize(ref writer, (global::Objectives_Drink) value, options);
            break;
          case 12:
            options.Resolver.GetFormatterWithVerify<global::Objectives_EatMeal>().Serialize(ref writer, (global::Objectives_EatMeal) value, options);
            break;
          case 13:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FindChildren>().Serialize(ref writer, (global::Objectives_FindChildren) value, options);
            break;
          case 14:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FindFollower>().Serialize(ref writer, (global::Objectives_FindFollower) value, options);
            break;
          case 15:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FinishRace>().Serialize(ref writer, (global::Objectives_FinishRace) value, options);
            break;
          case 16 /*0x10*/:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FlowerBaskets>().Serialize(ref writer, (global::Objectives_FlowerBaskets) value, options);
            break;
          case 17:
            options.Resolver.GetFormatterWithVerify<global::Objectives_GetAnimal>().Serialize(ref writer, (global::Objectives_GetAnimal) value, options);
            break;
          case 18:
            options.Resolver.GetFormatterWithVerify<global::Objectives_GiveItem>().Serialize(ref writer, (global::Objectives_GiveItem) value, options);
            break;
          case 19:
            options.Resolver.GetFormatterWithVerify<global::Objectives_KillEnemies>().Serialize(ref writer, (global::Objectives_KillEnemies) value, options);
            break;
          case 20:
            options.Resolver.GetFormatterWithVerify<global::Objectives_LegendaryWeaponRun>().Serialize(ref writer, (global::Objectives_LegendaryWeaponRun) value, options);
            break;
          case 21:
            options.Resolver.GetFormatterWithVerify<global::Objectives_Mating>().Serialize(ref writer, (global::Objectives_Mating) value, options);
            break;
          case 22:
            options.Resolver.GetFormatterWithVerify<global::Objectives_PerformRitual>().Serialize(ref writer, (global::Objectives_PerformRitual) value, options);
            break;
          case 23:
            options.Resolver.GetFormatterWithVerify<global::Objectives_PlaceStructure>().Serialize(ref writer, (global::Objectives_PlaceStructure) value, options);
            break;
          case 24:
            options.Resolver.GetFormatterWithVerify<global::Objectives_RecruitCursedFollower>().Serialize(ref writer, (global::Objectives_RecruitCursedFollower) value, options);
            break;
          case 25:
            options.Resolver.GetFormatterWithVerify<global::Objectives_RecruitFollower>().Serialize(ref writer, (global::Objectives_RecruitFollower) value, options);
            break;
          case 26:
            options.Resolver.GetFormatterWithVerify<global::Objectives_RemoveStructure>().Serialize(ref writer, (global::Objectives_RemoveStructure) value, options);
            break;
          case 27:
            options.Resolver.GetFormatterWithVerify<global::Objectives_ShootDummy>().Serialize(ref writer, (global::Objectives_ShootDummy) value, options);
            break;
          case 28:
            options.Resolver.GetFormatterWithVerify<global::Objectives_ShowFleece>().Serialize(ref writer, (global::Objectives_ShowFleece) value, options);
            break;
          case 29:
            options.Resolver.GetFormatterWithVerify<global::Objectives_Story>().Serialize(ref writer, (global::Objectives_Story) value, options);
            break;
          case 30:
            options.Resolver.GetFormatterWithVerify<global::Objectives_TalkToFollower>().Serialize(ref writer, (global::Objectives_TalkToFollower) value, options);
            break;
          case 31 /*0x1F*/:
            options.Resolver.GetFormatterWithVerify<global::Objectives_UnlockUpgrade>().Serialize(ref writer, (global::Objectives_UnlockUpgrade) value, options);
            break;
          case 32 /*0x20*/:
            options.Resolver.GetFormatterWithVerify<global::Objectives_UseRelic>().Serialize(ref writer, (global::Objectives_UseRelic) value, options);
            break;
          case 33:
            options.Resolver.GetFormatterWithVerify<global::Objectives_WinFlockadeBet>().Serialize(ref writer, (global::Objectives_WinFlockadeBet) value, options);
            break;
          case 34:
            options.Resolver.GetFormatterWithVerify<Objectives_RoomChallenge>().Serialize(ref writer, (Objectives_RoomChallenge) value, options);
            break;
          case 35:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoDodge>().Serialize(ref writer, (global::Objectives_NoDodge) value, options);
            break;
          case 36:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoDamage>().Serialize(ref writer, (global::Objectives_NoDamage) value, options);
            break;
          case 37:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoCurses>().Serialize(ref writer, (global::Objectives_NoCurses) value, options);
            break;
          case 38:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoHealing>().Serialize(ref writer, (global::Objectives_NoHealing) value, options);
            break;
          case 39:
            options.Resolver.GetFormatterWithVerify<global::Objectives_BuildWinterDecorations>().Serialize(ref writer, (global::Objectives_BuildWinterDecorations) value, options);
            break;
          case 40:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FeedAnimal>().Serialize(ref writer, (global::Objectives_FeedAnimal) value, options);
            break;
          case 41:
            options.Resolver.GetFormatterWithVerify<global::Objectives_WalkAnimal>().Serialize(ref writer, (global::Objectives_WalkAnimal) value, options);
            break;
          case 42:
            options.Resolver.GetFormatterWithVerify<global::Objectives_LegendarySwordReturn>().Serialize(ref writer, (global::Objectives_LegendarySwordReturn) value, options);
            break;
        }
      }
      else
        writer.WriteNil();
    }

    public ObjectivesData Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (ObjectivesData) null;
      if (reader.ReadArrayHeader() != 2)
        throw new InvalidOperationException("Invalid Union data was detected. Type:global::ObjectivesData");
      options.Security.DepthStep(ref reader);
      int key = reader.ReadInt32();
      if (!this.keyToJumpMap.TryGetValue(key, out key))
        key = -1;
      ObjectivesData objectivesData = (ObjectivesData) null;
      switch (key)
      {
        case 0:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objective_FindRelic>().Deserialize(ref reader, options);
          break;
        case 1:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_AssignClothing>().Deserialize(ref reader, options);
          break;
        case 2:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_BedRest>().Deserialize(ref reader, options);
          break;
        case 3:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_BlizzardOffering>().Deserialize(ref reader, options);
          break;
        case 4:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_BuildStructure>().Deserialize(ref reader, options);
          break;
        case 5:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_CollectItem>().Deserialize(ref reader, options);
          break;
        case 6:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_CookMeal>().Deserialize(ref reader, options);
          break;
        case 7:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_CraftClothing>().Deserialize(ref reader, options);
          break;
        case 8:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_Custom>().Deserialize(ref reader, options);
          break;
        case 9:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_DefeatKnucklebones>().Deserialize(ref reader, options);
          break;
        case 10:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_DepositFood>().Deserialize(ref reader, options);
          break;
        case 11:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_Drink>().Deserialize(ref reader, options);
          break;
        case 12:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_EatMeal>().Deserialize(ref reader, options);
          break;
        case 13:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_FindChildren>().Deserialize(ref reader, options);
          break;
        case 14:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_FindFollower>().Deserialize(ref reader, options);
          break;
        case 15:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_FinishRace>().Deserialize(ref reader, options);
          break;
        case 16 /*0x10*/:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_FlowerBaskets>().Deserialize(ref reader, options);
          break;
        case 17:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_GetAnimal>().Deserialize(ref reader, options);
          break;
        case 18:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_GiveItem>().Deserialize(ref reader, options);
          break;
        case 19:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_KillEnemies>().Deserialize(ref reader, options);
          break;
        case 20:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_LegendaryWeaponRun>().Deserialize(ref reader, options);
          break;
        case 21:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_Mating>().Deserialize(ref reader, options);
          break;
        case 22:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_PerformRitual>().Deserialize(ref reader, options);
          break;
        case 23:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_PlaceStructure>().Deserialize(ref reader, options);
          break;
        case 24:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_RecruitCursedFollower>().Deserialize(ref reader, options);
          break;
        case 25:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_RecruitFollower>().Deserialize(ref reader, options);
          break;
        case 26:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_RemoveStructure>().Deserialize(ref reader, options);
          break;
        case 27:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_ShootDummy>().Deserialize(ref reader, options);
          break;
        case 28:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_ShowFleece>().Deserialize(ref reader, options);
          break;
        case 29:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_Story>().Deserialize(ref reader, options);
          break;
        case 30:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_TalkToFollower>().Deserialize(ref reader, options);
          break;
        case 31 /*0x1F*/:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_UnlockUpgrade>().Deserialize(ref reader, options);
          break;
        case 32 /*0x20*/:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_UseRelic>().Deserialize(ref reader, options);
          break;
        case 33:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_WinFlockadeBet>().Deserialize(ref reader, options);
          break;
        case 34:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<Objectives_RoomChallenge>().Deserialize(ref reader, options);
          break;
        case 35:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_NoDodge>().Deserialize(ref reader, options);
          break;
        case 36:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_NoDamage>().Deserialize(ref reader, options);
          break;
        case 37:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_NoCurses>().Deserialize(ref reader, options);
          break;
        case 38:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_NoHealing>().Deserialize(ref reader, options);
          break;
        case 39:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_BuildWinterDecorations>().Deserialize(ref reader, options);
          break;
        case 40:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_FeedAnimal>().Deserialize(ref reader, options);
          break;
        case 41:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_WalkAnimal>().Deserialize(ref reader, options);
          break;
        case 42:
          objectivesData = (ObjectivesData) options.Resolver.GetFormatterWithVerify<global::Objectives_LegendarySwordReturn>().Deserialize(ref reader, options);
          break;
        default:
          reader.Skip();
          break;
      }
      --reader.Depth;
      return objectivesData;
    }
  }

  public sealed class ObjectivesDataFinalizedFormatter : 
    IMessagePackFormatter<ObjectivesDataFinalized>,
    IMessagePackFormatter
  {
    public Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap;
    public Dictionary<int, int> keyToJumpMap;

    public ObjectivesDataFinalizedFormatter()
    {
      this.typeToKeyAndJumpMap = new Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>(42, RuntimeTypeHandleEqualityComparer.Default)
      {
        {
          typeof (global::Objective_FindRelic.FinalizedData_FindRelic).TypeHandle,
          new KeyValuePair<int, int>(0, 0)
        },
        {
          typeof (global::Objectives_AssignClothing.FinalizedData_AssignClothing).TypeHandle,
          new KeyValuePair<int, int>(1, 1)
        },
        {
          typeof (global::Objectives_BedRest.FinalizedData_BedRest).TypeHandle,
          new KeyValuePair<int, int>(2, 2)
        },
        {
          typeof (global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering).TypeHandle,
          new KeyValuePair<int, int>(3, 3)
        },
        {
          typeof (global::Objectives_BuildStructure.FinalizedData_BuildStructure).TypeHandle,
          new KeyValuePair<int, int>(4, 4)
        },
        {
          typeof (global::Objectives_CollectItem.FinalizedData_CollectItem).TypeHandle,
          new KeyValuePair<int, int>(5, 5)
        },
        {
          typeof (global::Objectives_CookMeal.FinalizedData_CookMeal).TypeHandle,
          new KeyValuePair<int, int>(6, 6)
        },
        {
          typeof (global::Objectives_CraftClothing.FinalizedData_CraftClothing).TypeHandle,
          new KeyValuePair<int, int>(7, 7)
        },
        {
          typeof (global::Objectives_Custom.FinalizedData_Custom).TypeHandle,
          new KeyValuePair<int, int>(8, 8)
        },
        {
          typeof (global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones).TypeHandle,
          new KeyValuePair<int, int>(9, 9)
        },
        {
          typeof (global::Objectives_DepositFood.FinalizedData_DepositFood).TypeHandle,
          new KeyValuePair<int, int>(10, 10)
        },
        {
          typeof (global::Objectives_Drink.FinalizedData_Drink).TypeHandle,
          new KeyValuePair<int, int>(11, 11)
        },
        {
          typeof (global::Objectives_EatMeal.FinalizedData_EatMeal).TypeHandle,
          new KeyValuePair<int, int>(12, 12)
        },
        {
          typeof (global::Objectives_FindChildren.FinalizedData_FindChildren).TypeHandle,
          new KeyValuePair<int, int>(13, 13)
        },
        {
          typeof (global::Objectives_FindFollower.FinalizedData_FindFollower).TypeHandle,
          new KeyValuePair<int, int>(14, 14)
        },
        {
          typeof (global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace).TypeHandle,
          new KeyValuePair<int, int>(15, 15)
        },
        {
          typeof (global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets).TypeHandle,
          new KeyValuePair<int, int>(16 /*0x10*/, 16 /*0x10*/)
        },
        {
          typeof (global::Objectives_GetAnimal.FinalizedData_GetAnimal).TypeHandle,
          new KeyValuePair<int, int>(17, 17)
        },
        {
          typeof (global::Objectives_GiveItem.FinalizedData_GiveItem).TypeHandle,
          new KeyValuePair<int, int>(18, 18)
        },
        {
          typeof (global::Objectives_KillEnemies.FinalizedData_KillEnemies).TypeHandle,
          new KeyValuePair<int, int>(19, 19)
        },
        {
          typeof (global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun).TypeHandle,
          new KeyValuePair<int, int>(20, 20)
        },
        {
          typeof (global::Objectives_Mating.FinalizedData_Mating).TypeHandle,
          new KeyValuePair<int, int>(21, 21)
        },
        {
          typeof (global::Objectives_PerformRitual.FinalizedData_PerformRitual).TypeHandle,
          new KeyValuePair<int, int>(22, 22)
        },
        {
          typeof (global::Objectives_PlaceStructure.FinalizedData_PlaceStructure).TypeHandle,
          new KeyValuePair<int, int>(23, 23)
        },
        {
          typeof (global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower).TypeHandle,
          new KeyValuePair<int, int>(24, 24)
        },
        {
          typeof (global::Objectives_RecruitFollower.FinalizedData_RecruitFollower).TypeHandle,
          new KeyValuePair<int, int>(25, 25)
        },
        {
          typeof (global::Objectives_RemoveStructure.FinalizedData_RemoveStructure).TypeHandle,
          new KeyValuePair<int, int>(26, 26)
        },
        {
          typeof (global::Objectives_ShootDummy.FinalizedData_ShootDummy).TypeHandle,
          new KeyValuePair<int, int>(27, 27)
        },
        {
          typeof (global::Objectives_ShowFleece.FinalizedData_ShowFleece).TypeHandle,
          new KeyValuePair<int, int>(28, 28)
        },
        {
          typeof (global::Objectives_Story.FinalizedData).TypeHandle,
          new KeyValuePair<int, int>(29, 29)
        },
        {
          typeof (global::Objectives_TalkToFollower.FinalizedData_TalkToFollower).TypeHandle,
          new KeyValuePair<int, int>(30, 30)
        },
        {
          typeof (global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade).TypeHandle,
          new KeyValuePair<int, int>(31 /*0x1F*/, 31 /*0x1F*/)
        },
        {
          typeof (global::Objectives_UseRelic.FinalizedData_UseRelic).TypeHandle,
          new KeyValuePair<int, int>(32 /*0x20*/, 32 /*0x20*/)
        },
        {
          typeof (global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet).TypeHandle,
          new KeyValuePair<int, int>(33, 33)
        },
        {
          typeof (global::Objectives_NoDodge.FinalizedData_NoDodge).TypeHandle,
          new KeyValuePair<int, int>(34, 34)
        },
        {
          typeof (global::Objectives_NoDamage.FinalizedData_NoDamage).TypeHandle,
          new KeyValuePair<int, int>(35, 35)
        },
        {
          typeof (global::Objectives_NoCurses.FinalizedData_NoCurses).TypeHandle,
          new KeyValuePair<int, int>(36, 36)
        },
        {
          typeof (global::Objectives_NoHealing.FinalizedData_NoHealing).TypeHandle,
          new KeyValuePair<int, int>(37, 37)
        },
        {
          typeof (global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations).TypeHandle,
          new KeyValuePair<int, int>(38, 38)
        },
        {
          typeof (global::Objectives_FeedAnimal.FinalizedData_FeedAnimal).TypeHandle,
          new KeyValuePair<int, int>(39, 39)
        },
        {
          typeof (global::Objectives_WalkAnimal.FinalizedData_WalkAnimal).TypeHandle,
          new KeyValuePair<int, int>(40, 40)
        },
        {
          typeof (global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn).TypeHandle,
          new KeyValuePair<int, int>(41, 41)
        }
      };
      this.keyToJumpMap = new Dictionary<int, int>(42)
      {
        {
          0,
          0
        },
        {
          1,
          1
        },
        {
          2,
          2
        },
        {
          3,
          3
        },
        {
          4,
          4
        },
        {
          5,
          5
        },
        {
          6,
          6
        },
        {
          7,
          7
        },
        {
          8,
          8
        },
        {
          9,
          9
        },
        {
          10,
          10
        },
        {
          11,
          11
        },
        {
          12,
          12
        },
        {
          13,
          13
        },
        {
          14,
          14
        },
        {
          15,
          15
        },
        {
          16 /*0x10*/,
          16 /*0x10*/
        },
        {
          17,
          17
        },
        {
          18,
          18
        },
        {
          19,
          19
        },
        {
          20,
          20
        },
        {
          21,
          21
        },
        {
          22,
          22
        },
        {
          23,
          23
        },
        {
          24,
          24
        },
        {
          25,
          25
        },
        {
          26,
          26
        },
        {
          27,
          27
        },
        {
          28,
          28
        },
        {
          29,
          29
        },
        {
          30,
          30
        },
        {
          31 /*0x1F*/,
          31 /*0x1F*/
        },
        {
          32 /*0x20*/,
          32 /*0x20*/
        },
        {
          33,
          33
        },
        {
          34,
          34
        },
        {
          35,
          35
        },
        {
          36,
          36
        },
        {
          37,
          37
        },
        {
          38,
          38
        },
        {
          39,
          39
        },
        {
          40,
          40
        },
        {
          41,
          41
        }
      };
    }

    public void Serialize(
      ref MessagePackWriter writer,
      ObjectivesDataFinalized value,
      MessagePackSerializerOptions options)
    {
      KeyValuePair<int, int> keyValuePair;
      if (value != null && this.typeToKeyAndJumpMap.TryGetValue(value.GetType().TypeHandle, out keyValuePair))
      {
        writer.WriteArrayHeader(2);
        writer.WriteInt32(keyValuePair.Key);
        switch (keyValuePair.Value)
        {
          case 0:
            options.Resolver.GetFormatterWithVerify<global::Objective_FindRelic.FinalizedData_FindRelic>().Serialize(ref writer, (global::Objective_FindRelic.FinalizedData_FindRelic) value, options);
            break;
          case 1:
            options.Resolver.GetFormatterWithVerify<global::Objectives_AssignClothing.FinalizedData_AssignClothing>().Serialize(ref writer, (global::Objectives_AssignClothing.FinalizedData_AssignClothing) value, options);
            break;
          case 2:
            options.Resolver.GetFormatterWithVerify<global::Objectives_BedRest.FinalizedData_BedRest>().Serialize(ref writer, (global::Objectives_BedRest.FinalizedData_BedRest) value, options);
            break;
          case 3:
            options.Resolver.GetFormatterWithVerify<global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering>().Serialize(ref writer, (global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering) value, options);
            break;
          case 4:
            options.Resolver.GetFormatterWithVerify<global::Objectives_BuildStructure.FinalizedData_BuildStructure>().Serialize(ref writer, (global::Objectives_BuildStructure.FinalizedData_BuildStructure) value, options);
            break;
          case 5:
            options.Resolver.GetFormatterWithVerify<global::Objectives_CollectItem.FinalizedData_CollectItem>().Serialize(ref writer, (global::Objectives_CollectItem.FinalizedData_CollectItem) value, options);
            break;
          case 6:
            options.Resolver.GetFormatterWithVerify<global::Objectives_CookMeal.FinalizedData_CookMeal>().Serialize(ref writer, (global::Objectives_CookMeal.FinalizedData_CookMeal) value, options);
            break;
          case 7:
            options.Resolver.GetFormatterWithVerify<global::Objectives_CraftClothing.FinalizedData_CraftClothing>().Serialize(ref writer, (global::Objectives_CraftClothing.FinalizedData_CraftClothing) value, options);
            break;
          case 8:
            options.Resolver.GetFormatterWithVerify<global::Objectives_Custom.FinalizedData_Custom>().Serialize(ref writer, (global::Objectives_Custom.FinalizedData_Custom) value, options);
            break;
          case 9:
            options.Resolver.GetFormatterWithVerify<global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones>().Serialize(ref writer, (global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones) value, options);
            break;
          case 10:
            options.Resolver.GetFormatterWithVerify<global::Objectives_DepositFood.FinalizedData_DepositFood>().Serialize(ref writer, (global::Objectives_DepositFood.FinalizedData_DepositFood) value, options);
            break;
          case 11:
            options.Resolver.GetFormatterWithVerify<global::Objectives_Drink.FinalizedData_Drink>().Serialize(ref writer, (global::Objectives_Drink.FinalizedData_Drink) value, options);
            break;
          case 12:
            options.Resolver.GetFormatterWithVerify<global::Objectives_EatMeal.FinalizedData_EatMeal>().Serialize(ref writer, (global::Objectives_EatMeal.FinalizedData_EatMeal) value, options);
            break;
          case 13:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FindChildren.FinalizedData_FindChildren>().Serialize(ref writer, (global::Objectives_FindChildren.FinalizedData_FindChildren) value, options);
            break;
          case 14:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FindFollower.FinalizedData_FindFollower>().Serialize(ref writer, (global::Objectives_FindFollower.FinalizedData_FindFollower) value, options);
            break;
          case 15:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace>().Serialize(ref writer, (global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace) value, options);
            break;
          case 16 /*0x10*/:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets>().Serialize(ref writer, (global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets) value, options);
            break;
          case 17:
            options.Resolver.GetFormatterWithVerify<global::Objectives_GetAnimal.FinalizedData_GetAnimal>().Serialize(ref writer, (global::Objectives_GetAnimal.FinalizedData_GetAnimal) value, options);
            break;
          case 18:
            options.Resolver.GetFormatterWithVerify<global::Objectives_GiveItem.FinalizedData_GiveItem>().Serialize(ref writer, (global::Objectives_GiveItem.FinalizedData_GiveItem) value, options);
            break;
          case 19:
            options.Resolver.GetFormatterWithVerify<global::Objectives_KillEnemies.FinalizedData_KillEnemies>().Serialize(ref writer, (global::Objectives_KillEnemies.FinalizedData_KillEnemies) value, options);
            break;
          case 20:
            options.Resolver.GetFormatterWithVerify<global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun>().Serialize(ref writer, (global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun) value, options);
            break;
          case 21:
            options.Resolver.GetFormatterWithVerify<global::Objectives_Mating.FinalizedData_Mating>().Serialize(ref writer, (global::Objectives_Mating.FinalizedData_Mating) value, options);
            break;
          case 22:
            options.Resolver.GetFormatterWithVerify<global::Objectives_PerformRitual.FinalizedData_PerformRitual>().Serialize(ref writer, (global::Objectives_PerformRitual.FinalizedData_PerformRitual) value, options);
            break;
          case 23:
            options.Resolver.GetFormatterWithVerify<global::Objectives_PlaceStructure.FinalizedData_PlaceStructure>().Serialize(ref writer, (global::Objectives_PlaceStructure.FinalizedData_PlaceStructure) value, options);
            break;
          case 24:
            options.Resolver.GetFormatterWithVerify<global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower>().Serialize(ref writer, (global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower) value, options);
            break;
          case 25:
            options.Resolver.GetFormatterWithVerify<global::Objectives_RecruitFollower.FinalizedData_RecruitFollower>().Serialize(ref writer, (global::Objectives_RecruitFollower.FinalizedData_RecruitFollower) value, options);
            break;
          case 26:
            options.Resolver.GetFormatterWithVerify<global::Objectives_RemoveStructure.FinalizedData_RemoveStructure>().Serialize(ref writer, (global::Objectives_RemoveStructure.FinalizedData_RemoveStructure) value, options);
            break;
          case 27:
            options.Resolver.GetFormatterWithVerify<global::Objectives_ShootDummy.FinalizedData_ShootDummy>().Serialize(ref writer, (global::Objectives_ShootDummy.FinalizedData_ShootDummy) value, options);
            break;
          case 28:
            options.Resolver.GetFormatterWithVerify<global::Objectives_ShowFleece.FinalizedData_ShowFleece>().Serialize(ref writer, (global::Objectives_ShowFleece.FinalizedData_ShowFleece) value, options);
            break;
          case 29:
            options.Resolver.GetFormatterWithVerify<global::Objectives_Story.FinalizedData>().Serialize(ref writer, (global::Objectives_Story.FinalizedData) value, options);
            break;
          case 30:
            options.Resolver.GetFormatterWithVerify<global::Objectives_TalkToFollower.FinalizedData_TalkToFollower>().Serialize(ref writer, (global::Objectives_TalkToFollower.FinalizedData_TalkToFollower) value, options);
            break;
          case 31 /*0x1F*/:
            options.Resolver.GetFormatterWithVerify<global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade>().Serialize(ref writer, (global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade) value, options);
            break;
          case 32 /*0x20*/:
            options.Resolver.GetFormatterWithVerify<global::Objectives_UseRelic.FinalizedData_UseRelic>().Serialize(ref writer, (global::Objectives_UseRelic.FinalizedData_UseRelic) value, options);
            break;
          case 33:
            options.Resolver.GetFormatterWithVerify<global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet>().Serialize(ref writer, (global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet) value, options);
            break;
          case 34:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoDodge.FinalizedData_NoDodge>().Serialize(ref writer, (global::Objectives_NoDodge.FinalizedData_NoDodge) value, options);
            break;
          case 35:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoDamage.FinalizedData_NoDamage>().Serialize(ref writer, (global::Objectives_NoDamage.FinalizedData_NoDamage) value, options);
            break;
          case 36:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoCurses.FinalizedData_NoCurses>().Serialize(ref writer, (global::Objectives_NoCurses.FinalizedData_NoCurses) value, options);
            break;
          case 37:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoHealing.FinalizedData_NoHealing>().Serialize(ref writer, (global::Objectives_NoHealing.FinalizedData_NoHealing) value, options);
            break;
          case 38:
            options.Resolver.GetFormatterWithVerify<global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations>().Serialize(ref writer, (global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations) value, options);
            break;
          case 39:
            options.Resolver.GetFormatterWithVerify<global::Objectives_FeedAnimal.FinalizedData_FeedAnimal>().Serialize(ref writer, (global::Objectives_FeedAnimal.FinalizedData_FeedAnimal) value, options);
            break;
          case 40:
            options.Resolver.GetFormatterWithVerify<global::Objectives_WalkAnimal.FinalizedData_WalkAnimal>().Serialize(ref writer, (global::Objectives_WalkAnimal.FinalizedData_WalkAnimal) value, options);
            break;
          case 41:
            options.Resolver.GetFormatterWithVerify<global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn>().Serialize(ref writer, (global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn) value, options);
            break;
        }
      }
      else
        writer.WriteNil();
    }

    public ObjectivesDataFinalized Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (ObjectivesDataFinalized) null;
      if (reader.ReadArrayHeader() != 2)
        throw new InvalidOperationException("Invalid Union data was detected. Type:global::ObjectivesDataFinalized");
      options.Security.DepthStep(ref reader);
      int key = reader.ReadInt32();
      if (!this.keyToJumpMap.TryGetValue(key, out key))
        key = -1;
      ObjectivesDataFinalized objectivesDataFinalized = (ObjectivesDataFinalized) null;
      switch (key)
      {
        case 0:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objective_FindRelic.FinalizedData_FindRelic>().Deserialize(ref reader, options);
          break;
        case 1:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_AssignClothing.FinalizedData_AssignClothing>().Deserialize(ref reader, options);
          break;
        case 2:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_BedRest.FinalizedData_BedRest>().Deserialize(ref reader, options);
          break;
        case 3:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_BlizzardOffering.FinalizedData_BlizzardOffering>().Deserialize(ref reader, options);
          break;
        case 4:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_BuildStructure.FinalizedData_BuildStructure>().Deserialize(ref reader, options);
          break;
        case 5:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_CollectItem.FinalizedData_CollectItem>().Deserialize(ref reader, options);
          break;
        case 6:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_CookMeal.FinalizedData_CookMeal>().Deserialize(ref reader, options);
          break;
        case 7:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_CraftClothing.FinalizedData_CraftClothing>().Deserialize(ref reader, options);
          break;
        case 8:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_Custom.FinalizedData_Custom>().Deserialize(ref reader, options);
          break;
        case 9:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones>().Deserialize(ref reader, options);
          break;
        case 10:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_DepositFood.FinalizedData_DepositFood>().Deserialize(ref reader, options);
          break;
        case 11:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_Drink.FinalizedData_Drink>().Deserialize(ref reader, options);
          break;
        case 12:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_EatMeal.FinalizedData_EatMeal>().Deserialize(ref reader, options);
          break;
        case 13:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_FindChildren.FinalizedData_FindChildren>().Deserialize(ref reader, options);
          break;
        case 14:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_FindFollower.FinalizedData_FindFollower>().Deserialize(ref reader, options);
          break;
        case 15:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_FinishRace.FinalizedData_Objectives_FinishRace>().Deserialize(ref reader, options);
          break;
        case 16 /*0x10*/:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_FlowerBaskets.FinalizedData_FlowerBaskets>().Deserialize(ref reader, options);
          break;
        case 17:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_GetAnimal.FinalizedData_GetAnimal>().Deserialize(ref reader, options);
          break;
        case 18:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_GiveItem.FinalizedData_GiveItem>().Deserialize(ref reader, options);
          break;
        case 19:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_KillEnemies.FinalizedData_KillEnemies>().Deserialize(ref reader, options);
          break;
        case 20:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun>().Deserialize(ref reader, options);
          break;
        case 21:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_Mating.FinalizedData_Mating>().Deserialize(ref reader, options);
          break;
        case 22:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_PerformRitual.FinalizedData_PerformRitual>().Deserialize(ref reader, options);
          break;
        case 23:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_PlaceStructure.FinalizedData_PlaceStructure>().Deserialize(ref reader, options);
          break;
        case 24:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower>().Deserialize(ref reader, options);
          break;
        case 25:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_RecruitFollower.FinalizedData_RecruitFollower>().Deserialize(ref reader, options);
          break;
        case 26:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_RemoveStructure.FinalizedData_RemoveStructure>().Deserialize(ref reader, options);
          break;
        case 27:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_ShootDummy.FinalizedData_ShootDummy>().Deserialize(ref reader, options);
          break;
        case 28:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_ShowFleece.FinalizedData_ShowFleece>().Deserialize(ref reader, options);
          break;
        case 29:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_Story.FinalizedData>().Deserialize(ref reader, options);
          break;
        case 30:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_TalkToFollower.FinalizedData_TalkToFollower>().Deserialize(ref reader, options);
          break;
        case 31 /*0x1F*/:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade>().Deserialize(ref reader, options);
          break;
        case 32 /*0x20*/:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_UseRelic.FinalizedData_UseRelic>().Deserialize(ref reader, options);
          break;
        case 33:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet>().Deserialize(ref reader, options);
          break;
        case 34:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_NoDodge.FinalizedData_NoDodge>().Deserialize(ref reader, options);
          break;
        case 35:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_NoDamage.FinalizedData_NoDamage>().Deserialize(ref reader, options);
          break;
        case 36:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_NoCurses.FinalizedData_NoCurses>().Deserialize(ref reader, options);
          break;
        case 37:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_NoHealing.FinalizedData_NoHealing>().Deserialize(ref reader, options);
          break;
        case 38:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations>().Deserialize(ref reader, options);
          break;
        case 39:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_FeedAnimal.FinalizedData_FeedAnimal>().Deserialize(ref reader, options);
          break;
        case 40:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_WalkAnimal.FinalizedData_WalkAnimal>().Deserialize(ref reader, options);
          break;
        case 41:
          objectivesDataFinalized = (ObjectivesDataFinalized) options.Resolver.GetFormatterWithVerify<global::Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn>().Deserialize(ref reader, options);
          break;
        default:
          reader.Skip();
          break;
      }
      --reader.Depth;
      return objectivesDataFinalized;
    }
  }

  public sealed class Objectives_RoomChallengeFormatter : 
    IMessagePackFormatter<Objectives_RoomChallenge>,
    IMessagePackFormatter
  {
    public Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap;
    public Dictionary<int, int> keyToJumpMap;

    public Objectives_RoomChallengeFormatter()
    {
      this.typeToKeyAndJumpMap = new Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>(4, RuntimeTypeHandleEqualityComparer.Default)
      {
        {
          typeof (global::Objectives_NoDodge).TypeHandle,
          new KeyValuePair<int, int>(0, 0)
        },
        {
          typeof (global::Objectives_NoCurses).TypeHandle,
          new KeyValuePair<int, int>(1, 1)
        },
        {
          typeof (global::Objectives_NoDamage).TypeHandle,
          new KeyValuePair<int, int>(2, 2)
        },
        {
          typeof (global::Objectives_NoHealing).TypeHandle,
          new KeyValuePair<int, int>(3, 3)
        }
      };
      this.keyToJumpMap = new Dictionary<int, int>(4)
      {
        {
          0,
          0
        },
        {
          1,
          1
        },
        {
          2,
          2
        },
        {
          3,
          3
        }
      };
    }

    public void Serialize(
      ref MessagePackWriter writer,
      Objectives_RoomChallenge value,
      MessagePackSerializerOptions options)
    {
      KeyValuePair<int, int> keyValuePair;
      if (value != null && this.typeToKeyAndJumpMap.TryGetValue(value.GetType().TypeHandle, out keyValuePair))
      {
        writer.WriteArrayHeader(2);
        writer.WriteInt32(keyValuePair.Key);
        switch (keyValuePair.Value)
        {
          case 0:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoDodge>().Serialize(ref writer, (global::Objectives_NoDodge) value, options);
            break;
          case 1:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoCurses>().Serialize(ref writer, (global::Objectives_NoCurses) value, options);
            break;
          case 2:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoDamage>().Serialize(ref writer, (global::Objectives_NoDamage) value, options);
            break;
          case 3:
            options.Resolver.GetFormatterWithVerify<global::Objectives_NoHealing>().Serialize(ref writer, (global::Objectives_NoHealing) value, options);
            break;
        }
      }
      else
        writer.WriteNil();
    }

    public Objectives_RoomChallenge Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (Objectives_RoomChallenge) null;
      if (reader.ReadArrayHeader() != 2)
        throw new InvalidOperationException("Invalid Union data was detected. Type:global::Objectives_RoomChallenge");
      options.Security.DepthStep(ref reader);
      int key = reader.ReadInt32();
      if (!this.keyToJumpMap.TryGetValue(key, out key))
        key = -1;
      Objectives_RoomChallenge objectivesRoomChallenge = (Objectives_RoomChallenge) null;
      switch (key)
      {
        case 0:
          objectivesRoomChallenge = (Objectives_RoomChallenge) options.Resolver.GetFormatterWithVerify<global::Objectives_NoDodge>().Deserialize(ref reader, options);
          break;
        case 1:
          objectivesRoomChallenge = (Objectives_RoomChallenge) options.Resolver.GetFormatterWithVerify<global::Objectives_NoCurses>().Deserialize(ref reader, options);
          break;
        case 2:
          objectivesRoomChallenge = (Objectives_RoomChallenge) options.Resolver.GetFormatterWithVerify<global::Objectives_NoDamage>().Deserialize(ref reader, options);
          break;
        case 3:
          objectivesRoomChallenge = (Objectives_RoomChallenge) options.Resolver.GetFormatterWithVerify<global::Objectives_NoHealing>().Deserialize(ref reader, options);
          break;
        default:
          reader.Skip();
          break;
      }
      --reader.Depth;
      return objectivesRoomChallenge;
    }
  }
}
