// Decompiled with JetBrains decompiler
// Type: WorldManipulatorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Map;
using MMBiomeGeneration;
using MMTools;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public static class WorldManipulatorManager
{
  public static void TriggerManipulation(
    WorldManipulatorManager.Manipulations manipulation,
    float delay = 0.0f,
    bool twitch = false)
  {
    GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() =>
    {
      string nonLocalisedString = "";
      List<Follower> possibleFollowers;
      switch (manipulation)
      {
        case WorldManipulatorManager.Manipulations.GainRandomHeart:
          switch (HealthPlayer.GainRandomHeart())
          {
            case 0:
              nonLocalisedString = "Inventory/BLACK_HEART";
              break;
            case 1:
              nonLocalisedString = "Inventory/BLUE_HEART";
              break;
            case 2:
              nonLocalisedString = "Inventory/RED_HEART";
              break;
          }
          break;
        case WorldManipulatorManager.Manipulations.HealHearts:
          int healing = UnityEngine.Random.Range((int) ((double) PlayerFarming.Instance.health.totalHP / 2.0), (int) PlayerFarming.Instance.health.totalHP + 1);
          foreach (Component player in PlayerFarming.players)
            player.GetComponent<HealthPlayer>().Heal((float) healing);
          nonLocalisedString = healing.ToString();
          break;
        case WorldManipulatorManager.Manipulations.GainTarot:
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, PlayerFarming.Instance.transform.position + Vector3.down, 0.0f).GetComponent<Interaction_TarotCard>().ForceAllow = true;
          break;
        case WorldManipulatorManager.Manipulations.DealDamageToAllEnemies:
          Health.DamageAllEnemies((float) (5.0 + (double) DataManager.GetWeaponDamageMultiplier(PlayerFarming.Instance.currentWeaponLevel) * 2.0), Health.DamageAllEnemiesType.Manipulation);
          break;
        case WorldManipulatorManager.Manipulations.ReceiveDemon:
          int num1 = 0;
          while (++num1 < 30)
          {
            int type = UnityEngine.Random.Range(0, 5);
            if (!DataManager.Instance.Followers_Demons_Types.Contains(type))
            {
              BiomeGenerator.Instance.SpawnDemon(type, -1, doEffects: true);
              break;
            }
          }
          break;
        case WorldManipulatorManager.Manipulations.InvincibleForTime:
          float duration = 30f;
          foreach (PlayerFarming player in PlayerFarming.players)
            player.playerController.MakeUntouchable(duration);
          nonLocalisedString = duration.ToString();
          break;
        case WorldManipulatorManager.Manipulations.DropRandomWeaponCurse:
        case WorldManipulatorManager.Manipulations.DropRandomRelic:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.DropRandomWeaponRelic(manipulation));
          break;
        case WorldManipulatorManager.Manipulations.ChargeCurrentRelic:
          PlayerFarming.Instance.playerRelic.FullyCharge();
          break;
        case WorldManipulatorManager.Manipulations.SpecialAttacksDamageIncrease:
          DataManager.Instance.SpecialAttackDamageMultiplier += 0.5f;
          break;
        case WorldManipulatorManager.Manipulations.NextChestGold:
          DataManager.Instance.NextChestGold = true;
          break;
        case WorldManipulatorManager.Manipulations.FreezeTime:
          double maxDuration = (double) BiomeConstants.Instance.freezeTimeSequenceData.ImpactVFXObject.MaxDuration;
          if (Health.team2.Count == 0)
          {
            BiomeGenerator.OnBiomeEnteredCombatRoom += new BiomeGenerator.BiomeAction(WorldManipulatorManager.FreezeTime);
            break;
          }
          BiomeConstants.Instance.FreezeTime();
          break;
        case WorldManipulatorManager.Manipulations.EnemiesDropGold:
          DataManager.Instance.EnemiesDropGoldDuringRun = true;
          break;
        case WorldManipulatorManager.Manipulations.GainCorruptedTarotPositive:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.GiveCorruptedTarotPositiveRoutine());
          break;
        case WorldManipulatorManager.Manipulations.FreezeEnemies:
          BiomeGenerator.OnBiomeEnteredCombatRoom += new BiomeGenerator.BiomeAction(WorldManipulatorManager.FreezeEnemiesOnEnterCombatRoom);
          break;
        case WorldManipulatorManager.Manipulations.CharmEnemies:
          BiomeGenerator.OnBiomeEnteredCombatRoom += new BiomeGenerator.BiomeAction(WorldManipulatorManager.CharmEnemiesOnEnterCombatRoom);
          break;
        case WorldManipulatorManager.Manipulations.TakeDamage:
          int damage = Mathf.Max(1, UnityEngine.Random.Range(0, (int) ((double) PlayerFarming.Instance.health.HP / 2.0)));
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.TakeDamageIE(damage));
          nonLocalisedString = damage.ToString();
          break;
        case WorldManipulatorManager.Manipulations.IncreaseEnemyModifiersChance:
          DataManager.Instance.EnemyModifiersChanceMultiplier += 3f;
          break;
        case WorldManipulatorManager.Manipulations.SpawnBombs:
          BiomeGenerator.SpawnBombsInRoom(UnityEngine.Random.Range(15, 25), (bool) (UnityEngine.Object) PlayerFarming.Instance);
          break;
        case WorldManipulatorManager.Manipulations.LoseAllSpecialHearts:
          HealthPlayer.LoseAllSpecialHearts();
          break;
        case WorldManipulatorManager.Manipulations.DropPoisonOnAttack:
          DataManager.Instance.SpawnPoisonOnAttack = true;
          break;
        case WorldManipulatorManager.Manipulations.AllEnemiesHaveModifiersInNextRoom:
          DataManager.Instance.EnemiesInNextRoomHaveModifiers = true;
          DataManager.Instance.CurrentRoomCoordinates = new Vector2((float) BiomeGenerator.Instance.CurrentRoom.x, (float) BiomeGenerator.Instance.CurrentRoom.y);
          RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(WorldManipulatorManager.OnRoomCleared);
          break;
        case WorldManipulatorManager.Manipulations.LoseRelic:
          PlayerFarming.Instance.playerRelic.RemoveRelic();
          break;
        case WorldManipulatorManager.Manipulations.NoSpecialAttacks:
          DataManager.Instance.SpecialAttacksDisabled = true;
          break;
        case WorldManipulatorManager.Manipulations.AllEnemiesHealed:
          using (List<Health>.Enumerator enumerator = Health.team2.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Health current = enumerator.Current;
              if ((UnityEngine.Object) current != (UnityEngine.Object) null)
              {
                current.HP = current.totalHP;
                current.GetComponent<ShowHPBar>()?.OnHit(current.gameObject, Vector3.zero, Health.AttackTypes.Melee, false);
                AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", current.gameObject.transform.position);
                BiomeConstants.Instance.EmitHeartPickUpVFX(current.transform.position - Vector3.forward, 0.0f, "red", "burst_big");
              }
            }
            break;
          }
        case WorldManipulatorManager.Manipulations.IncreasedBossesHealth:
          DataManager.Instance.BossHealthMultiplier += 0.5f;
          break;
        case WorldManipulatorManager.Manipulations.CombatNodes:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.ConvertAllMapNodesIE());
          break;
        case WorldManipulatorManager.Manipulations.GainCorruptedTarotNegative:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.GiveCorruptedTarotNegativeRoutine());
          break;
        case WorldManipulatorManager.Manipulations.NoRollingInNextRoom:
          DataManager.Instance.NoRollInNextCombatRoom = true;
          RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(WorldManipulatorManager.OnRoomClearedWithoutRoll);
          break;
        case WorldManipulatorManager.Manipulations.NoHeartDrops:
          DataManager.Instance.NoHeartDrops = true;
          break;
        case WorldManipulatorManager.Manipulations.BombOnEnemyDeath:
          DataManager.Instance.EnemiesDropBombOnDeath = true;
          break;
        case WorldManipulatorManager.Manipulations.SpawnPoisons:
          BiomeGenerator.SpawnPoisonsInRoom(UnityEngine.Random.Range(40, 60), (bool) (UnityEngine.Object) PlayerFarming.Instance);
          break;
        case WorldManipulatorManager.Manipulations.ResetTempleCooldowns:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => UpgradeSystem.ClearAllCoolDowns())));
          break;
        case WorldManipulatorManager.Manipulations.InstantlyBuildStructures:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => StructureManager.BuildAllStructures())));
          break;
        case WorldManipulatorManager.Manipulations.GainFaith:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => CultFaithManager.AddThought(Thought.FaithIncreased))));
          break;
        case WorldManipulatorManager.Manipulations.ResurrectBuriedFollower:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.ResurrectBurriedFollower())));
          break;
        case WorldManipulatorManager.Manipulations.CureCursedFollowers:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.CureAllCursedFollowers())));
          break;
        case WorldManipulatorManager.Manipulations.ClearAllWaste:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => StructureManager.ClearAllWaste())));
          break;
        case WorldManipulatorManager.Manipulations.FollowerInstantlyLevelled:
          FollowerBrain followerBrain1 = FollowerManager.MakeFollowerGainLevel();
          if (followerBrain1 == null)
            return;
          nonLocalisedString = $"<color=#FFD201>{followerBrain1.Info.Name}</color>";
          break;
        case WorldManipulatorManager.Manipulations.CropsInstantlyFertilised:
          foreach (UIItemSelectorOverlayController selectorOverlay in UIItemSelectorOverlayController.SelectorOverlays)
          {
            if (selectorOverlay.Context == ItemSelector.Context.AddFertiliser)
              selectorOverlay.Hide(true);
          }
          BaseLocationManager.Instance.InstantlyFertilizeAllCrops();
          break;
        case WorldManipulatorManager.Manipulations.MissionarySuccessful:
          DataManager.Instance.NextMissionarySuccessful = true;
          break;
        case WorldManipulatorManager.Manipulations.InstantRefinedMaterials:
          BaseLocationManager.Instance.InstantlyRefineMaterials();
          break;
        case WorldManipulatorManager.Manipulations.ResetEndlessModeCooldown:
          DataManager.Instance.EndlessModeOnCooldown = false;
          DataManager.Instance.EndlessModeSinOncooldown = false;
          break;
        case WorldManipulatorManager.Manipulations.CraftRandomOutfit:
          List<Structures_Tailor> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Tailor>();
          if (structuresOfType1.Count > 0)
          {
            StructuresData.ClothingStruct clothingStruct = new StructuresData.ClothingStruct();
            List<FollowerClothingType> followerClothingTypeList = new List<FollowerClothingType>();
            foreach (FollowerClothingType clothingType in DataManager.Instance.UnlockedClothing)
            {
              ClothingData clothingData = TailorManager.GetClothingData(clothingType);
              if (!clothingData.SpecialClothing && clothingData.ClothingType != FollowerClothingType.None)
                followerClothingTypeList.Add(clothingType);
            }
            clothingStruct.ClothingType = followerClothingTypeList[UnityEngine.Random.Range(0, followerClothingTypeList.Count)];
            structuresOfType1[0].Data.AllClothing.Add(clothingStruct);
            nonLocalisedString = TailorManager.LocalizedName(clothingStruct.ClothingType);
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.EggsHatch:
          using (List<Structures_Hatchery>.Enumerator enumerator = StructureManager.GetAllStructuresOfType<Structures_Hatchery>().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Structures_Hatchery current = enumerator.Current;
              if (current.Data.HasEgg && !current.Data.EggReady)
                current.SetEggReady();
            }
            break;
          }
        case WorldManipulatorManager.Manipulations.GoldenEgg:
          DataManager.Instance.ForceGoldenEgg = true;
          break;
        case WorldManipulatorManager.Manipulations.SpecialPoo:
          DataManager.Instance.ForceSpecialPoo = true;
          possibleFollowers = new List<Follower>();
          foreach (Follower follower in Follower.Followers)
          {
            if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
              possibleFollowers.Add(follower);
          }
          if (possibleFollowers.Count > 0)
          {
            possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)].Brain.HardSwapToTask((FollowerTask) new FollowerTask_InstantPoop());
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.FillBrewery:
          List<Structures_Pub> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Pub>();
          if (structuresOfType2.Count > 0 && structuresOfType2[0].Data.QueuedMeals.Count < structuresOfType2[0].MaxQueue)
          {
            int num2 = 0;
            foreach (InventoryItem inventoryItem in structuresOfType2[0].FoodStorage.Data.Inventory)
            {
              if (inventoryItem != null)
                ++num2;
            }
            int num3 = structuresOfType2[0].MaxQueue - num2;
            for (int index = 0; index < num3; ++index)
            {
              InventoryItem.ITEM_TYPE allDrink = CookingData.GetAllDrinks()[UnityEngine.Random.Range(0, CookingData.GetAllDrinks().Length)];
              structuresOfType2[0].Data.QueuedResources.Add(allDrink);
              structuresOfType2[0].FoodStorage.DepositItemUnstacked(allDrink);
            }
            if (Interaction_Pub.Pubs.Count > 0)
            {
              Interaction_Pub.Pubs[0].UpdateDisplays();
              break;
            }
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.FollowerSin:
          possibleFollowers = new List<Follower>();
          foreach (Follower follower in Follower.Followers)
          {
            if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
              possibleFollowers.Add(follower);
          }
          if (possibleFollowers.Count > 0)
          {
            Follower follower = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
            follower.Brain.AddPleasure(FollowerBrain.PleasureActions.Twitch);
            nonLocalisedString = $"<color=#FFD201>{follower.Brain.Info.Name}</color>";
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.SpawnGreatMeals:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => WorldManipulatorManager.SpawnGreatMeals())));
          break;
        case WorldManipulatorManager.Manipulations.FillDevotion:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() =>
          {
            int Delta = DataManager.GetTargetXP(Mathf.Min(DataManager.Instance.Level, DataManager.TargetXP.Count - 1)) - DataManager.Instance.XP;
            PlayerFarming.Instance.GetXP((float) Delta);
          })));
          break;
        case WorldManipulatorManager.Manipulations.FillSermon:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => DoctrineUpgradeSystem.SetXPBySermon(SermonCategory.PlayerUpgrade, DoctrineUpgradeSystem.GetXPTargetBySermon(SermonCategory.PlayerUpgrade)))));
          break;
        case WorldManipulatorManager.Manipulations.LuckyKnucklebones:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => DataManager.Instance.NextKnucklbonesLucky = true)));
          break;
        case WorldManipulatorManager.Manipulations.FreeRitual:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => DataManager.Instance.NextRitualFree = true)));
          break;
        case WorldManipulatorManager.Manipulations.AllFollowersPoopOrVomit:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.MakeAllFollowersPoopOrVomit())));
          break;
        case WorldManipulatorManager.Manipulations.BreakAllBeds:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => StructureManager.BreakRandomBeds())));
          break;
        case WorldManipulatorManager.Manipulations.SkipTime:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => TimeManager.SkipTime(600f))));
          break;
        case WorldManipulatorManager.Manipulations.SleepFollowers:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerActive((System.Action) (() => FollowerManager.MakeAllFollowersFallAsleep()))))));
          break;
        case WorldManipulatorManager.Manipulations.RandomCursedState:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.GiveFollowersRandomCurse(UnityEngine.Random.Range(Mathf.Clamp(Mathf.RoundToInt((float) DataManager.Instance.Followers.Count / 6f), 1, DataManager.Instance.Followers.Count), Mathf.Clamp(Mathf.RoundToInt((float) DataManager.Instance.Followers.Count / 4f), 1, DataManager.Instance.Followers.Count) + 1)))));
          break;
        case WorldManipulatorManager.Manipulations.KillRandomFollower:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.KillRandomFollower(true))));
          break;
        case WorldManipulatorManager.Manipulations.ToiletsInstantlyFull:
          BaseLocationManager.Instance.InstantlyFillAllToilets();
          break;
        case WorldManipulatorManager.Manipulations.BodiesOutOfGraves:
          BaseLocationManager.Instance.PopOutDeadBodiesFromGraves(UnityEngine.Random.Range(5, 10));
          break;
        case WorldManipulatorManager.Manipulations.MealsVanish:
          BaseLocationManager.Instance.InstantlyClearKitchenQueues();
          break;
        case WorldManipulatorManager.Manipulations.FollowerLosesLevel:
          FollowerBrain followerBrain2 = FollowerManager.MakeFollowerLoseLevel();
          if (followerBrain2 == null)
            return;
          nonLocalisedString = $"<color=#FFD201>{followerBrain2.Info.Name}</color>";
          break;
        case WorldManipulatorManager.Manipulations.QuestForMurder:
          FollowerBrain nonLockedFollower = FollowerManager.GetRandomNonLockedFollower();
          if (nonLockedFollower == null)
            return;
          nonLocalisedString = $"<color=#FFD201>{nonLockedFollower.Info.Name}</color>";
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/TwitchQuest", Objectives.CustomQuestTypes.MurderFollower, nonLockedFollower.Info.ID, 4800f), true);
          break;
        case WorldManipulatorManager.Manipulations.EmptyBrewery:
          List<Structures_Pub> structuresOfType3 = StructureManager.GetAllStructuresOfType<Structures_Pub>();
          if (structuresOfType3.Count > 0)
          {
            structuresOfType3[0].Data.QueuedResources.Clear();
            structuresOfType3[0].FoodStorage.Data.Inventory.Clear();
            if (Interaction_Pub.Pubs.Count > 0)
            {
              Interaction_Pub.Pubs[0].UpdateDisplays();
              break;
            }
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.AbominationFollower:
          DataManager.Instance.ForceAbomination = true;
          break;
        case WorldManipulatorManager.Manipulations.JerkFollower:
          possibleFollowers = new List<Follower>();
          foreach (Follower follower in Follower.Followers)
          {
            if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && !follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard))
              possibleFollowers.Add(follower);
          }
          if (possibleFollowers.Count > 0)
          {
            Follower follower = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
            nonLocalisedString = $"<color=#FFD201>{follower.Brain.Info.Name}</color>";
            follower.Brain.AddTrait(FollowerTrait.TraitType.Bastard);
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.BefuddledFollowers:
          possibleFollowers = new List<Follower>();
          foreach (Follower follower in Follower.Followers)
          {
            if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
              possibleFollowers.Add(follower);
          }
          if (possibleFollowers.Count > 0)
          {
            int num4 = Mathf.RoundToInt((float) possibleFollowers.Count / 2f);
            possibleFollowers.Shuffle<Follower>();
            for (int index = 0; index < num4; ++index)
              possibleFollowers[index].Brain.MakeDrunk();
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.PossessedFollower:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerActive((System.Action) (() =>
          {
            possibleFollowers = new List<Follower>();
            foreach (Follower follower in Follower.Followers)
            {
              if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
                possibleFollowers.Add(follower);
            }
            if (possibleFollowers.Count <= 0)
              return;
            Follower follower2 = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
            nonLocalisedString = $"<color=#FFD201>{follower2.Brain.Info.Name}</color>";
            follower2.BecomePossessed((System.Action) null, true);
          }))))));
          break;
        case WorldManipulatorManager.Manipulations.DiscipleDissenter:
          possibleFollowers = new List<Follower>();
          foreach (Follower follower in Follower.Followers)
          {
            if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && follower.Brain.Info.IsDisciple && follower.Brain.Info.CursedState == Thought.None)
              possibleFollowers.Add(follower);
          }
          if (possibleFollowers.Count > 0)
          {
            Follower follower = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
            nonLocalisedString = $"<color=#FFD201>{follower.Brain.Info.Name}</color>";
            follower.Brain.MakeDissenter();
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.PoopNursery:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() =>
          {
            List<Interaction_Daycare> interactionDaycareList = new List<Interaction_Daycare>();
            foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
            {
              if (daycare.Structure.Inventory.Count <= 0 || daycare.Structure.Inventory[0].quantity < 20)
                interactionDaycareList.Add(daycare);
            }
            if (interactionDaycareList.Count <= 0)
              return;
            interactionDaycareList[UnityEngine.Random.Range(0, interactionDaycareList.Count)].MaxOutPoop();
          })));
          break;
        case WorldManipulatorManager.Manipulations.SnorerFollower:
          possibleFollowers = new List<Follower>();
          foreach (Follower follower in Follower.Followers)
          {
            if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && !follower.Brain.HasTrait(FollowerTrait.TraitType.Snorer))
              possibleFollowers.Add(follower);
          }
          if (possibleFollowers.Count > 0)
          {
            Follower follower = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
            nonLocalisedString = $"<color=#FFD201>{follower.Brain.Info.Name}</color>";
            follower.Brain.AddTrait(FollowerTrait.TraitType.Snorer);
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.CursedFollower:
          possibleFollowers = new List<Follower>();
          foreach (Follower follower in Follower.Followers)
          {
            if ((follower.Brain.Info.ID != 99996 || DataManager.Instance.SozoNoLongerBrainwashed) && !FollowerManager.FollowerLocked(follower.Brain.Info.ID, exludeChild: true) && !follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
              possibleFollowers.Add(follower);
          }
          if (possibleFollowers.Count > 0)
          {
            Follower follower = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
            nonLocalisedString = $"<color=#FFD201>{follower.Brain.Info.Name}</color>";
            GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.ConvertToCursed(follower));
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.ChildFollower:
          possibleFollowers = new List<Follower>();
          foreach (Follower follower in Follower.Followers)
          {
            if (!FollowerManager.UniqueFollowerIDs.Contains(follower.Brain.Info.ID) && !FollowerManager.FollowerLocked(follower.Brain.Info.ID))
              possibleFollowers.Add(follower);
          }
          if (possibleFollowers.Count > 0)
          {
            Follower follower = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
            nonLocalisedString = $"<color=#FFD201>{follower.Brain.Info.Name}</color>";
            GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.ConvertToChild(follower));
            break;
          }
          break;
        case WorldManipulatorManager.Manipulations.SpawnDeadlyDish:
          GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => WorldManipulatorManager.SpawnDeadlyMeal())));
          break;
        default:
          return;
      }
      if (!twitch)
        return;
      if (string.IsNullOrEmpty(nonLocalisedString))
        NotificationCentre.Instance.PlayTwitchNotification(WorldManipulatorManager.GetNotification(manipulation), "UI/Twitch/ThanksTwitchChat");
      else
        NotificationCentre.Instance.PlayTwitchNotification(WorldManipulatorManager.GetNotification(manipulation), nonLocalisedString, "UI/Twitch/ThanksTwitchChat");
    })));
  }

  public static IEnumerator WaitTillPlayerIsAtBase(System.Action callback)
  {
    while (PlayerFarming.Location != FollowerLocation.Base && !GameManager.IsDungeon(PlayerFarming.Location))
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static IEnumerator WaitTillPlayerActive(System.Action callback = null)
  {
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static void FreezeTime()
  {
    BiomeGenerator.OnBiomeEnteredCombatRoom -= new BiomeGenerator.BiomeAction(WorldManipulatorManager.FreezeTime);
    BiomeConstants.Instance.FreezeTime();
  }

  public static void FreezeEnemiesOnEnterCombatRoom()
  {
    BiomeGenerator.OnBiomeEnteredCombatRoom -= new BiomeGenerator.BiomeAction(WorldManipulatorManager.FreezeEnemiesOnEnterCombatRoom);
    BiomeConstants.Instance.FreezeAllEnemies(5f);
  }

  public static void CharmEnemiesOnEnterCombatRoom()
  {
    BiomeGenerator.OnBiomeEnteredCombatRoom -= new BiomeGenerator.BiomeAction(WorldManipulatorManager.CharmEnemiesOnEnterCombatRoom);
    BiomeConstants.Instance.CharmAllEnemies();
  }

  public static void OnRoomCleared()
  {
    if (!(DataManager.Instance.CurrentRoomCoordinates != new Vector2((float) BiomeGenerator.Instance.CurrentRoom.x, (float) BiomeGenerator.Instance.CurrentRoom.y)))
      return;
    DataManager.Instance.EnemiesInNextRoomHaveModifiers = false;
    DataManager.Instance.CurrentRoomCoordinates = Vector2.zero;
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(WorldManipulatorManager.OnRoomCleared);
  }

  public static void OnRoomClearedWithoutRoll()
  {
    DataManager.Instance.NoRollInNextCombatRoom = false;
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(WorldManipulatorManager.OnRoomClearedWithoutRoll);
  }

  public static string GetLocalisation(WorldManipulatorManager.Manipulations manipulation)
  {
    return LocalizationManager.GetTranslation($"Manipulations/{manipulation}");
  }

  public static string GetNotification(WorldManipulatorManager.Manipulations manipulation)
  {
    return $"Manipulations/{manipulation}/Notification";
  }

  public static List<WorldManipulatorManager.Manipulations> GetPossibleDungeonPositiveManipulations()
  {
    List<WorldManipulatorManager.Manipulations> ts = new List<WorldManipulatorManager.Manipulations>();
    ts.Add(WorldManipulatorManager.Manipulations.GainRandomHeart);
    ts.Add(WorldManipulatorManager.Manipulations.DealDamageToAllEnemies);
    ts.Add(WorldManipulatorManager.Manipulations.GainTarot);
    ts.Add(WorldManipulatorManager.Manipulations.ReceiveDemon);
    ts.Add(WorldManipulatorManager.Manipulations.InvincibleForTime);
    ts.Add(WorldManipulatorManager.Manipulations.DropRandomWeaponCurse);
    ts.Add(WorldManipulatorManager.Manipulations.NextChestGold);
    ts.Add(WorldManipulatorManager.Manipulations.FreezeTime);
    ts.Add(WorldManipulatorManager.Manipulations.FreezeEnemies);
    ts.Add(WorldManipulatorManager.Manipulations.CharmEnemies);
    if (!DataManager.Instance.EnemiesDropGoldDuringRun)
      ts.Add(WorldManipulatorManager.Manipulations.EnemiesDropGold);
    if ((double) PlayerFarming.Instance.health.HP < (double) PlayerFarming.Instance.health.totalHP)
      ts.Add(WorldManipulatorManager.Manipulations.HealHearts);
    if ((UnityEngine.Object) PlayerFarming.Instance.playerRelic.CurrentRelic != (UnityEngine.Object) null)
    {
      ts.Add(WorldManipulatorManager.Manipulations.DropRandomRelic);
      if ((double) PlayerFarming.Instance.playerRelic.ChargedAmount / (double) PlayerFarming.Instance.playerRelic.RequiredChargeAmount < 0.10000000149011612)
        ts.Add(WorldManipulatorManager.Manipulations.ChargeCurrentRelic);
    }
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && !DataManager.Instance.SpecialAttacksDisabled)
      ts.Add(WorldManipulatorManager.Manipulations.SpecialAttacksDamageIncrease);
    if (DataManager.Instance.UnlockedCorruptedRelicsAndTarots && !TrinketManager.HasTrinket(TarotCards.Card.NoCorruption) && !TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption))
      ts.Add(WorldManipulatorManager.Manipulations.GainCorruptedTarotPositive);
    ts.Shuffle<WorldManipulatorManager.Manipulations>();
    return ts;
  }

  public static List<WorldManipulatorManager.Manipulations> GetPossibleDungeonNegativeManipulations()
  {
    List<WorldManipulatorManager.Manipulations> ts = new List<WorldManipulatorManager.Manipulations>();
    ts.Add(WorldManipulatorManager.Manipulations.TakeDamage);
    ts.Add(WorldManipulatorManager.Manipulations.IncreaseEnemyModifiersChance);
    ts.Add(WorldManipulatorManager.Manipulations.SpawnBombs);
    ts.Add(WorldManipulatorManager.Manipulations.AllEnemiesHaveModifiersInNextRoom);
    ts.Add(WorldManipulatorManager.Manipulations.AllEnemiesHealed);
    ts.Add(WorldManipulatorManager.Manipulations.IncreasedBossesHealth);
    ts.Add(WorldManipulatorManager.Manipulations.NoRollingInNextRoom);
    ts.Add(WorldManipulatorManager.Manipulations.SpawnPoisons);
    if (!DataManager.Instance.SpawnPoisonOnAttack)
      ts.Add(WorldManipulatorManager.Manipulations.DropPoisonOnAttack);
    if (!DataManager.Instance.NoHeartDrops)
      ts.Add(WorldManipulatorManager.Manipulations.NoHeartDrops);
    if (!DataManager.Instance.EnemiesDropBombOnDeath)
      ts.Add(WorldManipulatorManager.Manipulations.BombOnEnemyDeath);
    if (((double) PlayerFarming.Instance.health.BlackHearts > 0.0 || (double) PlayerFarming.Instance.health.TotalSpiritHearts > 0.0 || (double) PlayerFarming.Instance.health.BlueHearts > 0.0 || (double) PlayerFarming.Instance.health.FireHearts > 0.0 || (double) PlayerFarming.Instance.health.IceHearts > 0.0) && DataManager.Instance.PlayerFleece != 5)
      ts.Add(WorldManipulatorManager.Manipulations.LoseAllSpecialHearts);
    if ((UnityEngine.Object) PlayerFarming.Instance.playerRelic.CurrentRelic != (UnityEngine.Object) null)
      ts.Add(WorldManipulatorManager.Manipulations.LoseRelic);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && !DataManager.Instance.SpecialAttacksDisabled)
      ts.Add(WorldManipulatorManager.Manipulations.NoSpecialAttacks);
    if ((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && (MapManager.Instance.CurrentLayer <= 2 || MapGenerator.Nodes.Count == 0))
    {
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Dungeon1_5:
          if (!DataManager.Instance.OnboardedRotstoneDungeon || !DataManager.Instance.OnboardedLightningShardDungeon)
            goto label_16;
          break;
        case FollowerLocation.IntroDungeon:
          goto label_16;
      }
      ts.Add(WorldManipulatorManager.Manipulations.CombatNodes);
    }
