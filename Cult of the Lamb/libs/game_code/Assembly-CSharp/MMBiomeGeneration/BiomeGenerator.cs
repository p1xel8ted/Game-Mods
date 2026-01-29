// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.BiomeGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using I2.Loc;
using Lamb.UI;
using Map;
using MessagePack;
using MMRoomGeneration;
using MMTools;
using src.UI.Overlays.TutorialOverlay;
using src.UINavigator;
using src.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

#nullable disable
namespace MMBiomeGeneration;

public class BiomeGenerator : BaseMonoBehaviour
{
  public int TotalFloors = 3;
  public const int MAX_ENDLESS_LEVELS = 3;
  public bool TestStartingLayer;
  public int StartingLayer = 1;
  public DungeonWorldMapIcon.NodeType DLCNode = DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss;
  [FormerlySerializedAs("Layer2")]
  public bool NewGamePlus;
  public int PostMiniBossDoorFollowerCount = 3;
  public int GoldToGive = 1;
  public FollowerLocation DungeonLocation;
  [EventRef(MigrateTo = "MusicToTrigger")]
  public string biomeMusicPath;
  public EventReference MusicToTrigger;
  [EventRef(MigrateTo = "AtmosToTrigger")]
  public string biomeAtmosPath;
  public EventReference AtmosToTrigger;
  public FMOD.Studio.EventInstance biomeAtmosInstance;
  public bool stopCurrentMusicOnLoad;
  [HideInInspector]
  public static List<string> UsedEncounters = new List<string>();
  public static BiomeGenerator.GetKey OnGetKey;
  public static BiomeGenerator.GetKey OnUseKey;
  [SerializeField]
  public bool _HasKey;
  public bool HasSpawnedRelic;
  public bool ReuseGeneratorRoom;
  public static BiomeGenerator Instance;
  public static Vector2Int BossCoords = new Vector2Int(0, 999);
  public static Vector2Int RespawnRoomCoords = new Vector2Int(-2147483647 /*0x80000001*/, -2147483647 /*0x80000001*/);
  [CompilerGenerated]
  public bool \u003COnboardingDungeon5\u003Ek__BackingField;
  public System.Random RandomSeed;
  [HideInInspector]
  public List<BiomeRoom> Rooms;
  public int Seed;
  public int NumberOfRooms = 20;
  public bool ForceResource;
  public List<RandomResource.Resource> Resources = new List<RandomResource.Resource>();
  [TermsPopup("")]
  public string DisplayName;
  public GenerateRoom GeneratorRoomPrefab;
  public string EntranceRoomPath;
  public bool LockAndKey = true;
  public string LockedRoomPath;
  public string KeyRoomPath;
  public string BossRoomPath;
  public string LeaderRoomPath;
  public string EndOfFloorRoomPath;
  public string PostBossRoomPath;
  public string BossDoorRoomPath;
  public bool StartWithBossRoomDoor;
  public string KeyPiecePath;
  public string EntranceDoorRoomPath;
  public string RespawnRoomPath;
  public string DeathCatRoomPath;
  public string YngyaRoomPath;
  public List<BiomeGenerator.ListOfStoryRooms> StoryRooms = new List<BiomeGenerator.ListOfStoryRooms>();
  public List<BiomeGenerator.ListOfCustomRoomPrefabs> CustomDynamicRooms = new List<BiomeGenerator.ListOfCustomRoomPrefabs>();
  public List<BiomeGenerator.FixedRoom> CustomRooms = new List<BiomeGenerator.FixedRoom>();
  public bool OverrideRandomWalk;
  public List<BiomeGenerator.OverrideRoom> OverrideRooms = new List<BiomeGenerator.OverrideRoom>();
  public BiomeRoom lastRoom;
  [Space]
  [SerializeField]
  public bool spawnDemons = true;
  public bool spawnedDemons;
  public int loadedRoomsWithoutAssetUnload;
  public WeatherSystemController.WeatherType weatherType;
  public WeatherSystemController.WeatherStrength weatherStrength;
  [CompilerGenerated]
  public int \u003CRoomsVisited\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CTargetPossessedEnemy\u003Ek__BackingField = -1;
  [CompilerGenerated]
  public int \u003CPossessedEnemyEncounterCount\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CEncounteredPossessedEnemyThisFloor\u003Ek__BackingField;
  public List<BiomeGenerator.OverrideRoom> dungeon5IntroRooms = new List<BiomeGenerator.OverrideRoom>();
  [CompilerGenerated]
  public List<BiomeRoom> \u003CLoreTotemRooms\u003Ek__BackingField = new List<BiomeRoom>();
  public bool spawnedLoreTotemsThisDungeon;
  public List<PlayerFarming> playersWithAppliedFleeceModifiers = new List<PlayerFarming>();
  public static List<FollowerLocation> DLC_DUNGEONS = new List<FollowerLocation>()
  {
    FollowerLocation.Dungeon1_5,
    FollowerLocation.Dungeon1_6
  };
  public static bool WeaponAtEnd;
  [HideInInspector]
  public BiomeRoom PostBossBiomeRoom;
  [HideInInspector]
  public BiomeRoom RoomEntrance;
  [HideInInspector]
  public BiomeRoom RoomExit;
  public List<BiomeRoom> CriticalPath;
  [CompilerGenerated]
  public BiomeRoom \u003CRespawnRoom\u003Ek__BackingField;
  [CompilerGenerated]
  public BiomeRoom \u003CDeathCatRoom\u003Ek__BackingField;
  [CompilerGenerated]
  public BiomeRoom \u003CYngyaRoom\u003Ek__BackingField;
  [CompilerGenerated]
  public BiomeRoom \u003CMysticSellerRoom\u003Ek__BackingField;
  public List<BiomeGenerator.ListOfStoryRooms> RandomiseStoryOrder;
  [HideInInspector]
  public List<BiomeGenerator.ListOfCustomRoomPrefabs> RandomiseOrder;
  public List<BiomeRoom> AvailableRooms;
  public List<GeneraterDecorations> BiomeDecorationSet;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public int CurrentX;
  public int CurrentY = -1;
  [HideInInspector]
  public bool IsTeleporting;
  [HideInInspector]
  public bool FirstArrival = true;
  public bool ShowDisplayName = true;
  public GameObject Player;
  public StateMachine PlayerState;
  public int PrevCurrentX = -2147483647 /*0x80000001*/;
  public int PrevCurrentY = -2147483647 /*0x80000001*/;
  public int StartX;
  public int StartY;
  public bool DoFirstArrivalRoutine = true;
  [Space]
  public float HumanoidHealthMultiplier = 1f;
  public static System.Action OnTutorialShown;
  public static bool RevealHudAbilityIcons = false;
  [HideInInspector]
  public BiomeRoom CurrentRoom;
  public BiomeRoom PrevRoom;
  [HideInInspector]
  public Door North;
  [HideInInspector]
  public Door East;
  [HideInInspector]
  public Door South;
  [HideInInspector]
  public Door West;

  public void InitMusic()
  {
    if (this.stopCurrentMusicOnLoad)
      AudioManager.Instance.StopCurrentMusic();
    if (!string.IsNullOrEmpty(this.biomeMusicPath))
      AudioManager.Instance.PlayMusic(this.biomeMusicPath, false);
    if (!string.IsNullOrEmpty(this.biomeAtmosPath))
      AudioManager.Instance.PlayAtmos(this.biomeAtmosPath);
    AudioManager.Instance.SetMusicCombatState(false);
  }

  public bool HasKey
  {
    get => this._HasKey;
    set
    {
      this._HasKey = value;
      if (this._HasKey)
      {
        BiomeGenerator.GetKey onGetKey = BiomeGenerator.OnGetKey;
        if (onGetKey != null)
          onGetKey();
      }
      if (this._HasKey)
        return;
      BiomeGenerator.GetKey onUseKey = BiomeGenerator.OnUseKey;
      if (onUseKey == null)
        return;
      onUseKey();
    }
  }

  public static event BiomeGenerator.BiomeAction OnBiomeGenerated;

  public static event BiomeGenerator.BiomeAction OnBiomeChangeRoom;

  public static event BiomeGenerator.BiomeAction OnBiomeLeftRoom;

  public static event BiomeGenerator.BiomeAction OnRoomActive;

  public static event BiomeGenerator.BiomeAction OnBiomeEnteredCombatRoom;

  public bool OnboardingDungeon5
  {
    get => this.\u003COnboardingDungeon5\u003Ek__BackingField;
    set => this.\u003COnboardingDungeon5\u003Ek__BackingField = value;
  }

  public void RandomiseSeed()
  {
    this.Seed = UnityEngine.Random.Range(-2147483647 /*0x80000001*/, int.MaxValue);
    DataManager.Instance.AddNewDungeonSeed(this.Seed);
    MonoSingleton<MMLogger>.Instance.AddToLog("DungeonSeed/Randomised: " + this.Seed.ToString());
  }

  public void DailyRun()
  {
    DateTime now = DateTime.Now;
    string[] strArray = new string[5]
    {
      now.Day < 10 ? "0" : "",
      now.Day.ToString(),
      now.Month < 10 ? "0" : "",
      null,
      null
    };
    int num = now.Month;
    strArray[3] = num.ToString();
    num = now.Year;
    strArray[4] = num.ToString();
    this.Seed = int.Parse(string.Concat(strArray));
  }

  public BiomeRoom LastRoom => this.lastRoom;

  public bool SpawnDemons
  {
    get => this.spawnDemons;
    set => this.spawnDemons = value;
  }

  public int RoomsVisited
  {
    get => this.\u003CRoomsVisited\u003Ek__BackingField;
    set => this.\u003CRoomsVisited\u003Ek__BackingField = value;
  }

  public int TargetPossessedEnemy
  {
    get => this.\u003CTargetPossessedEnemy\u003Ek__BackingField;
    set => this.\u003CTargetPossessedEnemy\u003Ek__BackingField = value;
  }

  public int PossessedEnemyEncounterCount
  {
    get => this.\u003CPossessedEnemyEncounterCount\u003Ek__BackingField;
    set => this.\u003CPossessedEnemyEncounterCount\u003Ek__BackingField = value;
  }

  public bool EncounteredPossessedEnemyThisFloor
  {
    get => this.\u003CEncounteredPossessedEnemyThisFloor\u003Ek__BackingField;
    set => this.\u003CEncounteredPossessedEnemyThisFloor\u003Ek__BackingField = value;
  }

  public List<BiomeRoom> LoreTotemRooms
  {
    get => this.\u003CLoreTotemRooms\u003Ek__BackingField;
    set => this.\u003CLoreTotemRooms\u003Ek__BackingField = value;
  }

  public bool SpawnedLoreTotemsThisDungeon
  {
    get => this.spawnedLoreTotemsThisDungeon;
    set => this.spawnedLoreTotemsThisDungeon = value;
  }

  public void Awake()
  {
    BiomeGenerator.Instance = this;
    if (this.TestStartingLayer && this.StartingLayer == 0)
      GameManager.CurrentDungeonLayer = 0;
    this.dungeon5IntroRooms = this.OverrideRooms;
  }

