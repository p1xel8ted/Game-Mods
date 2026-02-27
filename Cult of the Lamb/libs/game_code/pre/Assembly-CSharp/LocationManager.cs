// Decompiled with JetBrains decompiler
// Type: LocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
[DefaultExecutionOrder(-50)]
public abstract class LocationManager : BaseMonoBehaviour
{
  public static LocationManager _Instance;
  [SerializeField]
  private Transform SafeSpawnCheckTransform;
  public bool StartsActive = true;
  public GameObject PlayerPrefab;
  public static System.Action OnFollowersSpawned;
  [SerializeField]
  private BiomeLightingSettings bloodMoonLUT;
  private bool halloweenLutActive;
  public static System.Action OnPlayerLocationSet;
  private int structuresRequirePlacing;
  public static Dictionary<FollowerLocation, LocationManager> LocationManagers = new Dictionary<FollowerLocation, LocationManager>();
  private static Dictionary<FollowerLocation, LocationState> _locationStates = new Dictionary<FollowerLocation, LocationState>();
  private static List<FollowerLocation> _dungeonLocations = new List<FollowerLocation>()
  {
    FollowerLocation.Dungeon1_1,
    FollowerLocation.Dungeon1_2,
    FollowerLocation.Dungeon1_3,
    FollowerLocation.Dungeon1_4,
    FollowerLocation.Dungeon1_5,
    FollowerLocation.Dungeon2_2,
    FollowerLocation.Dungeon2_3,
    FollowerLocation.Dungeon2_4,
    FollowerLocation.Dungeon3_1,
    FollowerLocation.Dungeon3_2,
    FollowerLocation.Dungeon3_3,
    FollowerLocation.Dungeon3_4,
    FollowerLocation.Boss_1,
    FollowerLocation.Boss_2,
    FollowerLocation.Boss_3,
    FollowerLocation.Boss_4,
    FollowerLocation.Boss_5,
    FollowerLocation.BountyRoom1,
    FollowerLocation.BountyRoom2,
    FollowerLocation.BountyRoom3,
    FollowerLocation.BountyRoom4,
    FollowerLocation.BountyRoom5,
    FollowerLocation.IntroDungeon,
    FollowerLocation.None
  };

  public abstract FollowerLocation Location { get; }

  public abstract Transform UnitLayer { get; }

  public virtual bool SupportsStructures => false;

  public List<global::StructuresData> StructuresData
  {
    get => StructureManager.StructuresDataAtLocation(this.Location);
  }

  public virtual Transform StructureLayer => (Transform) null;

  public Vector3 SafeSpawnCheckPosition
  {
    get
    {
      return !((UnityEngine.Object) this.SafeSpawnCheckTransform == (UnityEngine.Object) null) ? this.SafeSpawnCheckTransform.position : Vector3.zero;
    }
  }

  public bool Activatable { get; set; } = true;

  public bool StructuresPlaced { get; private set; }

  public bool FollowersSpawned { get; private set; }

  public System.Random Random { get; private set; }

  protected virtual void Awake()
  {
    LocationManager._Instance = this;
    LocationManager locationManager = (LocationManager) null;
    LocationManager.LocationManagers.TryGetValue(this.Location, out locationManager);
    LocationManager.LocationManagers[this.Location] = this;
  }

  protected virtual void Start()
  {
    if (DataManager.Instance.LocationSeeds.FirstOrDefault<DataManager.LocationSeedsData>((Func<DataManager.LocationSeedsData, bool>) (x => x.Location == this.Location)) == null)
      DataManager.Instance.LocationSeeds.Add(new DataManager.LocationSeedsData()
      {
        Location = this.Location,
        Seed = UnityEngine.Random.Range(-2147483647 /*0x80000001*/, int.MaxValue)
      });
    this.Random = new System.Random(DataManager.Instance.LocationSeeds.FirstOrDefault<DataManager.LocationSeedsData>((Func<DataManager.LocationSeedsData, bool>) (x => x.Location == this.Location)).Seed);
    this.StartCoroutine((IEnumerator) this.LoadLocationRoutine());
    if (!FollowerBrainStats.IsBloodMoon || !((UnityEngine.Object) this.bloodMoonLUT != (UnityEngine.Object) null))
      return;
    this.EnableBloodMoon();
  }