label_16:
    if (DataManager.Instance.UnlockedCorruptedRelicsAndTarots && !TrinketManager.HasTrinket(TarotCards.Card.NoCorruption) && !TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption))
      ts.Add(WorldManipulatorManager.Manipulations.GainCorruptedTarotNegative);
    ts.Shuffle<WorldManipulatorManager.Manipulations>();
    return ts;
  }

  public static List<WorldManipulatorManager.Manipulations> GetPossibleBasePositiveManipulations()
  {
    List<WorldManipulatorManager.Manipulations> ts = new List<WorldManipulatorManager.Manipulations>();
    ts.Add(WorldManipulatorManager.Manipulations.GainFaith);
    bool flag1 = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
      {
        flag1 = true;
        break;
      }
    }
    bool flag2 = false;
    for (int index = DataManager.Instance.UpgradeCoolDowns.Count - 1; index >= 0; --index)
    {
      if (!UpgradeSystem.IsRitualActive(DataManager.Instance.UpgradeCoolDowns[index].Type))
      {
        flag2 = true;
        break;
      }
    }
    if (flag2)
      ts.Add(WorldManipulatorManager.Manipulations.ResetTempleCooldowns);
    List<StructureBrain> structureBrainList = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BUILD_SITE));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT));
    if (structureBrainList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.InstantlyBuildStructures);
    List<Structures_Grave> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Grave>(FollowerLocation.Base);
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Dead);
    bool flag3 = false;
    for (int index = followerInfoList.Count - 1; index >= 0; --index)
    {
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        if (structureBrain.Data.FollowerID == followerInfoList[index].ID)
        {
          flag3 = true;
          break;
        }
      }
      if (flag3)
        break;
    }
    if (flag3)
      ts.Add(WorldManipulatorManager.Manipulations.ResurrectBuriedFollower);
    bool flag4 = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.CursedState != Thought.None && allBrain.Info.CursedState != Thought.OldAge && !DataManager.Instance.Followers_Recruit.Contains(allBrain._directInfoAccess))
      {
        flag4 = true;
        break;
      }
    }
    if (flag4)
      ts.Add(WorldManipulatorManager.Manipulations.CureCursedFollowers);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.VOMIT);
    List<Structures_Poop> structuresOfType3 = StructureManager.GetAllStructuresOfType<Structures_Poop>(FollowerLocation.Base);
    if (structuresOfType2.Count > 0 || structuresOfType3.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.ClearAllWaste);
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = followerBrainList.Count - 1; index >= 0; --index)
    {
      if (FollowerManager.FollowerLocked(followerBrainList[index].Info.ID) || (double) followerBrainList[index].Stats.Adoration >= (double) followerBrainList[index].Stats.MAX_ADORATION)
        followerBrainList.RemoveAt(index);
    }
    if (followerBrainList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.FollowerInstantlyLevelled);
    List<Structures_FarmerPlot> structuresFarmerPlotList = new List<Structures_FarmerPlot>((IEnumerable<Structures_FarmerPlot>) StructureManager.GetAllStructuresOfType<Structures_FarmerPlot>());
    for (int index = structuresFarmerPlotList.Count - 1; index >= 0; --index)
    {
      if (!structuresFarmerPlotList[index].HasPlantedSeed() || structuresFarmerPlotList[index].HasFertilized())
        structuresFarmerPlotList.RemoveAt(index);
    }
    if (structuresFarmerPlotList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.CropsInstantlyFertilised);
    if (StructureManager.GetAllStructuresOfType<Structures_Missionary>().Count > 0 && !DataManager.Instance.NextMissionarySuccessful)
      ts.Add(WorldManipulatorManager.Manipulations.MissionarySuccessful);
    List<Structures_Refinery> structuresRefineryList = new List<Structures_Refinery>((IEnumerable<Structures_Refinery>) StructureManager.GetAllStructuresOfType<Structures_Refinery>());
    for (int index = structuresRefineryList.Count - 1; index >= 0; --index)
    {
      if (structuresRefineryList[index].Data.QueuedResources.Count <= 0)
        structuresRefineryList.RemoveAt(index);
    }
    if (structuresRefineryList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.InstantRefinedMaterials);
    if (DataManager.Instance.EndlessModeOnCooldown && TimeManager.CurrentPhase <= DayPhase.Morning)
      ts.Add(WorldManipulatorManager.Manipulations.ResetEndlessModeCooldown);
    if (StructureManager.GetAllStructuresOfType<Structures_Tailor>().Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.CraftRandomOutfit);
    foreach (Structures_Hatchery structuresHatchery in StructureManager.GetAllStructuresOfType<Structures_Hatchery>())
    {
      if (structuresHatchery.Data.HasEgg && !structuresHatchery.Data.EggReady)
      {
        ts.Add(WorldManipulatorManager.Manipulations.EggsHatch);
        break;
      }
    }
    if (StructureManager.GetAllStructuresOfType<Structures_MatingTent>().Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.GoldenEgg);
    List<Structures_Pub> structuresOfType4 = StructureManager.GetAllStructuresOfType<Structures_Pub>();
    if (structuresOfType4.Count > 0 && structuresOfType4[0].FoodStorage.Data.Inventory.Count < structuresOfType4[0].MaxQueue)
      ts.Add(WorldManipulatorManager.Manipulations.FillBrewery);
    if (DataManager.Instance.PleasureEnabled)
      ts.Add(WorldManipulatorManager.Manipulations.FollowerSin);
    if (flag1)
      ts.Add(WorldManipulatorManager.Manipulations.SpecialPoo);
    if (DataManager.Instance.HasBuiltCookingFire)
      ts.Add(WorldManipulatorManager.Manipulations.SpawnGreatMeals);
    if (DataManager.Instance.HasBuiltShrine1 && (double) UnityEngine.Random.value < 0.25)
      ts.Add(WorldManipulatorManager.Manipulations.FillDevotion);
    if (DataManager.Instance.GivenSermonQuest && GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true) && (double) UnityEngine.Random.value < 0.25)
      ts.Add(WorldManipulatorManager.Manipulations.FillSermon);
    if (DataManager.Instance.KnucklebonesIntroCompleted)
      ts.Add(WorldManipulatorManager.Manipulations.LuckyKnucklebones);
    if (DataManager.Instance.HasPerformedRitual)
      ts.Add(WorldManipulatorManager.Manipulations.FreeRitual);
    ts.Shuffle<WorldManipulatorManager.Manipulations>();
    return ts;
  }

  public static List<WorldManipulatorManager.Manipulations> GetPossibleBaseNegativeManipulations()
  {
    List<WorldManipulatorManager.Manipulations> ts = new List<WorldManipulatorManager.Manipulations>();
    if (TimeManager.CurrentPhase == DayPhase.Morning || TimeManager.CurrentPhase == DayPhase.Dawn)
      ts.Add(WorldManipulatorManager.Manipulations.SkipTime);
    bool flag1 = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
      {
        flag1 = true;
        break;
      }
    }
    if (flag1)
    {
      ts.Add(WorldManipulatorManager.Manipulations.AllFollowersPoopOrVomit);
      ts.Add(WorldManipulatorManager.Manipulations.KillRandomFollower);
      if (TimeManager.CurrentPhase != DayPhase.Night || TimeManager.CurrentPhase != DayPhase.Dusk)
        ts.Add(WorldManipulatorManager.Manipulations.SleepFollowers);
    }
    if (FollowerBrain.RandomAvailableBrainNoCurseState(FollowerBrain.AllBrains) != null)
      ts.Add(WorldManipulatorManager.Manipulations.RandomCursedState);
    List<Structures_Bed> structuresBedList = new List<Structures_Bed>((IEnumerable<Structures_Bed>) StructureManager.GetAllStructuresOfType<Structures_Bed>());
    for (int index = structuresBedList.Count - 1; index >= 0; --index)
    {
      if (structuresBedList[index].IsCollapsed || (double) structuresBedList[index].ChanceToCollapse == 0.0)
        structuresBedList.Remove(structuresBedList[index]);
    }
    if (structuresBedList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.BreakAllBeds);
    List<Structures_Outhouse> structuresOuthouseList = new List<Structures_Outhouse>((IEnumerable<Structures_Outhouse>) StructureManager.GetAllStructuresOfType<Structures_Outhouse>());
    for (int index = structuresOuthouseList.Count - 1; index >= 0; --index)
    {
      if (structuresOuthouseList[index].IsFull)
        structuresOuthouseList.RemoveAt(index);
    }
    if (structuresOuthouseList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.ToiletsInstantlyFull);
    List<Structures_Grave> structuresGraveList = new List<Structures_Grave>((IEnumerable<Structures_Grave>) StructureManager.GetAllStructuresOfType<Structures_Grave>());
    for (int index = structuresGraveList.Count - 1; index >= 0; --index)
    {
      if (structuresGraveList[index].Data.FollowerID == -1)
        structuresGraveList.RemoveAt(index);
    }
    if (structuresGraveList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.BodiesOutOfGraves);
    List<Structures_Kitchen> structuresKitchenList = new List<Structures_Kitchen>((IEnumerable<Structures_Kitchen>) StructureManager.GetAllStructuresOfType<Structures_Kitchen>());
    for (int index = structuresKitchenList.Count - 1; index >= 0; --index)
    {
      if (structuresKitchenList[index].Data.QueuedMeals.Count <= 0 && structuresKitchenList[index].Data.QueuedResources.Count <= 0)
        structuresKitchenList.RemoveAt(index);
    }
    if (structuresKitchenList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.MealsVanish);
    List<FollowerBrain> followerBrainList1 = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    int id;
    bool flag2;
    bool flag3;
    bool flag4;
    for (int index = followerBrainList1.Count - 1; index >= 0; --index)
    {
      id = followerBrainList1[index].Info.ID;
      ref int local1 = ref id;
      flag2 = false;
      ref bool local2 = ref flag2;
      // ISSUE: explicit reference operation
      ref bool local3 = @false;
      flag3 = false;
      ref bool local4 = ref flag3;
      // ISSUE: explicit reference operation
      ref bool local5 = @true;
      flag4 = false;
      ref bool local6 = ref flag4;
      if (FollowerManager.FollowerLocked(in local1, in local2, in local3, in local4, in local5, in local6))
        followerBrainList1.RemoveAt(index);
    }
    if (followerBrainList1.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.FollowerLosesLevel);
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
    {
      List<FollowerBrain> followerBrainList2 = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
      for (int index = followerBrainList2.Count - 1; index >= 0; --index)
      {
        id = followerBrainList2[index].Info.ID;
        ref int local7 = ref id;
        flag2 = false;
        ref bool local8 = ref flag2;
        // ISSUE: explicit reference operation
        ref bool local9 = @false;
        flag3 = false;
        ref bool local10 = ref flag3;
        // ISSUE: explicit reference operation
        ref bool local11 = @true;
        flag4 = false;
        ref bool local12 = ref flag4;
        if (FollowerManager.FollowerLocked(in local7, in local8, in local9, in local10, in local11, in local12))
          followerBrainList2.RemoveAt(index);
      }
      if (followerBrainList2.Count > 0)
        ts.Add(WorldManipulatorManager.Manipulations.QuestForMurder);
    }
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Hatchery>())
    {
      if (structureBrain.Data.HasEgg)
      {
        ts.Add(WorldManipulatorManager.Manipulations.AbominationFollower);
        break;
      }
    }
    List<Structures_Pub> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Pub>();
    if (structuresOfType.Count > 0)
    {
      if (structuresOfType[0].FoodStorage.Data.Inventory.Count > 2)
        ts.Add(WorldManipulatorManager.Manipulations.EmptyBrewery);
      ts.Add(WorldManipulatorManager.Manipulations.BefuddledFollowers);
    }
    if (DataManager.Instance.PleasureEnabled)
      ts.Add(WorldManipulatorManager.Manipulations.PossessedFollower);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.IsDisciple && allBrain.Info.CursedState == Thought.None)
      {
        ts.Add(WorldManipulatorManager.Manipulations.DiscipleDissenter);
        break;
      }
    }
    if (flag1)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && !follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard))
        {
          ts.Add(WorldManipulatorManager.Manipulations.JerkFollower);
          break;
        }
      }
    }
    foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
    {
      if (daycare.Structure.Inventory.Count <= 0 || daycare.Structure.Inventory[0].quantity < 20)
      {
        ts.Add(WorldManipulatorManager.Manipulations.PoopNursery);
        break;
      }
    }
    if (flag1)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && !follower.Brain.HasTrait(FollowerTrait.TraitType.Snorer))
        {
          ts.Add(WorldManipulatorManager.Manipulations.SnorerFollower);
          break;
        }
      }
    }
    if (flag1)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, exludeChild: true) && !follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
        {
          ts.Add(WorldManipulatorManager.Manipulations.CursedFollower);
          break;
        }
      }
    }
    if (flag1 && DataManager.Instance.PleasureEnabled)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if (!FollowerManager.UniqueFollowerIDs.Contains(follower.Brain.Info.ID) && !FollowerManager.FollowerLocked(follower.Brain.Info.ID))
        {
          ts.Add(WorldManipulatorManager.Manipulations.ChildFollower);
          break;
        }
      }
    }
    if (DataManager.Instance.HasBuiltCookingFire)
      ts.Add(WorldManipulatorManager.Manipulations.SpawnDeadlyDish);
    ts.Shuffle<WorldManipulatorManager.Manipulations>();
    return ts;
  }

  public static IEnumerator ConvertAllMapNodesIE()
  {
    while (Health.team2.Count > 0 || LetterBox.IsPlaying || MMConversation.isPlaying)
      yield return (object) null;
    while (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving)
      yield return (object) null;
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.ConvertAllNodesToCombatNodes();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
  }

  public static IEnumerator TakeDamageIE(int damage)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      HealthPlayer playerHealth = player.GetComponent<HealthPlayer>();
      while (player.state.CURRENT_STATE == StateMachine.State.InActive || player.state.CURRENT_STATE == StateMachine.State.TimedAction || !playerHealth.DealDamage((float) damage, player.gameObject, player.transform.position, false, Health.AttackTypes.Melee, true, (Health.AttackFlags) 0))
      {
        yield return (object) new WaitForSeconds(1.5f);
        if (!GameManager.IsDungeon(PlayerFarming.Location))
          yield break;
      }
      playerHealth = (HealthPlayer) null;
    }
  }

  public static void SpawnGreatMeals()
  {
    Interaction_Kitchen cookingFire = (Interaction_Kitchen) null;
    foreach (Interaction_Kitchen kitchen in Interaction_Kitchen.Kitchens)
    {
      if (kitchen.structure.Type == StructureBrain.TYPES.COOKING_FIRE)
      {
        cookingFire = kitchen;
        break;
      }
    }
    int num1 = UnityEngine.Random.Range(2, 6);
    InventoryItem.ITEM_TYPE[] allGreatMeals = CookingData.GetAllGreatMeals();
    for (int index = 0; index < num1; ++index)
    {
      int num2 = (int) allGreatMeals[UnityEngine.Random.Range(0, allGreatMeals.Length)];
      InventoryItem.Spawn((InventoryItem.ITEM_TYPE) num2, 1, cookingFire.SpawnMealPosition.position, (float) UnityEngine.Random.Range(9, 11), (Action<PickUp>) (pickUp =>
      {
        Meal component = pickUp.GetComponent<Meal>();
        component.CreateStructureLocation = cookingFire.StructureInfo.Location;
        component.CreateStructureOnStop = true;
      }));
      CookingData.CookedMeal((InventoryItem.ITEM_TYPE) num2);
      ++DataManager.Instance.MealsCooked;
      ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    }
  }

  public static void SpawnDeadlyMeal()
  {
    int num = 1;
    for (int index = 0; index < num; ++index)
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAL_DEADLY, 1, new Vector3(UnityEngine.Random.Range(PlacementRegion.X_Constraints.x + 5f, PlacementRegion.X_Constraints.y - 5f), UnityEngine.Random.Range(PlacementRegion.Y_Constraints.x + 5f, PlacementRegion.Y_Constraints.y - 5f)), (float) UnityEngine.Random.Range(3, 6), (Action<PickUp>) (pickUp =>
      {
        Meal component = pickUp.GetComponent<Meal>();
        component.CreateStructureLocation = FollowerLocation.Base;
        component.CreateStructureOnStop = true;
      }));
      CookingData.CookedMeal(InventoryItem.ITEM_TYPE.MEAL_DEADLY);
      ++DataManager.Instance.MealsCooked;
      ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    }
  }

  public static IEnumerator GiveCorruptedTarotPositiveRoutine()
  {
    while (PlayerFarming.Instance.GoToAndStopping || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive || LetterBox.IsPlaying || MMConversation.isPlaying || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        health.AddFreezeTime();
    }
    Health.isGlobalTimeFreeze = true;
    GameManager.GetInstance().OnConversationNew();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      GameManager.GetInstance().OnConversationNext(player.gameObject);
      TarotCards.TarotCard card = TarotCards.DrawRandomCorruptedCard(player);
      TrinketManager.AddEncounteredTrinket(card, player);
      TrinketManager.AddTrinket(card, player);
      player.CorruptedTrinketsOnlyPositive.Add(card);
      yield return (object) GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.GiveTarotRoutine(card, player));
    }
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        health.ClearFreezeTime();
    }
    Health.isGlobalTimeFreeze = false;
    GameManager.GetInstance().OnConversationEnd();
  }

  public static IEnumerator GiveCorruptedTarotNegativeRoutine()
  {
    while (PlayerFarming.Instance.GoToAndStopping || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive || LetterBox.IsPlaying || MMConversation.isPlaying || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        health.AddFreezeTime();
    }
    Health.isGlobalTimeFreeze = true;
    GameManager.GetInstance().OnConversationNew();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      GameManager.GetInstance().OnConversationNext(player.gameObject);
      TarotCards.TarotCard card = TarotCards.DrawRandomCorruptedCard(player);
      TrinketManager.AddEncounteredTrinket(card, player);
      TrinketManager.AddTrinket(card, player);
      player.CorruptedTrinketsOnlyNegative.Add(card);
      yield return (object) GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.GiveTarotRoutine(card, player));
    }
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        health.ClearFreezeTime();
    }
    Health.isGlobalTimeFreeze = false;
    GameManager.GetInstance().OnConversationEnd();
  }

  public static IEnumerator GiveTarotRoutine(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.facingAngle = -90f;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: playerFarming);
    yield return (object) new WaitForSecondsRealtime(0.35f);
    HUD_Manager.Instance.Hide(false, 0);
    LetterBox.Show(false);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", playerFarming.gameObject);
    playerFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
    UITarotPickUpOverlayController pickUpUI = MonoSingleton<UIManager>.Instance.ShowTarotPickUp(card);
    UITarotPickUpOverlayController overlayController = pickUpUI;
    overlayController.OnHide = overlayController.OnHide + (System.Action) (() => pickUpUI = (UITarotPickUpOverlayController) null);
    while ((UnityEngine.Object) pickUpUI != (UnityEngine.Object) null)
      yield return (object) null;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", playerFarming.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    yield return (object) null;
    playerFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.2f);
    playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    if (CoopManager.CoopActive)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) playerFarming)
          player.state.CURRENT_STATE = StateMachine.State.Idle;
      }
    }
  }

  public static IEnumerator ConvertToChild(Follower follower)
  {
    yield return (object) GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerActive());
    GameManager.GetInstance().OnConversationNew(PlayerFarming.Instance);
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Indoctrinate/indoctrinate-finish", false);
    yield return (object) new WaitForSeconds(1.1f);
    Vector3 position = follower.transform.position + Vector3.back * 1.2f;
    BiomeConstants.Instance.SpawnPuffEffect(position, follower.transform.parent);
    yield return (object) new WaitForSeconds(0.1f);
    follower.Brain.ResetStats();
    follower.Brain.MakeChild();
    yield return (object) new WaitForSeconds(2.8f);
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    follower.Brain.CompleteCurrentTask();
    follower.Brain.CheckChangeState();
    GameManager.GetInstance().OnConversationEnd();
  }

  public static IEnumerator ConvertToCursed(Follower follower)
  {
    yield return (object) GameManager.GetInstance().StartCoroutine(WorldManipulatorManager.WaitTillPlayerActive());
    GameManager.GetInstance().OnConversationNew(PlayerFarming.Instance);
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    follower.Spine.AnimationState.SetAnimation(1, "Zombie/zombie-resurrect", false).AnimationStart = 0.5f;
    yield return (object) new WaitForSeconds(2.1f);
    Vector3 position = follower.transform.position + Vector3.back * 2f;
    BiomeConstants.Instance.SpawnCursedPuffEffect(position, follower.transform.parent);
    yield return (object) new WaitForSeconds(3f);
    bool flag = false;
    FollowerState currentState = follower.Brain.CurrentState;
    if ((currentState != null ? (currentState.Type == FollowerStateType.Child ? 1 : 0) : 0) != 0)
    {
      follower.Brain.CurrentState = (FollowerState) new FollowerState_ChildZombie();
      flag = true;
    }
    else
      follower.Brain.CurrentState = (FollowerState) new FollowerState_Zombie();
    follower.Brain.ResetStats();
    follower.Brain.AddTrait(FollowerTrait.TraitType.Zombie);
    if (flag)
      follower.Brain.ApplyCurseState(Thought.Child);
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    follower.Brain.CompleteCurrentTask();
    GameManager.GetInstance().OnConversationEnd();
  }

  public static IEnumerator DropRandomWeaponRelic(WorldManipulatorManager.Manipulations manipulation)
  {
    while (PlayerFarming.AnyPlayerGotoAndStopping() || !GameManager.RoomActive)
      yield return (object) null;
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Weapon Choice.prefab", PlayerFarming.Instance.transform.position, Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Interaction_WeaponSelectionPodium component = obj.Result.GetComponent<Interaction_WeaponSelectionPodium>();
      if (manipulation == WorldManipulatorManager.Manipulations.DropRandomRelic)
        component.Type = Interaction_WeaponSelectionPodium.Types.Relic;
      component.CanOpenDoors = false;
      component.Reveal();
    }));
  }

  public enum Manipulations
  {
    GainRandomHeart = 0,
    HealHearts = 1,
    GainTarot = 2,
    DealDamageToAllEnemies = 3,
    ReceiveDemon = 4,
    InvincibleForTime = 5,
    DropRandomWeaponCurse = 6,
    ChargeCurrentRelic = 7,
    SpecialAttacksDamageIncrease = 8,
    DropRandomRelic = 9,
    NextChestGold = 10, // 0x0000000A
    FreezeTime = 11, // 0x0000000B
    EnemiesDropGold = 12, // 0x0000000C
    GainCorruptedTarotPositive = 13, // 0x0000000D
    FreezeEnemies = 14, // 0x0000000E
    CharmEnemies = 15, // 0x0000000F
    TakeDamage = 100, // 0x00000064
    IncreaseEnemyModifiersChance = 101, // 0x00000065
    SpawnBombs = 102, // 0x00000066
    LoseAllSpecialHearts = 103, // 0x00000067
    DropPoisonOnAttack = 104, // 0x00000068
    AllEnemiesHaveModifiersInNextRoom = 105, // 0x00000069
    LoseRelic = 106, // 0x0000006A
    NoSpecialAttacks = 107, // 0x0000006B
    AllEnemiesHealed = 108, // 0x0000006C
    IncreasedBossesHealth = 109, // 0x0000006D
    CombatNodes = 110, // 0x0000006E
    GainCorruptedTarotNegative = 111, // 0x0000006F
    NoRollingInNextRoom = 112, // 0x00000070
    NoHeartDrops = 113, // 0x00000071
    BombOnEnemyDeath = 114, // 0x00000072
    SpawnPoisons = 115, // 0x00000073
    ResetTempleCooldowns = 200, // 0x000000C8
    InstantlyBuildStructures = 201, // 0x000000C9
    GainFaith = 202, // 0x000000CA
    ResurrectBuriedFollower = 203, // 0x000000CB
    CureCursedFollowers = 204, // 0x000000CC
    ClearAllWaste = 205, // 0x000000CD
    FollowerInstantlyLevelled = 206, // 0x000000CE
    CropsInstantlyFertilised = 207, // 0x000000CF
    MissionarySuccessful = 208, // 0x000000D0
    InstantRefinedMaterials = 209, // 0x000000D1
    ResetEndlessModeCooldown = 210, // 0x000000D2
    CraftRandomOutfit = 211, // 0x000000D3
    EggsHatch = 212, // 0x000000D4
    GoldenEgg = 213, // 0x000000D5
    SpecialPoo = 214, // 0x000000D6
    FillBrewery = 215, // 0x000000D7
    FollowerSin = 216, // 0x000000D8
    SpawnGreatMeals = 217, // 0x000000D9
    FillDevotion = 218, // 0x000000DA
    FillSermon = 219, // 0x000000DB
    LuckyKnucklebones = 220, // 0x000000DC
    FreeRitual = 221, // 0x000000DD
    AllFollowersPoopOrVomit = 300, // 0x0000012C
    BreakAllBeds = 301, // 0x0000012D
    SkipTime = 302, // 0x0000012E
    SleepFollowers = 303, // 0x0000012F
    RandomCursedState = 304, // 0x00000130
    KillRandomFollower = 305, // 0x00000131
    ToiletsInstantlyFull = 306, // 0x00000132
    BodiesOutOfGraves = 307, // 0x00000133
    MealsVanish = 308, // 0x00000134
    FollowerLosesLevel = 309, // 0x00000135
    QuestForMurder = 310, // 0x00000136
    EmptyBrewery = 311, // 0x00000137
    AbominationFollower = 312, // 0x00000138
    JerkFollower = 313, // 0x00000139
    BefuddledFollowers = 314, // 0x0000013A
    PossessedFollower = 315, // 0x0000013B
    DiscipleDissenter = 316, // 0x0000013C
    PoopNursery = 317, // 0x0000013D
    SnorerFollower = 318, // 0x0000013E
    CursedFollower = 319, // 0x0000013F
    ChildFollower = 320, // 0x00000140
    SpawnDeadlyDish = 321, // 0x00000141
  }
}