  public void OnEnable()
  {
    BiomeGenerator.Instance = this;
    if (!this.TestStartingLayer || !Application.isEditor)
      return;
    DataManager.Instance.OnboardedIntroYngyaShrine = true;
    DataManager.Instance.OnboardedRelics = true;
    DataManager.Instance.HasEncounteredTarot = true;
    DataManager.Instance.HasBuiltPleasureShrine = true;
    DataManager.Instance.MAJOR_DLC = true;
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relic_Pack_Default);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relics_Blessed_1);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relics_Dammed_1);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relic_Pack1);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relic_Pack2);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relics_Ice);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relics_Fire);
    DataManager.Instance.DLCDungeon5MiniBossIndex = this.StartingLayer;
    DataManager.Instance.DLCDungeon6MiniBossIndex = this.StartingLayer;
    if (this.StartingLayer == 4 && (this.DungeonLocation == FollowerLocation.Dungeon1_5 || this.DungeonLocation == FollowerLocation.Dungeon1_6) && (this.DLCNode == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss || this.DLCNode == DungeonWorldMapIcon.NodeType.Dungeon6_MiniBoss))
    {
      this.StartingLayer = 3;
      GameManager.DungeonUseAllLayers = true;
    }
    GameManager.CurrentDungeonLayer = this.StartingLayer;
    DataManager.Instance.CurrentDLCNodeType = this.DLCNode;
    DataManager.Instance.DungeonBossFight = this.StartingLayer == 4;
    if (this.NewGamePlus)
      DataManager.Instance.DeathCatBeaten = true;
    if (GameManager.CurrentDungeonLayer == 5)
    {
      GameManager.CurrentDungeonLayer = 4;
      DataManager.Instance.BossesCompleted.Add(this.DungeonLocation);
    }
    DataManager.SetNewRun(this.DungeonLocation);
  }

  public void Start()
  {
    if (DataManager.UseDataManagerSeed && !this.TestStartingLayer)
    {
      this.Seed = DataManager.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue);
      DataManager.Instance.AddNewDungeonSeed(this.Seed);
      MonoSingleton<MMLogger>.Instance.AddToLog("DungeonSeed/Generated: " + this.Seed.ToString());
    }
    if (DungeonSandboxManager.Active)
      return;
    this.StartCoroutine((IEnumerator) this.GenerateRoutine());
  }

  public static bool EncounterAlreadyUsed(string EncounterName)
  {
    return BiomeGenerator.UsedEncounters.Contains(EncounterName);
  }

  public static void SetEncounterAsUsed(string EncounterName)
  {
    BiomeGenerator.UsedEncounters.Add(EncounterName);
  }

  public static void RemoveEncounterAsUsed(string EncounterName)
  {
    BiomeGenerator.UsedEncounters.Remove(EncounterName);
  }

  public IEnumerator GenerateRoutine()
  {
    BiomeGenerator biomeGenerator = this;
    if (biomeGenerator.DungeonLocation == FollowerLocation.Dungeon1_5 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Shrine)
    {
      PlayerReturnToBase.Disabled = true;
      biomeGenerator.OverrideRandomWalk = true;
      biomeGenerator.OnboardingDungeon5 = true;
      GameManager.CurrentDungeonLayer = 1;
      biomeGenerator.OverrideRooms = biomeGenerator.dungeon5IntroRooms;
    }
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    float Progress = -1f;
    float Total = 18f;
    biomeGenerator.transform.position = Vector3.zero;
    biomeGenerator.RandomSeed = new System.Random(biomeGenerator.Seed);
    biomeGenerator.CreateRandomWalk();
    GenerateRoom component = biomeGenerator.GeneratorRoomPrefab.GetComponent<GenerateRoom>();
    biomeGenerator.BiomeDecorationSet = new List<GeneraterDecorations>((IEnumerable<GeneraterDecorations>) component.DecorationSetList);
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 1");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 2");
    biomeGenerator.SetNeighbours();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 3");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 4");
    biomeGenerator.PlaceEntranceAndExit();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 5");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 6");
    biomeGenerator.GetCriticalPath();
    if (biomeGenerator.LockAndKey)
      biomeGenerator.PlaceLockAndKey();
    if (!string.IsNullOrEmpty(biomeGenerator.RespawnRoomPath))
      biomeGenerator.PlaceRespawnRoom();
    if (!string.IsNullOrEmpty(biomeGenerator.DeathCatRoomPath))
      biomeGenerator.PlaceDeathCatRoom();
    if (!string.IsNullOrEmpty(biomeGenerator.YngyaRoomPath) && YngyaRoomManager.ShowHeartRoom() && PlayerFarming.Location == FollowerLocation.Dungeon1_6)
      biomeGenerator.PlaceYngyaRoom();
    if (!DataManager.Instance.OnboardedMysticShop && !DataManager.Instance.ForeshadowedMysticShop && DataManager.Instance.GetDungeonLayer(FollowerLocation.Dungeon1_4) >= 2)
      biomeGenerator.PlaceMysticSellerRoom();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 7");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 8");
    biomeGenerator.PlaceFixedCustomRooms();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 9");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 10");
    yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.PlaceStoryRooms());
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 11");
    yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.PlaceDynamicCustomRooms());
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 12");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 13");
    if (GameManager.Layer2 && DataManager.Instance.LoreStonesRoomUpTo <= LoreSystem.LoreTotalLoreRoom && !biomeGenerator.spawnedLoreTotemsThisDungeon && !DungeonSandboxManager.Active)
      biomeGenerator.PlaceLoreTotems();
    yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.InstantiatePrefabs());
    biomeGenerator.InitMusic();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 14");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 15");
    while (LocationManager.GetLocationState(biomeGenerator.DungeonLocation) != LocationState.Active && !DungeonSandboxManager.Active)
      yield return (object) null;
    biomeGenerator.SetRoom(biomeGenerator.StartX, biomeGenerator.StartY);
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 16");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 17");
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 18");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " ");
    while (!biomeGenerator.CurrentRoom.generateRoom.GeneratedDecorations)
      yield return (object) null;
    BiomeGenerator.BiomeAction onBiomeGenerated = BiomeGenerator.OnBiomeGenerated;
    if (onBiomeGenerated != null)
      onBiomeGenerated();
    biomeGenerator.SetOverrideWeather();
    EnableRecieveShadowsSpriteRenderer.UpdateSpriteShadows();
    MMTransition.ResumePlay();
    SimulationManager.UnPause();
    stopwatch.Stop();
  }

  public void Generate()
  {
    this.transform.position = Vector3.zero;
    this.RandomSeed = new System.Random(this.Seed);
    this.CreateRandomWalk();
    this.SetNeighbours();
    this.PlaceEntranceAndExit();
    this.GetCriticalPath();
    if (this.LockAndKey)
      this.PlaceLockAndKey();
    this.PlaceStoryRooms();
    this.PlaceDynamicCustomRooms();
    this.PlaceFixedCustomRooms();
    this.StartCoroutine((IEnumerator) this.InstantiatePrefabs());
    LocationManager.LocationManagers[this.DungeonLocation].SpawnFollowers();
    this.SetRoom(this.StartX, this.StartY);
    BiomeGenerator.BiomeAction onBiomeGenerated = BiomeGenerator.OnBiomeGenerated;
    if (onBiomeGenerated != null)
      onBiomeGenerated();
    this.SetOverrideWeather();
    DataManager.ResetRunData();
  }

  public void CreateRandomWalk()
  {
    foreach (BiomeRoom room in this.Rooms)
      room.Clear();
    this.Rooms = new List<BiomeRoom>();
    int num1 = this.NumberOfRooms;
    if (GameManager.Layer2)
      num1 = 8;
    int x1 = 0;
    int y1 = 0;
    BiomeRoom NewRoom;
    if (this.OverrideRandomWalk)
    {
      this.StartX = 0;
      this.StartY = 0;
      foreach (BiomeGenerator.OverrideRoom overrideRoom in this.OverrideRooms)
      {
        NewRoom = new BiomeRoom(overrideRoom.x, overrideRoom.y, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab.gameObject);
        this.Rooms.Add(NewRoom);
        if (overrideRoom.North != GenerateRoom.ConnectionTypes.False)
          NewRoom.N_Room = new RoomConnection(overrideRoom.North);
        if (overrideRoom.East != GenerateRoom.ConnectionTypes.False)
          NewRoom.E_Room = new RoomConnection(overrideRoom.East);
        if (overrideRoom.South != GenerateRoom.ConnectionTypes.False)
          NewRoom.S_Room = new RoomConnection(overrideRoom.South);
        if (overrideRoom.West != GenerateRoom.ConnectionTypes.False)
          NewRoom.W_Room = new RoomConnection(overrideRoom.West);
        NewRoom.Active = overrideRoom.RoomActive;
        NewRoom.IsCustom = true;
        NewRoom.Generated = overrideRoom.Generated == BiomeGenerator.FixedRoom.Generate.DontGenerate;
        Addressables.LoadAssetAsync<GameObject>((object) overrideRoom.PrefabPath).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          this.loadedAddressableAssets.Add(obj);
          NewRoom.GameObject = obj.Result;
          if (PlayerFarming.Location != FollowerLocation.Dungeon1_6)
            return;
          GenerateRoom component = obj.Result.GetComponent<GenerateRoom>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          component.Addr_EastEntranceDoor = this.GeneratorRoomPrefab.Addr_EastEntranceDoor;
          component.Addr_WestEntranceDoor = this.GeneratorRoomPrefab.Addr_WestEntranceDoor;
          component.Addr_SouthEntranceDoor = this.GeneratorRoomPrefab.Addr_SouthEntranceDoor;
          component.Addr_NorthEntranceDoor = this.GeneratorRoomPrefab.Addr_NorthEntranceDoor;
        });
        NewRoom.GameObjectPath = overrideRoom.PrefabPath;
      }
      foreach (BiomeRoom room1 in this.Rooms)
      {
        BiomeRoom room2;
        if (room1.N_Room != null && (room2 = BiomeRoom.GetRoom(room1.x, room1.y + 1)) != null)
        {
          room1.N_Room.SetConnectionAndRoom(room2, room1.N_Room.ConnectionType);
          room2.S_Room = new RoomConnection(room1);
          room2.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        }
        BiomeRoom room3;
        if (room1.E_Room != null && (room3 = BiomeRoom.GetRoom(room1.x + 1, room1.y)) != null)
        {
          room1.E_Room.SetConnectionAndRoom(room3, room1.E_Room.ConnectionType);
          room3.W_Room = new RoomConnection(room1);
          room3.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        }
        BiomeRoom room4;
        if (room1.S_Room != null && (room4 = BiomeRoom.GetRoom(room1.x, room1.y - 1)) != null)
        {
          room1.S_Room.SetConnectionAndRoom(room4, room1.S_Room.ConnectionType);
          room4.N_Room = new RoomConnection(room1);
          room4.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        }
        BiomeRoom room5;
        if (room1.W_Room != null && (room5 = BiomeRoom.GetRoom(room1.x - 1, room1.y)) != null)
        {
          room1.W_Room.SetConnectionAndRoom(room5, room1.W_Room.ConnectionType);
          room5.E_Room = new RoomConnection(room1);
          room5.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        }
      }
    }
    else
    {
      this.Rooms.Add(new BiomeRoom(x1, y1, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab.gameObject));
      int num2 = 0;
      while (num1 > 0)
      {
        BiomeRoom room = this.Rooms[this.RandomSeed.Next(0, this.Rooms.Count)];
        int x2 = room.x;
        int y2 = room.y;
        int num3 = this.RandomSeed.Next(0, 4);
        NewRoom = (BiomeRoom) null;
        switch (num3)
        {
          case 0:
            if (BiomeRoom.GetRoom(x2, y2 + 1) == null)
            {
              NewRoom = new BiomeRoom(x2, y2 + 1, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab.gameObject);
              this.Rooms.Add(NewRoom);
              room.N_Room = new RoomConnection(NewRoom);
              NewRoom.S_Room = new RoomConnection(room);
              room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              NewRoom.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              break;
            }
            break;
          case 1:
            if (BiomeRoom.GetRoom(x2 + 1, y2) == null)
            {
              NewRoom = new BiomeRoom(x2 + 1, y2, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab.gameObject);
              this.Rooms.Add(NewRoom);
              room.E_Room = new RoomConnection(NewRoom);
              NewRoom.W_Room = new RoomConnection(room);
              room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              NewRoom.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              break;
            }
            break;
          case 2:
            if (BiomeRoom.GetRoom(x2, y2 - 1) == null)
            {
              NewRoom = new BiomeRoom(x2, y2 - 1, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab.gameObject);
              this.Rooms.Add(NewRoom);
              room.S_Room = new RoomConnection(NewRoom);
              NewRoom.N_Room = new RoomConnection(room);
              room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              NewRoom.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              break;
            }
            break;
          case 3:
            if (BiomeRoom.GetRoom(x2 - 1, y2) == null)
            {
              NewRoom = new BiomeRoom(x2 - 1, y2, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab.gameObject);
              this.Rooms.Add(NewRoom);
              room.W_Room = new RoomConnection(NewRoom);
              NewRoom.E_Room = new RoomConnection(room);
              room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              NewRoom.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              break;
            }
            break;
        }
        if (NewRoom != null)
          --num1;
        else
          ++num2;
      }
    }
  }

  public void ResetFloodDistance()
  {
    foreach (BiomeRoom room in this.Rooms)
      room.Distance = int.MaxValue;
  }

  public void FloodFillDistance(BiomeRoom r, int Distance)
  {
    if (r == null || r.Distance < Distance + 1)
      return;
    r.Distance = ++Distance;
    if (r.N_Room.Connected)
      this.FloodFillDistance(r.N_Room.Room, Distance);
    if (r.E_Room.Connected)
      this.FloodFillDistance(r.E_Room.Room, Distance);
    if (r.S_Room.Connected)
      this.FloodFillDistance(r.S_Room.Room, Distance);
    if (!r.W_Room.Connected)
      return;
    this.FloodFillDistance(r.W_Room.Room, Distance);
  }

  public void PlaceEntranceAndExit()
  {
    if (this.OverrideRandomWalk)
      return;
    List<BiomeGenerator.RoomEntranceExit> roomEntranceExitList1 = new List<BiomeGenerator.RoomEntranceExit>();
    int num1 = 0;
    int Layer = -1;
    while (++Layer <= 4)
    {
      if (DataManager.HasKeyPieceFromLocation(this.DungeonLocation, Layer))
        ++num1;
    }
    foreach (BiomeRoom room in this.Rooms)
    {
      if (room.NumConnections == 1 && room.N_Room.Connected && room.EmptyNeighbourPositionsIgnoreSouth.Count > 0)
        roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.E_Room.Connected && room.EmptyNeighbourPositionsIgnoreSouth.Count > 0)
        roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.S_Room.Connected && room.EmptyNeighbourPositionsIgnoreSouth.Count > 0)
        roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.W_Room.Connected && room.EmptyNeighbourPositionsIgnoreSouth.Count > 0)
        roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
    }
    List<BiomeGenerator.RoomEntranceExit> roomEntranceExitList2 = new List<BiomeGenerator.RoomEntranceExit>();
    foreach (BiomeRoom room in this.Rooms)
    {
      if (room.NumConnections == 1 && room.N_Room.Connected)
        roomEntranceExitList2.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.E_Room.Connected)
        roomEntranceExitList2.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.S_Room.Connected)
        roomEntranceExitList2.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.W_Room.Connected)
        roomEntranceExitList2.Add(new BiomeGenerator.RoomEntranceExit(room, false));
    }
    BiomeGenerator.RoomEntranceExit ConnectionRoom = (BiomeGenerator.RoomEntranceExit) null;
    BiomeGenerator.RoomEntranceExit roomEntranceExit1 = (BiomeGenerator.RoomEntranceExit) null;
    int num2 = 0;
    foreach (BiomeGenerator.RoomEntranceExit roomEntranceExit2 in roomEntranceExitList1)
    {
      this.ResetFloodDistance();
      this.FloodFillDistance(roomEntranceExit2.Room, 0);
      foreach (BiomeGenerator.RoomEntranceExit roomEntranceExit3 in roomEntranceExitList2)
      {
        if (num2 < roomEntranceExit3.Room.Distance && roomEntranceExit2 != roomEntranceExit3)
        {
          num2 = roomEntranceExit3.Room.Distance;
          ConnectionRoom = roomEntranceExit2;
          roomEntranceExit1 = roomEntranceExit3;
        }
      }
    }
    BiomeGenerator.WeaponAtEnd = GameManager.CurrentDungeonFloor > 1 && this.RandomSeed.NextDouble() < 0.64999997615814209 && MapManager.Instance.CurrentNode.nodeType != Map.NodeType.MiniBossFloor;
    if ((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && MapManager.Instance.CurrentNode != null && (this.DungeonLocation == FollowerLocation.Dungeon1_5 || this.DungeonLocation == FollowerLocation.Dungeon1_6))
      BiomeGenerator.WeaponAtEnd = this.RandomSeed.NextDouble() < 0.34999999403953552 && MapManager.Instance.CurrentNode.nodeType != Map.NodeType.MiniBossFloor;
    if ((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && MapManager.Instance.CurrentNode != null && !DataManager.Instance.DungeonBossFight && MapManager.Instance.CurrentNode.nodeType == Map.NodeType.MiniBossFloor)
    {
      ConnectionRoom.Room.IsCustom = true;
      bool flag = true;
      if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6)
      {
        if (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Follower || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Follower)
          this.BossRoomPath = $"Assets/Prefabs/Rescue Rooms/Follower Rescue Room {(this.DungeonLocation == FollowerLocation.Dungeon1_5 ? "D5" : "D6")}.prefab";
        else if (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_GhostNPC || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_GhostNPC)
        {
          this.BossRoomPath = "Assets/Prefabs/Puzzle Room.prefab";
          flag = false;
        }
      }
      ConnectionRoom.Room.GameObjectPath = this.BossRoomPath;
      if (GameManager.Layer2 && !ConnectionRoom.Room.GameObjectPath.Contains("_P2.prefab"))
        ConnectionRoom.Room.GameObjectPath = ConnectionRoom.Room.GameObjectPath.Replace(".prefab", "_P2.prefab");
      if (ConnectionRoom.Room.N_Room.Connected)
        ConnectionRoom.Room.N_Room.Room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.Boss);
      if (ConnectionRoom.Room.E_Room.Connected)
        ConnectionRoom.Room.E_Room.Room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.Boss);
      if (ConnectionRoom.Room.S_Room.Connected)
        ConnectionRoom.Room.S_Room.Room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.Boss);
      if (ConnectionRoom.Room.W_Room.Connected)
        ConnectionRoom.Room.W_Room.Room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.Boss);
      ConnectionRoom.Room.IsBoss = true;
      ConnectionRoom.Room.Generated = false;
      Vector2Int vector2Int = new Vector2Int(ConnectionRoom.Room.x, ConnectionRoom.Room.y);
      this.RoomExit = ConnectionRoom.Room;
      if (flag)
      {
        if (!string.IsNullOrEmpty(this.KeyPiecePath))
        {
          BiomeRoom Room = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositions, this.KeyPiecePath, ConnectionRoom);
          if (GameManager.Layer2 && !this.PostBossRoomPath.Contains("_P2.prefab"))
            this.PostBossRoomPath = this.PostBossRoomPath.Replace(".prefab", "_P2.prefab");
          this.PlacePostBossRoom(Room.EmptyNeighbourPositionsIgnoreSouth, this.PostBossRoomPath, new BiomeGenerator.RoomEntranceExit(Room, false)).N_Room.SetConnection(GenerateRoom.ConnectionTypes.LeaderBoss);
        }
        else
        {
          if (GameManager.Layer2 && !this.PostBossRoomPath.Contains("_P2.prefab"))
            this.PostBossRoomPath = this.PostBossRoomPath.Replace(".prefab", "_P2.prefab");
          if (GameManager.Layer2 && !this.EndOfFloorRoomPath.Contains("_P2.prefab"))
            this.EndOfFloorRoomPath = this.EndOfFloorRoomPath.Replace(".prefab", "_P2.prefab");
          if (GameManager.Layer2 && !this.BossDoorRoomPath.Contains("_P2.prefab"))
            this.BossDoorRoomPath = this.BossDoorRoomPath.Replace(".prefab", "_P2.prefab");
          if (DungeonSandboxManager.Active)
          {
            if (MapManager.Instance.CurrentNode == MapManager.Instance.CurrentMap.GetFinalBossNode())
            {
              BiomeRoom biomeRoom = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositionsIgnoreSouth, DataManager.Instance.DungeonCompleted(this.DungeonLocation, GameManager.Layer2) ? this.PostBossRoomPath : this.BossDoorRoomPath, ConnectionRoom);
              biomeRoom.N_Room.SetConnection(GenerateRoom.ConnectionTypes.False);
              this.lastRoom = biomeRoom;
            }
            else
            {
              BiomeRoom biomeRoom = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositionsIgnoreSouth, this.EndOfFloorRoomPath, ConnectionRoom);
              biomeRoom.N_Room.SetConnection(GenerateRoom.ConnectionTypes.NextLayer);
              this.lastRoom = biomeRoom;
            }
          }
          else if ((UnityEngine.Object) MapManager.Instance == (UnityEngine.Object) null || MapManager.Instance.CurrentMap == null || MapManager.Instance.CurrentNode == MapManager.Instance.CurrentMap.GetBossNode())
          {
            BiomeRoom biomeRoom = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositionsIgnoreSouth, DataManager.Instance.DungeonCompleted(this.DungeonLocation, GameManager.Layer2) ? this.PostBossRoomPath : this.BossDoorRoomPath, ConnectionRoom);
            if (GameManager.DungeonEndlessLevel >= 3 && DataManager.Instance.DungeonCompleted(this.DungeonLocation, GameManager.Layer2))
              biomeRoom.N_Room.SetConnection(GenerateRoom.ConnectionTypes.False);
            else
              biomeRoom.N_Room.SetConnection(DataManager.Instance.DungeonCompleted(this.DungeonLocation, GameManager.Layer2) ? GenerateRoom.ConnectionTypes.NextLayer : GenerateRoom.ConnectionTypes.LeaderBoss);
            this.lastRoom = biomeRoom;
          }
          else
          {
            BiomeRoom biomeRoom = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositionsIgnoreSouth, this.EndOfFloorRoomPath, ConnectionRoom);
            biomeRoom.N_Room.SetConnection(GenerateRoom.ConnectionTypes.NextLayer);
            this.lastRoom = biomeRoom;
          }
        }
      }
    }
    else
    {
      GenerateRoom.ConnectionTypes ConnectionType = GenerateRoom.ConnectionTypes.NextLayer;
      string RoomToPlace = this.EndOfFloorRoomPath;
      if (GameManager.Layer2 && !RoomToPlace.Contains("_P2.prefab"))
        RoomToPlace = RoomToPlace.Replace(".prefab", "_P2.prefab");
      if (GameManager.Layer2 && !this.BossDoorRoomPath.Contains("_P2.prefab"))
        this.BossDoorRoomPath = this.BossDoorRoomPath.Replace(".prefab", "_P2.prefab");
      if ((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && MapManager.Instance.CurrentNode != null && (MapManager.Instance.CurrentNode.nodeType == Map.NodeType.MiniBossFloor && DataManager.Instance.DungeonBossFight || MapManager.Instance.CurrentNode.nodeType == Map.NodeType.Boss))
      {
        ConnectionRoom.Room.GameObjectPath = this.BossDoorRoomPath;
        ConnectionType = GenerateRoom.ConnectionTypes.LeaderBoss;
        RoomToPlace = this.BossDoorRoomPath;
      }
      ConnectionRoom.Room.IsCustom = false;
      ConnectionRoom.Room.Generated = false;
      this.RoomExit = ConnectionRoom.Room;
      BiomeRoom biomeRoom = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositionsIgnoreSouth, RoomToPlace, ConnectionRoom);
      biomeRoom.N_Room.SetConnection(ConnectionType);
      biomeRoom.HasWeapon = BiomeGenerator.WeaponAtEnd;
      this.lastRoom = biomeRoom;
    }
    if (!string.IsNullOrEmpty(this.EntranceRoomPath))
    {
      roomEntranceExit1.Room.IsCustom = true;
      roomEntranceExit1.Room.GameObjectPath = this.EntranceRoomPath;
      if (GameManager.Layer2 && !roomEntranceExit1.Room.GameObjectPath.Contains("_P2.prefab"))
        roomEntranceExit1.Room.GameObjectPath = roomEntranceExit1.Room.GameObjectPath.Replace(".prefab", "_P2.prefab");
    }
    else
    {
      roomEntranceExit1.Room.IsCustom = false;
      roomEntranceExit1.Room.GameObject = this.GeneratorRoomPrefab.gameObject;
    }
    this.StartX = roomEntranceExit1.Room.x;
    this.StartY = roomEntranceExit1.Room.y;
    this.RoomEntrance = roomEntranceExit1.Room;
    roomEntranceExit1.Room.Active = false;
    if (!roomEntranceExit1.Room.S_Room.Connected)
      roomEntranceExit1.Room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.Entrance);
    else if (!roomEntranceExit1.Room.W_Room.Connected)
      roomEntranceExit1.Room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.Entrance);
    else if (!roomEntranceExit1.Room.E_Room.Connected)
      roomEntranceExit1.Room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.Entrance);
    else if (!roomEntranceExit1.Room.N_Room.Connected)
      roomEntranceExit1.Room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.Entrance);
    if (this.StartWithBossRoomDoor && GameManager.CurrentDungeonFloor == 1)
    {
      if (GameManager.Layer2 && !this.BossDoorRoomPath.Contains("_P2.prefab"))
        this.BossDoorRoomPath = this.BossDoorRoomPath.Replace(".prefab", "_P2.prefab");
      BiomeRoom biomeRoom = new BiomeRoom(-999, -999, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab.gameObject);
      this.Rooms.Add(biomeRoom);
      biomeRoom.GameObjectPath = this.BossDoorRoomPath;
      biomeRoom.Active = true;
      biomeRoom.IsCustom = true;
      biomeRoom.Generated = false;
      this.StartX = biomeRoom.x;
      this.StartY = biomeRoom.y;
      biomeRoom.N_Room = new RoomConnection(GenerateRoom.ConnectionTypes.Exit);
      biomeRoom.E_Room = new RoomConnection(GenerateRoom.ConnectionTypes.DungeonFirstRoom);
      biomeRoom.S_Room = new RoomConnection(GenerateRoom.ConnectionTypes.Entrance);
      biomeRoom.W_Room = new RoomConnection(GenerateRoom.ConnectionTypes.False);
    }
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_6 && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_6))
      this.LeaderRoomPath = "Assets/_Rooms/Boss Room Yngya.prefab";
    if (string.IsNullOrEmpty(this.LeaderRoomPath) || !((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null) || MapManager.Instance.CurrentNode == null || DungeonSandboxManager.Active)
      return;
    BiomeRoom biomeRoom1 = this.PlacePostBossRoom(new List<Vector2Int>()
    {
      BiomeGenerator.BossCoords
    }, this.LeaderRoomPath, ConnectionRoom, false);
    biomeRoom1.S_Room.SetConnectionAndRoom(ConnectionRoom.Room, GenerateRoom.ConnectionTypes.False);
    biomeRoom1.N_Room.ConnectionType = GenerateRoom.ConnectionTypes.DoorRoom;
    biomeRoom1.Generated = true;
    biomeRoom1.IsBoss = true;
  }

  public BiomeRoom PlacePostBossRoom(
    List<Vector2Int> EmptyNeighbourPositions,
    string RoomToPlace,
    BiomeGenerator.RoomEntranceExit ConnectionRoom,
    bool setConnections = true)
  {
    Vector2Int neighbourPosition = EmptyNeighbourPositions[this.RandomSeed.Next(0, EmptyNeighbourPositions.Count)];
    BiomeRoom b = new BiomeRoom(neighbourPosition.x, neighbourPosition.y, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), (GameObject) null);
    this.Rooms.Add(b);
    this.SetNeighbours(b);
    b.IsCustom = true;
    b.Generated = false;
    b.GameObjectPath = RoomToPlace;
    if (setConnections)
    {
      if (neighbourPosition.y > ConnectionRoom.Room.y)
      {
        b.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.S_Room.Room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.S_Room.Room.N_Room.Room = b;
      }
      else if (neighbourPosition.y < ConnectionRoom.Room.y)
      {
        b.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.N_Room.Room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.N_Room.Room.S_Room.Room = b;
      }
      else if (neighbourPosition.x > ConnectionRoom.Room.x)
      {
        b.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.W_Room.Room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.W_Room.Room.E_Room.Room = b;
      }
      else if (neighbourPosition.x < ConnectionRoom.Room.x)
      {
        b.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.E_Room.Room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.E_Room.Room.W_Room.Room = b;
      }
    }
    return b;
  }

  public Vector2Int GetBossRoom()
  {
    foreach (BiomeRoom room in this.Rooms)
    {
      if (room.IsBoss)
        return new Vector2Int(room.x, room.y);
    }
    return new Vector2Int(0, 0);
  }

  public Vector2Int GetLastRoom() => new Vector2Int(this.lastRoom.x, this.lastRoom.y);

  public void GetCriticalPath()
  {
    if (this.OverrideRandomWalk)
      return;
    this.ResetFloodDistance();
    this.FloodFillDistance(this.RoomEntrance, 0);
    int distance = this.RoomExit.Distance;
    BiomeRoom biomeRoom = this.RoomExit;
    this.CriticalPath = new List<BiomeRoom>();
    this.CriticalPath.Add(biomeRoom);
    while (--distance > 0)
    {
      if (biomeRoom.N_Room.Connected && biomeRoom.N_Room.Room.Distance == distance)
      {
        biomeRoom = biomeRoom.N_Room.Room;
        biomeRoom.CriticalPathDirection = BiomeRoom.Direction.South;
      }
      if (biomeRoom.E_Room.Connected && biomeRoom.E_Room.Room.Distance == distance)
      {
        biomeRoom = biomeRoom.E_Room.Room;
        biomeRoom.CriticalPathDirection = BiomeRoom.Direction.West;
      }
      if (biomeRoom.S_Room.Connected && biomeRoom.S_Room.Room.Distance == distance)
      {
        biomeRoom = biomeRoom.S_Room.Room;
        biomeRoom.CriticalPathDirection = BiomeRoom.Direction.North;
      }
      if (biomeRoom.W_Room.Connected && biomeRoom.W_Room.Room.Distance == distance)
      {
        biomeRoom = biomeRoom.W_Room.Room;
        biomeRoom.CriticalPathDirection = BiomeRoom.Direction.East;
      }
      this.CriticalPath.Add(biomeRoom);
    }
  }

  public void PlaceLockAndKey()
  {
    if (string.IsNullOrEmpty(this.LockedRoomPath) || string.IsNullOrEmpty(this.KeyRoomPath) || this.OverrideRandomWalk)
      return;
    this.ResetFloodDistance();
    this.FloodFillDistance(this.RoomEntrance, 0);
    int distance1 = this.RoomExit.Distance;
    BiomeRoom biomeRoom1 = this.RoomExit;
    this.CriticalPath = new List<BiomeRoom>();
    this.CriticalPath.Add(biomeRoom1);
    while (--distance1 > 0)
    {
      if (biomeRoom1.N_Room.Connected && biomeRoom1.N_Room.Room.Distance == distance1)
      {
        biomeRoom1 = biomeRoom1.N_Room.Room;
        biomeRoom1.CriticalPathDirection = BiomeRoom.Direction.South;
      }
      if (biomeRoom1.E_Room.Connected && biomeRoom1.E_Room.Room.Distance == distance1)
      {
        biomeRoom1 = biomeRoom1.E_Room.Room;
        biomeRoom1.CriticalPathDirection = BiomeRoom.Direction.West;
      }
      if (biomeRoom1.S_Room.Connected && biomeRoom1.S_Room.Room.Distance == distance1)
      {
        biomeRoom1 = biomeRoom1.S_Room.Room;
        biomeRoom1.CriticalPathDirection = BiomeRoom.Direction.North;
      }
      if (biomeRoom1.W_Room.Connected && biomeRoom1.W_Room.Room.Distance == distance1)
      {
        biomeRoom1 = biomeRoom1.W_Room.Room;
        biomeRoom1.CriticalPathDirection = BiomeRoom.Direction.East;
      }
      this.CriticalPath.Add(biomeRoom1);
    }
    float num = (float) int.MinValue;
    BiomeRoom biomeRoom2 = (BiomeRoom) null;
    foreach (BiomeRoom room in this.Rooms)
    {
      if (!room.IsCustom && (double) room.Distance > (double) num && !this.CriticalPath.Contains(room))
      {
        biomeRoom2 = room;
        num = (float) room.Distance;
      }
    }
    biomeRoom2.IsCustom = true;
    biomeRoom2.GameObjectPath = this.KeyRoomPath;
    BiomeRoom biomeRoom3 = biomeRoom2;
    int distance2 = biomeRoom3.Distance;
    bool flag = true;
    while (flag && !this.CriticalPath.Contains(biomeRoom3))
    {
      if (biomeRoom3.N_Room.Connected && biomeRoom3.N_Room.Room.Distance < distance2)
      {
        biomeRoom3 = biomeRoom3.N_Room.Room;
        distance2 = biomeRoom3.Distance;
      }
      else if (biomeRoom3.E_Room.Connected && biomeRoom3.E_Room.Room.Distance < distance2)
      {
        biomeRoom3 = biomeRoom3.E_Room.Room;
        distance2 = biomeRoom3.Distance;
      }
      else if (biomeRoom3.S_Room.Connected && biomeRoom3.S_Room.Room.Distance < distance2)
      {
        biomeRoom3 = biomeRoom3.S_Room.Room;
        distance2 = biomeRoom3.Distance;
      }
      else if (biomeRoom3.W_Room.Connected && biomeRoom3.W_Room.Room.Distance < distance2)
      {
        biomeRoom3 = biomeRoom3.W_Room.Room;
        distance2 = biomeRoom3.Distance;
      }
    }
    int index = this.RandomSeed.Next(1, this.CriticalPath.IndexOf(biomeRoom3));
    this.CriticalPath[index].IsCustom = true;
    this.CriticalPath[index].GameObjectPath = this.LockedRoomPath;
  }

  public BiomeRoom RespawnRoom
  {
    get => this.\u003CRespawnRoom\u003Ek__BackingField;
    set => this.\u003CRespawnRoom\u003Ek__BackingField = value;
  }

  public void PlaceRespawnRoom()
  {
    if (this.RespawnRoom != null)
      return;
    this.RespawnRoom = new BiomeRoom(BiomeGenerator.RespawnRoomCoords.x, BiomeGenerator.RespawnRoomCoords.y, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), (GameObject) null);
    this.Rooms.Add(this.RespawnRoom);
    this.RespawnRoom.IsCustom = true;
    this.RespawnRoom.GameObjectPath = this.RespawnRoomPath;
    this.RespawnRoom.Generated = true;
    this.RespawnRoom.IsRespawnRoom = true;
  }

  public BiomeRoom DeathCatRoom
  {
    get => this.\u003CDeathCatRoom\u003Ek__BackingField;
    set => this.\u003CDeathCatRoom\u003Ek__BackingField = value;
  }

  public void PlaceDeathCatRoom()
  {
    if (this.DeathCatRoom != null)
      return;
    this.DeathCatRoom = new BiomeRoom(int.MaxValue, int.MaxValue, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), (GameObject) null);
    this.Rooms.Add(this.DeathCatRoom);
    this.DeathCatRoom.IsCustom = true;
    this.DeathCatRoom.GameObjectPath = this.DeathCatRoomPath;
    this.DeathCatRoom.Generated = true;
    this.DeathCatRoom.IsDeathCatRoom = true;
  }

  public BiomeRoom YngyaRoom
  {
    get => this.\u003CYngyaRoom\u003Ek__BackingField;
    set => this.\u003CYngyaRoom\u003Ek__BackingField = value;
  }

  public void PlaceYngyaRoom()
  {
    if (this.YngyaRoom != null)
      return;
    this.YngyaRoom = new BiomeRoom(444444, 444444, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), (GameObject) null);
    this.Rooms.Add(this.YngyaRoom);
    this.YngyaRoom.IsCustom = true;
    this.YngyaRoom.GameObjectPath = this.YngyaRoomPath;
    this.YngyaRoom.Generated = true;
    this.YngyaRoom.IsYngyaRoom = true;
  }

  public BiomeRoom MysticSellerRoom
  {
    get => this.\u003CMysticSellerRoom\u003Ek__BackingField;
    set => this.\u003CMysticSellerRoom\u003Ek__BackingField = value;
  }

  public void PlaceMysticSellerRoom()
  {
    this.MysticSellerRoom = new BiomeRoom(33333, 33333, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), (GameObject) null);
    this.Rooms.Add(this.MysticSellerRoom);
    this.MysticSellerRoom.IsCustom = true;
    this.MysticSellerRoom.GameObjectPath = "Assets/_Rooms/Mystic Shop Keeper Room.prefab";
    this.MysticSellerRoom.Generated = true;
    this.MysticSellerRoom.IsMysticShopRoom = true;
  }

  public IEnumerator PlaceStoryRooms()
  {
    if (!this.OverrideRandomWalk)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.RandomiseStoryOrder = new List<BiomeGenerator.ListOfStoryRooms>();
      int count = this.StoryRooms.Count;
      while (this.RandomiseStoryOrder.Count < count)
      {
        int index = this.RandomSeed.Next(0, this.StoryRooms.Count);
        this.RandomiseStoryOrder.Add(this.StoryRooms[index]);
        this.StoryRooms.Remove(this.StoryRooms[index]);
      }
      this.StoryRooms = new List<BiomeGenerator.ListOfStoryRooms>((IEnumerable<BiomeGenerator.ListOfStoryRooms>) this.RandomiseStoryOrder);
      if (this.DungeonLocation != FollowerLocation.Dungeon1_5 && this.DungeonLocation != FollowerLocation.Dungeon1_6 && DataManager.Instance.RemovedStoryMomentsActive)
        this.StoryRooms.Clear();
      int i = -1;
      while (++i < this.StoryRooms.Count)
      {
        BiomeGenerator.ListOfStoryRooms storyRoom = this.StoryRooms[i];
        if (GameManager.CurrentDungeonLayer - 1 == DataManager.Instance.GetVariableInt(storyRoom.StoryVariable) && DataManager.Instance.GetVariableInt(storyRoom.LastRun) < DataManager.Instance.dungeonRun && !DataManager.Instance.GetVariable(storyRoom.DungeonBeaten) && DataManager.Instance.GetVariableInt(storyRoom.StoryVariable) < storyRoom.Rooms.Count && GameManager.CurrentDungeonLayer != 0 && !string.IsNullOrEmpty(storyRoom.Rooms[DataManager.Instance.GetVariableInt(storyRoom.StoryVariable)].RoomPath))
        {
          int num1 = this.RandomSeed.Next(0, 10);
          int variableInt = DataManager.Instance.GetVariableInt(storyRoom.StoryVariable);
          if (GameManager.CurrentDungeonFloor != 1 || num1 >= 5 && storyRoom.Rooms[variableInt].FloorOne || !storyRoom.Rooms[variableInt].FloorTwo && !storyRoom.Rooms[variableInt].FloorThree)
          {
            string roomPath = storyRoom.Rooms[DataManager.Instance.GetVariableInt(storyRoom.StoryVariable)].RoomPath;
            this.AvailableRooms = new List<BiomeRoom>();
            if (storyRoom.PutOnCriticalPath)
            {
              foreach (BiomeRoom biomeRoom in this.CriticalPath)
              {
                if (!biomeRoom.IsCustom)
                  this.AvailableRooms.Add(biomeRoom);
              }
            }
            if (!storyRoom.PutOnCriticalPath || storyRoom.PutOnCriticalPath && this.AvailableRooms.Count <= 0)
            {
              int num2 = 0;
              while (this.AvailableRooms.Count <= 0 && ++num2 <= 4)
              {
                foreach (BiomeRoom room in this.Rooms)
                {
                  if (!room.IsCustom && room.NumConnections == num2)
                    this.AvailableRooms.Add(room);
                }
              }
            }
            BiomeRoom availableRoom = this.AvailableRooms[this.RandomSeed.Next(0, this.AvailableRooms.Count)];
            availableRoom.IsCustom = true;
            availableRoom.GameObjectPath = roomPath;
            MMTransition.UpdateProgress((float) i / (float) this.CustomDynamicRooms.Count, ScriptLocalization.Interactions.GeneratingDungeon);
            if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
            {
              stopwatch.Reset();
              yield return (object) null;
            }
          }
        }
      }
      stopwatch.Stop();
    }
  }

  public IEnumerator PlaceDynamicCustomRooms()
  {
    bool spawnedWeaponRoom = false;
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    if (!this.OverrideRandomWalk)
    {
      this.RandomiseOrder = new List<BiomeGenerator.ListOfCustomRoomPrefabs>();
      int count = this.CustomDynamicRooms.Count;
      while (this.RandomiseOrder.Count < count)
      {
        int index = this.RandomSeed.Next(0, this.CustomDynamicRooms.Count);
        this.RandomiseOrder.Add(this.CustomDynamicRooms[index]);
        this.CustomDynamicRooms.Remove(this.CustomDynamicRooms[index]);
      }
      this.CustomDynamicRooms = new List<BiomeGenerator.ListOfCustomRoomPrefabs>((IEnumerable<BiomeGenerator.ListOfCustomRoomPrefabs>) this.RandomiseOrder);
      int i = -1;
      while (++i < this.CustomDynamicRooms.Count)
      {
        BiomeGenerator.ListOfCustomRoomPrefabs customDynamicRoom = this.CustomDynamicRooms[i];
        bool flag1 = false;
        foreach (BiomeGenerator.VariableAndCondition conditionalVariable in customDynamicRoom.ConditionalVariables)
        {
          if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
            flag1 = true;
        }
        if (!flag1)
        {
          bool flag2 = false;
          foreach (FollowerLocation followerLocation in customDynamicRoom.LocationIsUndiscovered)
          {
            if (DataManager.Instance.DiscoveredLocations.Contains(followerLocation))
              flag2 = true;
          }
          if (!flag2)
          {
            bool flag3 = false;
            if (customDynamicRoom.MinimumRun && DataManager.Instance.dungeonRun < customDynamicRoom.MinimumRunNumber)
              flag3 = true;
            if (!flag3)
            {
              bool flag4 = false;
              if (customDynamicRoom.MaximumRun && DataManager.Instance.dungeonRun > customDynamicRoom.MaximumRunNumber)
                flag4 = true;
              if (!flag4)
              {
                bool flag5 = false;
                if (customDynamicRoom.MinimumFollowerCount && DataManager.Instance.Followers.Count < customDynamicRoom.MinimumFollowerCountNumber)
                  flag5 = true;
                if (!flag5)
                {
                  bool flag6 = false;
                  if (customDynamicRoom.MaximumFollowerCount && DataManager.Instance.Followers.Count > customDynamicRoom.MaximumFollowerCountNumber)
                    flag6 = true;
                  if (!flag6)
                  {
                    if (customDynamicRoom.RoomPaths.Count == 1 && customDynamicRoom.RoomPaths[0] == "Assets/_Rooms/Reward Room Tarot.prefab" && DataManager.Instance.CanUnlockRelics && !DataManager.Instance.OnboardedRelics && !DungeonSandboxManager.Active)
                      customDynamicRoom.PutOnCriticalPath = true;
                    if (this.RandomSeed.NextDouble() <= (double) customDynamicRoom.Probability && customDynamicRoom.AvailableOnLayer() && customDynamicRoom.AvailableOnFoor())
                    {
                      if (customDynamicRoom.UseStaticRoomList)
                      {
                        customDynamicRoom.RoomPaths = customDynamicRoom.RoomPaths.Union<string>((IEnumerable<string>) BiomeGenerator.ListOfCustomRoomPrefabs.StaticRoomList).ToList<string>();
                        if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.FirstDoctrineStone)
                          customDynamicRoom.RoomPaths.Add("Assets/_Rooms/Reward Doctrine Room.prefab");
                        if (!DataManager.Instance.HaroConversationCompleted)
                          customDynamicRoom.RoomPaths.Add("Assets/_Rooms/Lore Haro.prefab");
                        if (PlayerFleeceManager.FleecePreventsHealthPickups())
                        {
                          customDynamicRoom.RoomPaths.Remove("Assets/_Rooms/Special Blood Sacrafice.prefab");
                          customDynamicRoom.RoomPaths.Remove("Assets/_Rooms/Special Free Health.prefab");
                        }
                      }
                      string roomPath = customDynamicRoom.RoomPaths[this.RandomSeed.Next(0, customDynamicRoom.RoomPaths.Count)];
                      this.AvailableRooms = new List<BiomeRoom>();
                      if (customDynamicRoom.PutOnCriticalPath)
                      {
                        foreach (BiomeRoom biomeRoom in this.CriticalPath)
                        {
                          if (!biomeRoom.IsCustom)
                            this.AvailableRooms.Add(biomeRoom);
                        }
                      }
                      if (!customDynamicRoom.PutOnCriticalPath || customDynamicRoom.PutOnCriticalPath && this.AvailableRooms.Count <= 0)
                      {
                        int num = 0;
                        while (this.AvailableRooms.Count <= 0 && ++num <= 4)
                        {
                          foreach (BiomeRoom room in this.Rooms)
                          {
                            if (customDynamicRoom.RemoveIfNoNonCriticalPath)
                            {
                              if (!room.IsCustom && room.NumConnections == num && !this.CriticalPath.Contains(room))
                                this.AvailableRooms.Add(room);
                            }
                            else if (!room.IsCustom && room.NumConnections == num)
                              this.AvailableRooms.Add(room);
                          }
                        }
                      }
                      if (this.AvailableRooms.Count > 0)
                      {
                        BiomeRoom availableRoom = this.AvailableRooms[this.RandomSeed.Next(0, this.AvailableRooms.Count)];
                        availableRoom.GameObjectPath = roomPath;
                        if (availableRoom.GameObjectPath == "Assets/_Rooms/Reward Room Tarot.prefab")
                        {
                          if ((DataManager.Instance.CanUnlockRelics && !DataManager.Instance.OnboardedRelics && !DungeonSandboxManager.Active || (DataManager.Instance.OnboardedRelics || DungeonSandboxManager.Active) && this.RandomSeed.Next(0, 100) >= 85) && !spawnedWeaponRoom)
                          {
                            spawnedWeaponRoom = true;
                            availableRoom.GameObjectPath = "Assets/_Rooms/Marketplace Relics.prefab";
                            customDynamicRoom.ConnectionType = GenerateRoom.ConnectionTypes.RelicShop;
                          }
                          else if (GameManager.CurrentDungeonFloor > 1 && this.RandomSeed.Next(0, 100) >= 65 && DataManager.Instance.WeaponPool.Count + DataManager.Instance.CursePool.Count > 3 && !spawnedWeaponRoom)
                          {
                            spawnedWeaponRoom = true;
                            availableRoom.GameObjectPath = "Assets/_Rooms/Marketplace Weapons.prefab";
                            customDynamicRoom.ConnectionType = GenerateRoom.ConnectionTypes.WeaponShop;
                            UnityEngine.Debug.Log((object) "2".Colour(Color.yellow));
                            foreach (BiomeRoom room in this.Rooms)
                              room.HasWeapon = false;
                          }
                          else if (DataManager.Instance.PlayerFleece != 4)
                          {
                            customDynamicRoom.ConnectionType = GenerateRoom.ConnectionTypes.Tarot;
                            availableRoom.GameObjectPath = roomPath;
                          }
                          else
                            continue;
                        }
                        availableRoom.IsCustom = true;
                        if (customDynamicRoom.SetCustomConnectionType)
                        {
                          if (availableRoom.N_Room.Connected)
                            availableRoom.N_Room.Room.S_Room.SetConnection(customDynamicRoom.ConnectionType);
                          if (availableRoom.E_Room.Connected)
                            availableRoom.E_Room.Room.W_Room.SetConnection(customDynamicRoom.ConnectionType);
                          if (availableRoom.S_Room.Connected)
                            availableRoom.S_Room.Room.N_Room.SetConnection(customDynamicRoom.ConnectionType);
                          if (availableRoom.W_Room.Connected)
                            availableRoom.W_Room.Room.E_Room.SetConnection(customDynamicRoom.ConnectionType);
                        }
                      }
                      MMTransition.UpdateProgress((float) i / (float) this.CustomDynamicRooms.Count, ScriptLocalization.Interactions.GeneratingDungeon);
                      if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
                      {
                        stopwatch.Reset();
                        yield return (object) null;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      List<BiomeRoom> biomeRoomList = new List<BiomeRoom>();
      foreach (BiomeRoom room in this.Rooms)
      {
        if (!room.IsCustom)
          biomeRoomList.Add(room);
      }
      if (!BiomeGenerator.WeaponAtEnd && GameManager.CurrentDungeonFloor > 1 && biomeRoomList.Count > 0 && PlayerFarming.Location != FollowerLocation.IntroDungeon)
        biomeRoomList[this.RandomSeed.Next(0, biomeRoomList.Count)].HasWeapon = true;
      stopwatch.Stop();
    }
  }

  public void PlaceFixedCustomRooms()
  {
    if (this.OverrideRandomWalk)
      return;
    this.HasSpawnedRelic = false;
    foreach (BiomeGenerator.FixedRoom customRoom in this.CustomRooms)
    {
      bool flag1 = false;
      foreach (BiomeGenerator.VariableAndCondition conditionalVariable in customRoom.ConditionalVariables)
      {
        if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
          flag1 = true;
      }
      if (!flag1 && this.RandomSeed.NextDouble() <= (double) customRoom.Probability)
      {
        this.AvailableRooms = new List<BiomeRoom>();
        foreach (BiomeRoom room in this.Rooms)
        {
          bool flag2 = true;
          if (room.IsCustom)
            flag2 = false;
          if (room.N_Room.Connected && customRoom.North == BiomeGenerator.FixedRoom.Connection.ForceOff)
            flag2 = false;
          if (room.E_Room.Connected && customRoom.East == BiomeGenerator.FixedRoom.Connection.ForceOff)
            flag2 = false;
          if (room.S_Room.Connected && customRoom.South == BiomeGenerator.FixedRoom.Connection.ForceOff)
            flag2 = false;
          if (room.W_Room.Connected && customRoom.West == BiomeGenerator.FixedRoom.Connection.ForceOff)
            flag2 = false;
          if (!room.N_Room.Connected && customRoom.North == BiomeGenerator.FixedRoom.Connection.ForceOn)
            flag2 = false;
          if (!room.E_Room.Connected && customRoom.East == BiomeGenerator.FixedRoom.Connection.ForceOn)
            flag2 = false;
          if (!room.S_Room.Connected && customRoom.South == BiomeGenerator.FixedRoom.Connection.ForceOn)
            flag2 = false;
          if (!room.W_Room.Connected && customRoom.West == BiomeGenerator.FixedRoom.Connection.ForceOn)
            flag2 = false;
          if (flag2)
            this.AvailableRooms.Add(room);
        }
        if (this.AvailableRooms.Count <= 0 && !customRoom.HasForcedOn && customRoom.HasOptional)
        {
          foreach (BiomeRoom room in this.Rooms)
          {
            int num1 = room.IsCustom ? 1 : 0;
            if (room.N_Room.Connected)
            {
              int north = (int) customRoom.North;
            }
            if (room.E_Room.Connected)
            {
              int east = (int) customRoom.East;
            }
            if (room.S_Room.Connected)
            {
              int south = (int) customRoom.South;
            }
            if (room.W_Room.Connected)
            {
              int west = (int) customRoom.West;
            }
            int num2 = 0;
            if (customRoom.North == BiomeGenerator.FixedRoom.Connection.Optional && room.N_Room.Connected)
              ++num2;
            if (customRoom.East == BiomeGenerator.FixedRoom.Connection.Optional && room.E_Room.Connected)
              ++num2;
            if (customRoom.South == BiomeGenerator.FixedRoom.Connection.Optional && room.S_Room.Connected)
              ++num2;
            if (customRoom.West == BiomeGenerator.FixedRoom.Connection.Optional && room.W_Room.Connected)
              ++num2;
            if (num2 > 0)
              this.AvailableRooms.Add(room);
          }
        }
        if (this.AvailableRooms.Count <= 0)
        {
          if (customRoom.RequiredConnections == 1)
          {
            int num = 0;
            while (this.AvailableRooms.Count <= 0 && ++num <= 3)
            {
              foreach (BiomeRoom room in this.Rooms)
              {
                if (!room.IsCustom && room.NumConnections == num && (customRoom.North != BiomeGenerator.FixedRoom.Connection.ForceOff && room.S_Room.Room == null || customRoom.East != BiomeGenerator.FixedRoom.Connection.ForceOff && room.W_Room.Room == null || customRoom.South != BiomeGenerator.FixedRoom.Connection.ForceOff && room.N_Room.Room == null || customRoom.West != BiomeGenerator.FixedRoom.Connection.ForceOff && room.E_Room.Room == null))
                  this.AvailableRooms.Add(room);
              }
            }
            if (this.AvailableRooms.Count > 0)
            {
              UnityEngine.Debug.Log((object) ("CREATE ! " + customRoom.prefab.name));
              BiomeRoom availableRoom = this.AvailableRooms[this.RandomSeed.Next(0, this.AvailableRooms.Count)];
              if (customRoom.North != BiomeGenerator.FixedRoom.Connection.ForceOff)
                this.CreateCustomRoom(availableRoom.x, availableRoom.y - 1, customRoom.prefab, BiomeGenerator.Direction.North, customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate);
              else if (customRoom.East != BiomeGenerator.FixedRoom.Connection.ForceOff)
                this.CreateCustomRoom(availableRoom.x - 1, availableRoom.y, customRoom.prefab, BiomeGenerator.Direction.East, customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate);
              else if (customRoom.South != BiomeGenerator.FixedRoom.Connection.ForceOff)
                this.CreateCustomRoom(availableRoom.x, availableRoom.y + 1, customRoom.prefab, BiomeGenerator.Direction.South, customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate);
              else if (customRoom.West != BiomeGenerator.FixedRoom.Connection.ForceOff)
                this.CreateCustomRoom(availableRoom.x + 1, availableRoom.y, customRoom.prefab, BiomeGenerator.Direction.West, customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate);
            }
          }
        }
        else
        {
          UnityEngine.Debug.Log((object) ("WORKS! " + customRoom.prefab.name));
          BiomeRoom availableRoom = this.AvailableRooms[this.RandomSeed.Next(0, this.AvailableRooms.Count)];
          availableRoom.IsCustom = true;
          availableRoom.GameObject = customRoom.prefab;
          availableRoom.Generated = customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate;
        }
      }
    }
  }

  public void CreateCustomRoom(
    int x,
    int y,
    GameObject Prefab,
    BiomeGenerator.Direction direction,
    bool Generated)
  {
    BiomeRoom b = new BiomeRoom(x, y, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), Prefab);
    b.IsCustom = true;
    b.Generated = Generated;
    this.Rooms.Add(b);
    UnityEngine.Debug.Log((object) $"NEW ROOM CREATED: {Prefab.name}  {b.Generated.ToString()}");
    switch (direction)
    {
      case BiomeGenerator.Direction.North:
        this.SetNeighbours(b);
        b.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.N_Room.Room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        break;
      case BiomeGenerator.Direction.East:
        this.SetNeighbours(b);
        b.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.E_Room.Room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        break;
      case BiomeGenerator.Direction.South:
        this.SetNeighbours(b);
        b.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.S_Room.Room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        break;
      case BiomeGenerator.Direction.West:
        this.SetNeighbours(b);
        b.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.W_Room.Room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        break;
    }
  }

  public void SetNeighbours(BiomeRoom b)
  {
    if (b.N_Room == null)
      b.N_Room = new RoomConnection(BiomeRoom.GetRoom(b.x, b.y + 1));
    if (b.E_Room == null)
      b.E_Room = new RoomConnection(BiomeRoom.GetRoom(b.x + 1, b.y));
    if (b.S_Room == null)
      b.S_Room = new RoomConnection(BiomeRoom.GetRoom(b.x, b.y - 1));
    if (b.W_Room != null)
      return;
    b.W_Room = new RoomConnection(BiomeRoom.GetRoom(b.x - 1, b.y));
  }

  public void SetNeighbours()
  {
    foreach (BiomeRoom room in this.Rooms)
    {
      if (room.N_Room == null)
        room.N_Room = new RoomConnection(BiomeRoom.GetRoom(room.x, room.y + 1));
      if (room.E_Room == null)
        room.E_Room = new RoomConnection(BiomeRoom.GetRoom(room.x + 1, room.y));
      if (room.S_Room == null)
        room.S_Room = new RoomConnection(BiomeRoom.GetRoom(room.x, room.y - 1));
      if (room.W_Room == null)
        room.W_Room = new RoomConnection(BiomeRoom.GetRoom(room.x - 1, room.y));
    }
  }

  public IEnumerator InstantiatePrefabs()
  {
    BiomeGenerator biomeGenerator = this;
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    int i = -1;
    foreach (BiomeRoom room in biomeGenerator.Rooms)
    {
      BiomeRoom r = room;
      if (r.IsCustom)
      {
        if (!string.IsNullOrEmpty(r.GameObjectPath))
        {
          bool loaded = false;
          bool isInstantiated = false;
          Addressables_wrapper.InstantiateAsync((object) r.GameObjectPath, biomeGenerator.transform, false, (Action<AsyncOperationHandle<GameObject>>) (obj =>
          {
            r.GameObject = obj.Result;
            if ((UnityEngine.Object) r.GameObject != (UnityEngine.Object) null)
              r.GameObject.SetActive(false);
            isInstantiated = true;
          }));
          while (!isInstantiated)
            yield return (object) null;
          r.generateRoom.generated = r.Generated;
          r.generateRoom.EastBossDoor = biomeGenerator.GeneratorRoomPrefab.EastBossDoor;
          r.generateRoom.EastBossDoor_P2 = biomeGenerator.GeneratorRoomPrefab.EastBossDoor_P2;
          r.generateRoom.NorthBossDoor = biomeGenerator.GeneratorRoomPrefab.NorthBossDoor;
          r.generateRoom.NorthBossDoor_P2 = biomeGenerator.GeneratorRoomPrefab.NorthBossDoor_P2;
          r.generateRoom.SouthBossDoor = biomeGenerator.GeneratorRoomPrefab.SouthBossDoor;
          r.generateRoom.SouthBossDoor_P2 = biomeGenerator.GeneratorRoomPrefab.SouthBossDoor_P2;
          r.generateRoom.WestBossDoor = biomeGenerator.GeneratorRoomPrefab.WestBossDoor;
          r.generateRoom.WestBossDoor_P2 = biomeGenerator.GeneratorRoomPrefab.WestBossDoor_P2;
          loaded = true;
          while (!loaded)
            yield return (object) null;
        }
        else
        {
          r.GameObject = UnityEngine.Object.Instantiate<GameObject>(r.GameObject, biomeGenerator.transform);
          r.GameObject.SetActive(false);
        }
      }
      else if (!biomeGenerator.ReuseGeneratorRoom)
      {
        r.GameObject = UnityEngine.Object.Instantiate<GameObject>(biomeGenerator.GeneratorRoomPrefab.gameObject, biomeGenerator.transform);
        r.GameObject.SetActive(false);
      }
      if (r.IsRespawnRoom)
      {
        r.GameObject.transform.parent = biomeGenerator.transform.parent;
        r.GameObject.gameObject.GetComponent<RespawnRoomManager>().Init(biomeGenerator);
      }
      if (r.IsDeathCatRoom)
      {
        r.GameObject.transform.parent = biomeGenerator.transform.parent;
        r.GameObject.gameObject.GetComponent<DeathCatRoomManager>().Init(biomeGenerator);
      }
      if (r.IsYngyaRoom)
      {
        r.GameObject.transform.parent = biomeGenerator.transform.parent;
        r.GameObject.gameObject.GetComponent<YngyaRoomManager>().Init(biomeGenerator);
      }
      if (r.IsMysticShopRoom)
        r.GameObject.gameObject.GetComponent<MysticShopKeeperManager>().Init(biomeGenerator);
      string str1 = LocalizeIntegration.ReverseText(biomeGenerator.Rooms.Count.ToString());
      string str2 = LocalizeIntegration.ReverseText(i.ToString());
      if (LocalizeIntegration.IsArabic())
        MMTransition.UpdateProgress((float) ((double) ++i / (double) biomeGenerator.Rooms.Count), $"{ScriptLocalization.Interactions.GeneratingDungeon} {ScriptLocalization.UI.PlacingRooms} {str1}/{str2}");
      else
        MMTransition.UpdateProgress((float) ((double) ++i / (double) biomeGenerator.Rooms.Count), $"{ScriptLocalization.Interactions.GeneratingDungeon} {ScriptLocalization.UI.PlacingRooms} {i.ToString()}/{str1}");
      yield return (object) new WaitForEndOfFrame();
    }
    if (!biomeGenerator.ReuseGeneratorRoom)
      biomeGenerator.GeneratorRoomPrefab.gameObject.SetActive(false);
    stopwatch.Stop();
  }

  public void OnDestroy()
  {
    this.Clear();
    this.StopMusicAndAtmos();
    Explosion.Clear();
    LightningRingExplosion.Clear();
    if (this.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      this.loadedAddressableAssets.Clear();
    }
    if (!((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) this))
      return;
    BiomeGenerator.Instance = (BiomeGenerator) null;
  }

  public void Clear()
  {
    foreach (BiomeRoom room in this.Rooms)
      room.Clear();
    this.Rooms.Clear();
  }

  public void StopMusicAndAtmos()
  {
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopLoop(this.biomeAtmosInstance);
  }

  public void Regenerate(System.Action Callback)
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", (System.Action) (() =>
    {
      this.Clear();
      ObjectPool.DestroyAll();
      BlackSoulUpdater.Instance.Clear();
      this.PrevRoom = (BiomeRoom) null;
      this.FirstArrival = true;
      for (int index = this.transform.childCount - 1; index > 0; --index)
      {
        if ((UnityEngine.Object) this.transform.GetChild(index).gameObject != (UnityEngine.Object) this.GeneratorRoomPrefab)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.transform.GetChild(index).gameObject);
      }
      this.Seed = DataManager.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue);
      DataManager.Instance.AddNewDungeonSeed(this.Seed);
      MonoSingleton<MMLogger>.Instance.AddToLog("DungeonSeed/Regenerated: " + this.Seed.ToString());
      this.StartCoroutine((IEnumerator) this.GenerateRoutine());
      System.Action action = Callback;
      if (action != null)
        action();
      this.EncounteredPossessedEnemyThisFloor = false;
    }));
  }

  public void SetRoom(int x, int y)
  {
    this.CurrentX = x;
    this.CurrentY = y;
    this.StartCoroutine((IEnumerator) this.ChangeRoomRoutine(BiomeRoom.GetRoom(this.CurrentX, this.CurrentY)));
  }

  public static void ChangeRoom(int X, int Y)
  {
    BiomeGenerator.Instance.CurrentX = X;
    BiomeGenerator.Instance.CurrentY = Y;
    BiomeGenerator.Instance.StartCoroutine((IEnumerator) BiomeGenerator.Instance.ChangeRoomRoutine(BiomeRoom.GetRoom(BiomeGenerator.Instance.CurrentX, BiomeGenerator.Instance.CurrentY)));
  }

  public static void ChangeRoom(Vector2Int Direction)
  {
    PlayerFarming.ResetMainPlayer();
    BiomeGenerator.Instance.CurrentX += Direction.x;
    BiomeGenerator.Instance.CurrentY += Direction.y;
    BiomeGenerator.Instance.StartCoroutine((IEnumerator) BiomeGenerator.Instance.ChangeRoomRoutine(BiomeRoom.GetRoom(BiomeGenerator.Instance.CurrentX, BiomeGenerator.Instance.CurrentY)));
  }

  public IEnumerator ChangeRoomRoutine(BiomeRoom CurrentRoom)
  {
    BiomeGenerator biomeGenerator = this;
    Health.team2.Clear();
    BiomeGenerator.BiomeAction onBiomeLeftRoom = BiomeGenerator.OnBiomeLeftRoom;
    if (onBiomeLeftRoom != null)
      onBiomeLeftRoom();
    biomeGenerator.CurrentRoom = CurrentRoom;
    if (!CurrentRoom.Visited)
      biomeGenerator.RoomsVisited++;
    CurrentRoom.Activate(biomeGenerator.PrevRoom, biomeGenerator.ReuseGeneratorRoom);
    yield return (object) new WaitForEndOfFrame();
    biomeGenerator.GetDoors();
    while (!CurrentRoom.generateRoom.GeneratedDecorations)
      yield return (object) null;
    CurrentRoom.generateRoom.SetColliderAndUpdatePathfinding();
    UnityEngine.Resources.UnloadUnusedAssets();
    biomeGenerator.PlacePlayer();
    BiomeGenerator.BiomeAction onBiomeChangeRoom = BiomeGenerator.OnBiomeChangeRoom;
    if (onBiomeChangeRoom != null)
      onBiomeChangeRoom();
    if (biomeGenerator.PrevRoom == null)
    {
      if (CurrentRoom.generateRoom.roomMusicID == SoundConstants.RoomID.StandardRoom && GameManager.Layer2)
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.AltStandardRoom);
      else
        AudioManager.Instance.SetMusicRoomID(CurrentRoom.generateRoom.roomMusicID);
      AudioManager.Instance.StartMusic();
    }
    else
    {
      if (biomeGenerator.PrevRoom.generateRoom.roomMusicID == SoundConstants.RoomID.NoMusic && CurrentRoom.generateRoom.roomMusicID != SoundConstants.RoomID.NoMusic)
        AudioManager.Instance.PlayMusic(biomeGenerator.biomeMusicPath);
      if (CurrentRoom.generateRoom.roomMusicID == SoundConstants.RoomID.StandardRoom && GameManager.Layer2)
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.AltStandardRoom);
      else
        AudioManager.Instance.SetMusicRoomID(CurrentRoom.generateRoom.roomMusicID);
    }
    string soundPath = CurrentRoom.generateRoom.biomeAtmosOverridePath != string.Empty ? CurrentRoom.generateRoom.biomeAtmosOverridePath : biomeGenerator.biomeAtmosPath;
    if (!AudioManager.Instance.CurrentEventInstanceIsPlayingPath(biomeGenerator.biomeAtmosInstance, soundPath))
      AudioManager.Instance.PlayAtmos(soundPath);
    bool flag = false;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.Team2 && (double) allUnit.HP > 0.0)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      AudioManager.Instance.SetMusicCombatState(false);
    biomeGenerator.PrevRoom = CurrentRoom;
    if (CurrentRoom != null && CurrentRoom.GameObjectPath != null && CurrentRoom.GameObjectPath.Contains("D5 Mountain Top") && biomeGenerator.DungeonLocation != FollowerLocation.Boss_Wolf)
      biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.PlayDLCVideoIE());
  }

  public static void SpawnBombsInRoom(
    int amount,
    bool enemyBombs = true,
    PlayerFarming playerFarming = null,
    float damageMultiplier = 1f)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    BiomeGenerator.\u003C\u003Ec__DisplayClass190_0 displayClass1900 = new BiomeGenerator.\u003C\u003Ec__DisplayClass190_0();
    // ISSUE: reference to a compiler-generated field
    displayClass1900.enemyBombs = enemyBombs;
    float delay = 0.0f;
    float num = 0.25f;
    for (int index = 0; index < amount; ++index)
    {
      Vector3 positionInIsland = BiomeGenerator.GetRandomPositionInIsland();
      // ISSUE: reference to a compiler-generated method
      GameManager.GetInstance().StartCoroutine((IEnumerator) displayClass1900.\u003CSpawnBombsInRoom\u003Eg__SpawnBomb\u007C0(positionInIsland, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, delay, playerFarming, damageMultiplier));
      delay += num;
    }
  }

  public static void SpawnPoisonsInRoom(
    int amount,
    bool enemyBombs = true,
    PlayerFarming playerFarming = null,
    float damageMultiplier = 1f)
  {
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island")) | 1 << LayerMask.NameToLayer("Obstacles Player Ignore"));
    float x = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) Vector3.right, 100f, (int) layerMask).point.x;
    float y = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) Vector3.up, 100f, (int) layerMask).point.y;
    float delay = 0.0f;
    float num = 0.1f;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      Vector3 position = player.transform.position;
      GameManager.GetInstance().StartCoroutine((IEnumerator) BiomeGenerator.\u003CSpawnPoisonsInRoom\u003Eg__SpawnPoison\u007C191_0(position, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, delay, player, damageMultiplier));
    }
    for (int index = 0; index < amount; ++index)
    {
      Vector3 position = new Vector3(UnityEngine.Random.Range(-x, x), UnityEngine.Random.Range(-y, y));
      GameManager.GetInstance().StartCoroutine((IEnumerator) BiomeGenerator.\u003CSpawnPoisonsInRoom\u003Eg__SpawnPoison\u007C191_0(position, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, delay, playerFarming, damageMultiplier));
      delay += num;
    }
  }

  public static Vector3 GetRandomPositionInIsland()
  {
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) ((int) (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island")) | 1 << LayerMask.NameToLayer("Obstacles Player Ignore")) | 1 << LayerMask.NameToLayer("Obstacles"));
    float x = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) Vector3.right, 100f, (int) layerMask).point.x;
    float y = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) Vector3.up, 100f, (int) layerMask).point.y;
    int num = 10;
    Vector3 positionInIsland = Vector3.zero;
    while (num > 0)
    {
      --num;
      positionInIsland = new Vector3(UnityEngine.Random.Range((float) (-(double) x + 0.5), x - 0.5f), UnityEngine.Random.Range((float) (-(double) y + 0.5), y - 0.5f));
      if ((UnityEngine.Object) Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) positionInIsland.normalized, positionInIsland.magnitude, (int) layerMask).collider == (UnityEngine.Object) null)
        return positionInIsland;
    }
    return positionInIsland;
  }

  public static bool PointWithinIsland(Vector3 point, out Vector3 closestPoint)
  {
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) ((int) (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island")) | 1 << LayerMask.NameToLayer("Obstacles Player Ignore")) | 1 << LayerMask.NameToLayer("Obstacles"));
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) point.normalized, point.magnitude, (int) layerMask);
    closestPoint = (UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null ? (Vector3) raycastHit2D.point : point;
    return (UnityEngine.Object) raycastHit2D.collider == (UnityEngine.Object) null;
  }

  public IEnumerator DelayEndConversation()
  {
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().OnConversationEnd(false);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().AddPlayerToCamera();
  }

  public IEnumerator PlayersFirstEnterRoomDelayedGotoAndStop(
    GameObject mainPlayer,
    Door entranceDoor,
    Vector3 TargetPosition)
  {
    BiomeGenerator biomeGenerator = this;
    while (!biomeGenerator.CurrentRoom.generateRoom.GeneratedPathing)
      yield return (object) null;
    mainPlayer.transform.position = entranceDoor.PlayerPosition.position;
    mainPlayer.GetComponent<PlayerFarming>().state.facingAngle = Vector3.Angle(Vector3.right, entranceDoor.GetDoorDirection());
    PlayerFarming.RefreshPlayersCount(false);
    PlayerFarming.ResetMainPlayer();
    PlayerFarming.PositionAllPlayers(mainPlayer.transform.position, false);
    int i;
    for (i = 0; i < PlayerFarming.playersCount; ++i)
    {
      PlayerFarming playerFarming = PlayerFarming.players[i];
      if (i > 0)
      {
        TargetPosition += Vector3.right;
        yield return (object) new WaitForSeconds(0.25f);
      }
      playerFarming.GoToAndStop(TargetPosition, IdleOnEnd: true, DisableCollider: true, GoToCallback: (System.Action) (() =>
      {
        if (!playerFarming.isLamb)
          return;
        if ((UnityEngine.Object) this.PlayerState != (UnityEngine.Object) null)
          this.PlayerState.facingAngle = Utils.GetAngle(Door.GetEntranceDoor().transform.position, TargetPosition);
        this.StartCoroutine((IEnumerator) this.DelayEndConversation());
        if (this.CurrentRoom == this.RoomEntrance || DungeonSandboxManager.Active)
          GenerateRoom.Instance.LockEntranceBehindPlayer = true;
        Door entranceDoor1 = Door.GetEntranceDoor();
        if (GenerateRoom.Instance.LockEntranceBehindPlayer)
        {
          entranceDoor1.RoomLockController.gameObject.SetActive(true);
          entranceDoor1.RoomLockController.DoorUp();
          foreach (PlayerDistanceMovement componentsInChild in entranceDoor1.GetComponentsInChildren<PlayerDistanceMovement>())
          {
            componentsInChild.ForceReset();
            componentsInChild.enabled = false;
          }
          entranceDoor1.VisitedIcon.SetActive(false);
        }
        entranceDoor1.PlayerFinishedEnteringDoor();
        PlayerFarming.ResetMainPlayer();
        CoopManager.RefreshCoopPlayerRewired();
        this.StartCoroutine((IEnumerator) this.DelayActivateRoom(this.DungeonLocation != FollowerLocation.Boss_5));
      }), maxDuration: -1f, forcePositionOnTimeout: true);
    }
    yield return (object) new WaitForSeconds(0.5f);
    if (biomeGenerator.ShowDisplayName)
    {
      if (DataManager.Instance.dungeonRun == 1 && PlayerFarming.Location == FollowerLocation.Dungeon1_1 && !DataManager.Instance.QuickStartActive)
      {
        yield return (object) new WaitForSeconds(1f);
        UIHeartsIntro heartsIntro = UnityEngine.Object.Instantiate<UIHeartsIntro>(UnityEngine.Resources.Load<UIHeartsIntro>("Prefabs/UI/UI Hearts Intro"), GameObject.FindGameObjectWithTag("Canvas").transform);
        for (i = 0; i < PlayerFarming.playersCount; ++i)
        {
          if (i == 0)
          {
            PlayerFarming player = PlayerFarming.players[i];
            yield return (object) biomeGenerator.StartCoroutine((IEnumerator) heartsIntro.HeartRoutine(player));
            DataManager.Instance.PlayerHasBeenGivenHearts = true;
          }
        }
        heartsIntro = (UIHeartsIntro) null;
      }
      HUD_DisplayName.Play(biomeGenerator.DisplayName, 3, HUD_DisplayName.Positions.Centre);
      yield return (object) new WaitForSeconds(1f);
    }
    biomeGenerator.ShowDisplayName = false;
    DataManager.Instance.FirstTimeInDungeon = true;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GoToDungeon);
    if (biomeGenerator.DungeonLocation == FollowerLocation.Dungeon1_5 || biomeGenerator.DungeonLocation == FollowerLocation.Dungeon1_6)
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GoToDLCDungeon);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GoToDLCDungeonFirstTime);
    }
  }

  public void RoomBecameActive(PlayerFarming playerFarming = null)
  {
    BiomeGenerator.BiomeAction onRoomActive = BiomeGenerator.OnRoomActive;
    if (onRoomActive != null)
      onRoomActive();
    PlayerFarming.PositionAllPlayers((UnityEngine.Object) playerFarming == (UnityEngine.Object) null ? PlayerFarming.Instance.transform.position : playerFarming.transform.position, false);
  }

  public IEnumerator DelayActivateRoom(bool applyModifiers)
  {
    BiomeGenerator biomeGenerator = this;
    if (applyModifiers)
    {
      yield return (object) new WaitForEndOfFrame();
      while (PlayerFarming.AnyPlayerGotoAndStopping())
        yield return (object) null;
      for (int i = 0; i < PlayerFarming.playersCount; ++i)
      {
        PlayerFarming playerFarming = PlayerFarming.players[i];
        if (GameManager.InitialDungeonEnter)
        {
          yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.ApplyCurrentFleeceModifiersIE(playerFarming));
          yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.ApplyCurrentDemonModifiersIE(playerFarming));
          yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.ShowHeavyAttackTutorial());
          yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.ShowOmniTutorial());
        }
        yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.ApplyCurrentDungeonModifiersIE(playerFarming));
        playerFarming = (PlayerFarming) null;
      }
    }
    if (CoopManager.CoopActive && PlayerFarming.playersCount == 1)
      CoopManager.Instance.SpawnCoopPlayer(1);
    yield return (object) new WaitForSeconds(1f);
    GameManager.InitialDungeonEnter = false;
    biomeGenerator.CurrentRoom.Active = true;
    BiomeGenerator.BiomeAction onRoomActive = BiomeGenerator.OnRoomActive;
    if (onRoomActive != null)
      onRoomActive();
    biomeGenerator.CurrentRoom.Completed = true;
  }

  public IEnumerator ShowHeavyAttackTutorial()
  {
    bool Loop = false;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.HeavyAttacks))
    {
      Loop = true;
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.HeavyAttacks);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        Loop = false;
        System.Action onTutorialShown = BiomeGenerator.OnTutorialShown;
        if (onTutorialShown == null)
          return;
        onTutorialShown();
      });
    }
    while (Loop)
      yield return (object) null;
  }

  public IEnumerator ShowOmniTutorial()
  {
    if (DataManager.Instance.SurvivalModeActive)
    {
      bool Loop = false;
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Omnipresence))
      {
        Loop = true;
        UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Omnipresence);
        overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => Loop = false);
      }
      while (Loop)
        yield return (object) null;
    }
  }

  public GameObject PlacePlayer()
  {
    bool autoHideOnEnter = false;
    if (DungeonSandboxManager.Active)
    {
      foreach (KeyValuePair<FollowerLocation, LocationManager> locationManager in LocationManager.LocationManagers)
      {
        if ((UnityEngine.Object) locationManager.Value != (UnityEngine.Object) null && GameManager.IsDungeon(locationManager.Key))
          this.DungeonLocation = locationManager.Key;
      }
    }
    else
      PlayerFarming.LastLocation = PlayerFarming.Location;
    if (!LocationManager.LocationManagers.ContainsKey(this.DungeonLocation))
      UnityEngine.Debug.LogError((object) ("Dungeon Location not found in Location Manager: " + this.DungeonLocation.ToString()));
    if ((UnityEngine.Object) LocationManager.LocationManagers[this.DungeonLocation] != (UnityEngine.Object) null)
      PlayerFarming.Location = LocationManager.LocationManagers[this.DungeonLocation].Location;
    else
      UnityEngine.Debug.LogError((object) ("Dungeon Location not found in Location Manager: " + this.DungeonLocation.ToString()));
    if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
    {
      if (DungeonSandboxManager.Active)
      {
        foreach (KeyValuePair<FollowerLocation, LocationManager> locationManager in LocationManager.LocationManagers)
        {
          if ((UnityEngine.Object) locationManager.Value != (UnityEngine.Object) null)
          {
            this.Player = locationManager.Value.PlacePlayer();
            break;
          }
        }
      }
      if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
        this.Player = LocationManager.LocationManagers[this.DungeonLocation].PlacePlayer();
      BiomeGenerator.RevealHudAbilityIcons = true;
    }
    else
    {
      System.Action playerLocationSet = LocationManager.OnPlayerLocationSet;
      if (playerLocationSet != null)
        playerLocationSet();
    }
    if ((UnityEngine.Object) this.Player != (UnityEngine.Object) null)
      this.PlayerState = this.Player.GetComponent<StateMachine>();
    if (this.FirstArrival && this.DoFirstArrivalRoutine)
    {
      PlayerFarming.ResetMainPlayer();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
      GameManager.GetInstance().CameraSetZoom(6f);
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -1f));
      Door entranceDoor = Door.GetEntranceDoor();
      if ((UnityEngine.Object) entranceDoor != (UnityEngine.Object) null)
        this.StartCoroutine((IEnumerator) this.PlayersFirstEnterRoomDelayedGotoAndStop(this.Player, entranceDoor, entranceDoor.PlayerPosition.position + entranceDoor.GetDoorDirection() * 7.3f));
      foreach (Objective_FindRelic objectiveFindRelic in new List<Objective_FindRelic>((IEnumerable<Objective_FindRelic>) ObjectiveManager.GetObjectivesOfType<Objective_FindRelic>()))
      {
        if (objectiveFindRelic.TargetLocation == this.DungeonLocation && !objectiveFindRelic.IsComplete)
        {
          DataManager.Instance.CanFindLeaderRelic = true;
          break;
        }
      }
      this.FirstArrival = false;
      this.DoSpawnDemons();
    }
    else
    {
      if (this.IsTeleporting)
        this.Player.transform.position = (UnityEngine.Object) Interaction_Chest.Instance == (UnityEngine.Object) null ? Vector3.zero : Interaction_Chest.Instance.transform.position;
      else if (this.PrevCurrentX < this.CurrentX && (bool) (UnityEngine.Object) this.West)
      {
        this.Player.transform.position = this.West.PlayerPosition.position;
        this.SetPlayerStateFacingAngle(0.0f);
      }
      else if (this.PrevCurrentX > this.CurrentX && (bool) (UnityEngine.Object) this.East)
      {
        this.Player.transform.position = this.East.PlayerPosition.position;
        this.SetPlayerStateFacingAngle(180f);
      }
      else if (this.PrevCurrentY > this.CurrentY && (bool) (UnityEngine.Object) this.North)
      {
        this.Player.transform.position = this.North.PlayerPosition.position;
        this.SetPlayerStateFacingAngle(270f);
      }
      else if (this.PrevCurrentY < this.CurrentY && (bool) (UnityEngine.Object) this.South)
      {
        this.Player.transform.position = this.South.PlayerPosition.position;
        this.SetPlayerStateFacingAngle(90f);
      }
      else if ((UnityEngine.Object) this.South != (UnityEngine.Object) null)
      {
        UnityEngine.Debug.Log((object) "NO WHERE TO PLACE YOU - so put you south!");
        this.Player.transform.position = this.South.PlayerPosition.position;
        this.SetPlayerStateFacingAngle(90f);
      }
      else
        UnityEngine.Debug.Log((object) "NO WHERE TO PLACE YOU!");
      if (!this.CurrentRoom.Completed)
      {
        foreach (PlayerFarming player in PlayerFarming.players)
        {
          if ((UnityEngine.Object) player != (UnityEngine.Object) null && (UnityEngine.Object) player.playerController != (UnityEngine.Object) null)
            player.playerController.MakeUntouchable(TrinketManager.GetInvincibilityTimeEnteringNewRoom(player));
        }
        int critterCardCount = TrinketManager.CountTrinket(TarotCards.Card.RoomEnterCritter);
        if (critterCardCount > 0)
          GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
          {
            Health.team2.Shuffle<Health>();
            for (int i = Health.team2.Count - 1; i >= 0; i--)
            {
              if ((UnityEngine.Object) Health.team2[i] != (UnityEngine.Object) null && !Health.team2[i].HasShield && (double) Health.team2[i].DamageModifier >= 1.0 && Health.team2[i].CanBeTurnedIntoCritter && (UnityEngine.Object) Health.team2[i].protector == (UnityEngine.Object) null && Health.team2[i].gameObject.activeSelf && (UnityEngine.Object) Health.team2[i].GetComponent<UnitObject>() != (UnityEngine.Object) null && !Health.team2[i].GetComponent<UnitObject>().IsBoss)
              {
                EquipmentManager.GetRelicData(RelicType.RandomEnemyIntoCritter).VFXData.PlayNewSequence(Health.team2[i].transform, new Transform[1]
                {
                  Health.team2[i].transform
                });
                AudioManager.Instance.PlayOneShot("event:/relics/rainbow_bubble", Health.team2[i].gameObject);
                GameManager.GetInstance().WaitForSeconds(0.1f, (System.Action) (() =>
                {
                  if (Health.team2.Count <= i || !((UnityEngine.Object) Health.team2[i] != (UnityEngine.Object) null))
                    return;
                  BiomeGenerator.Instance.CurrentRoom.generateRoom.TurnEnemyIntoCritter(Health.team2[i].GetComponent<UnitObject>());
                  AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/staff_magic", this.gameObject);
                }));
                --critterCardCount;
                if (critterCardCount <= 0)
                  break;
              }
            }
          }));
        bool containsEnemies = false;
        foreach (UnitObject componentsInChild in this.CurrentRoom.generateRoom.GetComponentsInChildren<UnitObject>())
        {
          if (componentsInChild.health.team == Health.Team.Team2)
          {
            containsEnemies = true;
            break;
          }
        }
        autoHideOnEnter = (UnityEngine.Object) this.CurrentRoom.generateRoom.GetComponentInChildren<DungeonLeaderMechanics>() != (UnityEngine.Object) null;
        bool doorsWillClose = true;
        if (RoomLockController.RoomLockControllers.Count > 0)
        {
          RoomLockController roomLockController = RoomLockController.RoomLockControllers[0];
          if (roomLockController.Standalone || roomLockController.Completed)
            doorsWillClose = false;
        }
        for (int index = 0; index < PlayerFarming.playersCount; ++index)
        {
          if (containsEnemies && TrinketManager.HasTrinket(TarotCards.Card.Arrows, PlayerFarming.players[index]))
            PlayerFarming.players[index].GetBlackSoul(Mathf.RoundToInt(PlayerFarming.players[index].playerSpells.faithAmmo.Total - PlayerFarming.players[index].playerSpells.faithAmmo.Ammo), false);
        }
        if (containsEnemies && TrinketManager.HasTrinket(TarotCards.Card.FrostedEnemies))
          GameManager.GetInstance().WaitForSeconds(0.7f, (System.Action) (() => BiomeConstants.Instance.PlayFrostedEnemies(5f)));
        if ((bool) (UnityEngine.Object) PlayerFarming.Instance && containsEnemies | autoHideOnEnter | doorsWillClose && Door.Doors.Count > 0)
        {
          PlayerFarming.ResetMainPlayer();
          PlayerFarming.PositionAllPlayers(PlayerFarming.Instance.transform.position, false);
          PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
          for (int index = 0; index < PlayerFarming.playersCount; ++index)
          {
            PlayerFarming playerFarming = PlayerFarming.players[index];
            Vector3 vector2 = (Vector3) Utils.DegreeToVector2(playerFarming.state.facingAngle);
            playerFarming.transform.position += vector2 * (float) index;
            Vector3 TargetPosition = playerFarming.transform.position + vector2 * (float) (3 + index / 2);
            System.Action action = (System.Action) null;
            if (playerFarming.isLamb)
              action += (System.Action) (() =>
              {
                this.StartCoroutine((IEnumerator) this.DelayActivateRoom());
                if (containsEnemies)
                {
                  BiomeGenerator.BiomeAction enteredCombatRoom = BiomeGenerator.OnBiomeEnteredCombatRoom;
                  if (enteredCombatRoom != null)
                    enteredCombatRoom();
                }
                if (!(containsEnemies | doorsWillClose))
                  return;
                RoomLockController.CloseAll();
              });
            System.Action GoToCallback = action + (System.Action) (() => this.\u003CPlacePlayer\u003Eg__HandleBossHeal\u007C212_3(playerFarming)) + (System.Action) (() => BiomeGenerator.\u003CPlacePlayer\u003Eg__HandleJokerTarot\u007C212_2(playerFarming));
            playerFarming.GoToAndStop(TargetPosition, IdleOnEnd: true, GoToCallback: GoToCallback, maxDuration: 3f, forcePositionOnTimeout: true);
          }
        }
        else if ((bool) (UnityEngine.Object) PlayerFarming.Instance && PlayerFarming.Location != FollowerLocation.Boss_5)
        {
          PlayerFarming.RefreshPlayersCount();
          PlayerFarming.ResetMainPlayer();
          PlayerFarming.PositionAllPlayers(PlayerFarming.Instance.transform.position, false);
          for (int index = 0; index < PlayerFarming.playersCount; ++index)
          {
            PlayerFarming playerFarming = PlayerFarming.players[index];
            Vector3 vector2 = (Vector3) Utils.DegreeToVector2(playerFarming.state.facingAngle);
            Vector3 TargetPosition = playerFarming.transform.position + vector2;
            playerFarming.GoToAndStop(TargetPosition, IdleOnEnd: true, GoToCallback: (System.Action) (() =>
            {
              if (playerFarming.isLamb)
                this.StartCoroutine((IEnumerator) this.DelayActivateRoom());
              this.\u003CPlacePlayer\u003Eg__HandleBossHeal\u007C212_3(playerFarming);
              BiomeGenerator.\u003CPlacePlayer\u003Eg__HandleJokerTarot\u007C212_2(playerFarming);
            }), maxDuration: 5f, forcePositionOnTimeout: true);
          }
        }
        else
        {
          for (int index = 0; index < PlayerFarming.playersCount; ++index)
          {
            PlayerFarming player = PlayerFarming.players[index];
            this.\u003CPlacePlayer\u003Eg__HandleBossHeal\u007C212_3(player);
            BiomeGenerator.\u003CPlacePlayer\u003Eg__HandleJokerTarot\u007C212_2(player);
          }
          this.StartCoroutine((IEnumerator) this.DelayActivateRoom());
        }
      }
      else
      {
        Vector3 TargetPosition = this.Player.transform.position + (Vector3) Utils.DegreeToVector2(this.PlayerState.facingAngle);
        PlayerFarming.PositionAllPlayers(PlayerFarming.Instance.transform.position, false);
        PlayerFarming.Instance?.GoToAndStop(TargetPosition, IdleOnEnd: true, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DelayActivateRoom())), forcePositionOnTimeout: true, groupAction: true);
      }
    }
    if ((bool) (UnityEngine.Object) this.Player)
      this.IsTeleporting = false;
    this.PrevCurrentX = this.CurrentX;
    this.PrevCurrentY = this.CurrentY;
    GameManager.GetInstance().CameraSnapToPosition(this.Player.transform.position);
    GameManager.GetInstance().AddPlayerToCamera();
    if (SettingsManager.Settings.Game.PerformanceMode && (UnityEngine.Object) Camera.main != (UnityEngine.Object) null)
      Camera.main.depthTextureMode = DepthTextureMode.None;
    CompanionCrusade.ResetCompanions(autoHideOnEnter);
    return this.Player;
  }

  public void SetPlayerStateFacingAngle(float angle)
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      player.state.facingAngle = angle;
      player.state.LookAngle = angle;
    }
  }

  public IEnumerator DelayActivateRoom()
  {
    yield return (object) new WaitForSeconds(0.5f);
    BiomeGenerator.Instance.CurrentRoom.Active = true;
    yield return (object) new WaitForSeconds(2f);
    if (CoopManager.CoopActive && PlayerFarming.playersCount == 1)
    {
      for (int slot = 0; slot < CoopManager.AllPlayerGameObjects.Length; ++slot)
      {
        if ((UnityEngine.Object) CoopManager.AllPlayerGameObjects[slot] != (UnityEngine.Object) null)
        {
          PlayerFarming component = CoopManager.AllPlayerGameObjects[slot].GetComponent<PlayerFarming>();
          if (!PlayerFarming.players.Contains(component))
            CoopManager.Instance.SpawnCoopPlayer(slot);
        }
      }
    }
  }

  public void DoSpawnDemons(bool force = false)
  {
    if (this.spawnedDemons)
    {
      for (int index = 0; index < DataManager.Instance.Followers_Demons_Types.Count; ++index)
      {
        if (DataManager.Instance.Followers_Demons_Types[index] == 13)
        {
          if (Demon_Shooty.RotDemons.Count < 3)
          {
            this.SpawnDemon(13, -1, FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index]).XPLevel, true, PlayerFarming.Instance.gameObject, (Action<Demon>) (demon =>
            {
              AudioManager.Instance.PlayOneShot("event:/dlc/tarot/rotdemon_summon_trigger", demon.transform.position);
              demon.SetMaster(PlayerFarming.Instance.gameObject);
            }));
            break;
          }
          break;
        }
      }
    }
    if (DungeonSandboxManager.Active || !this.spawnDemons || this.spawnedDemons && !force || PlayerFarming.Location == FollowerLocation.IntroDungeon)
      return;
    this.spawnedDemons = true;
    int index1 = -1;
    while (++index1 < DataManager.Instance.Followers_Demons_IDs.Count)
      this.SpawnDemon(DataManager.Instance.Followers_Demons_Types[index1], DataManager.Instance.Followers_Demons_IDs[index1]);
  }

  public void SpawnDemon(
    int type,
    int followerID,
    int forcedLevel = -1,
    bool doEffects = false,
    GameObject player = null,
    Action<Demon> spawnedDemon = null)
  {
    List<string> stringList = new List<string>();
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Shooty.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Chomp.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Arrows.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Collector.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Exploder.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Spirit.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Baal.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Aym.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Leshy.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Heket.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Kallamar.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Shamura.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_ChosenChild.prefab");
    stringList.Add("Assets/Prefabs/Enemies/Demons/Demon_Rot.prefab");
    if ((UnityEngine.Object) player == (UnityEngine.Object) null)
      player = this.Player;
    Addressables_wrapper.InstantiateAsync((object) stringList[type], player.transform.position, Quaternion.identity, player.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Demon component = obj.Result.GetComponent<Demon>();
      obj.Result.transform.position = player.transform.position;
      component.Init(followerID, forcedLevel);
      if (doEffects && component is Demon_Spirit demonSpirit2)
        PlayerFarming.Instance.health.TotalSpiritHearts += Mathf.Ceil((float) demonSpirit2.Level / 2f);
      Action<Demon> action = spawnedDemon;
      if (action == null)
        return;
      action(component);
    }));
  }

  public void ApplyCurrentDungeonModifiers(PlayerFarming playerFarming)
  {
    this.StartCoroutine((IEnumerator) this.ApplyCurrentDungeonModifiersIE(playerFarming));
  }

  public IEnumerator ApplyCurrentDungeonModifiersIE(PlayerFarming playerFarming)
  {
    BiomeGenerator biomeGenerator = this;
    HealthPlayer health = playerFarming.health;
    if (DataManager.Instance.PlayerFleece == 7)
    {
      playerFarming.RedHeartsTemporarilyRemoved += (int) ((double) playerFarming.health.PLAYER_HEALTH - (double) PlayerFleeceManager.OneHitKillHP);
      playerFarming.health.totalHP = PlayerFleeceManager.OneHitKillHP;
      health.HP = PlayerFleeceManager.OneHitKillHP;
      health.BlackHearts = 0.0f;
      health.BlueHearts = 0.0f;
    }
    else if (DungeonModifier.HasNeutralModifier(DungeonNeutralModifier.LoseRedGainBlackHeart))
    {
      playerFarming.RedHeartsTemporarilyRemoved += 2;
      health.totalHP -= 2f;
      health.BlackHearts += 4f;
    }
    else if (DungeonModifier.HasNeutralModifier(DungeonNeutralModifier.LoseRedGainTarot))
    {
      playerFarming.RedHeartsTemporarilyRemoved += 2;
      health.totalHP -= 2f;
      if (health.IsNoHeartSlotsLeft)
        health.DealDamage(1f, playerFarming.gameObject, playerFarming.transform.position, false, Health.AttackTypes.Melee, true, (Health.AttackFlags) 0);
      else
        yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.DoTarotRoutine(Vector3.zero, Vector3.one, playerFarming));
    }
  }

  public IEnumerator ApplyCurrentFleeceModifiersIE(PlayerFarming playerFarming)
  {
    if (!this.playersWithAppliedFleeceModifiers.Contains(playerFarming))
    {
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_BlackHeart))
        playerFarming.health.BlackHearts += 2f;
      if (DataManager.Instance.PlayerFleece == 5)
      {
        if ((int) playerFarming.health.PLAYER_HEALTH <= 0)
        {
          if (playerFarming.RedHeartsTemporarilyRemoved > 0)
          {
            playerFarming.health.BlueHearts = (float) playerFarming.RedHeartsTemporarilyRemoved * 1.5f;
            playerFarming.health.totalHP = 0.0f;
          }
          else
          {
            playerFarming.health.BlueHearts = PlayerFarming.Instance.health.BlueHearts;
            playerFarming.health.totalHP = 0.0f;
          }
        }
        else
        {
          playerFarming.RedHeartsTemporarilyRemoved += (int) playerFarming.health.PLAYER_HEALTH;
          playerFarming.health.BlueHearts += playerFarming.health.PLAYER_HEALTH * 1.5f;
          playerFarming.health.totalHP -= playerFarming.health.PLAYER_HEALTH;
        }
      }
      else if (DataManager.Instance.PlayerFleece == 2)
      {
        playerFarming.RedHeartsTemporarilyRemoved += (playerFarming.health.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEARTS_LEVEL) / 2;
        playerFarming.health.totalHP -= (float) ((playerFarming.health.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEARTS_LEVEL) / 2);
      }
      else if (DataManager.Instance.PlayerFleece == 678)
      {
        for (int index = 0; index < 3; ++index)
          Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Familiars/Ghost Familiar.prefab", PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle, Quaternion.identity, PlayerFarming.Instance.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (o =>
          {
            Familiar component = o.Result.GetComponent<Familiar>();
            component.SetMaster(PlayerFarming.Instance);
            component.Container.transform.localScale = Vector3.zero;
          }));
      }
      if (PlayerFleeceManager.GetFreeTarotCards() > 0)
        yield return (object) this.DoFleeceTarotRoutine(playerFarming);
      this.playersWithAppliedFleeceModifiers.Add(playerFarming);
    }
  }

  public IEnumerator ApplyCurrentDemonModifiersIE(PlayerFarming playerFarming)
  {
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      if ((bool) (UnityEngine.Object) demon.GetComponent<Demon_Spirit>() && !PlayerFleeceManager.FleeceNoRedHeartsToUse())
      {
        Demon_Spirit component = demon.GetComponent<Demon_Spirit>();
        playerFarming.health.TotalSpiritHearts += Mathf.Ceil((float) component.Level / 2f);
      }
    }
    yield return (object) new WaitForEndOfFrame();
  }

  public IEnumerator DoFleeceTarotRoutine(PlayerFarming playerFarming)
  {
    BiomeGenerator biomeGenerator = this;
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.facingAngle = -90f;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: playerFarming);
    yield return (object) new WaitForSecondsRealtime(0.35f);
    HUD_Manager.Instance.Hide(false, 0);
    LetterBox.Show(false);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", biomeGenerator.gameObject);
    playerFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    TarotCards.TarotCard drawnCard1 = (TarotCards.TarotCard) null;
    TarotCards.TarotCard drawnCard2 = (TarotCards.TarotCard) null;
    TarotCards.TarotCard drawnCard3 = (TarotCards.TarotCard) null;
    TarotCards.TarotCard drawnCard4 = (TarotCards.TarotCard) null;
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse())
    {
      drawnCard1 = new TarotCards.TarotCard(TarotCards.Card.BlackSoulAutoRecharge, 1);
      drawnCard2 = new TarotCards.TarotCard(TarotCards.Card.IncreaseBlackSoulsDrop, 1);
      drawnCard3 = new TarotCards.TarotCard(TarotCards.Card.AmmoEfficient, 1);
      drawnCard4 = new TarotCards.TarotCard(TarotCards.Card.Arrows, 0);
      TrinketManager.AddEncounteredTrinket(drawnCard1, playerFarming);
      TrinketManager.AddEncounteredTrinket(drawnCard2, playerFarming);
      TrinketManager.AddEncounteredTrinket(drawnCard3, playerFarming);
      TrinketManager.AddEncounteredTrinket(drawnCard4, playerFarming);
    }
    else
    {
      drawnCard1 = TarotCards.DrawRandomCard(playerFarming);
      TrinketManager.AddEncounteredTrinket(drawnCard1, playerFarming);
      drawnCard2 = TarotCards.DrawRandomCard(playerFarming);
      TrinketManager.AddEncounteredTrinket(drawnCard2, playerFarming);
      drawnCard3 = TarotCards.DrawRandomCard(playerFarming);
      TrinketManager.AddEncounteredTrinket(drawnCard3, playerFarming);
      drawnCard4 = TarotCards.DrawRandomCard(playerFarming);
      TrinketManager.AddEncounteredTrinket(drawnCard4, playerFarming);
    }
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
    UIFleeceTarotRewardOverlayController tarotRewardOverlay = MonoSingleton<UIManager>.Instance.ShowFleeceTarotReward(drawnCard1, drawnCard2, drawnCard3, drawnCard4);
    UIFleeceTarotRewardOverlayController overlayController = tarotRewardOverlay;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => tarotRewardOverlay = (UIFleeceTarotRewardOverlayController) null);
    while ((UnityEngine.Object) tarotRewardOverlay != (UnityEngine.Object) null)
      yield return (object) null;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", biomeGenerator.gameObject);
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
    if (drawnCard1 != null)
      TrinketManager.AddTrinket(drawnCard1, playerFarming);
    if (drawnCard2 != null)
      TrinketManager.AddTrinket(drawnCard2, playerFarming);
    if (drawnCard3 != null)
      TrinketManager.AddTrinket(drawnCard3, playerFarming);
    if (drawnCard4 != null)
      TrinketManager.AddTrinket(drawnCard4, playerFarming);
  }

  public IEnumerator DoTarotRoutine(Vector3 position, Vector3 scale, PlayerFarming playerFarming)
  {
    BiomeGenerator biomeGenerator = this;
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.facingAngle = -90f;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) playerFarming)
        player.state.CURRENT_STATE = StateMachine.State.InActive;
    }
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", biomeGenerator.gameObject);
    playerFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    TarotCards.TarotCard drawnCard = TarotCards.DrawRandomCard(playerFarming);
    if (drawnCard == null)
    {
      for (int x = 0; x < 25; ++x)
      {
        AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", biomeGenerator.gameObject);
        CameraManager.shakeCamera(UnityEngine.Random.Range(0.4f, 0.6f));
        PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, playerFarming.transform.position + Vector3.back, 0.0f);
        pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        pickUp.MagnetDistance = 3f;
        pickUp.CanStopFollowingPlayer = false;
        yield return (object) new WaitForSeconds(0.01f);
      }
      yield return (object) new WaitForSeconds(1.5f);
      biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.BackToIdleRoutine(drawnCard, playerFarming));
    }
    else
    {
      TrinketManager.AddEncounteredTrinket(drawnCard, playerFarming);
      yield return (object) new WaitForSeconds(1f);
      GameManager.GetInstance().CameraSetTargetZoom(6f);
      bool waiting = true;
      GameObject gameObject = UITrinketCards.Play(drawnCard, (System.Action) (() => waiting = false));
      if ((bool) (UnityEngine.Object) gameObject.GetComponent<MMButton>())
        gameObject.GetComponent<MMButton>().enabled = false;
      gameObject.transform.localScale = scale;
      ((RectTransform) gameObject.transform).anchoredPosition = (Vector2) position;
      while (waiting)
        yield return (object) null;
      biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.BackToIdleRoutine(drawnCard, playerFarming));
    }
  }

  public IEnumerator BackToIdleRoutine(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    BiomeGenerator biomeGenerator = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", biomeGenerator.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    playerFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    if (card != null)
      GameManager.GetInstance().StartCoroutine((IEnumerator) biomeGenerator.DelayEffectsRoutine(card, playerFarming));
  }

  public void PlaceLoreTotems()
  {
    List<BiomeRoom> biomeRoomList = new List<BiomeRoom>();
    foreach (BiomeRoom room in this.Rooms)
    {
      if (!room.IsCustom && !room.IsBoss && !room.IsDeathCatRoom && !room.IsYngyaRoom)
        biomeRoomList.Add(room);
    }
    if (biomeRoomList.Count <= 0)
      return;
    BiomeRoom biomeRoom1 = (BiomeRoom) null;
    foreach (BiomeRoom biomeRoom2 in biomeRoomList)
    {
      if (!this.CriticalPath.Contains(biomeRoom2) && biomeRoom2.NumConnections == 1 && biomeRoom2.S_Room.Connected)
      {
        biomeRoom1 = biomeRoom2;
        break;
      }
    }
    if (biomeRoom1 == null)
      return;
    biomeRoom1.IsCustom = true;
    biomeRoom1.GameObjectPath = "Assets/_Rooms/Reward Room Lore Stone.prefab";
    biomeRoom1.Hidden = true;
    this.spawnedLoreTotemsThisDungeon = true;
    biomeRoomList.Remove(biomeRoom1);
    foreach (BiomeRoom room in this.Rooms)
    {
      if (room.E_Room != null && room.E_Room.Room != null && room.E_Room.Room.GameObjectPath == biomeRoom1.GameObjectPath && room.E_Room.ConnectionType == GenerateRoom.ConnectionTypes.True)
      {
        room.E_Room.ConnectionType = GenerateRoom.ConnectionTypes.LoreStoneRoom;
        this.LoreTotemRooms.Add(room);
      }
      if (room.N_Room != null && room.N_Room.Room != null && room.N_Room.Room.GameObjectPath == biomeRoom1.GameObjectPath && room.N_Room.ConnectionType == GenerateRoom.ConnectionTypes.True)
      {
        room.N_Room.ConnectionType = GenerateRoom.ConnectionTypes.LoreStoneRoom;
        this.LoreTotemRooms.Add(room);
      }
      if (room.S_Room != null && room.S_Room.Room != null && room.S_Room.Room.GameObjectPath == biomeRoom1.GameObjectPath && room.S_Room.ConnectionType == GenerateRoom.ConnectionTypes.True)
      {
        room.S_Room.ConnectionType = GenerateRoom.ConnectionTypes.LoreStoneRoom;
        this.LoreTotemRooms.Add(room);
      }
      if (room.W_Room != null && room.W_Room.Room != null && room.W_Room.Room.GameObjectPath == biomeRoom1.GameObjectPath && room.W_Room.ConnectionType == GenerateRoom.ConnectionTypes.True)
      {
        room.W_Room.ConnectionType = GenerateRoom.ConnectionTypes.LoreStoneRoom;
        this.LoreTotemRooms.Add(room);
      }
    }
  }

  public void SetOverrideWeather()
  {
    if (this.weatherType == WeatherSystemController.WeatherType.None)
      return;
    WeatherSystemController.Instance.SetWeather(this.weatherType, this.weatherStrength, 0.0f);
  }

  public IEnumerator DelayEffectsRoutine(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(card, playerFarming);
  }

  public void GetDoors()
  {
    this.North = GameObject.FindGameObjectWithTag("North Door")?.GetComponent<Door>();
    this.East = GameObject.FindGameObjectWithTag("East Door")?.GetComponent<Door>();
    this.South = GameObject.FindGameObjectWithTag("South Door")?.GetComponent<Door>();
    this.West = GameObject.FindGameObjectWithTag("West Door")?.GetComponent<Door>();
    if (this.CurrentRoom.N_Room != null)
      this.North?.Init(this.CurrentRoom.N_Room.ConnectionType);
    if (this.CurrentRoom.E_Room != null)
      this.East?.Init(this.CurrentRoom.E_Room.ConnectionType);
    if (this.CurrentRoom.S_Room != null)
      this.South?.Init(this.CurrentRoom.S_Room.ConnectionType);
    if (this.CurrentRoom.W_Room == null)
      return;
    this.West?.Init(this.CurrentRoom.W_Room.ConnectionType);
  }

  public IEnumerator PlayDLCVideoIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BiomeGenerator biomeGenerator = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    AudioManager.Instance.StopCurrentMusic();
    GameManager.GetInstance().OnConversationNew();
    HUD_Manager.Instance.Hide(true);
    MMVideoPlayer.Play("DLC_Intro", new System.Action(biomeGenerator.VideoComplete), MMVideoPlayer.Options.DISABLE, MMVideoPlayer.Options.DISABLE, false);
    AudioManager.Instance.PlayOneShot("event:/music/intro/dlc_intro_video");
    MMTransition.ResumePlay();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void VideoComplete()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) MMVideoPlayer.Instance.gameObject);
    MMVideoPlayer.Instance = (GameObject) null;
    SeasonsManager.ActivateSeasons();
    DataManager.Instance.OnboardedWolf = true;
    DataManager.Instance.ForeshadowedWolf = true;
    DataManager.Instance.RevealDLCDungeonNode = true;
    DataManager.Instance.SpokeToYngyaOnMountainTop = true;
    AudioManager.Instance.StartMusic();
    PlayerFarming.PositionAllPlayers(new Vector3(-0.325f, -4f, 0.0f));
    GameManager.GetInstance().OnConversationEnd();
    Color white = Color.white with { a = 0.65f };
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme, 0.0f);
    WeatherSystemController.Instance.ShowBlizzardOverlay(white, 0.0f);
  }

  public void Left() => BiomeGenerator.ChangeRoom(new Vector2Int(-1, 0));

  public void Right() => BiomeGenerator.ChangeRoom(new Vector2Int(1, 0));

  public void Up() => BiomeGenerator.ChangeRoom(new Vector2Int(0, 1));

  public void Down() => BiomeGenerator.ChangeRoom(new Vector2Int(0, -1));

  [CompilerGenerated]
  public static IEnumerator \u003CSpawnPoisonsInRoom\u003Eg__SpawnPoison\u007C191_0(
    Vector3 position,
    Transform parent,
    float delay,
    PlayerFarming playerFarming,
    float damageMultiplier)
  {
    yield return (object) new WaitForSeconds(delay);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_spawn", position);
    TrapPoison.CreatePoison(position, 5, 0.1f, parent);
    AudioManager.Instance.PlayOneShot("event:/player/poison_damage", position);
  }

  [CompilerGenerated]
  public static void \u003CPlacePlayer\u003Eg__HandleJokerTarot\u007C212_2(PlayerFarming player)
  {
    if (!TrinketManager.HasTrinket(TarotCards.Card.Joker, player) || PlayerFleeceManager.FleecePreventsHealthPickups() || Health.team2.Count <= 0)
      return;
    BiomeConstants.Instance.ShowTarotCardDamage(player.transform, Vector3.up);
    bool willDealDamage = (double) UnityEngine.Random.value < 0.5;
    AudioManager.Instance.PlayOneShot(willDealDamage ? "event:/dlc/tarot/joker_trigger" : "event:/dlc/tarot/joker_trigger_heal", player.transform.position);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      if (willDealDamage)
      {
        player.health.DealDamage(1f, player.gameObject, player.transform.position, false, Health.AttackTypes.Melee, false, (Health.AttackFlags) 0);
      }
      else
      {
        player.health.Heal((float) TrinketManager.GetHealthAmountMultiplier(player));
        AudioManager.Instance.PlayOneShot("event:/player/collect_heart", player.transform.position);
        BiomeConstants.Instance.EmitHeartPickUpVFX(player.transform.position, 0.0f, "red", "burst_small");
      }
    }));
  }

  [CompilerGenerated]
  public void \u003CPlacePlayer\u003Eg__HandleBossHeal\u007C212_3(PlayerFarming player)
  {
    if (!TrinketManager.HasTrinket(TarotCards.Card.BossHeal, player) || !this.CurrentRoom.IsBoss || PlayerFleeceManager.FleecePreventsHealthPickups())
      return;
    player.health.FullHeal();
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", player.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(player.transform.position, 0.0f, "red", "burst_big", false);
  }

  [CompilerGenerated]
  public void \u003CPlacePlayer\u003Eb__212_10()
  {
    this.StartCoroutine((IEnumerator) this.DelayActivateRoom());
  }

  public delegate void GetKey();

  public delegate void BiomeAction();

  [Serializable]
  public class OverrideRoom
  {
    public int x;
    public int y;
    public GenerateRoom.ConnectionTypes North;
    public GenerateRoom.ConnectionTypes East;
    public GenerateRoom.ConnectionTypes South;
    public GenerateRoom.ConnectionTypes West;
    public BiomeGenerator.FixedRoom.Generate Generated = BiomeGenerator.FixedRoom.Generate.DontGenerate;
    public string PrefabPath;
    public bool RoomActive = true;
  }

  public class RoomEntranceExit
  {
    public BiomeRoom Room;

    public RoomEntranceExit(BiomeRoom Room, bool Create) => this.Room = Room;
  }

  [Serializable]
  public class ListOfStoryRooms
  {
    public List<BiomeGenerator.RoomAndStoryPosition> Rooms = new List<BiomeGenerator.RoomAndStoryPosition>();
    public DataManager.Variables StoryVariable;
    public DataManager.Variables LastRun;
    public DataManager.Variables DungeonBeaten;
    public bool PutOnCriticalPath = true;
  }

  [Serializable]
  public class RoomAndStoryPosition
  {
    public int StoryCountRequirement;
    public string RoomPath;
    public bool FloorOne = true;
    public bool FloorTwo = true;
    public bool FloorThree = true;
  }

  [Serializable]
  public class ListOfCustomRoomPrefabs
  {
    public List<string> RoomPaths;
    public bool UseStaticRoomList;
    public static List<string> StaticRoomList = new List<string>()
    {
      "Assets/_Rooms/Reward Room Gold Rare.prefab",
      "Assets/_Rooms/Special Blood Sacrafice.prefab",
      "Assets/_Rooms/Special Coin Gamble.prefab",
      "Assets/_Rooms/Special Free Tarot Cards.prefab",
      "Assets/_Rooms/Special Free Health.prefab",
      "Assets/_Rooms/Special Secret Room 1.prefab"
    };
    public bool PutOnCriticalPath;
    public bool RemoveIfNoNonCriticalPath;
    [Range(0.0f, 1f)]
    public float Probability = 1f;
    public bool Redecorate = true;
    public List<BiomeGenerator.VariableAndCondition> ConditionalVariables = new List<BiomeGenerator.VariableAndCondition>();
    public bool MinimumRun;
    public int MinimumRunNumber;
    public bool MaximumRun;
    public int MaximumRunNumber;
    public bool MinimumFollowerCount;
    public int MinimumFollowerCountNumber;
    public bool MaximumFollowerCount;
    public int MaximumFollowerCountNumber;
    public bool SetCustomConnectionType;
    public GenerateRoom.ConnectionTypes ConnectionType;
    public List<FollowerLocation> LocationIsUndiscovered = new List<FollowerLocation>();
    public bool LayerOne;
    public bool LayerTwo;
    public bool LayerThree;
    public bool LayerFour;
    public bool LayerFive;
    public bool LayerSix;
    public bool FloorOne = true;
    public bool FloorTwo = true;
    public bool FloorThree = true;

    public bool AvailableOnFoor()
    {
      switch (GameManager.CurrentDungeonFloor)
      {
        case 1:
          return this.FloorOne;
        case 2:
          return this.FloorTwo;
        case 3:
          return this.FloorThree;
        default:
          return false;
      }
    }

    public bool AvailableOnLayer()
    {
      switch (GameManager.CurrentDungeonLayer)
      {
        case 1:
          return this.LayerOne;
        case 2:
          return this.LayerTwo;
        case 3:
          return this.LayerThree;
        case 4:
          return this.LayerFour;
        case 5:
          return this.LayerFive;
        case 6:
          return this.LayerSix;
        default:
          return false;
      }
    }
  }

  [MessagePackObject(false)]
  [Serializable]
  public class VariableAndCondition
  {
    [Key(0)]
    public DataManager.Variables Variable;
    [Key(1)]
    public bool Condition = true;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class VariableAndCount
  {
    [Key(0)]
    public DataManager.Variables Variable;
    [Key(1)]
    public int Count;
  }

  [Serializable]
  public class FixedRoom
  {
    public GameObject prefab;
    public BiomeGenerator.FixedRoom.Generate GenerateRoom;
    public BiomeGenerator.FixedRoom.Connection North;
    public BiomeGenerator.FixedRoom.Connection East;
    public BiomeGenerator.FixedRoom.Connection South;
    public BiomeGenerator.FixedRoom.Connection West;
    [Range(0.0f, 1f)]
    public float Probability = 1f;
    public List<BiomeGenerator.VariableAndCondition> ConditionalVariables = new List<BiomeGenerator.VariableAndCondition>();

    public bool HasOptional
    {
      get
      {
        return this.North == BiomeGenerator.FixedRoom.Connection.Optional || this.East == BiomeGenerator.FixedRoom.Connection.Optional || this.South == BiomeGenerator.FixedRoom.Connection.Optional || this.West == BiomeGenerator.FixedRoom.Connection.Optional;
      }
    }

    public bool HasForcedOn
    {
      get
      {
        return this.North == BiomeGenerator.FixedRoom.Connection.ForceOn || this.East == BiomeGenerator.FixedRoom.Connection.ForceOn || this.South == BiomeGenerator.FixedRoom.Connection.ForceOn || this.West == BiomeGenerator.FixedRoom.Connection.ForceOn;
      }
    }

    public int RequiredConnections
    {
      get
      {
        int requiredConnections = 0;
        if (this.North != BiomeGenerator.FixedRoom.Connection.ForceOff)
          ++requiredConnections;
        if (this.East != BiomeGenerator.FixedRoom.Connection.ForceOff)
          ++requiredConnections;
        if (this.South != BiomeGenerator.FixedRoom.Connection.ForceOff)
          ++requiredConnections;
        if (this.West != BiomeGenerator.FixedRoom.Connection.ForceOff)
          ++requiredConnections;
        return requiredConnections;
      }
    }

    public enum Generate
    {
      GenerateOnLoad,
      DontGenerate,
      GenerateCustomOnLoad,
    }

    public enum Connection
    {
      ForceOn,
      Optional,
      ForceOff,
    }
  }

  public enum Direction
  {
    North,
    East,
    South,
    West,
  }
}
