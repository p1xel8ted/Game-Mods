// Decompiled with JetBrains decompiler
// Type: CheatConsole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using CodeStage.AdvancedFPSCounter;
using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Lamb.UI.MainMenu;
using Map;
using MMBiomeGeneration;
using MMTools;
using src.Extensions;
using src.Managers;
using src.UI.Menus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CheatConsole : BaseMonoBehaviour
{
  public static bool _inDemo = false;
  public const float DEMO_ATTRACT_MODE_TIMER = 20f;
  public const float DEMO_INACTIVE_TIMER = 120f;
  public const float DEMO_HOLD_TO_RESET_TIMER = 5f;
  public const float DEMO_MAX_TIMER = 1200f;
  public static float DemoBeginTime = 0.0f;
  public static bool MAJOR_DLC_DISABLED = false;
  public static bool WIREFRAME_ENABLED = false;
  public Text text;
  public Text backgroundText;
  public Text autoCompleteItemText;
  public static CheatConsole.Phase CurrentPhase;
  public float Timer;
  public static CheatConsole.FaithTypes FaithType = CheatConsole.FaithTypes.DRIP;
  public List<Text> autoCompleteItems = new List<Text>();
  public static GameObject[] resourcesToSpawn;
  public static GameObject SubmitReportPrefab;
  public static bool ShowAllMapLocations = false;
  public static bool Robes = false;
  public static bool ForceSpiderMiniBoss = false;
  public static bool UglyWeeds = false;
  public static bool ForceSmoochEnabled = false;
  public static bool ForceBlessEnabled = false;
  public static bool QuickUnlock = false;
  public static bool BleatKillAll = false;
  public static bool AllMapNodesAttainable = false;
  public static bool ForceRoom = false;
  public static string ForceRoomPath = "";
  public static bool ForceRatoo = false;
  public static bool ForceBaal = false;
  public static int ForcedMiniBossIndex = -1;
  public static Objectives.TYPES ForcedObjective = Objectives.TYPES.NONE;
  public static Objectives.CustomQuestTypes ForcedCustomObjective = Objectives.CustomQuestTypes.None;
  public static UpgradeSystem.Type ForcedRitualObjective;
  public static InventoryItem.ITEM_TYPE ForcedItem = InventoryItem.ITEM_TYPE.NONE;
  public static Thought ForcedFollowerCursedState = Thought.None;
  public static bool playerHidden = false;
  public static int OverrideDungeonSeed = 0;
  public static bool DoOverrideDungeonSeed = false;
  public static GameObject GrassBits;
  public static GameObject GrassTufts;
  public static bool SHOWING_LOC_KEYS = false;
  public static Dictionary<string, string> TRANSLATED_TEXT = new Dictionary<string, string>();
  public Dictionary<string, System.Action> Cheats = new Dictionary<string, System.Action>();
  public static CheatConsole Instance;
  public Color TextColor = Color.white;
  public static bool ForceAutoAttractMode = false;
  public static float LastKeyPressTime;
  public string lastCheat = "";
  public List<char> CheatsKey = new List<char>();
  public static bool BuildingsFree = false;
  public static bool AllBuildingsUnlocked = false;
  public static bool UnlockAllRituals = false;
  public static global::HideUI HideUIObject;
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

  public static IEnumerator CombatNodes()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.ConvertAllNodesToCombatNodes();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
  }

  public static void ShowYngyaHeartRoom()
  {
    if ((UnityEngine.Object) YngyaRoomManager.Instance == (UnityEngine.Object) null)
      Debug.LogError((object) "Please run this command inside Dungeon 6");
    else
      YngyaRoomManager.Play();
  }

  public static void SpawnSnowball()
  {
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SNOW_CHUNK, 1, PlayerFarming.Instance.transform.position, (float) UnityEngine.Random.Range(9, 11), (Action<PickUp>) (pickUp => GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      Interaction_IceBlock component = pickUp.GetComponent<Interaction_IceBlock>();
      component.Structure.CreateStructure(FollowerLocation.Base, component.transform.position);
      component.Structure.Brain.AddToGrid();
    }))));
  }

  public static void NurserySnowballFight()
  {
    foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
    {
      if (!((UnityEngine.Object) daycare == (UnityEngine.Object) null))
      {
        using (List<int>.Enumerator enumerator = daycare.Structure.Brain.Data.MultipleFollowerIDs.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Follower followerById = FollowerManager.FindFollowerByID(enumerator.Current);
            if (!((UnityEngine.Object) followerById == (UnityEngine.Object) null))
            {
              FollowerTask_SnowballFight nextTask = new FollowerTask_SnowballFight(FollowerManager.FindFollowerByID(FollowerManager.GetRandomNonLockedFollower().Info.ID), false);
              followerById.Brain.HardSwapToTask((FollowerTask) nextTask);
              break;
            }
          }
          break;
        }
      }
    }
  }

  public static void UnlockWinterMode()
  {
    PersistenceManager.PersistentData.UnlockedWinterMode = true;
    PersistenceManager.Save();
  }

  public static void LOCToggle()
  {
    LocalizationManager.HighlightLocalizedTargets = !LocalizationManager.HighlightLocalizedTargets;
    LocalizationManager.LocalizeAll(true);
  }

  public static void IceBlockHell()
  {
    for (int index = 0; index < 100; ++index)
      StructureManager.PlaceIceBlock(FollowerLocation.Base, new List<Structures_PlacementRegion>()
      {
        PlacementRegion.Instance.structureBrain
      });
  }

  public static IEnumerator BossNodes()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.ConvertMiniBossNodeToBossNode();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
  }

  public static IEnumerator RandomizeNodes()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.RandomiseNextNodes();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
  }

  public static IEnumerator TeleportNode()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    Map.Node randomNodeOnLayer = MapGenerator.GetRandomNodeOnLayer(MapManager.Instance.CurrentLayer + 2);
    if (randomNodeOnLayer != null)
      yield return (object) adventureMapOverlayController.TeleportNode(randomNodeOnLayer);
  }

  public static void SoakTest()
  {
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful6);
    DataManager.Instance.PLAYER_HEARTS_LEVEL = 5;
    UnifyComponent.Instance.gameObject.AddComponent<CheatChainRunner>();
    UnifyComponent.Instance.GetComponent<CheatChainRunner>().RunChainForEver(new string[196]
    {
      "SETDUNGEON1LAYER1",
      "LOADD1",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON1LAYER2",
      "LOADD1",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON1LAYER3",
      "LOADD1",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON1LAYER4",
      "LOADD1",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "FIXFOLLOWERS",
      "SETDUNGEON2LAYER1",
      "LOADD2",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON2LAYER2",
      "LOADD2",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON2LAYER3",
      "LOADD2",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON2LAYER4",
      "LOADD2",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "FIXFOLLOWERS",
      "SETDUNGEON3LAYER1",
      "LOADD3",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON3LAYER2",
      "LOADD3",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON3LAYER3",
      "LOADD3",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON3LAYER4",
      "LOADD3",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "FIXFOLLOWERS",
      "SETDUNGEON4LAYER1",
      "LOADD4",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON4LAYER2",
      "LOADD4",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON4LAYER3",
      "LOADD4",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "SETDUNGEON4LAYER4",
      "LOADD4",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "N",
      "NODELAST",
      "GOD",
      "NEXTROOM",
      "NEXTROOM",
      "BOSSROOM",
      "BASE",
      "FIXFOLLOWERS"
    }, 60f);
  }

  public static void BossRoom(int layer, int Dungeon)
  {
    UnifyComponent.Instance.gameObject.AddComponent<CheatChainRunner>();
    UnifyComponent.Instance.GetComponent<CheatChainRunner>().RunChain(new string[5]
    {
      $"SETDUNGEON{Dungeon.ToString()}LAYER{layer.ToString()}",
      "LOADD" + Dungeon.ToString(),
      "N",
      "NODELAST",
      "BOSSROOM"
    }, new float[5]{ 5f, 5f, 5f, 5f, 5f });
  }

  public static void SkipBossRoom()
  {
    UnifyComponent.Instance.gameObject.AddComponent<CheatChainRunner>();
    UnifyComponent.Instance.GetComponent<CheatChainRunner>().RunChain(new string[5]
    {
      "NOCLIP",
      "GOD",
      "N",
      "NODELAST",
      "SWORD"
    }, new float[5]{ 1f, 1f, 2f, 2f, 2f });
  }

  public static void SubmitReport() => GameManager.GetInstance();

  public static void LoadGame(int SaveSlot)
  {
    SaveAndLoad.SAVE_SLOT = SaveSlot;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 3f, "", (System.Action) (() =>
    {
      AudioManager.Instance.StopCurrentMusic();
      SaveAndLoad.Load(SaveAndLoad.SAVE_SLOT);
    }));
  }

  public void OnEnable()
  {
    CheatConsole.Instance = this;
    this.text.text = "";
    this.Cheats = this.Cheats.OrderBy<KeyValuePair<string, System.Action>, string>((Func<KeyValuePair<string, System.Action>, string>) (x => x.Key)).ToDictionary<KeyValuePair<string, System.Action>, string, System.Action>((Func<KeyValuePair<string, System.Action>, string>) (x => x.Key), (Func<KeyValuePair<string, System.Action>, System.Action>) (x => x.Value));
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) CheatConsole.Instance == (UnityEngine.Object) this))
      return;
    CheatConsole.Instance = (CheatConsole) null;
  }

  public void PollForKeyPresses()
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

  public void UpdateAutoComplete()
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

  public void Start()
  {
    AFPSCounter.AddToScene(false);
    AFPSCounter.Instance.enabled = false;
    GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Canvas");
    if (!((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null))
      return;
    this.transform.parent = gameObjectWithTag.transform;
    this.transform.SetAsLastSibling();
  }

  public void Cancel()
  {
    this.text.text = "Invalid Cheat.";
    this.backgroundText.text = "";
    this.text.color = Color.red;
    this.Timer = 0.0f;
    CheatConsole.CurrentPhase = CheatConsole.Phase.RESPONSE;
  }

  public void CheatAccepted()
  {
    DataManager.Instance.CheatHistory.Add($"{this.text.text} {TimeManager.CurrentGameTime.ToString()}");
    this.text.text = "Cheat Accepted!";
    this.text.color = Color.green;
    this.backgroundText.text = "";
    this.Timer = 0.0f;
    CheatConsole.CurrentPhase = CheatConsole.Phase.RESPONSE;
  }

  public static void AllBuildingsFree() => CheatConsole.BuildingsFree = true;

  public static void FPS()
  {
    if (!AFPSCounter.Instance.enabled)
      AFPSCounter.Instance.enabled = true;
    else
      AFPSCounter.Instance.enabled = false;
  }

  public static void SkipHour() => TimeManager.CurrentGameTime += 240f;

  public static void FollowerDebug()
  {
    SimulationManager.ShowFollowerDebugInfo = !SimulationManager.ShowFollowerDebugInfo;
    SimulationManager.ShowStructureDebugInfo = false;
  }

  public static void StructureDebug()
  {
    SimulationManager.ShowFollowerDebugInfo = false;
    SimulationManager.ShowStructureDebugInfo = !SimulationManager.ShowStructureDebugInfo;
  }

  public static void Heal()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().Heal(2f);
  }

  public static void AddHeart(int amount = 2)
  {
    HUD_Manager.Instance.Show(0);
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<HealthPlayer>().totalHP += (float) amount;
  }

  public static void Damage()
  {
    HUD_Manager.Instance.Show(0);
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(1f, withTag, withTag.transform.position);
  }

  public static void Damage5()
  {
    HUD_Manager.Instance.Show(0);
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(5f, withTag, withTag.transform.position);
  }

  public static void Die()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(9999f, withTag, withTag.transform.position);
  }

  public static void Die2()
  {
    DataManager.Instance.CurrentChallengeModeXP = 30;
    DataManager.Instance.CurrentChallengeModeLevel = 1;
    Inventory.AddItem(128 /*0x80*/, 400);
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(9999f, withTag, withTag.transform.position);
  }

  public static void Die3()
  {
    DataManager.Instance.CurrentChallengeModeXP = 30;
    DataManager.Instance.CurrentChallengeModeLevel = 1;
    Inventory.AddItem(119, 1);
    Inventory.AddItem(128 /*0x80*/, 200);
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(9999f, withTag, withTag.transform.position);
  }

  public static void MoreHearts()
  {
    PlayerFarming.Instance.health.PLAYER_TOTAL_HEALTH = 10f;
    PlayerFarming.Instance.health.PLAYER_HEALTH = PlayerFarming.Instance.health.PLAYER_TOTAL_HEALTH;
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<Health>().DealDamage(0.0f, withTag, withTag.transform.position);
  }

  public static void NextSandboxLayer()
  {
    DungeonSandboxManager.Instance.SetDungeonType(FollowerLocation.Dungeon1_4);
    MapManager.Instance.MapGenerated = false;
    UIAdventureMapOverlayController overlayController = MapManager.Instance.ShowMap(true);
    MapManager.Instance.StartCoroutine((IEnumerator) overlayController.NextSandboxLayer());
  }

  public static void BlueHearts()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<HealthPlayer>().BlueHearts += 2f;
  }

  public static void FireHearts()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<HealthPlayer>().FireHearts += 2f;
  }

  public static void IceHearts()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<HealthPlayer>().IceHearts += 2f;
  }

  public static void SpiritHeartsFull()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    withTag.GetComponent<HealthPlayer>().TotalSpiritHearts += 2f;
  }

  public static void Rituals()
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
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Warmth);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Snowman);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FollowerWedding);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Divorce);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Midwinter);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FishingRitual);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HarvestRitual);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AlmsToPoor);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignFaithEnforcer);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignTaxCollector);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_WorkThroughNight);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_BecomeDisciple);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Cannibal);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Nudism);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Purge);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AtoneSin);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_RanchHarvest);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_RanchMeat);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_ConvertToRot);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Winter_RemoveRot);
    CheatConsole.GiveResources();
    CheatConsole.GiveResources();
    CheatConsole.GiveResources();
  }

  public static void SpiritHeartsHalf()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    ++withTag.GetComponent<HealthPlayer>().TotalSpiritHearts;
  }

  public static void MoreSouls()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.GetSoul(100);
  }

  public static void MoreBlackSouls()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.GetBlackSoul(200);
  }

  public static void MoreArrows()
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

  public static void HideUI()
  {
    System.Action onHideUi = CheatConsole.OnHideUI;
    if (onHideUi != null)
      onHideUi();
    MMTransition.OnTransitionCompelte -= new System.Action(CheatConsole.HideUI);
    CheatConsole.HidingUI = true;
    if ((UnityEngine.Object) CheatConsole.HideUIObject == (UnityEngine.Object) null)
    {
      GameObject target = new GameObject("Hide UI");
      CheatConsole.HideUIObject = target.AddComponent<global::HideUI>();
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
    }
    if (!((UnityEngine.Object) global::HideUI.Instance != (UnityEngine.Object) null))
      return;
    global::HideUI.Instance.ResetHideTime();
  }

  public static void ShowUI()
  {
    System.Action onShowUi = CheatConsole.OnShowUI;
    if (onShowUi != null)
      onShowUi();
    MMTransition.OnTransitionCompelte -= new System.Action(CheatConsole.HideUI);
    if ((UnityEngine.Object) CheatConsole.HideUIObject != (UnityEngine.Object) null)
      CheatConsole.HideUIObject.ShowUI();
    CheatConsole.HidingUI = false;
  }

  public static void TurnOffResourceHighlight()
  {
    Shader.SetGlobalInt("_GlobalResourceHighlight", 0);
  }

  public static void GiveGhostFleeces()
  {
    Inventory.AddItem(216, 1);
    Inventory.AddItem(217, 1);
    Inventory.AddItem(218, 1);
    Inventory.AddItem(219, 1);
    Inventory.AddItem(220, 1);
    Inventory.AddItem(221, 1);
    Inventory.AddItem(222, 1);
    Inventory.AddItem(223, 1);
    Inventory.AddItem(224 /*0xE0*/, 1);
    Inventory.AddItem(225, 1);
    Inventory.AddItem(235, 1);
  }

  public static void ResetDLCMap()
  {
    DataManager.Instance.DLCDungeonNodeCurrent = -1;
    DataManager.Instance.DLCDungeonNodesCompleted.Clear();
  }

  public static void GiveResources()
  {
    GameObject.FindWithTag("Player");
    Inventory.AddItem(56, 100);
    Inventory.AddItem(77, 100);
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
    Inventory.AddItem(133, 100);
    Inventory.AddItem(150, 100);
    Inventory.AddItem(151, 100);
    Inventory.AddItem(167, 100);
    Inventory.AddItem(159, 100);
    Inventory.AddItem(140, 100);
    Inventory.AddItem(169, 100);
    Inventory.AddItem(152, 100);
    Inventory.AddItem(166, 100);
    Inventory.AddItem(153, 100);
    Inventory.AddItem(132, 100);
    Inventory.AddItem(165, 100);
    Inventory.AddItem(188, 100);
    Inventory.AddItem(189, 100);
    Inventory.AddItem(170, 100);
    Inventory.AddItem(177, 100);
    Inventory.AddItem(179, 100);
    Inventory.AddItem(178, 100);
    Inventory.AddItem(176 /*0xB0*/, 100);
    Inventory.AddItem(139, 100);
    Inventory.AddItem(172, 100);
    Inventory.AddItem(186, 100);
    Inventory.AddItem(29, 100);
    Inventory.AddItem(230, 100);
    Inventory.AddItem(231, 100);
  }

  public static void GiveSomeAnimals()
  {
    Inventory.AddItem(188, 5);
    Inventory.AddItem(189, 5);
    Inventory.AddItem(176 /*0xB0*/, 5);
  }

  public static void GivePoop()
  {
    Inventory.AddItem(39, 100);
    Inventory.AddItem(142, 100);
    Inventory.AddItem(144 /*0x90*/, 100);
    Inventory.AddItem(143, 100);
    Inventory.AddItem(162, 100);
    Inventory.AddItem(187, 100);
  }

  public static void GiveStartingPack()
  {
    Inventory.AddItem(1, 30);
    Inventory.AddItem(20, 30);
    Inventory.AddItem(2, 30);
    Inventory.AddItem(35, 30);
    Inventory.AddItem(29, 5);
    Inventory.AddItem(89, 5);
    Inventory.AddItem(90, 5);
    Inventory.AddItem(154, 1);
  }

  public static void GiveKeys()
  {
    GameObject withTag = GameObject.FindWithTag("Player");
    for (int index = 0; index < 3; ++index)
      UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/Resources/Key Piece"), withTag.transform.position, Quaternion.identity);
  }

  public static void GiveFood()
  {
    Inventory.AddItem(21, 10);
    Inventory.AddItem(105, 10);
    Inventory.AddItem(6, 10);
    Inventory.AddItem(50, 10);
    Inventory.AddItem(97, 10);
    Inventory.AddItem(102, 10);
    Inventory.AddItem(62, 5);
    Inventory.AddItem(159, 5);
    Inventory.AddItem(167, 5);
    Inventory.AddItem(151, 5);
    Inventory.AddItem(150, 5);
    Inventory.AddItem(168, 5);
    Inventory.AddItem(197, 5);
    Inventory.AddItem(167, 5);
  }

  public static void Fish()
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
    Inventory.AddItem(135, 5);
    Inventory.AddItem(136, 5);
    Inventory.AddItem(137, 5);
  }

  public static void MonsterHeart() => Inventory.AddItem(22, 5);

  public static void Egg()
  {
    if ((UnityEngine.Object) Interaction_MatingTent.Instance == (UnityEngine.Object) null)
    {
      Debug.Log((object) "No Mating Tent built, Egg command requires a Mating Tent.");
    }
    else
    {
      List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
      foreach (Follower follower in Follower.Followers)
      {
        if (follower.Brain.Info.IsSnowman && !follower.Brain.Info.IsGoodSnowman)
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.Unavailable));
        else if (!DataManager.Instance.Followers_Recruit.Contains(follower.Brain._directInfoAccess) && !follower.Brain.Info.SkinName.Contains("Webber"))
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain, true)));
      }
      System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadMatingMenuAssets();
      GameManager.GetInstance().StartCoroutine((IEnumerator) UIManager.LoadAssets(task, (System.Action) (() =>
      {
        UIMatingMenuController matingMenuController = MonoSingleton<UIManager>.Instance.MatingMenuControllerTemplate.Instantiate<UIMatingMenuController>();
        Interaction_MatingTent tent = Interaction_MatingTent.Instance;
        matingMenuController.Show(tent, followerSelectEntries);
        matingMenuController.OnFollowersChosen += (Action<FollowerInfo, FollowerInfo>) ((f1, f2) =>
        {
          Follower followerById3 = FollowerManager.FindFollowerByID(f1.ID);
          Follower followerById4 = FollowerManager.FindFollowerByID(f2.ID);
          Transform transform = PlayerFarming.Instance.transform;
          Vector3 position = transform.position;
          StructuresData egg = StructuresData.GetInfoByType(StructureBrain.TYPES.EGG_FOLLOWER, 0);
          Structures_MatingTent brain = tent.Structure.Brain as Structures_MatingTent;
          brain.SetEggInfo(followerById3.Brain, followerById4.Brain, 1f);
          brain.SetEggReady();
          egg.EggInfo = brain.Data.EggInfo;
          brain.CollectEgg();
          StructureManager.BuildStructure(FollowerLocation.Base, egg, position, Vector2Int.one, false, (Action<GameObject>) (obj =>
          {
            obj.transform.DOPunchScale(obj.transform.localScale * 0.3f, 0.3f);
            obj.GetComponent<Interaction_EggFollower>().UpdateEgg(false, false, egg.EggInfo.Rotting, egg.EggInfo.Golden, egg.EggInfo.Special);
            obj.GetComponent<PickUp>().Bounce = false;
            if (!egg.EggInfo.Rotting || !((UnityEngine.Object) PathTileManager.Instance != (UnityEngine.Object) null) || PathTileManager.Instance.GetTileTypeAtPosition(transform.position) != StructureBrain.TYPES.NONE)
              return;
            PathTileManager.Instance.SetTile(StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR, transform.position);
          }));
        });
        matingMenuController.OnHidden = matingMenuController.OnHidden + (System.Action) (() =>
        {
          GameManager.GetInstance().OnConversationEnd();
          Time.timeScale = 1f;
        });
      })));
    }
  }

  public static void GotoTest()
  {
    PlayerFarming.Instance.GoToAndStop(Vector3.zero, IdleOnEnd: true);
  }

  public static void BuildAll()
  {
    foreach (Structures_BuildSite structuresBuildSite in StructureManager.GetAllStructuresOfType<Structures_BuildSite>())
      structuresBuildSite.BuildProgress = (float) StructuresData.BuildDurationGameMinutes(structuresBuildSite.Data.ToBuildType);
    foreach (Structures_BuildSiteProject buildSiteProject in StructureManager.GetAllStructuresOfType<Structures_BuildSiteProject>())
      buildSiteProject.BuildProgress = (float) StructuresData.BuildDurationGameMinutes(buildSiteProject.Data.ToBuildType);
  }

  public static void Mushroom() => Inventory.AddItem(29, 100);

  public static void ReturnToBase() => GameManager.ToShip();

  public static void DebugInfo()
  {
    Debug.Log((object) ("DataManager.Instance.BlueprintsChest.Count " + DataManager.Instance.PlayerBluePrints.Count.ToString()));
    Debug.Log((object) ("DataManager.Instance.Blueprints.Count " + DataManager.Instance.PlayerBluePrints.Count.ToString()));
  }

  public static void AllCurses()
  {
  }

  public static void AllTrinkets()
  {
    DataManager.Instance.PlayerFoundTrinkets.Clear();
    foreach (TarotCards.Card allTrinket in DataManager.AllTrinkets)
      DataManager.Instance.PlayerFoundTrinkets.Add(allTrinket);
  }

  public static void ImplementedTrinkets()
  {
  }

  public static void ToggleCollider()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance.circleCollider2D != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.circleCollider2D.enabled = !PlayerFarming.Instance.circleCollider2D.enabled;
  }

  public static void Left() => BiomeGenerator.ChangeRoom(new Vector2Int(-1, 0));

  public static void Right() => BiomeGenerator.ChangeRoom(new Vector2Int(1, 0));

  public static void Up() => BiomeGenerator.ChangeRoom(new Vector2Int(0, 1));

  public static void Down() => BiomeGenerator.ChangeRoom(new Vector2Int(0, -1));

  public static void ShowMap() => UnityEngine.Object.FindObjectOfType<MiniMap>()?.VisitAll();

  public static void UnlockCrownAbility(CrownAbilities.TYPE Type)
  {
    CrownAbilities.UnlockAbility(Type);
  }

  public static void FollowerToken() => ++Inventory.FollowerTokens;

  public static void FollowerTokens() => Inventory.FollowerTokens += 100;

  public static void TestObjectives()
  {
    CheatConsole.CompleteCurrentObjectives();
    Quests.IS_DEBUG = true;
    DataManager.Instance.TimeSinceLastQuest = float.MaxValue;
  }

  public static void NameCult() => DataManager.Instance.OnboardedCultName = false;

  public static void CustomObjective_Create()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_FinishRace("Objectives/GroupTitles/Quest", expireTimestamp: 3600f));
  }

  public static void CustomObjective_Complete()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.Test);
  }

  public static void ResetCooldowns()
  {
    for (int index = 0; index < DataManager.Instance.LastUsedSermonRitualDayIndex.Length; ++index)
      DataManager.Instance.LastUsedSermonRitualDayIndex[index] = -1;
  }

  public static void KillFollowers()
  {
    for (int index = DataManager.Instance.Followers.Count - 1; index >= 0; --index)
      FollowerBrain.FindBrainByID(DataManager.Instance.Followers[index].ID).HardSwapToTask((FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.Died));
  }

  public static void KillRandomFollower()
  {
    FollowerBrain.FindBrainByID(DataManager.Instance.Followers[UnityEngine.Random.Range(0, DataManager.Instance.Followers.Count)].ID).HardSwapToTask((FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.Died));
  }

  public static void KillRotFollower()
  {
    List<FollowerBrain> ts = new List<FollowerBrain>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(follower.ID);
      if (brainById != null && brainById.HasTrait(FollowerTrait.TraitType.Mutated))
        ts.Add(brainById);
    }
    if (ts.Count <= 0)
      return;
    ts.Shuffle<FollowerBrain>();
    FollowerBrain followerBrain = ts[UnityEngine.Random.Range(0, ts.Count)];
    followerBrain.DiedFromRot = true;
    followerBrain._directInfoAccess.DayBecameMutated = 1;
    followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromRot));
  }

  public static void RemoveFaith()
  {
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
      follower.Faith -= 10f;
  }

  public static void UnlockAllSermons()
  {
    for (int index = 0; index < 23; ++index)
    {
      SermonsAndRituals.SermonRitualType sermonRitualType = (SermonsAndRituals.SermonRitualType) index;
      if (sermonRitualType != SermonsAndRituals.SermonRitualType.NONE && !DataManager.Instance.UnlockedSermonsAndRituals.Contains(sermonRitualType))
        DataManager.Instance.UnlockedSermonsAndRituals.Add(sermonRitualType);
    }
  }

  public static IEnumerator ResetMap()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.RegenerateMapRoutine();
  }

  public static void UnlockAll()
  {
    CheatConsole.UnlockAllRituals = true;
    for (int index = 0; index < Enum.GetNames(typeof (UpgradeSystem.Type)).Length; ++index)
      UpgradeSystem.UnlockAbility((UpgradeSystem.Type) index);
    GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
  }

  public static void UnlockWeapons()
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
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Curses_Barrier);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Curses_Fire);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Curses_Teleport);
    GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
  }

  public static void RunTrinket(TarotCards.TarotCard card)
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      TrinketManager.AddTrinket(card, player);
    }
  }

  public static void RunTrinket(TarotCards.Card card)
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      TrinketManager.AddTrinket(new TarotCards.TarotCard(card, 0), player);
    }
  }

  public static void EnableTarot() => DataManager.Instance.HasTarotBuilding = true;

  public static void EnableBlackSouls()
  {
    DataManager.Instance.BlackSoulsEnabled = true;
    UnityEngine.Object.FindObjectOfType<HUD_BlackSoul>().RingsObject.gameObject.SetActive(true);
  }

  public static void SetResolution() => Screen.SetResolution(1920, 1080, true);

  public static void ToggleFullScreen() => Screen.fullScreen = !Screen.fullScreen;

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

  public static void ClearRubble()
  {
    List<StructureBrain> structureBrainList = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.StructuresAtLocation(FollowerLocation.Base));
    for (int index = 0; index < structureBrainList.Count; ++index)
    {
      StructureBrain structureBrain = structureBrainList[index];
      if (structureBrain.Data.Type == StructureBrain.TYPES.RUBBLE || structureBrain.Data.Type == StructureBrain.TYPES.RUBBLE_BIG || structureBrain.Data.Type == StructureBrain.TYPES.ICE_BLOCK)
        (structureBrain as Structures_Rubble).Remove();
    }
  }

  public static void ClearWeed()
  {
    List<StructureBrain> structureBrainList = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.StructuresAtLocation(FollowerLocation.Base));
    for (int index = 0; index < structureBrainList.Count; ++index)
    {
      StructureBrain structureBrain = structureBrainList[index];
      if (structureBrain.Data.Type == StructureBrain.TYPES.WEEDS)
        (structureBrain as Structures_Weeds).Remove();
    }
  }

  public static void UnlockAllStructures()
  {
    foreach (StructureBrain.TYPES Types in Enum.GetValues(typeof (StructureBrain.TYPES)))
    {
      if (!StructuresData.GetUnlocked(Types))
        DataManager.Instance.UnlockedStructures.Add(Types);
    }
  }

  public static void CompleteCurrentObjectives()
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

  public static void AddReturnToRancherQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Rancher", Objectives.CustomQuestTypes.ReturnToRancherJobBoard), true, true);
    CheatConsole.CompleteQuestGroupExcept("Objectives/GroupTitles/JobBoard/Rancher", Objectives.CustomQuestTypes.ReturnToRancherJobBoard);
  }

  public static void AddReturnToBlacksmithQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Blacksmith", Objectives.CustomQuestTypes.ReturnToBlacksmithJobBoard), true, true);
    CheatConsole.CompleteQuestGroupExcept("Objectives/GroupTitles/JobBoard/Blacksmith", Objectives.CustomQuestTypes.ReturnToBlacksmithJobBoard);
  }

  public static void AddReturnToDecoQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Deco", Objectives.CustomQuestTypes.ReturnToDecoJobBoard), true, true);
    CheatConsole.CompleteQuestGroupExcept("Objectives/GroupTitles/JobBoard/Deco", Objectives.CustomQuestTypes.ReturnToDecoJobBoard);
  }

  public static void AddReturnToTarotQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Tarot", Objectives.CustomQuestTypes.ReturnToTarotJobBoard), true, true);
    CheatConsole.CompleteQuestGroupExcept("Objectives/GroupTitles/JobBoard/Tarot", Objectives.CustomQuestTypes.ReturnToTarotJobBoard);
  }

  public static void AddReturnToGraveyardQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Priest", Objectives.CustomQuestTypes.ReturnToPriestJobBoard), true, true);
    CheatConsole.CompleteQuestGroupExcept("Objectives/GroupTitles/JobBoard/Priest", Objectives.CustomQuestTypes.ReturnToPriestJobBoard);
  }

  public static void AddReturnToFlockadeQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Flockade", Objectives.CustomQuestTypes.ReturnToFlockadeJobBoard), true, true);
    CheatConsole.CompleteQuestGroupExcept("Objectives/GroupTitles/JobBoard/Flockade", Objectives.CustomQuestTypes.ReturnToFlockadeJobBoard);
  }

  public static void CompleteQuestGroupExcept(
    string group,
    Objectives.CustomQuestTypes customQuestType)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (!(objective.GroupId != group) && (!(objective is Objectives_Custom objectivesCustom) || objectivesCustom.CustomQuestType != customQuestType))
      {
        objective.IsComplete = true;
        objective.IsFailed = false;
        DataManager.Instance.AddToCompletedQuestHistory(objective.GetFinalizedData());
      }
    }
    ObjectiveManager.CheckObjectives();
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (!(completedObjective.GroupId != group))
      {
        completedObjective.IsComplete = true;
        completedObjective.IsFailed = false;
      }
    }
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.CurrentPlayerQuest != null && !(follower.CurrentPlayerQuest.GroupId != group))
        follower.CurrentPlayerQuest.IsComplete = true;
    }
    DataManager.Instance.Objectives.Clear();
  }

  public static void ToggleGodMode()
  {
    bool flag = PlayerFarming.players[0].health.GodMode == Health.CheatMode.God;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (flag)
        player.health.GodMode = Health.CheatMode.None;
      else
        player.health.GodMode = Health.CheatMode.God;
    }
  }

  public static void ToggleDemiGodMode()
  {
    if (PlayerFarming.Instance.health.GodMode == Health.CheatMode.Demigod)
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.None;
    else
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.Demigod;
  }

  public static void ToggleImmortalMode()
  {
    if (PlayerFarming.Instance.health.GodMode == Health.CheatMode.Immortal)
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.None;
    else
      PlayerFarming.Instance.health.GodMode = Health.CheatMode.Immortal;
  }

  public static void ToggleNoClip()
  {
    Collider2D collider2D = (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? PlayerFarming.Instance.GetComponent<Collider2D>() : PlayerPrisonerController.Instance.GetComponent<Collider2D>();
    collider2D.isTrigger = !collider2D.isTrigger;
  }

  public static void MakeChild() => CheatConsole.CreateFollower(1, isChild: true);

  public static void CreateFollower(int Num, FollowerLocation location = FollowerLocation.None, bool isChild = false)
  {
    if (location == FollowerLocation.None)
      location = PlayerFarming.Location;
    foreach (WorshipperData.SkinAndData character in WorshipperData.Instance.Characters)
    {
      if (!DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin) && !DataManager.OnBlackList(character.Skin[0].Skin))
        DataManager.SetFollowerSkinUnlocked(character.Skin[0].Skin);
    }
    while (--Num >= 0)
    {
      if (location != PlayerFarming.Location)
      {
        FollowerInfo info = FollowerInfo.NewCharacter(location);
        DataManager.Instance.Followers.Add(info);
        DataManager.Instance.Followers.Sort((Comparison<FollowerInfo>) ((a, b) => a.ID.CompareTo(b.ID)));
        FollowerBrain brain = FollowerBrain.GetOrCreateBrain(info);
        FollowerManager.SimFollowersAtLocation(location).Add(new SimFollower(brain));
      }
      else
      {
        Follower newFollower = FollowerManager.CreateNewFollower(location, PlayerFarming.Instance.transform.position);
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
        if (isChild)
          newFollower.Brain.MakeChild();
      }
    }
  }

  public static void CreateSnowmanFollower(int Num)
  {
    foreach (WorshipperData.SkinAndData character in WorshipperData.Instance.Characters)
    {
      if (!DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin) && !DataManager.OnBlackList(character.Skin[0].Skin))
        DataManager.SetFollowerSkinUnlocked(character.Skin[0].Skin);
    }
    while (--Num >= 0)
    {
      FollowerSpecialType followerSpecialType = FollowerSpecialType.Snowman_Great;
      string str = $"Snowman/Good_{UnityEngine.Random.Range(1, 4)}";
      FollowerInfo f = FollowerInfo.NewCharacter(FollowerLocation.Base, str);
      f.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(str);
      f.IsSnowman = true;
      f.FollowerRole = FollowerRole.Worshipper;
      f.IsSnowman = true;
      f.Special = followerSpecialType;
      f.XPLevel = 9;
      f.SkinColour = 0;
      f.TraitsSet = true;
      f.Traits.Clear();
      f.Traits.Add(FollowerTrait.TraitType.MasterfulSnowman);
      FollowerManager.CreateNewFollower(f, PlayerFarming.Instance.transform.position).Brain?.CurrentTask?.Arrive();
    }
  }

  public static void EndKnucklebones() => UnityEngine.Object.FindObjectOfType<KBGameScreen>().ForceEndGame();

  public static void KillAllEnemiesInRoom()
  {
    List<Health> healthList = new List<Health>((IEnumerable<Health>) Health.team2);
    healthList.AddRange((IEnumerable<Health>) Health.killAll);
    foreach (Health health in healthList)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      {
        health.invincible = false;
        health.enabled = true;
        health.DealDamage(float.PositiveInfinity, health.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
      }
    }
  }

  public static void SetOrderEncoutnersProbability(float layer1Probability, float layer2Probability)
  {
    EnemyEncounterChanceEvents component = BiomeGenerator.Instance.GetComponent<EnemyEncounterChanceEvents>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    component.layer1ChanceOfGroupedEnemies = layer1Probability;
    component.layer2ChanceOfGroupedEnemies = layer2Probability;
  }

  public static void FinishAllMissions()
  {
    foreach (int followersOnMissionaryId in DataManager.Instance.Followers_OnMissionary_IDs)
    {
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(followersOnMissionaryId));
      if (brain.CurrentTask is FollowerTask_OnMissionary)
      {
        brain._directInfoAccess.MissionaryTimestamp = TimeManager.TotalElapsedGameTime;
        brain._directInfoAccess.MissionaryDuration = 1f;
      }
    }
  }

  public static void AddTraitToAllFollowers(FollowerTrait.TraitType traitToAdd)
  {
    foreach (FollowerBrain followerBrain in new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains))
      followerBrain.AddTrait(traitToAdd, true);
  }

  public static void MakeSpiesLeave()
  {
    foreach (FollowerBrain followerBrain in new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains))
    {
      if (followerBrain.HasTrait(FollowerTrait.TraitType.Spy))
        followerBrain.LeavingCult = true;
    }
  }

  public static void AddTraitToClosestFollower(FollowerTrait.TraitType trait)
  {
    Follower follower1 = (Follower) null;
    float num1 = float.MaxValue;
    foreach (Follower follower2 in Follower.Followers)
    {
      float num2 = Vector3.Distance(PlayerFarming.Instance.transform.position, follower2.transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        follower1 = follower2;
      }
    }
    follower1.AddTrait(trait, true);
  }

  public static List<Follower> GetPossibleQuestFollowers()
  {
    List<Follower> possibleQuestFollowers = new List<Follower>();
    foreach (Follower follower in Follower.Followers)
    {
      if (FollowerBrain.CanFollowerGiveQuest(follower.Brain._directInfoAccess) && !FollowerManager.UniqueFollowerIDs.Contains(follower.Brain.Info.ID))
        possibleQuestFollowers.Add(follower);
    }
    return possibleQuestFollowers;
  }

  public static void EmptyFurnaceFuel()
  {
    List<StructureBrain> structureBrainList = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.StructuresAtLocation(FollowerLocation.Base));
    for (int index = 0; index < structureBrainList.Count; ++index)
    {
      StructureBrain structureBrain = structureBrainList[index];
      if (structureBrain.Data.Type == StructureBrain.TYPES.FURNACE_1 || structureBrain.Data.Type == StructureBrain.TYPES.FURNACE_2 || structureBrain.Data.Type == StructureBrain.TYPES.FURNACE_3)
        structureBrain.Data.Fuel = 0;
    }
  }

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
