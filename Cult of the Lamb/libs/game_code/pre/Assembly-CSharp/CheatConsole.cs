// Decompiled with JetBrains decompiler
// Type: CheatConsole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using CodeStage.AdvancedFPSCounter;
using Data.ReadWrite.Conversion;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.MainMenu;
using Map;
using MMBiomeGeneration;
using MMTools;
using src.Extensions;
using src.UI.Menus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unify;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable disable
public class CheatConsole : BaseMonoBehaviour
{
  private static bool _inDemo = false;
  public const float DEMO_ATTRACT_MODE_TIMER = 20f;
  public const float DEMO_INACTIVE_TIMER = 120f;
  public const float DEMO_HOLD_TO_RESET_TIMER = 5f;
  public const float DEMO_MAX_TIMER = 1200f;
  public static float DemoBeginTime = 0.0f;
  public static bool WIREFRAME_ENABLED = false;
  public Text text;
  public Text backgroundText;
  public Text autoCompleteItemText;
  public static CheatConsole.Phase CurrentPhase;
  private float Timer;
  public static CheatConsole.FaithTypes FaithType = CheatConsole.FaithTypes.DRIP;
  private List<Text> autoCompleteItems = new List<Text>();
  public static GameObject[] resourcesToSpawn;
  public static GameObject SubmitReportPrefab;
  public static bool ShowAllMapLocations = false;
  public static bool Robes = false;
  public static bool ForceSpiderMiniBoss = false;
  public static bool UglyWeeds = false;
  public static bool ForceSmoochEnabled = false;
  public static bool ForceBlessEnabled = false;
  public static bool QuickUnlock = false;
  public Dictionary<string, System.Action> Cheats = new Dictionary<string, System.Action>()
  {
    {
      "GOG_TEST",
      (System.Action) (() => AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_SKINS_UNLOCKED")))
    },
    {
      "GOG_RESET",
      (System.Action) (() => { })
    },
    {
      "SPEED100",
      (System.Action) (() => Time.timeScale = 100f)
    },
    {
      "SPEED1",
      (System.Action) (() => Time.timeScale = 1f)
    },
    {
      "LOG",
      (System.Action) (() => { })
    },
    {
      "W1",
      (System.Action) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_DefeatKnucklebones(ScriptLocalization.UI_Knucklebones.Knucklebones, "NAMES/Knucklebones/Knucklebones_NPC_1")))
    },
    {
      "WW1",
      (System.Action) (() => ObjectiveManager.CompleteDefeatKnucklebones("NAMES/Knucklebones/Knucklebones_NPC_1"))
    },
    {
      "W2",
      (System.Action) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_DefeatKnucklebones(ScriptLocalization.UI_Knucklebones.Knucklebones, "NAMES/Knucklebones/Knucklebones_NPC_2")))
    },
    {
      "WW2",
      (System.Action) (() => ObjectiveManager.CompleteDefeatKnucklebones("NAMES/Knucklebones/Knucklebones_NPC_2"))
    },
    {
      "W3",
      (System.Action) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_DefeatKnucklebones(ScriptLocalization.UI_Knucklebones.Knucklebones, "NAMES/Ratau")))
    },
    {
      "WW3",
      (System.Action) (() => ObjectiveManager.CompleteDefeatKnucklebones("NAMES/Ratau"))
    },
    {
      "R",
      (System.Action) (() =>
      {
        --DataManager.Instance.CurrentRunWeaponLevel;
        --DataManager.Instance.CurrentRunCurseLevel;
        foreach (Interaction_WeaponSelectionPodium weaponSelectionPodium in UnityEngine.Object.FindObjectsOfType<Interaction_WeaponSelectionPodium>())
          weaponSelectionPodium.ResetRandom();
      })
    },
    {
      "L",
      (System.Action) (() =>
      {
        ++DataManager.Instance.CurrentRunWeaponLevel;
        ++DataManager.Instance.CurrentRunCurseLevel;
        foreach (Interaction_WeaponSelectionPodium weaponSelectionPodium in UnityEngine.Object.FindObjectsOfType<Interaction_WeaponSelectionPodium>())
          weaponSelectionPodium.ResetRandom();
      })
    },
    {
      "LL",
      (System.Action) (() =>
      {
        CheatConsole.AddHeart();
        CheatConsole.Heal();
        CheatConsole.UnlockWeapons();
        UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Sword, 16 /*0x10*/);
        UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Fireball, 16 /*0x10*/);
        foreach (RoomLockController roomLockController in RoomLockController.RoomLockControllers)
        {
          roomLockController.DoorDown();
          roomLockController.Completed = true;
        }
      })
    },
    {
      "DEMO",
      new System.Action(CheatConsole.EnableDemo)
    },
    {
      "DEMO_OFF",
      (System.Action) (() => CheatConsole.IN_DEMO = false)
    },
    {
      "DOCTRINE",
      (System.Action) (() =>
      {
        UnityEngine.Object.FindObjectOfType<PlayerDoctrineStone>().GivePiece(3);
        DataManager.Instance.SetVariable(DataManager.Variables.FirstDoctrineStone, true);
      })
    },
    {
      "FEAST",
      (System.Action) (() => DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast))
    },
    {
      "AFTERLIFE",
      (System.Action) (() => DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitDesensitisedToDeath))
    },
    {
      "JAY",
      (System.Action) (() =>
      {
        Debug.Log((object) ("MMConversation.IsPlaying: " + MMConversation.isPlaying.ToString()));
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PerformRitual);
      })
    },
    {
      "DLC_PRE_PURCHASE",
      (System.Action) (() =>
      {
        DataManager.ActivatePrePurchaseDLC();
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CthuluPack");
      })
    },
    {
      "DLC_CULTIST",
      (System.Action) (() =>
      {
        DataManager.ActivateCultistDLC();
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CultistEdition");
      })
    },
    {
      "DLC_PLUSH",
      (System.Action) (() => DataManager.ActivatePlushBonusDLC())
    },
    {
      "Q",
      (System.Action) (() =>
      {
        ResurrectOnHud.ResurrectionType = ResurrectionType.Pyre;
        CheatConsole.CreateFollower(20);
        FollowerBrain.AllBrains[0].MakeDissenter();
        FollowerBrain.AllBrains[1].MakeOld();
        FollowerBrain.AllBrains[2].MakeSick();
        FollowerBrain.AllBrains[3].MakeExhausted();
        FollowerBrain.AllBrains[4].MakeStarve();
        FollowerBrain.AllBrains[5].Info.MarriedToLeader = true;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          allBrain.DesiredLocation = FollowerLocation.Base;
          allBrain.Info.XPLevel = UnityEngine.Random.Range(1, 10);
        }
      })
    },
    {
      "MUSHROOMRITUAL",
      (System.Action) (() =>
      {
        if (!DataManager.Instance.PerformedMushroomRitual)
        {
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Objectives.CustomQuestTypes.SozoReturn));
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoPerformRitual);
        }
        DataManager.Instance.PerformedMushroomRitual = true;
      })
    },
    {
      "FAITH",
      (System.Action) (() => CultFaithManager.AddThought(Thought.TestPositive))
    },
    {
      "FAITHNEGATIVE",
      (System.Action) (() => CultFaithManager.AddThought(Thought.TestNegative))
    },
    {
      "ILLNESS",
      (System.Action) (() =>
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          allBrain.MakeSick();
      })
    },
    {
      "HUNGER",
      (System.Action) (() =>
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          allBrain.Stats.Satiation = 0.0f;
      })
    },
    {
      "JIMP",
      (System.Action) (() => DataManager.Instance.CookedFirstFood = true)
    },
    {
      "SPELLSENABLED",
      (System.Action) (() => DataManager.Instance.EnabledSpells = true)
    },
    {
      "JULIAN",
      (System.Action) (() => DataManager.Instance.ShowLoyaltyBars = true)
    },
    {
      "LOYALTY",
      (System.Action) (() => DataManager.Instance.ShowLoyaltyBars = true)
    },
    {
      "WILL",
      (System.Action) (() => NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/DissenterLeavingTomorrow", "Brian", "200"))
    },
    {
      "DRIP",
      (System.Action) (() => CheatConsole.FaithType = CheatConsole.FaithTypes.DRIP)
    },
    {
      "DISCIPLEPOINT",
      (System.Action) (() => PlayerFarming.Instance.GetDisciple(1000f))
    },
    {
      "REMOVEILLNESS",
      (System.Action) (() =>
      {
        DataManager.Instance.OnboardedSickFollower = false;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          allBrain.Stats.Illness = 0.0f;
          FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
          if (illnessStateChanged != null)
            illnessStateChanged(allBrain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        }
      })
    },
    {
      "GOLD",
      (System.Action) (() => Inventory.AddItem(20, 500))
    },
    {
      "BEHOLDEREYE",
      (System.Action) (() => Inventory.AddItem(101, 4))
    },
    {
      "NIGHTFOX0",
      (System.Action) (() => DataManager.Instance.CurrentFoxEncounter = 0)
    },
    {
      "NIGHTFOX1",
      (System.Action) (() => DataManager.Instance.CurrentFoxEncounter = 1)
    },
    {
      "NIGHTFOX2",
      (System.Action) (() => DataManager.Instance.CurrentFoxEncounter = 2)
    },
    {
      "NIGHTFOX3",
      (System.Action) (() => DataManager.Instance.CurrentFoxEncounter = 3)
    },
    {
      "VIDEO",
      (System.Action) (() => ScreenshotCamera.Instance.ExportVideoCV())
    },
    {
      "DEATHCATFIGHTPACK",
      (System.Action) (() =>
      {
        CheatConsole.UnlockAll();
        CheatConsole.AllCurses();
        PlayerFarming.Instance.health.totalHP += 6f;
        PlayerFarming.Instance.health.HP = PlayerFarming.Instance.health.totalHP;
        CheatConsole.CreateFollower(20);
      })
    },
    {
      "UNLOCKTELEPORTER",
      (System.Action) (() =>
      {
        CheatConsole.ShowAllMapLocations = true;
        DataManager.Instance.UnlockBaseTeleporter = true;
      })
    },
    {
      "E",
      (System.Action) (() => MonoSingleton<UIManager>.Instance.InventoryPromptTemplate.Instantiate<UIInventoryPromptOverlay>())
    },
    {
      "FLEECE0",
      (System.Action) (() => PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + (DataManager.Instance.PlayerFleece = 0).ToString()))
    },
    {
      "FLEECE1",
      (System.Action) (() => PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + (DataManager.Instance.PlayerFleece = 1).ToString()))
    },
    {
      "FLEECE2",
      (System.Action) (() => PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + (DataManager.Instance.PlayerFleece = 2).ToString()))
    },
    {
      "FLEECE3",
      (System.Action) (() => PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + (DataManager.Instance.PlayerFleece = 3).ToString()))
    },
    {
      "FLEECE4",
      (System.Action) (() => PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + (DataManager.Instance.PlayerFleece = 4).ToString()))
    },
    {
      "FLEECE5",
      (System.Action) (() => PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + (DataManager.Instance.PlayerFleece = 5).ToString()))
    },
    {
      "MANIPULATION",
      (System.Action) (() => WorldManipulatorManager.TriggerManipulation(WorldManipulatorManager.Manipulations.DropPoisonOnAttack, twitch: true))
    },
    {
      "RESETCAMERA",
      (System.Action) (() =>
      {
        GameManager.GetInstance().RemoveAllFromCamera();
        GameManager.GetInstance().AddPlayerToCamera();
      })
    },
    {
      "SIMULATION",
      (System.Action) (() => Debug.Log((object) ("Paused?  " + SimulationManager.IsPaused.ToString())))
    },
    {
      "UNPAUSE",
      (System.Action) (() => SimulationManager.UnPause())
    },
    {
      "DODGETUTORIAL",
      (System.Action) (() =>
      {
        DataManager.Instance.ShownDodgeTutorialCount = 0;
        DataManager.Instance.ShownDodgeTutorial = false;
      })
    },
    {
      "DUNGEONRUN",
      (System.Action) (() => Debug.Log((object) ("DUNGEON RUN: " + (object) DataManager.Instance.dungeonRun)))
    },
    {
      "FCOUNT",
      (System.Action) (() => Debug.Log((object) DataManager.Instance.Followers.Count))
    },
    {
      "MINE2",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Depreciated5))
    },
    {
      "CHOP",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Temple_DonationBox))
    },
    {
      "CHOP2",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Depreciated4))
    },
    {
      "FORAGE",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Depreciated3))
    },
    {
      "FORAGE2",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Depreciated6))
    },
    {
      "RESURRECT",
      (System.Action) (() =>
      {
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ability_Resurrection);
        ResurrectOnHud.ResurrectionType = ResurrectionType.Pyre;
      })
    },
    {
      "OMNI",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ability_TeleportHome))
    },
    {
      "BLACKHEART_ABILITY",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ability_BlackHeart))
    },
    {
      "CHECKDOCTRINE",
      (System.Action) (() =>
      {
        foreach (int doctrineUnlockedUpgrade in DataManager.Instance.DoctrineUnlockedUpgrades)
          Debug.Log((object) (DoctrineUpgradeSystem.DoctrineType) doctrineUnlockedUpgrade);
      })
    },
    {
      "P",
      (System.Action) (() => BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "strength", "strength"))
    },
    {
      "TEMPLE",
      (System.Action) (() =>
      {
        BiomeBaseManager.Instance.ToggleChurch();
        PlayerFarming.Instance.transform.position = ChurchFollowerManager.Instance.RitualCenterPosition.position;
      })
    },
    {
      "ASCEND",
      (System.Action) (() => DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.LawOrder_AscendFollower))
    },
    {
      "EMPTYINVENTORY",
      (System.Action) (() => Inventory.items.Clear())
    },
    {
      "EXTORT",
      (System.Action) (() => DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes))
    },
    {
      "BRIBE",
      (System.Action) (() => DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.Possessions_Bribe))
    },
    {
      "INSPIRE",
      (System.Action) (() =>
      {
        DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire);
        DataManager.Instance.DoctrineUnlockedUpgrades.Remove(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate);
      })
    },
    {
      "INTIMIDATE",
      (System.Action) (() =>
      {
        DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate);
        DataManager.Instance.DoctrineUnlockedUpgrades.Remove(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire);
      })
    },
    {
      "SMOOCH",
      (System.Action) (() => CheatConsole.ForceSmoochEnabled = true)
    },
    {
      "BLESS",
      (System.Action) (() =>
      {
        CheatConsole.ForceBlessEnabled = true;
        DataManager.Instance.DoctrineUnlockedUpgrades.Remove(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire);
        DataManager.Instance.DoctrineUnlockedUpgrades.Remove(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate);
      })
    },
    {
      "FAITHFUL",
      (System.Action) (() => DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.WorkWorship_FaithfulTrait))
    },
    {
      "GOOD WORKER",
      (System.Action) (() => DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.WorkWorship_GoodWorkerTrait))
    },
    {
      "POSITION",
      (System.Action) (() =>
      {
        PlayerFarming.Instance.transform.position = new Vector3(0.3f, -13.5f, 0.0f);
        PlayerFarming.Instance.state.facingAngle = -90f;
        GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
      })
    },
    {
      "UGLYWEEDS",
      (System.Action) (() => CheatConsole.UglyWeeds = !CheatConsole.UglyWeeds)
    },
    {
      "SPIDERS",
      (System.Action) (() => DataManager.Instance.SpidersCaught += 20)
    },
    {
      "CANNIBAL",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.Cannibal))
    },
    {
      "GRASS",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.GrassEater))
    },
    {
      "D",
      (System.Action) (() => UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/UI/UI Unlock Curse"), GameObject.FindWithTag("Canvas").transform))
    },
    {
      "TWITCH_HH_START",
      (System.Action) (() => TwitchHelpHinder.StartHHEvent(GameManager.IsDungeon(PlayerFarming.Location)))
    },
    {
      "TWITCH_TUTORIAL",
      (System.Action) (() =>
      {
        if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Twitch))
          return;
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Twitch);
      })
    },
    {
      "KF",
      (System.Action) (() =>
      {
        Follower follower1 = (Follower) null;
        float num1 = float.MaxValue;
        foreach (Follower follower2 in Follower.Followers)
        {
          float num2 = Vector3.Distance(PlayerFarming.Instance.transform.position, follower2.transform.position);
          if ((double) num2 < (double) num1 && follower2.Brain.Info.CursedState == Thought.None)
          {
            num1 = num2;
            follower1 = follower2;
          }
        }
        follower1.Die();
      })
    },
    {
      "KA",
      (System.Action) (() =>
      {
        for (int index = Follower.Followers.Count - 1; index >= 0; --index)
          Follower.Followers[index].Die();
      })
    },
    {
      "OLDAGE",
      (System.Action) (() =>
      {
        Follower follower3 = (Follower) null;
        float num3 = float.MaxValue;
        foreach (Follower follower4 in Follower.Followers)
        {
          float num4 = Vector3.Distance(PlayerFarming.Instance.transform.position, follower4.transform.position);
          if ((double) num4 < (double) num3)
          {
            num3 = num4;
            follower3 = follower4;
          }
        }
        follower3.Brain.Info.Age = follower3.Brain.Info.LifeExpectancy;
        follower3.Brain.ApplyCurseState(Thought.OldAge);
      })
    },
    {
      "DISCIPLE",
      (System.Action) (() =>
      {
        Follower follower5 = (Follower) null;
        float num5 = float.MaxValue;
        foreach (Follower follower6 in Follower.Followers)
        {
          float num6 = Vector3.Distance(PlayerFarming.Instance.transform.position, follower6.transform.position);
          if ((double) num6 < (double) num5 && !follower6.Brain.Stats.HasLevelledUp)
          {
            num5 = num6;
            follower5 = follower6;
          }
        }
        follower5.Brain.AddAdoration(FollowerBrain.AdorationActions.Quest, (System.Action) null);
        follower5.Brain.AddAdoration(FollowerBrain.AdorationActions.Quest, (System.Action) null);
        follower5.Brain.AddAdoration(FollowerBrain.AdorationActions.Quest, (System.Action) null);
      })
    },
    {
      "ZOMBIE",
      (System.Action) (() =>
      {
        Follower follower7 = (Follower) null;
        float num7 = float.MaxValue;
        foreach (Follower follower8 in Follower.Followers)
        {
          float num8 = Vector3.Distance(PlayerFarming.Instance.transform.position, follower8.transform.position);
          if ((double) num8 < (double) num7 && follower8.Brain.Info.CursedState == Thought.None)
          {
            num7 = num8;
            follower7 = follower8;
          }
        }
        follower7.Brain.ApplyCurseState(Thought.Zombie);
      })
    },
    {
      "VOMIT",
      (System.Action) (() =>
      {
        Follower follower9 = (Follower) null;
        float num9 = float.MaxValue;
        foreach (Follower follower10 in Follower.Followers)
        {
          float num10 = Vector3.Distance(PlayerFarming.Instance.transform.position, follower10.transform.position);
          if ((double) num10 < (double) num9)
          {
            num9 = num10;
            follower9 = follower10;
          }
        }
        follower9.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
      })
    },
    {
      "BUILDPOOP",
      (System.Action) (() => StructureManager.BuildStructure(PlayerFarming.Location, StructuresData.GetInfoByType(StructureBrain.TYPES.POOP, 0), PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f, Vector2Int.one, false))
    },
    {
      "BUILDVOMIT",
      (System.Action) (() => StructureManager.BuildStructure(PlayerFarming.Location, StructuresData.GetInfoByType(StructureBrain.TYPES.VOMIT, 0), PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f, Vector2Int.one, false))
    },
    {
      "DOPOOP",
      (System.Action) (() =>
      {
        for (int index = 0; index < Follower.Followers.Count; ++index)
          Follower.Followers[index].Brain.Stats.Bathroom = 30f;
      })
    },
    {
      "DISSENT",
      (System.Action) (() =>
      {
        foreach (Follower follower in Follower.Followers)
          follower.Brain.MakeDissenter();
      })
    },
    {
      "FAKELEISURE",
      (System.Action) (() =>
      {
        Follower follower11 = (Follower) null;
        float num11 = float.MaxValue;
        foreach (Follower follower12 in Follower.Followers)
        {
          float num12 = Vector3.Distance(PlayerFarming.Instance.transform.position, follower12.transform.position);
          if ((double) num12 < (double) num11)
          {
            num11 = num12;
            follower11 = follower12;
          }
        }
        if (!(bool) (UnityEngine.Object) follower11)
          return;
        follower11.Brain.HardSwapToTask((FollowerTask) new FollowerTask_FakeLeisure());
      })
    },
    {
      "CLEARDISSENTS",
      (System.Action) (() =>
      {
        foreach (Follower follower in Follower.Followers)
        {
          if (follower.Brain.Info.CursedState == Thought.Dissenter)
            follower.Brain.RemoveCurseState(Thought.Dissenter);
        }
      })
    },
    {
      "SPIDER",
      (System.Action) (() => CheatConsole.ForceSpiderMiniBoss = !CheatConsole.ForceSpiderMiniBoss)
    },
    {
      "ALLMAP",
      (System.Action) (() =>
      {
        DataManager.Instance.DiscoveredLocations.Add(FollowerLocation.HubShore);
        DataManager.Instance.DiscoveredLocations.Add(FollowerLocation.Hub1_Sozo);
        DataManager.Instance.DiscoveredLocations.Add(FollowerLocation.Dungeon_Decoration_Shop1);
        DataManager.Instance.DiscoveredLocations.Add(FollowerLocation.Dungeon_Location_4);
        DataManager.Instance.DiscoveredLocations.Add(FollowerLocation.Hub1_RatauOutside);
      })
    },
    {
      "WIREFRAME",
      (System.Action) (() => CheatConsole.WIREFRAME_ENABLED = !CheatConsole.WIREFRAME_ENABLED)
    },
    {
      "TIME1",
      (System.Action) (() => Time.timeScale = 1f)
    },
    {
      "TIME2",
      (System.Action) (() => Time.timeScale = 3f)
    },
    {
      "TIME3",
      (System.Action) (() => Time.timeScale = 3f)
    },
    {
      "TIME4",
      (System.Action) (() => Time.timeScale = 4f)
    },
    {
      "TIME5",
      (System.Action) (() => Time.timeScale = 5f)
    },
    {
      "TIME6",
      (System.Action) (() => Time.timeScale = 6f)
    },
    {
      "TIME7",
      (System.Action) (() => Time.timeScale = 7f)
    },
    {
      "TIME8",
      (System.Action) (() => Time.timeScale = 8f)
    },
    {
      "TIME20",
      (System.Action) (() => Time.timeScale = 20f)
    },
    {
      "TIME40",
      (System.Action) (() => Time.timeScale = 40f)
    },
    {
      "MURDER",
      (System.Action) (() => DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
    },
    {
      "GIFTS",
      (System.Action) (() =>
      {
        Inventory.AddItem(45, 5);
        Inventory.AddItem(46, 5);
        Inventory.AddItem(47, 5);
        Inventory.AddItem(48 /*0x30*/, 5);
        Inventory.AddItem(49, 5);
        Inventory.AddItem(43, 5);
        Inventory.AddItem(44, 5);
      })
    },
    {
      "TUTORIAL",
      (System.Action) (() => DataManager.Instance.SetTutorialVariables())
    },
    {
      "TUTORIAL2",
      (System.Action) (() =>
      {
        DataManager.Instance.SetTutorialVariables();
        DataManager.Instance.InTutorial = true;
      })
    },
    {
      "INTROSPEED",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerPrisonerController>().MaxSpeed = 15f)
    },
    {
      "REGIONS",
      (System.Action) (() =>
      {
        Interactor.UseRegions = !Interactor.UseRegions;
        Debug.Log((object) ("Interactor using regions: " + Interactor.UseRegions.ToString()));
      })
    },
    {
      "CHECKRES",
      (System.Action) (() => Debug.Log((object) ("ResurrectOnHud.ResurrectionType: " + (object) ResurrectOnHud.ResurrectionType)))
    },
    {
      "CHECKTIMEPAUSED",
      (System.Action) (() => Debug.Log((object) ("time paused: " + TimeManager.PauseGameTime.ToString())))
    },
    {
      "UNLOCK",
      (System.Action) (() => CheatConsole.AllBuildingsUnlocked = !CheatConsole.AllBuildingsUnlocked)
    },
    {
      "ALLBUILDINGS",
      (System.Action) (() => CheatConsole.AllBuildingsUnlocked = !CheatConsole.AllBuildingsUnlocked)
    },
    {
      "ALLSTRUCTURES",
      (System.Action) (() => CheatConsole.AllBuildingsUnlocked = !CheatConsole.AllBuildingsUnlocked)
    },
    {
      "UNLOCKBUILDINGS",
      (System.Action) (() => CheatConsole.AllBuildingsUnlocked = !CheatConsole.AllBuildingsUnlocked)
    },
    {
      "UNLOCKSTRUCTURES",
      (System.Action) (() => CheatConsole.AllBuildingsUnlocked = !CheatConsole.AllBuildingsUnlocked)
    },
    {
      "RELOAD",
      (System.Action) (() => SceneManager.LoadScene(SceneManager.GetActiveScene().name))
    },
    {
      "F10",
      (System.Action) (() => CheatConsole.CreateFollower(10))
    },
    {
      "F20",
      (System.Action) (() => CheatConsole.CreateFollower(20))
    },
    {
      "BOSS",
      (System.Action) (() =>
      {
        if (DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
          return;
        DataManager.Instance.BossesCompleted.Add(PlayerFarming.Location);
      })
    },
    {
      "BOSS1",
      (System.Action) (() =>
      {
        if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
          return;
        DataManager.Instance.BossesCompleted.Add(FollowerLocation.Dungeon1_1);
      })
    },
    {
      "BOSS2",
      (System.Action) (() =>
      {
        if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2))
          return;
        DataManager.Instance.BossesCompleted.Add(FollowerLocation.Dungeon1_2);
      })
    },
    {
      "BOSS3",
      (System.Action) (() =>
      {
        if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3))
          return;
        DataManager.Instance.BossesCompleted.Add(FollowerLocation.Dungeon1_3);
      })
    },
    {
      "BOSS4",
      (System.Action) (() =>
      {
        if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
          return;
        DataManager.Instance.BossesCompleted.Add(FollowerLocation.Dungeon1_4);
      })
    },
    {
      "TENTACLE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Tentacles, 1))
    },
    {
      "TENTACLEICE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Tentacles_Ice, 1))
    },
    {
      "TENTACLENECROMANCY",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Tentacles_Necromancy, 1))
    },
    {
      "TENTACLECIRCULAR",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Tentacles_Circular, 1))
    },
    {
      "FIREBALLCHARM",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Fireball_Charm, 1))
    },
    {
      "FIREBALL",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Fireball, 1))
    },
    {
      "FIREBALLSWARM",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Fireball_Swarm, 1))
    },
    {
      "FIREBALLTRIPLE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.Fireball_Triple, 1))
    },
    {
      "MEGASLASH",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.MegaSlash, 1))
    },
    {
      "MEGASLASHCHARM",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.MegaSlash_Charm, 1))
    },
    {
      "MEGASLASHICE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.MegaSlash_Ice, 1))
    },
    {
      "MEGASLASHNECRO",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.MegaSlash_Necromancy, 1))
    },
    {
      "PROJECTILEAOE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.ProjectileAOE, 1))
    },
    {
      "PROJECTILEAOECHARM",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.ProjectileAOE_Charm, 1))
    },
    {
      "PROJECTILEAOEGOOPTRAIL",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.ProjectileAOE_GoopTrail, 1))
    },
    {
      "RITUAL",
      (System.Action) (() => CheatConsole.Rituals())
    },
    {
      "PROJECTILEAOEEXPLOSIVE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.ProjectileAOE_ExplosiveImpact, 1))
    },
    {
      "ENEMYBLAST",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.EnemyBlast, 1))
    },
    {
      "ENEMYBLASTICE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.EnemyBlast_Ice, 1))
    },
    {
      "ENEMYBLASTPOISON",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.EnemyBlast_Poison, 1))
    },
    {
      "ENEMYBLASTDEFLECT",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerSpells>().SetSpell(EquipmentType.EnemyBlast_DeflectsProjectiles, 1))
    },
    {
      "DOORROOM",
      (System.Action) (() => { })
    },
    {
      "DUN1",
      (System.Action) (() => BaseChainDoor.Instance.PlayDoor1((System.Action) null))
    },
    {
      "DUN2",
      (System.Action) (() => { })
    },
    {
      "DUN3",
      (System.Action) (() => { })
    },
    {
      "DUN4",
      (System.Action) (() => { })
    },
    {
      "BOSSROOM",
      (System.Action) (() =>
      {
        if (GameManager.CurrentDungeonLayer == 4)
        {
          Vector2Int lastRoom = BiomeGenerator.Instance.GetLastRoom();
          BiomeGenerator.ChangeRoom(lastRoom.x, lastRoom.y);
        }
        else
        {
          Vector2Int bossRoom = BiomeGenerator.Instance.GetBossRoom();
          BiomeGenerator.ChangeRoom(bossRoom.x, bossRoom.y);
        }
      })
    },
    {
      "ENDOFFLOORROOM",
      (System.Action) (() =>
      {
        Vector2Int lastRoom = BiomeGenerator.Instance.GetLastRoom();
        BiomeGenerator.ChangeRoom(lastRoom.x, lastRoom.y);
      })
    },
    {
      "SKIP",
      (System.Action) (() =>
      {
        DataManager.Instance.InTutorial = true;
        DataManager.Instance.Tutorial_Second_Enter_Base = true;
        DataManager.Instance.AllowBuilding = true;
      })
    },
    {
      "S",
      (System.Action) (() =>
      {
        CheatConsole.CreateFollower(20);
        UnityEngine.Object.FindObjectOfType<Interaction_DeathCatRitual>().Skip();
      })
    },
    {
      "SEED",
      (System.Action) (() =>
      {
        Inventory.AddItem(8, 10);
        Inventory.AddItem(72, 10);
        Inventory.AddItem(70, 10);
        Inventory.AddItem(51, 10);
        Inventory.AddItem(98, 10);
        Inventory.AddItem(103, 10);
      })
    },
    {
      "A",
      new System.Action(FaithAmmo.Reload)
    },
    {
      "APOTHECARY",
      (System.Action) (() =>
      {
        Inventory.AddItem(55, 10);
        Inventory.AddItem(56, 10);
        Inventory.AddItem(9, 10);
      })
    },
    {
      "DAY",
      (System.Action) (() => ++DataManager.Instance.CurrentDayIndex)
    },
    {
      "DAY100",
      (System.Action) (() => DataManager.Instance.CurrentDayIndex += 100)
    },
    {
      "XPBLACK",
      (System.Action) (() => UpgradeSystem.UpgradeType = UpgradeSystem.UpgradeTypes.BlackSouls)
    },
    {
      "XPWHITE",
      (System.Action) (() => UpgradeSystem.UpgradeType = UpgradeSystem.UpgradeTypes.Devotion)
    },
    {
      "XPDEVOTION",
      (System.Action) (() => UpgradeSystem.UpgradeType = UpgradeSystem.UpgradeTypes.Devotion)
    },
    {
      "XPBOTH",
      (System.Action) (() => UpgradeSystem.UpgradeType = UpgradeSystem.UpgradeTypes.Both)
    },
    {
      "XP",
      (System.Action) (() => PlayerFarming.Instance.GetXP(100f))
    },
    {
      "XPFULL",
      (System.Action) (() => PlayerFarming.Instance.GetXP(1000f))
    },
    {
      "SERMONUNLOCK",
      (System.Action) (() => DoctrineUpgradeSystem.GiveInstantCheat = true)
    },
    {
      "ABILITY",
      (System.Action) (() =>
      {
        ++UpgradeSystem.DisciplePoints;
        ++UpgradeSystem.AbilityPoints;
      })
    },
    {
      "ABILITY100",
      (System.Action) (() =>
      {
        UpgradeSystem.DisciplePoints += 100;
        UpgradeSystem.AbilityPoints += 100;
      })
    },
    {
      "ABILITY0",
      (System.Action) (() =>
      {
        UpgradeSystem.DisciplePoints = 0;
        UpgradeSystem.AbilityPoints = 0;
      })
    },
    {
      "HIDEHUD",
      (System.Action) (() => CheatConsole.HideUI())
    },
    {
      "HIDEUI",
      (System.Action) (() => CheatConsole.HideUI())
    },
    {
      "SHOWHUD",
      (System.Action) (() => CheatConsole.ShowUI())
    },
    {
      "SHOWUI",
      (System.Action) (() => CheatConsole.ShowUI())
    },
    {
      "BEATDEATHCAT",
      (System.Action) (() => DataManager.Instance.DeathCatBeaten = true)
    },
    {
      "PLAYERDAMAGEUP",
      (System.Action) (() => ++DataManager.Instance.PLAYER_DAMAGE_LEVEL)
    },
    {
      "DIFFICULTY",
      (System.Action) (() =>
      {
        Debug.Log((object) ("Difficult 1: " + (object) DifficultyManager.PrimaryDifficulty));
        Debug.Log((object) ("Difficult 2: " + (object) DifficultyManager.SecondaryDifficulty));
      })
    },
    {
      "DIFFICULTY_JULIAN",
      (System.Action) (() => DifficultyManager.ForceDifficulty(DifficultyManager.Difficulty.Easy))
    },
    {
      "DIFFICULTY_JIMP",
      (System.Action) (() => DifficultyManager.ForceDifficulty(DifficultyManager.Difficulty.Hard))
    },
    {
      "COOKINGEFFECTS",
      (System.Action) (() => CookingData.REQUIRES_LOC = !CookingData.REQUIRES_LOC)
    },
    {
      "UNLOCKALL",
      (System.Action) (() => CheatConsole.UnlockAll())
    },
    {
      "UNLOCKWEAPONS",
      (System.Action) (() => CheatConsole.UnlockWeapons())
    },
    {
      "LVL",
      (System.Action) (() => Debug.Log((object) ("Weapon level: " + (object) DataManager.Instance.CurrentWeaponLevel)))
    },
    {
      "SWORD",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Sword, 1))
    },
    {
      "SWORDFERVOUR",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Sword_Fervour, 1))
    },
    {
      "SWORDCRITICAL",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Sword_Critical, 1))
    },
    {
      "SWORDNECROMANCER",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Sword_Nercomancy, 1))
    },
    {
      "SWORDPOISON",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Sword_Poison, 1))
    },
    {
      "NECROAXE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Axe_Nercomancy, 1))
    },
    {
      "AXE",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Axe, 1))
    },
    {
      "DAGGER",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Dagger, 1))
    },
    {
      "DAGGER_NECRO",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Dagger_Nercomancy, 1))
    },
    {
      "GAUNTLET",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Gauntlet, 1))
    },
    {
      "HAMMER",
      (System.Action) (() => UnityEngine.Object.FindObjectOfType<PlayerWeapon>().SetWeapon(EquipmentType.Hammer, 1))
    },
    {
      "OPENLIGHTHOUSE",
      (System.Action) (() => DataManager.Instance.ShrineDoor = true)
    },
    {
      "ALLHOME",
      (System.Action) (() =>
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.Location != FollowerLocation.Base)
          {
            allBrain.DesiredLocation = FollowerLocation.Base;
            allBrain.CompleteCurrentTask();
          }
        }
      })
    },
    {
      "ALLRESET",
      (System.Action) (() =>
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          allBrain.CompleteCurrentTask();
      })
    },
    {
      "FIXFOLLOWERS",
      (System.Action) (() =>
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          allBrain.Stats.Rest = 100f;
          allBrain.Stats.Satiation = 0.0f;
          allBrain.Stats.Illness = 0.0f;
        }
      })
    },
    {
      "RESET",
      (System.Action) (() =>
      {
        SimulationManager.Pause();
        FollowerManager.Reset();
        StructureManager.Reset();
        UIDynamicNotificationCenter.Reset();
        SaveAndLoad.ResetSave(SaveAndLoad.SAVE_SLOT, false);
        MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Base Biome 1", 0.5f, "", (System.Action) null);
      })
    },
    {
      "M",
      (System.Action) (() =>
      {
        if (CheatConsole.HidingUI)
        {
          CheatConsole.ShowUI();
          MMTransition.OnTransitionCompelte += new System.Action(CheatConsole.HideUI);
        }
        MonoSingleton<UIManager>.Instance.ShowWorldMap().Show();
      })
    },
    {
      "K",
      (System.Action) (() =>
      {
        foreach (Health health in new List<Health>((IEnumerable<Health>) Health.team2))
        {
          if ((UnityEngine.Object) health != (UnityEngine.Object) null)
          {
            health.invincible = false;
            health.enabled = true;
            health.DealDamage(float.PositiveInfinity, PlayerFarming.Instance.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
          }
        }
      })
    },
    {
      "KEY",
      (System.Action) (() =>
      {
        ++Inventory.KeyPieces;
        MonoSingleton<UIManager>.Instance.ShowKeyScreen().OnHidden += (System.Action) (() =>
        {
          if (DataManager.Instance.HadFirstTempleKey || Inventory.TempleKeys <= 0 || !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Fleeces))
            return;
          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Fleeces).OnHidden += (System.Action) (() => DataManager.Instance.HadFirstTempleKey = true);
        });
      })
    },
    {
      "TKEY",
      (System.Action) (() => Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.TALISMAN, 1))
    },
    {
      "KEYALL",
      (System.Action) (() =>
      {
        Inventory.KeyPieces = 4;
        MonoSingleton<UIManager>.Instance.ShowKeyScreen().OnHidden += (System.Action) (() =>
        {
          if (DataManager.Instance.HadFirstTempleKey || Inventory.TempleKeys <= 0 || !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Fleeces))
            return;
          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Fleeces).OnHidden += (System.Action) (() => DataManager.Instance.HadFirstTempleKey = true);
        });
      })
    },
    {
      "CLEARINVENTORY",
      (System.Action) (() => Inventory.items.Clear())
    },
    {
      "MISSIONS",
      (System.Action) (() =>
      {
        DataManager.Instance.ActiveMissions.Clear();
        DataManager.Instance.AvailableMissions.Clear();
        UnityEngine.Object.FindObjectOfType<Interaction_MissionShrine>().AddNewMission();
      })
    },
    {
      "FREE",
      new System.Action(CheatConsole.AllBuildingsFree)
    },
    {
      "HEAL",
      new System.Action(CheatConsole.Heal)
    },
    {
      "DAMAGE",
      new System.Action(CheatConsole.Damage)
    },
    {
      "DD",
      new System.Action(CheatConsole.Damage5)
    },
    {
      "HEART",
      (System.Action) (() => CheatConsole.AddHeart())
    },
    {
      "HALFHEART",
      (System.Action) (() => CheatConsole.AddHeart(1))
    },
    {
      "HEARTS",
      new System.Action(CheatConsole.Heal)
    },
    {
      "MOREHEARTS",
      new System.Action(CheatConsole.MoreHearts)
    },
    {
      "SOULS",
      new System.Action(CheatConsole.MoreSouls)
    },
    {
      "BLACKSOULS",
      new System.Action(CheatConsole.MoreBlackSouls)
    },
    {
      "BS",
      new System.Action(CheatConsole.MoreBlackSouls)
    },
    {
      "DIE",
      new System.Action(CheatConsole.Die)
    },
    {
      "RESOURCES",
      new System.Action(CheatConsole.GiveResources)
    },
    {
      "POOP",
      new System.Action(CheatConsole.GivePoop)
    },
    {
      "KEYS",
      new System.Action(CheatConsole.GiveKeys)
    },
    {
      "FOOD",
      new System.Action(CheatConsole.GiveFood)
    },
    {
      "SPIRIT",
      new System.Action(CheatConsole.SpiritHeartsFull)
    },
    {
      "SPIRITHALF",
      new System.Action(CheatConsole.SpiritHeartsHalf)
    },
    {
      "BLUE",
      new System.Action(CheatConsole.BlueHearts)
    },
    {
      "ARROWS",
      new System.Action(CheatConsole.MoreArrows)
    },
    {
      "BASE",
      new System.Action(CheatConsole.ReturnToBase)
    },
    {
      "B",
      new System.Action(CheatConsole.ReturnToBase)
    },
    {
      "INFO",
      new System.Action(CheatConsole.DebugInfo)
    },
    {
      "ALLCURSES",
      new System.Action(CheatConsole.AllCurses)
    },
    {
      "ALLTRINKETS",
      new System.Action(CheatConsole.AllTrinkets)
    },
    {
      "IMPLEMENTEDTRINKETS",
      new System.Action(CheatConsole.ImplementedTrinkets)
    },
    {
      "COLLIDER",
      new System.Action(CheatConsole.ToggleCollider)
    },
    {
      "UP",
      new System.Action(CheatConsole.Up)
    },
    {
      "DOWN",
      new System.Action(CheatConsole.Down)
    },
    {
      "LEFT",
      new System.Action(CheatConsole.Left)
    },
    {
      "RIGHT",
      new System.Action(CheatConsole.Right)
    },
    {
      "FISH",
      new System.Action(CheatConsole.Fish)
    },
    {
      "NEXTSANDBOXLAYER",
      new System.Action(CheatConsole.NextSandboxLayer)
    },
    {
      "NEXTDUNGEONLAYER",
      (System.Action) (() => GameManager.NextDungeonLayer(GameManager.PreviousDungeonLayer + 1))
    },
    {
      "MUSHROOM",
      new System.Action(CheatConsole.Mushroom)
    },
    {
      "TALISMAN",
      (System.Action) (() => Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.TALISMAN, 1))
    },
    {
      "SOZO1",
      (System.Action) (() => DataManager.Instance.SozoStoryProgress = 1)
    },
    {
      "SOZO2",
      (System.Action) (() => DataManager.Instance.SozoStoryProgress = 2)
    },
    {
      "SOZO3",
      (System.Action) (() => DataManager.Instance.SozoStoryProgress = 3)
    },
    {
      "SOZO4",
      (System.Action) (() => DataManager.Instance.SozoStoryProgress = 4)
    },
    {
      "SOZO5",
      (System.Action) (() => DataManager.Instance.SozoStoryProgress = 5)
    },
    {
      "SOZODEAD",
      (System.Action) (() =>
      {
        DataManager.Instance.SozoStoryProgress = 5;
        DataManager.Instance.SozoDead = true;
      })
    },
    {
      "STARVECLOSEST",
      (System.Action) (() =>
      {
        Follower follower13 = (Follower) null;
        float num13 = float.MaxValue;
        foreach (Follower follower14 in Follower.Followers)
        {
          float num14 = Vector3.Distance(PlayerFarming.Instance.transform.position, follower14.transform.position);
          if ((double) num14 < (double) num13)
          {
            num13 = num14;
            follower13 = follower14;
          }
        }
        DataManager.Instance.CookedFirstFood = true;
        follower13.Brain.MakeStarve();
      })
    },
    {
      "EXHAUSTCLOSEST",
      (System.Action) (() =>
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          allBrain.MakeExhausted();
      })
    },
    {
      "SICKFOLLOWERS",
      (System.Action) (() =>
      {
        foreach (FollowerInfo follower in DataManager.Instance.Followers)
          FollowerBrain.GetOrCreateBrain(follower).MakeSick();
      })
    },
    {
      "SKIPDUNGEON2LAYERS",
      (System.Action) (() => DataManager.Instance.Dungeon2_Layer = 4)
    },
    {
      "SKIPDUNGEON3LAYERS",
      (System.Action) (() => DataManager.Instance.Dungeon3_Layer = 4)
    },
    {
      "SKIPDUNGEON4LAYERS",
      (System.Action) (() => DataManager.Instance.Dungeon4_Layer = 4)
    },
    {
      "MAP",
      new System.Action(CheatConsole.ShowMap)
    },
    {
      "DMODIFIER",
      (System.Action) (() =>
      {
        DungeonModifier.ChanceOfModifier = 1f;
        DungeonModifier.SetActiveModifier(DungeonModifier.GetModifier(1f));
        BiomeGenerator.Instance.ApplyCurrentDungeonModifiers();
      })
    },
    {
      "EMODIFIER",
      (System.Action) (() =>
      {
        EnemyModifier.ChanceOfModifier = 1f;
        EnemyModifier.ForceModifiers = true;
      })
    },
    {
      "NEWRECRUIT",
      (System.Action) (() =>
      {
        DataManager.Instance.Followers_Recruit.Add(FollowerInfo.NewCharacter(FollowerLocation.Base));
        if (!BiomeBaseManager.Instance.SpawnExistingRecruits)
          return;
        BiomeBaseManager.Instance.SpawnExistingRecruits = false;
      })
    },
    {
      "NEWRECRUIT_DEATHCAT",
      (System.Action) (() =>
      {
        DataManager.Instance.Followers_Recruit.Add(FollowerInfo.NewCharacter(FollowerLocation.Base, "Boss Death Cat"));
        if (!BiomeBaseManager.Instance.SpawnExistingRecruits)
          return;
        BiomeBaseManager.Instance.SpawnExistingRecruits = false;
      })
    },
    {
      "UNLOCKSKIN",
      (System.Action) (() => DataManager.SetFollowerSkinUnlocked(DataManager.GetRandomLockedSkin()))
    },
    {
      "UNLOCKSKINS",
      (System.Action) (() =>
      {
        foreach (WorshipperData.SkinAndData character in WorshipperData.Instance.Characters)
        {
          if (!DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin))
            DataManager.SetFollowerSkinUnlocked(character.Skin[0].Skin);
        }
      })
    },
    {
      "LOCATION",
      (System.Action) (() => Debug.Log((object) ("Location: " + (object) PlayerFarming.Location)))
    },
    {
      "ROBES",
      (System.Action) (() =>
      {
        CheatConsole.Robes = !CheatConsole.Robes;
        foreach (Follower follower in Follower.Followers)
          follower.SetOutfit(FollowerOutfitType.Follower, true);
      })
    },
    {
      "RITUALCOOLDOWN",
      (System.Action) (() => UpgradeSystem.ClearAllCoolDowns())
    },
    {
      "GRAPPLE",
      (System.Action) (() => CheatConsole.UnlockCrownAbility(CrownAbilities.TYPE.Abilities_GrappleHook))
    },
    {
      "ABILITYARROWS",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Combat_Arrows))
    },
    {
      "GAINALLTAROTCARDS",
      (System.Action) (() =>
      {
        while (TarotCards.DrawRandomCard() != null)
          CheatConsole.RunTrinket(TarotCards.DrawRandomCard());
      })
    },
    {
      "ABILITYFISH",
      (System.Action) (() =>
      {
        CheatConsole.UnlockCrownAbility(CrownAbilities.TYPE.Abilities_FishingRod);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Economy_FishingRod);
      })
    },
    {
      "ABILITYHEAVY",
      (System.Action) (() => CheatConsole.UnlockCrownAbility(CrownAbilities.TYPE.Combat_HeavyAttack))
    },
    {
      "FOLLOWERTOKEN",
      new System.Action(CheatConsole.FollowerToken)
    },
    {
      "ALLDECORATIONS",
      new System.Action(DataManager.UnlockAllDecorations)
    },
    {
      "OFFERING",
      new System.Action(CheatConsole.FollowerToken)
    },
    {
      "OFFERINGS",
      new System.Action(CheatConsole.FollowerTokens)
    },
    {
      "TESTOBJECTIVES",
      new System.Action(CheatConsole.TestObjectives)
    },
    {
      "NAMECULT",
      new System.Action(CheatConsole.NameCult)
    },
    {
      "OBJECTIVETEST",
      new System.Action(CheatConsole.CustomObjective_Create)
    },
    {
      "OBJECTIVECOMPLETE",
      new System.Action(CheatConsole.CustomObjective_Complete)
    },
    {
      "COMPLETEOBJECTIVES",
      new System.Action(CheatConsole.CompleteCurrentObjectives)
    },
    {
      "RESETCOOLDOWNS",
      new System.Action(CheatConsole.ResetCooldowns)
    },
    {
      "KILLFOLLOWERS",
      new System.Action(CheatConsole.KillFollowers)
    },
    {
      "REMOVEFAITH",
      new System.Action(CheatConsole.RemoveFaith)
    },
    {
      "SERMONS",
      new System.Action(CheatConsole.UnlockAllSermons)
    },
    {
      "MONSTER",
      new System.Action(CheatConsole.MonsterHeart)
    },
    {
      "MONSTERHEART",
      new System.Action(CheatConsole.MonsterHeart)
    },
    {
      "BUILD",
      new System.Action(CheatConsole.BuildAll)
    },
    {
      "ENABLETAROT",
      new System.Action(CheatConsole.EnableTarot)
    },
    {
      "STARTINGPACK",
      new System.Action(CheatConsole.GiveStartingPack)
    },
    {
      "FPS",
      new System.Action(CheatConsole.FPS)
    },
    {
      "T",
      new System.Action(CheatConsole.SkipHour)
    },
    {
      "RAIN",
      new System.Action(CheatConsole.Rain)
    },
    {
      "WIND",
      new System.Action(CheatConsole.Wind)
    },
    {
      "RESOURCEHIGHLIGHTOFF",
      new System.Action(CheatConsole.TurnOffResourceHighlight)
    },
    {
      "FOLLOWERDEBUG",
      new System.Action(CheatConsole.FollowerDebug)
    },
    {
      "STRUCTUREDEBUG",
      new System.Action(CheatConsole.StructureDebug)
    },
    {
      "RANDOMTRINKET",
      (System.Action) (() => InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, PlayerFarming.Instance.transform.position + Vector3.down, 0.0f))
    },
    {
      "T_HEARTS1",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Hearts1))
    },
    {
      "T_HEARTS2",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Hearts2))
    },
    {
      "T_HEARTS3",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Hearts3))
    },
    {
      "T_LOVERS1",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Lovers1))
    },
    {
      "T_LOVERS2",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Lovers2))
    },
    {
      "T_MOON",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Moon))
    },
    {
      "T_ARROWS",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Arrows))
    },
    {
      "T_NATURESGIFT",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.NaturesGift))
    },
    {
      "T_SUN",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Sun))
    },
    {
      "T_EYE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.EyeOfWeakness))
    },
    {
      "T_TELESCOPE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Telescope))
    },
    {
      "T_HANDSOFRAGE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.HandsOfRage))
    },
    {
      "T_SKULL",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Skull))
    },
    {
      "T_SPIDER",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Spider))
    },
    {
      "T_DISEASEDHEART",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.DiseasedHeart))
    },
    {
      "T_THEDEAL",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.TheDeal))
    },
    {
      "T_DEATHSDOOR",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.DeathsDoor))
    },
    {
      "T_MOVEMENTSPEED",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.MovementSpeed))
    },
    {
      "T_ATTACKRATE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.AttackRate))
    },
    {
      "T_INCREASEDDAMAGE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.IncreasedDamage))
    },
    {
      "T_INCREASEDXP",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.IncreaseBlackSoulsDrop))
    },
    {
      "T_HEALCHANCE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.HealChance))
    },
    {
      "T_NEGATEDAMAGE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.NegateDamageChance))
    },
    {
      "T_BOMBONROLL",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.BombOnRoll))
    },
    {
      "T_GOOPONROLL",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.GoopOnRoll))
    },
    {
      "T_GOOPONDAMAGED",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.GoopOnDamaged))
    },
    {
      "T_POISONIMMUNE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.PoisonImmune))
    },
    {
      "T_DAMAGEONROLL",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.DamageOnRoll))
    },
    {
      "T_HEALDOUBLE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.HealTwiceAmount))
    },
    {
      "T_INVINCIBLEHEALING",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.InvincibleWhileHealing))
    },
    {
      "T_AMMOEFFICIENT",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.AmmoEfficient))
    },
    {
      "T_BLACKSOULRECHARGE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.BlackSoulAutoRecharge))
    },
    {
      "T_BLACKSOULONDAMAGE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.BlackSoulOnDamage))
    },
    {
      "T_NEPTUNESCURSE",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.NeptunesCurse))
    },
    {
      "T_GIFTFROMBELOW",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.GiftFromBelow))
    },
    {
      "T_POTION",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.Potion))
    },
    {
      "T_HOLDTOHEAL",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.HoldToHeal))
    },
    {
      "T_RABBITFOOT",
      (System.Action) (() => CheatConsole.RunTrinket(TarotCards.Card.RabbitFoot))
    },
    {
      "TRAIT_HATEELDERLY",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.HateElderly))
    },
    {
      "TRAIT_LOVEELDERLY",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.LoveElderly))
    },
    {
      "TRAIT_SACRIFICE",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.DesensitisedToDeath))
    },
    {
      "TRAIT_MUSHROOMENCOURAGED",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.MushroomEncouraged))
    },
    {
      "TRAIT_MUSHROOMBANNED",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.MushroomBanned))
    },
    {
      "TRAIT_GRASSEATER",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.GrassEater))
    },
    {
      "TRAIT_MATERIALISTIC",
      (System.Action) (() => DataManager.Instance.CultTraits.Add(FollowerTrait.TraitType.Materialistic))
    },
    {
      "ENABLEBLACKSOULS",
      new System.Action(CheatConsole.EnableBlackSouls)
    },
    {
      "1920",
      new System.Action(CheatConsole.SetResolution)
    },
    {
      "PHASE",
      (System.Action) (() => Debug.Log((object) TimeManager.CurrentPhase))
    },
    {
      "TIME",
      (System.Action) (() => Debug.Log((object) TimeManager.CurrentGameTime))
    },
    {
      "FULLSCREEN",
      new System.Action(CheatConsole.ToggleFullScreen)
    },
    {
      "DAWN",
      (System.Action) (() => CheatConsole.SkipToPhase(DayPhase.Dawn))
    },
    {
      "MORNING",
      (System.Action) (() => CheatConsole.SkipToPhase(DayPhase.Morning))
    },
    {
      "AFTERNOON",
      (System.Action) (() => CheatConsole.SkipToPhase(DayPhase.Afternoon))
    },
    {
      "DUSK",
      (System.Action) (() => CheatConsole.SkipToPhase(DayPhase.Dusk))
    },
    {
      "NIGHT",
      (System.Action) (() => CheatConsole.SkipToPhase(DayPhase.Night))
    },
    {
      "NEXTPHASE",
      (System.Action) (() => CheatConsole.SkipToPhase(TimeManager.CurrentPhase + 1))
    },
    {
      "RUBBLE",
      new System.Action(CheatConsole.ClearRubble)
    },
    {
      "WEED",
      new System.Action(CheatConsole.ClearWeed)
    },
    {
      "REPORT",
      new System.Action(CheatConsole.SubmitReport)
    },
    {
      "ADDDUNGEONRUN",
      (System.Action) (() => DataManager.Instance.dungeonRun += 3)
    },
    {
      "SCHEDULING",
      (System.Action) (() => DataManager.Instance.SchedulingEnabled = true)
    },
    {
      "GOD",
      new System.Action(CheatConsole.ToggleGodMode)
    },
    {
      "DEMIGOD",
      new System.Action(CheatConsole.ToggleDemiGodMode)
    },
    {
      "IMMORTAL",
      new System.Action(CheatConsole.ToggleImmortalMode)
    },
    {
      "DUNGEONCHALLENGE",
      (System.Action) (() => ObjectiveManager.Add(Quests.GetRandomDungeonChallenge()))
    },
    {
      "NOCLIP",
      new System.Action(CheatConsole.ToggleNoClip)
    },
    {
      "SAVE",
      (System.Action) (() => SaveAndLoad.Save())
    },
    {
      "LOAD1",
      (System.Action) (() => CheatConsole.LoadGame(0))
    },
    {
      "LOAD2",
      (System.Action) (() => CheatConsole.LoadGame(1))
    },
    {
      "LOAD3",
      (System.Action) (() => CheatConsole.LoadGame(2))
    },
    {
      "LOAD4",
      (System.Action) (() => CheatConsole.LoadGame(3))
    },
    {
      "LOAD5",
      (System.Action) (() => CheatConsole.LoadGame(4))
    },
    {
      "LOAD6",
      (System.Action) (() => CheatConsole.LoadGame(5))
    },
    {
      "DEBUGSTRUCTURES",
      (System.Action) (() =>
      {
        foreach (Structure structure in Structure.Structures)
        {
          Debug.Log((object) ">>>>>");
          Debug.Log((object) structure.Type);
          Debug.Log((object) structure.Structure_Info);
        }
      })
    },
    {
      "FOLLOWER",
      (System.Action) (() => CheatConsole.CreateFollower(1))
    },
    {
      "FOLLOWER1",
      (System.Action) (() => CheatConsole.CreateFollower(1))
    },
    {
      "FOLLOWER2",
      (System.Action) (() => CheatConsole.CreateFollower(2))
    },
    {
      "FOLLOWER3",
      (System.Action) (() => CheatConsole.CreateFollower(3))
    },
    {
      "FOLLOWER4",
      (System.Action) (() => CheatConsole.CreateFollower(4))
    },
    {
      "KNUCKLEBONES_OPPONENTS_ALL",
      (System.Action) (() =>
      {
        DataManager.Instance.SetVariable(DataManager.Variables.Knucklebones_Opponent_Ratau_Won, true);
        DataManager.Instance.SetVariable(DataManager.Variables.Knucklebones_Opponent_0, true);
        DataManager.Instance.SetVariable(DataManager.Variables.Knucklebones_Opponent_1, true);
        DataManager.Instance.SetVariable(DataManager.Variables.Knucklebones_Opponent_2, true);
      })
    },
    {
      "KNUCKLEBONES_0",
      (System.Action) (() => DataManager.Instance.SetVariable(DataManager.Variables.Knucklebones_Opponent_0, true))
    },
    {
      "KNUCKLEBONES_1",
      (System.Action) (() => DataManager.Instance.SetVariable(DataManager.Variables.Knucklebones_Opponent_1, true))
    },
    {
      "KNUCKLEBONES_2",
      (System.Action) (() => DataManager.Instance.SetVariable(DataManager.Variables.Knucklebones_Opponent_2, true))
    },
    {
      "GIVECOINS",
      (System.Action) (() => Inventory.AddItem(20, 1000))
    },
    {
      "REMOVECOINS",
      (System.Action) (() => Inventory.ChangeItemQuantity(20, 0))
    },
    {
      "HOTF1",
      (System.Action) (() =>
      {
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1);
        DataManager.Instance.PLAYER_HEARTS_LEVEL = 0;
      })
    },
    {
      "HOTF2",
      (System.Action) (() =>
      {
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful2);
        DataManager.Instance.PLAYER_HEARTS_LEVEL = 1;
      })
    },
    {
      "HOTF3",
      (System.Action) (() =>
      {
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful3);
        DataManager.Instance.PLAYER_HEARTS_LEVEL = 2;
      })
    },
    {
      "HOTF4",
      (System.Action) (() =>
      {
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful4);
        DataManager.Instance.PLAYER_HEARTS_LEVEL = 3;
      })
    },
    {
      "HOTF5",
      (System.Action) (() =>
      {
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful5);
        DataManager.Instance.PLAYER_HEARTS_LEVEL = 4;
      })
    },
    {
      "HOTF6",
      (System.Action) (() =>
      {
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful6);
        DataManager.Instance.PLAYER_HEARTS_LEVEL = 5;
      })
    },
    {
      "CLEARHOTF",
      (System.Action) (() =>
      {
        DataManager.Instance.PLAYER_HEARTS_LEVEL = 0;
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful2);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful3);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful4);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful5);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful6);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful7);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful8);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful9);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful10);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful11);
        DataManager.Instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful12);
      })
    },
    {
      "KILLRATAU",
      (System.Action) (() => DataManager.Instance.RatauKilled = true)
    },
    {
      "N",
      (System.Action) (() => MapManager.Instance.ShowMap())
    },
    {
      "NODELAST",
      (System.Action) (() =>
      {
        if (!MapManager.Instance.MapGenerated)
          MapManager.Instance.GenerateNewMap();
        Map.Node bossNode = MapManager.Instance.CurrentMap.GetBossNode();
        MapManager.Instance.AddNodeToPath(bossNode);
        MapManager.Instance.EnterNode(bossNode);
      })
    },
    {
      "RESETMAP",
      (System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) CheatConsole.ResetMap()))
    },
    {
      "ENDROOM",
      (System.Action) (() =>
      {
        Debug.Log((object) $"End Room: {(object) BiomeGenerator.Instance.RoomExit.x} {(object) BiomeGenerator.Instance.RoomExit.y}");
        BiomeGenerator.ChangeRoom(BiomeGenerator.Instance.RoomExit.x, BiomeGenerator.Instance.RoomExit.y);
      })
    },
    {
      "SKIPDUNGEON1LAYERS",
      (System.Action) (() => DataManager.Instance.Dungeon1_Layer = 4)
    },
    {
      "KNUCKLEBONESEND",
      (System.Action) (() => CheatConsole.EndKnucklebones())
    },
    {
      "EK",
      (System.Action) (() => CheatConsole.EndKnucklebones())
    },
    {
      "UNLOCKRITUALS",
      (System.Action) (() =>
      {
        CheatConsole.UnlockAllRituals = true;
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Ascend);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Brainwashing);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Enlightenment);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Fast);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Feast);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Fightpit);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Funeral);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Holiday);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Ressurect);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Sacrifice);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Wedding);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_ConsumeFollower);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_DonationRitual);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FasterBuilding);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FirePit);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FishingRitual);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HarvestRitual);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AlmsToPoor);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignFaithEnforcer);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignTaxCollector);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_WorkThroughNight);
      })
    },
    {
      "WEDDING",
      (System.Action) (() => UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Wedding))
    },
    {
      "COLORBLINDOFF",
      (System.Action) (() => Singleton<AccessibilityManager>.Instance.SetColorblindMode(0))
    },
    {
      "COLORBLIND1",
      (System.Action) (() => Singleton<AccessibilityManager>.Instance.SetColorblindMode(1))
    },
    {
      "COLORBLIND2",
      (System.Action) (() => Singleton<AccessibilityManager>.Instance.SetColorblindMode(2))
    },
    {
      "COLORBLIND3",
      (System.Action) (() => Singleton<AccessibilityManager>.Instance.SetColorblindMode(3))
    },
    {
      "COLORBLIND4",
      (System.Action) (() => Singleton<AccessibilityManager>.Instance.SetColorblindMode(4))
    },
    {
      "COLORBLIND5",
      (System.Action) (() => Singleton<AccessibilityManager>.Instance.SetColorblindMode(5))
    },
    {
      "COLORBLIND6",
      (System.Action) (() => Singleton<AccessibilityManager>.Instance.SetColorblindMode(6))
    },
    {
      "COLORBLIND7",
      (System.Action) (() => Singleton<AccessibilityManager>.Instance.SetColorblindMode(7))
    },
    {
      "CLEARTUTORIALS",
      (System.Action) (() =>
      {
        DataManager.Instance.RevealedTutorialTopics.Clear();
        DataManager.Instance.Alerts.Tutorial.ClearAll();
      })
    },
    {
      "CHAT",
      (System.Action) (() => FollowerBrain.AllBrains.LastElement<FollowerBrain>().HardSwapToTask((FollowerTask) new FollowerTask_Chat(FollowerBrain.AllBrains[0].Info.ID, true)))
    },
    {
      "TESTNOTIFICATIONS",
      (System.Action) (() =>
      {
        NotificationCentre.Instance.PlayGenericNotification("UI/TarotMenu/NewTarotCardUnlocked");
        NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.LowFaithDonation);
        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/DissenterLeavingTomorrow", "Brian", "200");
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/Structure/StructureEffectStarted", StructureEffectManager.GetLocalizedKey(StructureEffectManager.EffectType.Shrine_DevotionEffeciency));
        NotificationCentre.Instance.PlayFaithNotification(FollowerThoughts.GetNotificationOnLocalizationKey(Thought.Cult_Holiday), 10f, NotificationBase.Flair.None);
        if (FollowerBrain.AllBrains.Count <= 1)
          return;
        FollowerBrain followerBrain = FollowerBrain.AllBrains.LastElement<FollowerBrain>();
        FollowerBrain allBrain = FollowerBrain.AllBrains[0];
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.AteRottenFood, followerBrain.Info, NotificationFollower.Animation.Sick);
        NotificationCentre.Instance.PlayRelationshipNotification(NotificationCentre.NotificationType.RelationshipEnemy, followerBrain._directInfoAccess, NotificationFollower.Animation.Angry, allBrain._directInfoAccess, NotificationFollower.Animation.Angry);
      })
    },
    {
      "LOADD1",
      (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon1", 1f, "", (System.Action) null))
    },
    {
      "LOADD2",
      (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon2", 1f, "", (System.Action) null))
    },
    {
      "LOADD3",
      (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon3", 1f, "", (System.Action) null))
    },
    {
      "LOADD4",
      (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon4", 1f, "", (System.Action) null))
    },
    {
      "SETDUNGEON1LAYER1",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon1_Layer = 1;
      })
    },
    {
      "SETDUNGEON1LAYER2",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon1_Layer = 2;
      })
    },
    {
      "SETDUNGEON1LAYER3",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon1_Layer = 3;
      })
    },
    {
      "SETDUNGEON1LAYER4",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = true;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon1_Layer = 4;
      })
    },
    {
      "SETDUNGEON2LAYER1",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon2_Layer = 1;
      })
    },
    {
      "SETDUNGEON2LAYER2",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon2_Layer = 2;
      })
    },
    {
      "SETDUNGEON2LAYER3",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon2_Layer = 3;
      })
    },
    {
      "SETDUNGEON2LAYER4",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = true;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon2_Layer = 4;
      })
    },
    {
      "SETDUNGEON3LAYER1",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon3_Layer = 1;
      })
    },
    {
      "SETDUNGEON3LAYER2",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon3_Layer = 2;
      })
    },
    {
      "SETDUNGEON3LAYER3",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon3_Layer = 3;
      })
    },
    {
      "SETDUNGEON3LAYER4",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = true;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon3_Layer = 4;
      })
    },
    {
      "SETDUNGEON4LAYER1",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon4_Layer = 1;
      })
    },
    {
      "SETDUNGEON4LAYER2",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon4_Layer = 2;
      })
    },
    {
      "SETDUNGEON4LAYER3",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = false;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon4_Layer = 3;
      })
    },
    {
      "SETDUNGEON4LAYER4",
      (System.Action) (() =>
      {
        DataManager.Instance.DungeonBossFight = true;
        GameManager.CurrentDungeonLayer = DataManager.Instance.Dungeon4_Layer = 4;
      })
    },
    {
      "D1BOSS1",
      (System.Action) (() => CheatConsole.BossRoom(1, 1))
    },
    {
      "D1BOSS2",
      (System.Action) (() => CheatConsole.BossRoom(2, 1))
    },
    {
      "D1BOSS3",
      (System.Action) (() => CheatConsole.BossRoom(3, 1))
    },
    {
      "D1BOSS4",
      (System.Action) (() => CheatConsole.BossRoom(4, 1))
    },
    {
      "D2BOSS1",
      (System.Action) (() => CheatConsole.BossRoom(1, 2))
    },
    {
      "D2BOSS2",
      (System.Action) (() => CheatConsole.BossRoom(2, 2))
    },
    {
      "D2BOSS3",
      (System.Action) (() => CheatConsole.BossRoom(3, 2))
    },
    {
      "D2BOSS4",
      (System.Action) (() => CheatConsole.BossRoom(4, 2))
    },
    {
      "D3BOSS1",
      (System.Action) (() => CheatConsole.BossRoom(1, 3))
    },
    {
      "D3BOSS2",
      (System.Action) (() => CheatConsole.BossRoom(2, 3))
    },
    {
      "D3BOSS3",
      (System.Action) (() => CheatConsole.BossRoom(3, 3))
    },
    {
      "D3BOSS4",
      (System.Action) (() => CheatConsole.BossRoom(4, 3))
    },
    {
      "D4BOSS1",
      (System.Action) (() => CheatConsole.BossRoom(1, 4))
    },
    {
      "D4BOSS2",
      (System.Action) (() => CheatConsole.BossRoom(2, 4))
    },
    {
      "D4BOSS3",
      (System.Action) (() => CheatConsole.BossRoom(3, 4))
    },
    {
      "D4BOSS4",
      (System.Action) (() => CheatConsole.BossRoom(4, 4))
    },
    {
      "SKIPTOBOSS",
      (System.Action) (() => CheatConsole.SkipBossRoom())
    },
    {
      "TESTSAVEERROR",
      (System.Action) (() => MonoSingleton<UIManager>.Instance.ShowSaveError())
    },
    {
      "COMMANDMENTSTONES",
      (System.Action) (() => PlayerDoctrineStone.Instance.CompletedDoctrineStones += 10)
    },
    {
      "COMBATNODES",
      (System.Action) (() => MapManager.Instance.StartCoroutine((IEnumerator) CheatConsole.CombatNodes()))
    },
    {
      "BOSSNODE",
      (System.Action) (() => MapManager.Instance.StartCoroutine((IEnumerator) CheatConsole.BossNodes()))
    },
    {
      "RANDOMNODES",
      (System.Action) (() => MapManager.Instance.StartCoroutine((IEnumerator) CheatConsole.RandomizeNodes()))
    },
    {
      "TELEPORTNODE",
      (System.Action) (() => MapManager.Instance.StartCoroutine((IEnumerator) CheatConsole.TeleportNode()))
    },
    {
      "QUICKUNLOCK",
      (System.Action) (() => CheatConsole.QuickUnlock = !CheatConsole.QuickUnlock)
    },
    {
      "CONVERTSAVES",
      (System.Action) (() => MonoSingleton<UIManager>.Instance.StartCoroutine((IEnumerator) COTLDataConversion.ConvertFiles((System.Action) (() => Singleton<SettingsManager>.Instance.LoadAndApply(true)))))
    },
    {
      "OBJECTIVESFIX",
      (System.Action) (() =>
      {
        COTLDataConversion.ConvertObjectiveIDs(DataManager.Instance);
        SaveAndLoad.Save();
      })
    },
    {
      "LOWFPS10",
      (System.Action) (() =>
      {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 10;
      })
    },
    {
      "LOWFPS20",
      (System.Action) (() =>
      {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 20;
      })
    }
  };
  public static CheatConsole Instance;
  public Color TextColor = Color.white;
  public static bool ForceAutoAttractMode = false;
  private static float LastKeyPressTime;
  public static bool BuildingsFree = false;
  public static bool AllBuildingsUnlocked = false;
  public static bool UnlockAllRituals = false;
  private static global::HideUI HideUIObject;
  public static bool HidingUI = false;
  public static System.Action OnHideUI;
  public static System.Action OnShowUI;

  public static bool IN_DEMO
  {
    get => CheatConsole._inDemo;
    set => CheatConsole._inDemo = value;
  }

  public static void EnableDemo()
  {
    CheatConsole.IN_DEMO = true;
    MainMenuController objectOfType1 = UnityEngine.Object.FindObjectOfType<MainMenuController>();
    if ((UnityEngine.Object) objectOfType1 != (UnityEngine.Object) null)
      objectOfType1.AttractMode();
    Lamb.UI.MainMenu.MainMenu objectOfType2 = UnityEngine.Object.FindObjectOfType<Lamb.UI.MainMenu.MainMenu>();
    if ((UnityEngine.Object) objectOfType2 != (UnityEngine.Object) null)
      objectOfType2.EnableDemo(false);
    DataManager.Instance.CanReadMinds = true;
    DemoWatermark.Play();
  }

  private static IEnumerator CombatNodes()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.ConvertAllNodesToCombatNodes();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
  }

  private static IEnumerator BossNodes()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.ConvertMiniBossNodeToBossNode();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
  }

  private static IEnumerator RandomizeNodes()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.RandomiseNextNodes();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
  }

  private static IEnumerator TeleportNode()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    Map.Node randomNodeOnLayer = MapGenerator.GetRandomNodeOnLayer(MapManager.Instance.CurrentLayer + 2);
    if (randomNodeOnLayer != null)
      yield return (object) adventureMapOverlayController.TeleportNode(randomNodeOnLayer);
  }

  private static void BossRoom(int layer, int Dungeon)
  {
    UnifyComponent.Instance.gameObject.AddComponent<CheatChainRunner>();
    UnifyComponent.Instance.GetComponent<CheatChainRunner>().RunChain(new string[5]
    {
      $"SETDUNGEON{(object) Dungeon}LAYER{(object) layer}",
      "LOADD" + (object) Dungeon,
      "N",
      "NODELAST",
      "BOSSROOM"
    }, new float[5]{ 5f, 5f, 5f, 5f, 5f });
  }

  private static void SkipBossRoom()
  {
    UnifyComponent.Instance.gameObject.AddComponent<CheatChainRunner>();
    UnifyComponent.Instance.GetComponent<CheatChainRunner>().RunChain(new string[3]
    {
      "N",
      "NODELAST",
      "BOSSROOM"
    }, new float[5]{ 5f, 5f, 5f, 5f, 5f });
  }

  private static void SubmitReport() => GameManager.GetInstance();

  private static void LoadGame(int SaveSlot)
  {
    SaveAndLoad.SAVE_SLOT = SaveSlot;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 3f, "", (System.Action) (() =>
    {
      AudioManager.Instance.StopCurrentMusic();
      SaveAndLoad.Load(SaveAndLoad.SAVE_SLOT);
    }));
  }

  private void OnEnable()
  {
    CheatConsole.Instance = this;
    this.text.text = "";
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) CheatConsole.Instance == (UnityEngine.Object) this))
      return;
    CheatConsole.Instance = (CheatConsole) null;
  }

  private void PollForKeyPresses()
  {
    if ((double) Input.GetAxis("Mouse X") == 0.0 && (double) Input.GetAxis("Mouse Y") == 0.0 && !InputManager.General.GetAnyButton() && (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis()) <= 0.20000000298023224 && (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis()) <= 0.20000000298023224 && !LetterBox.IsPlaying && !MMConversation.isPlaying && !MMTransition.IsPlaying)
      return;
    CheatConsole.LastKeyPressTime = Time.unscaledTime;
  }

  public static void ForceResetTimeSinceLastKeyPress()
  {
    CheatConsole.LastKeyPressTime = Time.unscaledTime;
  }

  public static float TimeSinceLastKeyPress => Time.unscaledTime - CheatConsole.LastKeyPressTime;

  public void DisplayText(string Message, Color color)
  {
    this.text.text = Message;
    this.text.color = color;
  }

  private void OnGUI() => this.PollForKeyPresses();

  private void UpdateAutoComplete()
  {
    for (int index = this.autoCompleteItems.Count - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.autoCompleteItems[index].gameObject);
    this.autoCompleteItems.Clear();
    if ((UnityEngine.Object) this.backgroundText != (UnityEngine.Object) null)
      this.backgroundText.text = "";
    if (!(this.text.text != ""))
      return;
    this.Cheats = this.Cheats.OrderBy<KeyValuePair<string, System.Action>, string>((Func<KeyValuePair<string, System.Action>, string>) (x => x.Key)).ToDictionary<KeyValuePair<string, System.Action>, string, System.Action>((Func<KeyValuePair<string, System.Action>, string>) (x => x.Key), (Func<KeyValuePair<string, System.Action>, System.Action>) (x => x.Value));
    foreach (KeyValuePair<string, System.Action> cheat in this.Cheats)
    {
      if (cheat.Key.StartsWith(this.text.text))
      {
        if ((UnityEngine.Object) this.backgroundText != (UnityEngine.Object) null)
          this.backgroundText.text = cheat.Key;
        Text text = UnityEngine.Object.Instantiate<Text>(this.autoCompleteItemText, this.autoCompleteItemText.transform.parent);
        text.gameObject.SetActive(true);
        text.text = cheat.Key;
        this.autoCompleteItems.Add(text);
      }
    }
  }

  private void Start()
  {
    AFPSCounter.AddToScene(false);
    AFPSCounter.Instance.enabled = false;
    GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Canvas");
    if (!((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null))
      return;
    this.transform.parent = gameObjectWithTag.transform;
    this.transform.SetAsLastSibling();
  }

  private void Cancel()
  {
    this.text.text = "Invalid Cheat.";
    this.backgroundText.text = "";
    this.text.color = Color.red;
    this.Timer = 0.0f;
    CheatConsole.CurrentPhase = CheatConsole.Phase.RESPONSE;
  }

  private void CheatAccepted()
  {
    this.text.text = "Cheat Accepted!";
    this.text.color = Color.green;
    this.backgroundText.text = "";
    this.Timer = 0.0f;
    CheatConsole.CurrentPhase = CheatConsole.Phase.RESPONSE;
  }

  private static void AllBuildingsFree() => CheatConsole.BuildingsFree = true;

  private static void FPS()
  {
    if (!AFPSCounter.Instance.enabled)
      AFPSCounter.Instance.enabled = true;
    else
      AFPSCounter.Instance.enabled = false;
  }

  private static void SkipHour() => TimeManager.CurrentGameTime += 240f;

  private static void FollowerDebug()
  {
    SimulationManager.ShowFollowerDebugInfo = !SimulationManager.ShowFollowerDebugInfo;
    SimulationManager.ShowStructureDebugInfo = false;
  }

  private static void StructureDebug()
  {
    SimulationManager.ShowFollowerDebugInfo = false;
    SimulationManager.ShowStructureDebugInfo = !SimulationManager.ShowStructureDebugInfo;
  }

  private static void Heal()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().Heal(2f);
  }

  private static void AddHeart(int amount = 2)
  {
    HUD_Manager.Instance.Show(0);
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<HealthPlayer>().totalHP += (float) amount;
  }

  private static void Damage()
  {
    HUD_Manager.Instance.Show(0);
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(1f, withTag, withTag.transform.position);
  }

  private static void Damage5()
  {
    HUD_Manager.Instance.Show(0);
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(5f, withTag, withTag.transform.position);
  }

  private static void Die()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(9999f, withTag, withTag.transform.position);
  }

  private static void MoreHearts()
  {
    DataManager.Instance.PLAYER_TOTAL_HEALTH = 10f;
    DataManager.Instance.PLAYER_HEALTH = DataManager.Instance.PLAYER_TOTAL_HEALTH;
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(0.0f, withTag, withTag.transform.position);
  }

  private static void NextSandboxLayer()
  {
    DungeonSandboxManager.Instance.SetDungeonType(FollowerLocation.Dungeon1_4);
    MapManager.Instance.MapGenerated = false;
    UIAdventureMapOverlayController overlayController = MapManager.Instance.ShowMap(true);
    MapManager.Instance.StartCoroutine((IEnumerator) overlayController.NextSandboxLayer());
  }

  private static void BlueHearts()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<HealthPlayer>().BlueHearts += 2f;
  }

  private static void SpiritHeartsFull()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<HealthPlayer>().TotalSpiritHearts += 2f;
  }

  private static void Rituals()
  {
    UpgradeSystem.ClearAllCoolDowns();
    CheatConsole.UnlockAllRituals = true;
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Ascend);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Brainwashing);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Enlightenment);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Fast);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Feast);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Fightpit);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Funeral);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Holiday);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Ressurect);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Sacrifice);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Wedding);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_ConsumeFollower);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_DonationRitual);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FasterBuilding);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FirePit);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FishingRitual);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HarvestRitual);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AlmsToPoor);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignFaithEnforcer);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignTaxCollector);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_WorkThroughNight);
    CheatConsole.GiveResources();
    CheatConsole.GiveResources();
    CheatConsole.GiveResources();
  }

  private static void SpiritHeartsHalf()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    ++withTag.GetComponent<HealthPlayer>().TotalSpiritHearts;
  }

  private static void MoreSouls()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.GetSoul(100);
  }

  private static void MoreBlackSouls()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.GetBlackSoul(200);
  }

  private static void MoreArrows()
  {
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Combat_Arrows))
    {
      UnityEngine.Object.FindObjectOfType<HUD_Ammo>().Play();
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Combat_Arrows);
    }
    PlayerArrows objectOfType = UnityEngine.Object.FindObjectOfType<PlayerArrows>();
    if (!((UnityEngine.Object) objectOfType != (UnityEngine.Object) null))
      return;
    objectOfType.RestockAllArrows();
  }

  private static void HideUI()
  {
    System.Action onHideUi = CheatConsole.OnHideUI;
    if (onHideUi != null)
      onHideUi();
    MMTransition.OnTransitionCompelte -= new System.Action(CheatConsole.HideUI);
    if (!((UnityEngine.Object) CheatConsole.HideUIObject == (UnityEngine.Object) null))
      return;
    GameObject gameObject = new GameObject();
    gameObject.name = "Hide UI";
    CheatConsole.HideUIObject = gameObject.AddComponent<global::HideUI>();
    CheatConsole.HidingUI = true;
  }

  private static void ShowUI()
  {
    System.Action onShowUi = CheatConsole.OnShowUI;
    if (onShowUi != null)
      onShowUi();
    if (!((UnityEngine.Object) CheatConsole.HideUIObject != (UnityEngine.Object) null))
      return;
    CheatConsole.HideUIObject.ShowUI();
    CheatConsole.HideUIObject = (global::HideUI) null;
    CheatConsole.HidingUI = false;
  }

  private static void TurnOffResourceHighlight()
  {
    Shader.SetGlobalInt("_GlobalResourceHighlight", 0);
  }

  private static void Rain() => WeatherController.Instance.SetRain();

  private static void Wind() => WeatherController.Instance.SetWind();

  private static void GiveResources()
  {
    GameObject.FindWithTag("Player");
    Inventory.AddItem(1, 100);
    Inventory.AddItem(2, 100);
    Inventory.AddItem(35, 100);
    Inventory.AddItem(20, 100);
    Inventory.AddItem(9, 100);
    Inventory.AddItem(83, 100);
    Inventory.AddItem(86, 100);
    Inventory.AddItem(81, 100);
    Inventory.AddItem(82, 100);
    Inventory.AddItem(55, 100);
    Inventory.AddItem(89, 100);
    Inventory.AddItem(90, 100);
    Inventory.AddItem(117, 3);
  }

  private static void GivePoop() => Inventory.AddItem(39, 100);

  private static void GiveStartingPack()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    for (int index = 0; index < 30; ++index)
    {
      UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/Resources/Log"), withTag.transform.position, Quaternion.identity);
      UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/Resources/BlackGold"), withTag.transform.position, Quaternion.identity);
      UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/Resources/Grass"), withTag.transform.position, Quaternion.identity);
    }
  }

  private static void GiveKeys()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    for (int index = 0; index < 3; ++index)
      UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/Resources/Key Piece"), withTag.transform.position, Quaternion.identity);
  }

  private static void GiveFood()
  {
    Inventory.AddItem(21, 10);
    Inventory.AddItem(105, 10);
    Inventory.AddItem(6, 10);
    Inventory.AddItem(50, 10);
    Inventory.AddItem(97, 10);
    Inventory.AddItem(102, 10);
    Inventory.AddItem(62, 5);
  }

  private static void Fish()
  {
    Inventory.AddItem(28, 5);
    Inventory.AddItem(34, 5);
    Inventory.AddItem(33, 5);
    Inventory.AddItem(94, 5);
    Inventory.AddItem(91, 5);
    Inventory.AddItem(92, 5);
    Inventory.AddItem(93, 5);
    Inventory.AddItem(96 /*0x60*/, 5);
    Inventory.AddItem(95, 5);
  }

  private static void MonsterHeart() => Inventory.AddItem(22, 5);

  private static void BuildAll()
  {
    foreach (Structures_BuildSite structuresBuildSite in StructureManager.GetAllStructuresOfType<Structures_BuildSite>())
      structuresBuildSite.BuildProgress = (float) StructuresData.BuildDurationGameMinutes(structuresBuildSite.Data.ToBuildType);
    foreach (Structures_BuildSiteProject buildSiteProject in StructureManager.GetAllStructuresOfType<Structures_BuildSiteProject>())
      buildSiteProject.BuildProgress = (float) StructuresData.BuildDurationGameMinutes(buildSiteProject.Data.ToBuildType);
  }

  private static void Mushroom() => Inventory.AddItem(29, 100);

  private static void ReturnToBase() => GameManager.ToShip();

  private static void DebugInfo()
  {
    Debug.Log((object) ("DataManager.Instance.BlueprintsChest.Count " + (object) DataManager.Instance.PlayerBluePrints.Count));
    Debug.Log((object) ("DataManager.Instance.Blueprints.Count " + (object) DataManager.Instance.PlayerBluePrints.Count));
  }

  private static void AllCurses()
  {
  }

  private static void AllTrinkets()
  {
    DataManager.Instance.PlayerFoundTrinkets.Clear();
    foreach (TarotCards.Card allTrinket in DataManager.AllTrinkets)
      DataManager.Instance.PlayerFoundTrinkets.Add(allTrinket);
  }

  private static void ImplementedTrinkets()
  {
  }

  private static void ToggleCollider()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance.circleCollider2D != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.circleCollider2D.enabled = !PlayerFarming.Instance.circleCollider2D.enabled;
  }

  private static void Left() => BiomeGenerator.ChangeRoom(new Vector2Int(-1, 0));

  private static void Right() => BiomeGenerator.ChangeRoom(new Vector2Int(1, 0));

  private static void Up() => BiomeGenerator.ChangeRoom(new Vector2Int(0, 1));

  private static void Down() => BiomeGenerator.ChangeRoom(new Vector2Int(0, -1));

  private static void ShowMap() => UnityEngine.Object.FindObjectOfType<MiniMap>()?.VisitAll();

  private static void UnlockCrownAbility(CrownAbilities.TYPE Type)
  {
    CrownAbilities.UnlockAbility(Type);
  }

  private static void FollowerToken() => ++Inventory.FollowerTokens;

  private static void FollowerTokens() => Inventory.FollowerTokens += 100;

  private static void TestObjectives()
  {
    CheatConsole.CompleteCurrentObjectives();
    Quests.IsDebug = true;
    DataManager.Instance.TimeSinceLastQuest = float.MaxValue;
  }

  private static void NameCult() => DataManager.Instance.OnboardedCultName = false;

  private static void CustomObjective_Create()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/Quest", StructureBrain.TYPES.DECORATION_BONE_CANDLE, expireTimestamp: 3600f));
  }

  private static void CustomObjective_Complete()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.Test);
  }

  private static void ResetCooldowns()
  {
    for (int index = 0; index < DataManager.Instance.LastUsedSermonRitualDayIndex.Length; ++index)
      DataManager.Instance.LastUsedSermonRitualDayIndex[index] = -1;
  }

  private static void KillFollowers()
  {
    for (int index = DataManager.Instance.Followers.Count - 1; index >= 0; --index)
      FollowerBrain.FindBrainByID(DataManager.Instance.Followers[index].ID).HardSwapToTask((FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.Died));
  }

  private static void RemoveFaith()
  {
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
      follower.Faith -= 10f;
  }

  private static void UnlockAllSermons()
  {
    for (int index = 0; index < 23; ++index)
    {
      SermonsAndRituals.SermonRitualType sermonRitualType = (SermonsAndRituals.SermonRitualType) index;
      if (sermonRitualType != SermonsAndRituals.SermonRitualType.NONE && !DataManager.Instance.UnlockedSermonsAndRituals.Contains(sermonRitualType))
        DataManager.Instance.UnlockedSermonsAndRituals.Add(sermonRitualType);
    }
  }

  private static IEnumerator ResetMap()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.RegenerateMapRoutine();
  }

  private static void UnlockAll()
  {
    CheatConsole.UnlockAllRituals = true;
    for (int index = 0; index < Enum.GetNames(typeof (UpgradeSystem.Type)).Length; ++index)
      UpgradeSystem.UnlockAbility((UpgradeSystem.Type) index);
    GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
  }

  private static void UnlockWeapons()
  {
    DataManager.Instance.AddWeapon(EquipmentType.Axe);
    DataManager.Instance.AddWeapon(EquipmentType.Dagger);
    DataManager.Instance.AddWeapon(EquipmentType.Gauntlet);
    DataManager.Instance.AddWeapon(EquipmentType.Hammer);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_CursePack1);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_CursePack2);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_CursePack3);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_CursePack4);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_CursePack5);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_WeaponFervor);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_WeaponGodly);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_WeaponNecromancy);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_WeaponPoison);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.PUpgrade_WeaponCritHit);
    GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
  }

  private static void RunTrinket(TarotCards.TarotCard card) => TrinketManager.AddTrinket(card);

  private static void RunTrinket(TarotCards.Card card)
  {
    TrinketManager.AddTrinket(new TarotCards.TarotCard(card, 0));
  }

  private static void EnableTarot() => DataManager.Instance.HasTarotBuilding = true;

  private static void EnableBlackSouls()
  {
    DataManager.Instance.BlackSoulsEnabled = true;
    UnityEngine.Object.FindObjectOfType<HUD_BlackSoul>().RingsObject.gameObject.SetActive(true);
  }

  private static void SetResolution() => Screen.SetResolution(1920, 1080, true);

  private static void ToggleFullScreen() => Screen.fullScreen = !Screen.fullScreen;

  public static void SkipToPhase(DayPhase phase)
  {
    StateMachine.State prevState = PlayerFarming.Instance.state.CURRENT_STATE;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(GameObject.FindWithTag("Player Camera Bone"), 3f);
    SimulationManager.SkipToPhase(phase, (System.Action) (() =>
    {
      PlayerFarming.Instance.state.CURRENT_STATE = prevState;
      GameManager.GetInstance().OnConversationEnd();
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", new System.Action(MMTransition.ResumePlay));
    }));
  }

  private static void ClearRubble()
  {
    List<StructureBrain> structureBrainList = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.StructuresAtLocation(FollowerLocation.Base));
    for (int index = 0; index < structureBrainList.Count; ++index)
    {
      StructureBrain structureBrain = structureBrainList[index];
      if (structureBrain.Data.Type == StructureBrain.TYPES.RUBBLE || structureBrain.Data.Type == StructureBrain.TYPES.RUBBLE_BIG)
        (structureBrain as Structures_Rubble).Remove();
    }
  }

  private static void ClearWeed()
  {
    List<StructureBrain> structureBrainList = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.StructuresAtLocation(FollowerLocation.Base));
    for (int index = 0; index < structureBrainList.Count; ++index)
    {
      StructureBrain structureBrain = structureBrainList[index];
      if (structureBrain.Data.Type == StructureBrain.TYPES.WEEDS)
        (structureBrain as Structures_Weeds).Remove();
    }
  }

  private static void UnlockAllStructures()
  {
    foreach (StructureBrain.TYPES Types in Enum.GetValues(typeof (StructureBrain.TYPES)))
    {
      if (!StructuresData.GetUnlocked(Types))
        DataManager.Instance.UnlockedStructures.Add(Types);
    }
  }

  private static void CompleteCurrentObjectives()
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      objective.IsComplete = true;
      objective.IsFailed = false;
      DataManager.Instance.AddToCompletedQuestHistory(objective.GetFinalizedData());
    }
    ObjectiveManager.CheckObjectives();
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      completedObjective.IsComplete = true;
      completedObjective.IsFailed = false;
    }
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.CurrentPlayerQuest != null)
        follower.CurrentPlayerQuest.IsComplete = true;
    }
    DataManager.Instance.Objectives.Clear();
  }

  private static void ToggleGodMode()
  {
    if (PlayerFarming.Instance.health.GodMode == Health.CheatMode.God)
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.None;
    else
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.God;
  }

  private static void ToggleDemiGodMode()
  {
    if (PlayerFarming.Instance.health.GodMode == Health.CheatMode.Demigod)
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.None;
    else
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.Demigod;
  }

  private static void ToggleImmortalMode()
  {
    if (PlayerFarming.Instance.health.GodMode == Health.CheatMode.Immortal)
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.None;
    else
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.Immortal;
  }

  private static void ToggleNoClip()
  {
    Collider2D collider2D = (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? PlayerFarming.Instance.GetComponent<Collider2D>() : PlayerPrisonerController.Instance.GetComponent<Collider2D>();
    collider2D.isTrigger = !collider2D.isTrigger;
  }

  private static void CreateFollower(int Num)
  {
    foreach (WorshipperData.SkinAndData character in WorshipperData.Instance.Characters)
    {
      if (!DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin))
        DataManager.SetFollowerSkinUnlocked(character.Skin[0].Skin);
    }
    while (--Num >= 0)
    {
      Follower newFollower = FollowerManager.CreateNewFollower(PlayerFarming.Location, PlayerFarming.Instance.transform.position);
      if ((double) UnityEngine.Random.value < 0.5)
      {
        newFollower.Brain.Info.FollowerRole = FollowerRole.Worshipper;
        newFollower.Brain.Info.Outfit = FollowerOutfitType.Follower;
        newFollower.SetOutfit(FollowerOutfitType.Follower, false);
        newFollower.Brain.CheckChangeTask();
      }
      else
      {
        newFollower.Brain.Info.FollowerRole = FollowerRole.Worker;
        newFollower.Brain.Info.Outfit = FollowerOutfitType.Follower;
        newFollower.SetOutfit(FollowerOutfitType.Follower, false);
        newFollower.Brain.Info.WorkerPriority = WorkerPriority.None;
        newFollower.Brain.Stats.WorkerBeenGivenOrders = true;
        newFollower.Brain.CheckChangeTask();
      }
    }
  }

  private static void EndKnucklebones() => UnityEngine.Object.FindObjectOfType<KBGameScreen>().ForceEndGame();

  public enum Phase
  {
    OFF,
    ON,
    RESPONSE,
  }

  public enum FaithTypes
  {
    OFF,
    DRIP,
  }
}
