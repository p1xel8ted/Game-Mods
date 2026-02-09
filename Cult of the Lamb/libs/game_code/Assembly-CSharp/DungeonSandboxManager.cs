// Decompiled with JetBrains decompiler
// Type: DungeonSandboxManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Map;
using MessagePack;
using MMRoomGeneration;
using MMTools;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class DungeonSandboxManager : BaseMonoBehaviour
{
  public static DungeonSandboxManager Instance;
  public static global::ScenarioData[] scenarioData;
  public const string ScenarioPath = "Data/Scenario Data";
  public global::ScenarioData currentScenario;
  [SerializeField]
  public DungeonSandboxManager.Dungeon[] dungeons;
  [SerializeField]
  public global::ScenarioData DEBUG_ScenarioData;
  public MMBiomeGeneration.BiomeGenerator biomeGenerator;
  public GenerateRoom roomGenerator;
  public static global::ScenarioData CurrentScenario;
  public static int CurrentFleece;
  [CompilerGenerated]
  public List<FollowerLocation> \u003CBossesCompleted\u003Ek__BackingField = new List<FollowerLocation>();
  public int cachedFleece;
  public int cachedVisualFleece;
  public List<string> EncounteredMiniBosses = new List<string>();

  public static bool Active => (UnityEngine.Object) DungeonSandboxManager.Instance != (UnityEngine.Object) null;

  public static global::ScenarioData[] ScenarioData
  {
    get
    {
      if (DungeonSandboxManager.scenarioData == null)
        DungeonSandboxManager.scenarioData = Resources.LoadAll<global::ScenarioData>("Data/Scenario Data");
      return DungeonSandboxManager.scenarioData;
    }
  }

  public DungeonSandboxManager.ScenarioType CurrentScenarioType
  {
    get => this.currentScenario.ScenarioType;
  }

  public List<FollowerLocation> BossesCompleted
  {
    get => this.\u003CBossesCompleted\u003Ek__BackingField;
    set => this.\u003CBossesCompleted\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    DungeonSandboxManager.Instance = this;
    this.biomeGenerator = this.GetComponentInChildren<MMBiomeGeneration.BiomeGenerator>();
    this.roomGenerator = this.GetComponentInChildren<GenerateRoom>();
    DataManager.Instance.SandboxModeEnabled = true;
    GameManager.CurrentDungeonLayer = 4;
  }

  public void Start()
  {
    if (!((UnityEngine.Object) DungeonSandboxManager.CurrentScenario != (UnityEngine.Object) null))
      return;
    this.LoadScenario(DungeonSandboxManager.CurrentScenario);
  }

  public void OnDestroy()
  {
    TimeManager.PauseGameTime = false;
    DataManager.Instance.PlayerFleece = this.cachedFleece;
    DataManager.Instance.PlayerVisualFleece = this.cachedVisualFleece;
    SimulationManager.UnPause();
    if (PlayerFarming.LastLocation != FollowerLocation.None)
      PlayerFarming.LastLocation = FollowerLocation.Endless;
    DataManager.Instance.SandboxModeEnabled = false;
  }

  public void Update() => SimulationManager.Pause();

  public void LoadScenario(global::ScenarioData data)
  {
    this.currentScenario = data;
    this.cachedFleece = DataManager.Instance.PlayerFleece;
    this.cachedVisualFleece = DataManager.Instance.PlayerVisualFleece;
    DataManager.Instance.PlayerFleece = DungeonSandboxManager.CurrentFleece;
    DataManager.Instance.PlayerVisualFleece = DungeonSandboxManager.CurrentFleece;
    foreach (Vector2 customisedFleeceOption in DataManager.Instance.CustomisedFleeceOptions)
    {
      if ((double) customisedFleeceOption.x == (double) DungeonSandboxManager.CurrentFleece)
      {
        DataManager.Instance.PlayerVisualFleece = (int) customisedFleeceOption.y;
        break;
      }
    }
    MapManager.Instance.DungeonConfig = data.MapConfig;
    MMTransition.ResumePlay();
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap();
    UIAdventureMapOverlayController overlayController = adventureMapOverlayController;
    overlayController.OnShow = overlayController.OnShow + (System.Action) (() => adventureMapOverlayController.NodeFromPoint(adventureMapOverlayController.Map.path.LastElement<Point>()).gameObject.SetActive(false));
    adventureMapOverlayController.Cancellable = false;
  }

  public void SetDungeonType(FollowerLocation location)
  {
    this.roomGenerator.Addr_StartPieces = this.GetIslandPieces(location);
    this.roomGenerator.Addr_DecorationSetList = this.GetDecorationList(location);
    PlayerFarming.Location = location;
    this.biomeGenerator.DungeonLocation = location;
    this.biomeGenerator.BossRoomPath = this.GetMiniBossRoom(location);
    this.biomeGenerator.LeaderRoomPath = this.GetLeaderRoom(location);
    this.biomeGenerator.EntranceRoomPath = this.GetEntranceRoom(location);
    this.biomeGenerator.BossDoorRoomPath = this.GetTempleDoorRoom(location);
    this.biomeGenerator.biomeMusicPath = this.GetBiomeMusicPath(location);
    this.biomeGenerator.biomeAtmosPath = this.GetBiomeAtmosPath(location);
    DungeonSandboxManager.Dungeon dungeon = this.GetDungeon(location);
    LightingManager.Instance.dawnSettings = dungeon.AfternoonSettings;
    LightingManager.Instance.morningSettings = dungeon.AfternoonSettings;
    LightingManager.Instance.afternoonSettings = dungeon.AfternoonSettings;
    LightingManager.Instance.duskSettings = dungeon.AfternoonSettings;
    LightingManager.Instance.nightSettings = dungeon.NightSettings;
    LightingManager.Instance.PrepareLightingSettings();
    if ((UnityEngine.Object) LocationManager._Instance != (UnityEngine.Object) null)
      ((DungeonLocationManager) LocationManager._Instance)._location = location;
    LocationManager.UpdateLocation();
  }

  public void UpdateDungeonType()
  {
    FollowerLocation location = (FollowerLocation) (7 + this.BossesCompleted.Count);
    if (this.BossesCompleted.Count >= 4)
      location = FollowerLocation.Dungeon1_4;
    this.SetDungeonType(location);
  }

  public DungeonSandboxManager.Dungeon GetDungeon(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon;
    }
    return this.dungeons[0];
  }

  public List<AssetReferenceGameObject> GetDecorationList(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.Addr_Decorations;
    }
    return new List<AssetReferenceGameObject>();
  }

  public List<AssetReferenceGameObject> GetIslandPieces(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.Addr_IslandPieces;
    }
    return new List<AssetReferenceGameObject>();
  }

  public string GetMiniBossRoom(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.MiniBossRooms[UnityEngine.Random.Range(0, dungeon.MiniBossRooms.Length)];
    }
    return this.dungeons[0].MiniBossRooms[UnityEngine.Random.Range(0, this.dungeons[0].MiniBossRooms.Length)];
  }

  public string GetLeaderRoom(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.LeaderRooms;
    }
    return this.dungeons[0].LeaderRooms;
  }

  public string GetEntranceRoom(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.EntranceRoom;
    }
    return this.dungeons[0].EntranceRoom;
  }

  public string GetTempleDoorRoom(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.TempleDoorRoom;
    }
    return this.dungeons[0].TempleDoorRoom;
  }

  public string GetBiomeMusicPath(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.BiomeMusicPath;
    }
    return this.dungeons[0].BiomeMusicPath;
  }

  public string GetBiomeAtmosPath(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.BiomeAtmosPath;
    }
    return this.dungeons[0].BiomeAtmosPath;
  }

  public static List<global::ScenarioData> GetDataForScenarioType(
    DungeonSandboxManager.ScenarioType scenarioType)
  {
    List<global::ScenarioData> dataForScenarioType = new List<global::ScenarioData>();
    foreach (global::ScenarioData scenarioData in DungeonSandboxManager.ScenarioData)
    {
      if (scenarioData.ScenarioType == scenarioType)
        dataForScenarioType.Add(scenarioData);
    }
    ((IList<global::ScenarioData>) DungeonSandboxManager.scenarioData).Sort<global::ScenarioData>((Comparison<global::ScenarioData>) ((x, y) => x.ScenarioIndex.CompareTo(y.ScenarioIndex)));
    return dataForScenarioType;
  }

  public static DungeonSandboxManager.ProgressionSnapshot GetProgressionForScenario(
    DungeonSandboxManager.ScenarioType scenarioType,
    PlayerFleeceManager.FleeceType fleeceType)
  {
    foreach (DungeonSandboxManager.ProgressionSnapshot progressionForScenario in DataManager.Instance.SandboxProgression)
    {
      if (scenarioType == progressionForScenario.ScenarioType && fleeceType == progressionForScenario.FleeceType)
        return progressionForScenario;
    }
    DungeonSandboxManager.ProgressionSnapshot progressionForScenario1 = new DungeonSandboxManager.ProgressionSnapshot()
    {
      ScenarioType = scenarioType,
      FleeceType = fleeceType
    };
    DataManager.Instance.SandboxProgression.Add(progressionForScenario1);
    return progressionForScenario1;
  }

  public static int GetCompletedRunCount()
  {
    int completedRunCount = 0;
    foreach (DungeonSandboxManager.ProgressionSnapshot progressionSnapshot in DataManager.Instance.SandboxProgression)
      completedRunCount += progressionSnapshot.CompletedRuns;
    return completedRunCount;
  }

  public static bool HasFinishedAnyWithDefaultFleece()
  {
    foreach (DungeonSandboxManager.ProgressionSnapshot progressionSnapshot in DataManager.Instance.SandboxProgression)
    {
      if (progressionSnapshot.FleeceType == PlayerFleeceManager.FleeceType.Default)
        return progressionSnapshot.CompletedRuns > 0;
    }
    return false;
  }

  public enum ScenarioType
  {
    DungeonMode,
    BossRushMode,
  }

  [Flags]
  public enum ScenarioModifier
  {
    None = 0,
    HarderEnemies = 1,
    NoHealthDrops = 2,
    NoSpecialAttacks = 4,
  }

  [Serializable]
  public struct Dungeon
  {
    public FollowerLocation Location;
    public List<AssetReferenceGameObject> Addr_Decorations;
    public List<AssetReferenceGameObject> Addr_IslandPieces;
    public string[] MiniBossRooms;
    public string LeaderRooms;
    public string EntranceRoom;
    public string TempleDoorRoom;
    [Space]
    public BiomeLightingSettings AfternoonSettings;
    public BiomeLightingSettings NightSettings;
    public BiomeLightingSettings BossSettings;
    [Space]
    public string BiomeMusicPath;
    public string BiomeAtmosPath;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class ProgressionSnapshot
  {
    [Key(0)]
    public DungeonSandboxManager.ScenarioType ScenarioType;
    [Key(1)]
    public PlayerFleeceManager.FleeceType FleeceType;
    [Key(2)]
    public int CompletedRuns;
    [Key(3)]
    public int CompletionSeen;
  }
}