  private IEnumerator LoadLocationRoutine()
  {
    LocationManager locationManager = this;
    LocationManager.BeginLoadLocation(locationManager.Location);
    if (locationManager.SupportsStructures)
    {
      yield return (object) new WaitForEndOfFrame();
      yield return (object) locationManager.StartCoroutine((IEnumerator) locationManager.PlaceStructures());
      locationManager.PostPlaceStructures();
    }
    if (GameManager.IsDungeon(locationManager.Location))
      BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 0.0f);
    else
      BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    locationManager.SpawnFollowers();
    locationManager.FollowersSpawned = true;
    System.Action followersSpawned = LocationManager.OnFollowersSpawned;
    if (followersSpawned != null)
      followersSpawned();
    LocationManager.EndLoadLocation(locationManager.Location);
    if (!locationManager.StartsActive)
      locationManager.gameObject.SetActive(false);
  }

  protected virtual void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.ActivateLocationRoutine());
  }

  private IEnumerator ActivateLocationRoutine()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      PlayerFarming.Instance.transform.SetParent(this.UnitLayer);
    yield return (object) new WaitForEndOfFrame();
    if ((LocationManager.GetLocationState(this.Location) == LocationState.Inactive || LocationManager.GetLocationState(this.Location) == LocationState.Loading) && this.Activatable)
      LocationManager.ActivateLocation(this.Location);
  }

  protected virtual void OnDisable() => LocationManager.DeactivateLocation(this.Location);

  public static void UpdateLocation()
  {
    System.Action playerLocationSet = LocationManager.OnPlayerLocationSet;
    if (playerLocationSet == null)
      return;
    playerLocationSet();
  }

  protected virtual void Update()
  {
    if (!this.halloweenLutActive || FollowerBrainStats.IsBloodMoon)
      return;
    this.DisableBloodMoon();
  }

  private void OnDestroy()
  {
    if (LocationManager.LocationManagers.ContainsKey(this.Location))
      LocationManager.LocationManagers[this.Location] = (LocationManager) null;
    LocationManager.UnloadLocation(this.Location);
    if (!((UnityEngine.Object) LocationManager._Instance == (UnityEngine.Object) this))
      return;
    LocationManager._Instance = (LocationManager) null;
  }

  public GameObject PlacePlayer()
  {
    PlayerFarming.LastLocation = PlayerFarming.Location;
    PlayerFarming.Location = this.Location;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPrefab, this.GetStartPosition(PlayerFarming.LastLocation), Quaternion.identity, this.UnitLayer);
    gameObject.GetComponent<StateMachine>().facingAngle = Utils.GetAngle(gameObject.transform.position, Vector3.zero);
    System.Action playerLocationSet = LocationManager.OnPlayerLocationSet;
    if (playerLocationSet != null)
      playerLocationSet();
    return gameObject;
  }

  public void PositionPlayer()
  {
    PlayerFarming.Instance.transform.position = this.GetStartPosition(PlayerFarming.Location);
  }

  public FollowerRecruit SpawnRecruit(FollowerBrain brain, Vector3 position)
  {
    FollowerRecruit followerRecruit = UnityEngine.Object.Instantiate<FollowerRecruit>(FollowerManager.RecruitPrefab, position, Quaternion.identity, this.UnitLayer);
    followerRecruit.name = "Recruit " + brain.Info.Name;
    followerRecruit.Follower.Init(brain, brain.CreateOutfit());
    followerRecruit.Follower.Spine.transform.localScale = new Vector3(-1f, 1f, 1f);
    return followerRecruit;
  }

  public Follower SpawnFollower(FollowerBrain brain, Vector3 position)
  {
    Follower follower = UnityEngine.Object.Instantiate<Follower>(FollowerManager.FollowerPrefab, position, Quaternion.identity, this.UnitLayer);
    follower.name = "Follower " + brain.Info.Name;
    follower.Init(brain, brain.CreateOutfit());
    return follower;
  }

  public void SpawnFollowers()
  {
    foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation(this.Location))
      simFollower.Retire();
    for (int index = DataManager.Instance.Followers.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers[index] == null || DataManager.Instance.Followers_Dead_IDs.Contains(DataManager.Instance.Followers[index].ID))
        DataManager.Instance.Followers.RemoveAt(index);
    }
    foreach (FollowerInfo follower1 in DataManager.Instance.Followers)
    {
      if (follower1.Location == this.Location)
      {
        bool flag = false;
        foreach (Follower follower2 in Follower.Followers)
        {
          if (follower2.Brain.Info.ID == follower1.ID || DataManager.Instance.Followers_Dead_IDs.Contains(follower1.ID))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          Vector3 startPosition = this.GetStartPosition(this.Location);
          Follower follower3 = this.SpawnFollower(FollowerBrain.GetOrCreateBrain(follower1), startPosition + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
          this.AddFollower(follower3, false);
          follower3.StartTeleportToTransitionPosition();
        }
      }
    }
  }

  public void SpawnFollower(SimFollower simFollower, bool resume = true)
  {
    simFollower.Retire();
    Vector3 position = this.GetStartPosition(simFollower.Brain.Location);
    List<Structures_Missionary> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Missionary>(this.Location);
    if (simFollower.Brain.Location == FollowerLocation.Missionary && structuresOfType1.Count > 0)
      position = structuresOfType1[simFollower.Brain._directInfoAccess.MissionaryIndex].Data.Position;
    List<Structures_Demon_Summoner> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Demon_Summoner>(this.Location);
    if (simFollower.Brain.Location == FollowerLocation.Demon && structuresOfType2.Count > 0)
      position = structuresOfType2[0].Data.Position;
    this.AddFollower(this.SpawnFollower(simFollower.Brain, position), resume);
  }

  public void AddFollower(Follower follower, bool resume = true)
  {
    FollowerLocation location = follower.Brain.Location;
    follower.transform.SetParent(this.UnitLayer);
    if (location != this.Location)
      follower.transform.position = this.GetStartPosition(location);
    else
      follower.transform.position = follower.Brain.LastPosition;
    foreach (FollowerPet followerPet in FollowerPet.FollowerPets)
    {
      if ((UnityEngine.Object) followerPet.Follower == (UnityEngine.Object) follower)
      {
        followerPet.transform.SetParent(this.UnitLayer);
        followerPet.transform.position = follower.transform.position;
        followerPet.gameObject.SetActive(true);
      }
    }
    List<Structures_Missionary> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Missionary>(this.Location);
    if (location == FollowerLocation.Missionary && structuresOfType.Count > 0)
      follower.transform.position = structuresOfType[follower.Brain._directInfoAccess.MissionaryIndex].Data.Position;
    follower.Brain.Location = this.Location;
    FollowerManager.FollowersAtLocation(this.Location).Add(follower);
    if (resume)
      follower.Brain.LastPosition = follower.transform.position;
    FollowerManager.RetireSimFollowerByID(follower.Brain.Info.ID);
    switch (LocationManager.GetLocationState(this.Location))
    {
      case LocationState.Loading:
      case LocationState.Active:
        if (!resume)
          break;
        follower.Resume();
        break;
      default:
        for (int index = FollowerPet.FollowerPets.Count - 1; index >= 0; --index)
        {
          if ((UnityEngine.Object) FollowerPet.FollowerPets[index].Follower == (UnityEngine.Object) follower)
            FollowerPet.FollowerPets[index].gameObject.SetActive(false);
        }
        SimFollower simFollower = new SimFollower(follower.Brain);
        FollowerManager.SimFollowersAtLocation(this.Location).Add(simFollower);
        break;
    }
  }

  protected abstract Vector3 GetStartPosition(FollowerLocation prevLocation);

  public virtual Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    throw new ArgumentException($"Unexpected GetExitPosition(FollowerLocation.{destLocation}) from Location.{this.Location}");
  }

  public IEnumerator PlaceStructures()
  {
    this.CheckExistingStructures();
    this.structuresRequirePlacing = 0;
    for (int index = this.StructuresData.Count - 1; index >= 0; --index)
    {
      global::StructuresData structuresData = this.StructuresData[index];
      if (structuresData.Type != StructureBrain.TYPES.NONE)
      {
        if (!DataManager.Instance.DLC_Cultist_Pack && (DataManager.CultistDLCStructures.Contains(structuresData.Type) || DataManager.CultistDLCStructures.Contains(structuresData.ToBuildType)))
          this.StructuresData.RemoveAt(index);
        else if (structuresData.DontLoadMe)
        {
          bool flag = false;
          foreach (Structure structure in Structure.Structures)
          {
            if (structuresData.Position == structure.transform.position)
            {
              structure.Brain = StructureBrain.GetOrCreateBrain(structuresData);
              structure.Brain.AddToGrid();
              flag = true;
              break;
            }
          }
          if (!flag)
            this.StructuresData.RemoveAt(index);
        }
        else
        {
          if (structuresData.Location == FollowerLocation.None)
          {
            Debug.LogWarning((object) $"Placing Structure {structuresData.Type}.{structuresData.ID} with Location.None, updating to {this.Location}");
            structuresData.Location = this.Location;
          }
          ++this.structuresRequirePlacing;
          this.InstantiateStructureAsync(structuresData);
        }
      }
    }
    float timer = 0.0f;
    while (this.structuresRequirePlacing > 0)
    {
      timer += Time.unscaledDeltaTime;
      if ((double) timer < 15.0)
        yield return (object) null;
      else
        break;
    }
    this.StructuresPlaced = true;
    StructureManager.StructuresPlaced structuresPlaced = StructureManager.OnStructuresPlaced;
    if (structuresPlaced != null)
      structuresPlaced();
  }

  private async void InstantiateStructureAsync(global::StructuresData structuresData)
  {
    if (!structuresData.PrefabPath.Contains("Assets"))
      structuresData.PrefabPath = $"Assets/{structuresData.PrefabPath}.prefab";
    Task<GameObject> operation = Addressables.InstantiateAsync((object) structuresData.PrefabPath).Task;
    GameObject gameObject = await operation;
    --this.structuresRequirePlacing;
    if ((UnityEngine.Object) operation.Result == (UnityEngine.Object) null)
      Debug.Log((object) "STRUCTURE COULDN'T LOAD");
    else
      this.PlaceStructure(structuresData, operation.Result.gameObject);
  }

  public GameObject PlaceStructure(global::StructuresData structure, GameObject g)
  {
    if (!((UnityEngine.Object) PlacementRegion.Instance == (UnityEngine.Object) null) && PlacementRegion.Instance.structureBrain != null)
      PlacementRegion.Instance.structureBrain.AddStructureToGrid(structure);
    StructureBrain.ApplyConfigToData(structure);
    g.transform.parent = this.StructureLayer;
    g.transform.position = structure.Position + structure.Offset;
    g.transform.localScale = new Vector3((float) structure.Direction, g.transform.localScale.y, g.transform.localScale.z);
    Structure structure1 = g.GetComponent<Structure>();
    if ((UnityEngine.Object) structure1 == (UnityEngine.Object) null)
      structure1 = g.GetComponentInChildren<Structure>();
    if ((UnityEngine.Object) structure1 != (UnityEngine.Object) null)
      structure1.Brain = StructureBrain.GetOrCreateBrain(structure);
    WorkPlace component = g.GetComponent<WorkPlace>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.SetID($"{(object) g.transform.position.x}_{(object) g.transform.position.y}");
    return g;
  }

  private void CheckExistingStructures()
  {
    for (int index = Structure.Structures.Count - 1; index >= 0; --index)
    {
      if (Structure.Structures[index].Type == StructureBrain.TYPES.PLACEMENT_REGION)
        this.CheckExistingStructure(Structure.Structures[index]);
    }
    for (int index = Structure.Structures.Count - 1; index >= 0; --index)
    {
      if (Structure.Structures[index].Type != StructureBrain.TYPES.PLACEMENT_REGION)
        this.CheckExistingStructure(Structure.Structures[index]);
    }
  }

  private void CheckExistingStructure(Structure s)
  {
    LocationManager componentInParent = s.GetComponentInParent<LocationManager>();
    if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null && componentInParent.Location != this.Location || s.Structure_Info != null)
      return;
    bool flag = false;
    for (int index = 0; index < this.StructuresData.Count; ++index)
    {
      if (this.StructuresData[index].Position == s.transform.position)
      {
        s.Brain = StructureBrain.GetOrCreateBrain(this.StructuresData[index]);
        flag = true;
        break;
      }
    }
    if (!flag)
      s.CreateStructure(this.Location, s.transform.position);
    if (s.Brain == null || s.Type == StructureBrain.TYPES.PLACEMENT_REGION)
      return;
    using (List<PlacementRegion>.Enumerator enumerator = PlacementRegion.PlacementRegions.GetEnumerator())
    {
      do
        ;
      while (enumerator.MoveNext() && !enumerator.Current.TryPlaceExistingStructureAtWorldPosition(s));
    }
  }

  private bool CanPlaceStructureAtWorldPosition(global::StructuresData s)
  {
    foreach (PlacementRegion placementRegion in PlacementRegion.PlacementRegions)
    {
      PlacementRegion.TileGridTile tileGridTile = placementRegion.GetTileGridTile(s.GridTilePosition);
      if (tileGridTile == null && !s.IgnoreGrid || tileGridTile != null && !tileGridTile.CanPlaceStructure && tileGridTile.ObjectID != -1 && tileGridTile.ObjectID != s.ID && tileGridTile.ObjectOnTile != StructureBrain.TYPES.BUILD_SITE && tileGridTile.ObjectOnTile != StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT)
        return false;
    }
    return true;
  }

  protected virtual void PostPlaceStructures()
  {
  }

  public static IEnumerable<FollowerLocation> LocationsInState(LocationState state)
  {
    for (int i = 0; i < 84; ++i)
    {
      FollowerLocation location = (FollowerLocation) i;
      if (LocationManager.GetLocationState(location) == state)
        yield return location;
    }
  }

  public static LocationState GetLocationState(FollowerLocation location)
  {
    LocationState locationState = LocationState.Unloaded;
    LocationManager._locationStates.TryGetValue(location, out locationState);
    return locationState;
  }

  public static bool IsDungeonActive()
  {
    bool flag = false;
    foreach (FollowerLocation dungeonLocation in LocationManager._dungeonLocations)
    {
      if (LocationManager.GetLocationState(dungeonLocation) == LocationState.Active)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public static bool LocationIsDungeon(FollowerLocation location)
  {
    return LocationManager._dungeonLocations.Contains(location);
  }

  private static void BeginLoadLocation(FollowerLocation location)
  {
    LocationManager._locationStates[location] = LocationState.Loading;
  }

  private static void EndLoadLocation(FollowerLocation location)
  {
    LocationManager._locationStates[location] = LocationState.Active;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.HomeLocation == location && allBrain.CurrentTaskType == FollowerTaskType.FollowPlayer)
      {
        allBrain.FollowingPlayer = false;
        allBrain.CurrentTask.End();
      }
    }
  }

  public static void ActivateLocation(FollowerLocation location)
  {
    LocationManager._locationStates[location] = LocationState.Active;
    foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation(location))
      simFollower.Retire();
    foreach (Follower follower in FollowerManager.FollowersAtLocation(location))
    {
      follower.ClearPath();
      follower.StartTeleportToTransitionPosition();
    }
    PlayerFarming.LastLocation = PlayerFarming.Location;
    PlayerFarming.Location = location;
    TwitchManager.LocationChanged(location);
  }

  public static void DeactivateLocation(FollowerLocation location)
  {
    LocationManager._locationStates[location] = LocationState.Inactive;
    List<Follower> followerList = FollowerManager.FollowersAtLocation(location);
    for (int index = followerList.Count - 1; index >= 0; --index)
    {
      followerList[index].Pause();
      if (followerList[index].Brain.CurrentTaskType == FollowerTaskType.GreetPlayer)
        followerList[index].Brain.CurrentTask.Abort();
      SimFollower simFollower = new SimFollower(followerList[index].Brain);
      simFollower.TransitionFromFollower(followerList[index]);
      FollowerManager.SimFollowersAtLocation(location).Add(simFollower);
    }
  }

  public static void UnloadLocation(FollowerLocation location)
  {
    LocationManager._locationStates[location] = LocationState.Unloaded;
    foreach (Follower follower in FollowerManager.FollowersAtLocation(location))
      ;
    FollowerManager.Followers[location].Clear();
  }

  public void EnableBloodMoon()
  {
    this.halloweenLutActive = true;
    LightingManager.Instance.isTODTransition = true;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.globalOverrideSettings = this.bloodMoonLUT;
    LightingManager.Instance.inGlobalOverride = true;
    LightingManager.Instance.transitionDurationMultiplier = 0.0f;
    LightingManager.Instance.UpdateLighting(false);
  }

  public void DisableBloodMoon()
  {
    this.halloweenLutActive = false;
    LightingManager.Instance.isTODTransition = true;
    LightingManager.Instance.lerpActive = true;
    LightingManager.Instance.globalOverrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.inGlobalOverride = false;
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    LightingManager.Instance.UpdateLighting(false);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.StandardAmbience);
  }
}
