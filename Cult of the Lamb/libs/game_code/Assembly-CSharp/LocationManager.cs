// Decompiled with JetBrains decompiler
// Type: LocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
[DefaultExecutionOrder(-50)]
public abstract class LocationManager : BaseMonoBehaviour
{
  public static LocationManager _Instance;
  public static List<FollowerLocation> IndoorLocations = new List<FollowerLocation>()
  {
    FollowerLocation.Church,
    FollowerLocation.Hub1_RatauInside,
    FollowerLocation.HubShore_Lighthouse,
    FollowerLocation.Sozo_Cave,
    FollowerLocation.Blacksmith_Inside,
    FollowerLocation.TarotShop_Inside,
    FollowerLocation.DecorationShop_Inside,
    FollowerLocation.Flockade_Inside,
    FollowerLocation.Dead_Whale_Inside,
    FollowerLocation.Dungeon_Location_3,
    FollowerLocation.Boss_Yngya,
    FollowerLocation.Yngya_Room
  };
  public static List<FollowerLocation> DLCShrineRoomLocations = new List<FollowerLocation>()
  {
    FollowerLocation.DLC_ShrineRoom,
    FollowerLocation.Blacksmith_Inside,
    FollowerLocation.TarotShop_Inside,
    FollowerLocation.DecorationShop_Inside,
    FollowerLocation.Flockade_Inside
  };
  [SerializeField]
  public Transform SafeSpawnCheckTransform;
  public bool StartsActive = true;
  public AsyncOperationHandle<GameObject> player_AddressableHandle;
  public GameObject _playerPrefab;
  public AssetReferenceGameObject Addr_PlayerPrefab;
  [CompilerGenerated]
  public bool \u003CActivatable\u003Ek__BackingField = true;
  [CompilerGenerated]
  public bool \u003CStructuresPlaced\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CFollowersSpawned\u003Ek__BackingField;
  public static System.Action OnFollowersSpawned;
  [SerializeField]
  public BiomeLightingSettings bloodMoonLUT;
  [SerializeField]
  public BiomeLightingSettings nudismLUT;
  [SerializeField]
  public BiomeLightingSettings purgeLUT;
  [CompilerGenerated]
  public System.Random \u003CRandom\u003Ek__BackingField;
  public bool halloweenLutActive;
  public bool nuidismLutActive;
  public bool purgeLutActive;
  public bool isInitialized;
  public float initializerTimer = 5f;
  public static System.Action OnPlayerLocationSet;
  public List<global::StructuresData> structuresRequirePlacing = new List<global::StructuresData>();
  public static Dictionary<FollowerLocation, LocationManager> LocationManagers = new Dictionary<FollowerLocation, LocationManager>();
  public static Dictionary<FollowerLocation, LocationState> _locationStates = new Dictionary<FollowerLocation, LocationState>();
  public static List<FollowerLocation> _dungeonLocations = new List<FollowerLocation>()
  {
    FollowerLocation.Dungeon1_1,
    FollowerLocation.Dungeon1_2,
    FollowerLocation.Dungeon1_3,
    FollowerLocation.Dungeon1_4,
    FollowerLocation.Dungeon1_5,
    FollowerLocation.Dungeon1_6,
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
    FollowerLocation.Boss_Yngya,
    FollowerLocation.Boss_Wolf,
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

  public GameObject PlayerPrefab
  {
    get
    {
      if ((UnityEngine.Object) this._playerPrefab == (UnityEngine.Object) null && this.Addr_PlayerPrefab.RuntimeKeyIsValid())
      {
        this.player_AddressableHandle = Addressables.LoadAssetAsync<GameObject>((object) this.Addr_PlayerPrefab);
        this.player_AddressableHandle.WaitForCompletion();
        this._playerPrefab = this.player_AddressableHandle.Result;
      }
      return this._playerPrefab;
    }
    set => this._playerPrefab = value;
  }

  public bool Activatable
  {
    get => this.\u003CActivatable\u003Ek__BackingField;
    set => this.\u003CActivatable\u003Ek__BackingField = value;
  }

  public bool StructuresPlaced
  {
    get => this.\u003CStructuresPlaced\u003Ek__BackingField;
    set => this.\u003CStructuresPlaced\u003Ek__BackingField = value;
  }

  public bool FollowersSpawned
  {
    get => this.\u003CFollowersSpawned\u003Ek__BackingField;
    set => this.\u003CFollowersSpawned\u003Ek__BackingField = value;
  }

  public System.Random Random
  {
    get => this.\u003CRandom\u003Ek__BackingField;
    set => this.\u003CRandom\u003Ek__BackingField = value;
  }

  public bool IsInitialized => this.isInitialized;

  public virtual void Awake()
  {
    LocationManager locationManager = (LocationManager) null;
    LocationManager.LocationManagers.TryGetValue(this.Location, out locationManager);
    LocationManager.LocationManagers[this.Location] = this;
  }

  public virtual void Start()
  {
    if (DataManager.Instance.LocationSeeds.FirstOrDefault<DataManager.LocationSeedsData>((Func<DataManager.LocationSeedsData, bool>) (x => x.Location == this.Location)) == null)
      DataManager.Instance.LocationSeeds.Add(new DataManager.LocationSeedsData()
      {
        Location = this.Location,
        Seed = UnityEngine.Random.Range(-2147483647 /*0x80000001*/, int.MaxValue)
      });
    this.Random = new System.Random(DataManager.Instance.LocationSeeds.FirstOrDefault<DataManager.LocationSeedsData>((Func<DataManager.LocationSeedsData, bool>) (x => x.Location == this.Location)).Seed);
    this.StartCoroutine((IEnumerator) this.LoadLocationRoutine());
    if (FollowerBrainStats.IsBloodMoon && (UnityEngine.Object) this.bloodMoonLUT != (UnityEngine.Object) null)
    {
      this.EnableBloodMoon();
    }
    else
    {
      if (!FollowerBrainStats.IsNudism || !((UnityEngine.Object) this.nudismLUT != (UnityEngine.Object) null))
        return;
      this.EnableNudism();
    }
  }

  public IEnumerator LoadLocationRoutine()
  {
    LocationManager locationManager = this;
    LocationManager.BeginLoadLocation(locationManager.Location);
    if (locationManager.SupportsStructures)
    {
      locationManager.PrePlacingStructures();
      yield return (object) new WaitForEndOfFrame();
      yield return (object) locationManager.StartCoroutine((IEnumerator) locationManager.PlaceStructures());
      yield return (object) new WaitForEndOfFrame();
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
    locationManager.isInitialized = true;
  }

  public virtual void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.ActivateLocationRoutine());
  }

  public IEnumerator ActivateLocationRoutine()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.transform.SetParent(this.UnitLayer);
      foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
      {
        if (ranchable.CurrentState == Interaction_Ranchable.State.Leashed)
        {
          ranchable.transform.SetParent(this.UnitLayer);
          ranchable.transform.position = player.transform.position;
        }
      }
    }
    yield return (object) new WaitForEndOfFrame();
    if ((LocationManager.GetLocationState(this.Location) == LocationState.Inactive || LocationManager.GetLocationState(this.Location) == LocationState.Loading) && this.Activatable)
      LocationManager.ActivateLocation(this.Location);
    else if (LocationManager.GetLocationState(this.Location) == LocationState.Active)
      PlayerFarming.Location = this.Location;
    if (FollowerBrainStats.IsBloodMoon && (UnityEngine.Object) this.bloodMoonLUT != (UnityEngine.Object) null)
      this.EnableBloodMoon();
    else if (FollowerBrainStats.IsNudism && (UnityEngine.Object) this.nudismLUT != (UnityEngine.Object) null)
      this.EnableNudism();
  }

  public virtual void OnDisable() => LocationManager.DeactivateLocation(this.Location);

  public static void UpdateLocation()
  {
    System.Action playerLocationSet = LocationManager.OnPlayerLocationSet;
    if (playerLocationSet == null)
      return;
    playerLocationSet();
  }

  public virtual void Update()
  {
    if (this.halloweenLutActive && !FollowerBrainStats.IsBloodMoon)
      this.DisableBloodMoon();
    else if (this.nuidismLutActive && !FollowerBrainStats.IsNudism)
      this.DisableNudism();
    this.CheckForInitialization();
  }

  public virtual void OnDestroy()
  {
    if (LocationManager.LocationManagers.ContainsKey(this.Location))
      LocationManager.LocationManagers.Remove(this.Location);
    LocationManager.UnloadLocation(this.Location);
    if ((UnityEngine.Object) LocationManager._Instance == (UnityEngine.Object) this)
      LocationManager._Instance = (LocationManager) null;
    this.CleanUpAddressable();
  }

  public void CleanUpAddressable()
  {
    this._playerPrefab = (GameObject) null;
    if (!this.player_AddressableHandle.IsValid())
      return;
    Addressables.Release<GameObject>(this.player_AddressableHandle);
  }

  public void CheckForInitialization()
  {
    if (this.IsInitialized || (double) (this.initializerTimer -= Time.deltaTime) > 0.0)
      return;
    this.initializerTimer = 5f;
    this.isInitialized = true;
  }

  public virtual GameObject PlacePlayer()
  {
    if (PlayerFarming.Location != this.Location)
      PlayerFarming.LastLocation = PlayerFarming.Location;
    PlayerFarming.Location = this.Location;
    Vector3 startPosition = this.GetStartPosition(PlayerFarming.LastLocation);
    GameObject gameObject;
    if (PlayerFarming.playersCount == 0)
    {
      gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPrefab, startPosition, Quaternion.identity, this.UnitLayer);
      PlayerFarming component = gameObject.GetComponent<PlayerFarming>();
      PlayerFarming.Instance = component;
      Debug.Log((object) ("Player farming Instance? " + ((object) component)?.ToString()));
    }
    else
    {
      PlayerFarming.ResetMainPlayer();
      gameObject = PlayerFarming.Instance.gameObject;
    }
    if (CoopManager.CoopActive)
      CoopManager.Instance.SpawnCoopPlayer(1, false);
    this.PositionPlayer();
    gameObject.GetComponent<StateMachine>().facingAngle = Utils.GetAngle(gameObject.transform.position, Vector3.zero);
    System.Action playerLocationSet = LocationManager.OnPlayerLocationSet;
    if (playerLocationSet != null)
      playerLocationSet();
    return gameObject;
  }

  public static FollowerLocation GetLocation()
  {
    return (UnityEngine.Object) LocationManager._Instance == (UnityEngine.Object) null ? FollowerLocation.Base : LocationManager._Instance.Location;
  }

  public virtual void PositionPlayer(GameObject player = null)
  {
    if ((UnityEngine.Object) player == (UnityEngine.Object) null && (bool) (UnityEngine.Object) PlayerFarming.Instance)
      player = PlayerFarming.Instance.gameObject;
    if (!(bool) (UnityEngine.Object) player)
      return;
    player.transform.position = this.GetStartPosition(PlayerFarming.Location);
    PlayerFarming.PositionAllPlayers(player.transform.position);
  }

  public FollowerRecruit SpawnRecruit(FollowerBrain brain, Vector3 position)
  {
    FollowerRecruit followerRecruit = ObjectPool.Spawn<FollowerRecruit>(FollowerManager.RecruitPrefab, this.UnitLayer, position, Quaternion.identity);
    followerRecruit.gameObject.transform.position = position;
    followerRecruit.name = "Recruit " + LocalizeIntegration.Arabic_ReverseNonRTL(brain.Info.Name);
    followerRecruit.Follower.Init(brain, brain.CreateOutfit());
    followerRecruit.Follower.Spine.transform.localScale = new Vector3(-1f, 1f, 1f);
    return followerRecruit;
  }

  public Follower SpawnFollower(FollowerBrain brain, Vector3 position)
  {
    Follower follower = UnityEngine.Object.Instantiate<Follower>(FollowerManager.FollowerPrefab, position, Quaternion.identity, this.UnitLayer);
    follower.name = "Follower " + LocalizeIntegration.Arabic_ReverseNonRTL(brain.Info.Name);
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
    if (simFollower.Brain.Location == FollowerLocation.Missionary && structuresOfType1.Count > 0 && structuresOfType1.Count < simFollower.Brain._directInfoAccess.MissionaryIndex)
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
    if (location == FollowerLocation.Missionary && structuresOfType.Count > 0 && structuresOfType.Count < follower.Brain._directInfoAccess.MissionaryIndex)
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

  public abstract Vector3 GetStartPosition(FollowerLocation prevLocation);

  public virtual Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    throw new ArgumentException($"Unexpected GetExitPosition(FollowerLocation.{destLocation}) from Location.{this.Location}");
  }

  public virtual IEnumerator PlaceStructures()
  {
    List<Vector2Int> vector2IntList = new List<Vector2Int>();
    bool flag1 = GameManager.AuthenticateMajorDLC();
    this.CheckExistingStructures();
    this.structuresRequirePlacing.Clear();
    Vector2[] points = new Vector2[0];
    PolygonCollider2D collider = (PolygonCollider2D) null;
    if ((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null)
    {
      collider = BiomeBaseManager.Instance.Room.Pieces[0].Collider;
      points = collider.points;
    }
    for (int index = this.StructuresData.Count - 1; index >= 0; --index)
    {
      global::StructuresData structuresData = this.StructuresData[index];
      if (structuresData.Type != StructureBrain.TYPES.NONE)
      {
        if (!DataManager.Instance.DLC_Cultist_Pack && (DataManager.CultistDLCStructures.Contains(structuresData.Type) || DataManager.CultistDLCStructures.Contains(structuresData.ToBuildType)) || !DataManager.Instance.DLC_Sinful_Pack && (DataManager.SinfulDLCStructures.Contains(structuresData.Type) || DataManager.SinfulDLCStructures.Contains(structuresData.ToBuildType)) || !DataManager.Instance.DLC_Pilgrim_Pack && (DataManager.PilgrimDLCStructures.Contains(structuresData.Type) || DataManager.PilgrimDLCStructures.Contains(structuresData.ToBuildType)) || !DataManager.Instance.DLC_Heretic_Pack && (DataManager.HereticDLCStructures.Contains(structuresData.Type) || DataManager.HereticDLCStructures.Contains(structuresData.ToBuildType)))
          this.StructuresData.RemoveAt(index);
        else if (!flag1 && (DataManager.MajorDLCStructures.Contains(structuresData.Type) || DataManager.MajorDLCStructures.Contains(structuresData.ToBuildType)))
          this.StructuresData.RemoveAt(index);
        else if (structuresData.DontLoadMe)
        {
          bool flag2 = false;
          foreach (Structure structure in Structure.Structures)
          {
            if (structuresData.Position == structure.transform.position)
            {
              structure.Brain = StructureBrain.GetOrCreateBrain(structuresData);
              structure.Brain.AddToGrid();
              flag2 = true;
              break;
            }
          }
          if (!flag2)
            this.StructuresData.RemoveAt(index);
        }
        else
        {
          if (structuresData.Location == FollowerLocation.None)
          {
            Debug.LogWarning((object) $"Placing Structure {structuresData.Type}.{structuresData.ID} with Location.None, updating to {this.Location}");
            structuresData.Location = this.Location;
          }
          if (vector2IntList.Contains(structuresData.GridTilePosition) && structuresData.GridTilePosition != new Vector2Int(-2147483647 /*0x80000001*/, -2147483647 /*0x80000001*/))
            this.StructuresData.RemoveAt(index);
          else if (!this.EnsureWithinBounds(structuresData.Position, points, collider))
          {
            this.StructuresData.RemoveAt(index);
          }
          else
          {
            vector2IntList.Add(structuresData.GridTilePosition);
            for (int x = 0; x < structuresData.Bounds.x; ++x)
            {
              for (int y = 0; y < structuresData.Bounds.y; ++y)
                vector2IntList.Add(structuresData.GridTilePosition + new Vector2Int(x, y));
            }
            this.structuresRequirePlacing.Add(structuresData);
            this.InstantiateStructureAsync(structuresData);
          }
        }
      }
    }
    while (this.structuresRequirePlacing.Count > 0)
      yield return (object) null;
    this.StructuresPlaced = true;
    StructureManager.StructuresPlaced structuresPlaced = StructureManager.OnStructuresPlaced;
    if (structuresPlaced != null)
      structuresPlaced();
  }

  public async void InstantiateStructureAsync(global::StructuresData structuresData)
  {
    if (!structuresData.PrefabPath.Contains("Assets"))
      structuresData.PrefabPath = $"Assets/{structuresData.PrefabPath}.prefab";
    Task<GameObject> operation = Addressables.InstantiateAsync((object) structuresData.PrefabPath).Task;
    GameObject gameObject = await operation;
    if ((UnityEngine.Object) operation.Result == (UnityEngine.Object) null)
    {
      Debug.Log((object) "STRUCTURE COULDN'T LOAD");
      this.structuresRequirePlacing.Remove(structuresData);
      operation = (Task<GameObject>) null;
    }
    else
    {
      try
      {
        this.PlaceStructure(structuresData, operation.Result.gameObject);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
      }
      this.structuresRequirePlacing.Remove(structuresData);
      operation = (Task<GameObject>) null;
    }
  }

  public GameObject PlaceStructure(global::StructuresData structure, GameObject g)
  {
    if (!((UnityEngine.Object) PlacementRegion.Instance == (UnityEngine.Object) null) && PlacementRegion.Instance.structureBrain != null)
      PlacementRegion.Instance.structureBrain.AddStructureToGrid(structure);
    StructureBrain.ApplyConfigToData(structure);
    g.transform.parent = this.StructureLayer;
    g.transform.position = new Vector3(structure.Position.x + structure.Offset.x, structure.Position.y + structure.Offset.y, Mathf.Min(0.0f, structure.Position.z));
    g.transform.localScale = new Vector3((float) structure.Direction, g.transform.localScale.y, g.transform.localScale.z);
    Structure structure1 = g.GetComponent<Structure>();
    if ((UnityEngine.Object) structure1 == (UnityEngine.Object) null)
      structure1 = g.GetComponentInChildren<Structure>();
    if ((UnityEngine.Object) structure1 != (UnityEngine.Object) null)
      structure1.Brain = StructureBrain.GetOrCreateBrain(structure);
    WorkPlace component = g.GetComponent<WorkPlace>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.SetID($"{g.transform.position.x.ToString()}_{g.transform.position.y.ToString()}");
    return g;
  }

  public void CheckExistingStructures()
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

  public bool EnsureWithinBounds(Vector3 pos, Vector2[] points, PolygonCollider2D collider)
  {
    return PlayerFarming.Location != FollowerLocation.Base || Utils.PointWithinPolygon(pos, points) || collider.OverlapPoint((Vector2) pos);
  }

  public virtual void CheckExistingStructure(Structure s)
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

  public virtual void PrePlacingStructures()
  {
  }

  public virtual void PostPlaceStructures()
  {
  }

  public static IEnumerable<FollowerLocation> LocationsInState(LocationState state)
  {
    for (int i = 0; i < 98; ++i)
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

  public static void BeginLoadLocation(FollowerLocation location)
  {
    LocationManager._locationStates[location] = LocationState.Loading;
  }

  public static void EndLoadLocation(FollowerLocation location)
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
    if (PlayerFarming.Location != location)
      PlayerFarming.LastLocation = PlayerFarming.Location;
    PlayerFarming.Location = location;
    if (PlayerFarming.LastLocation != FollowerLocation.Church && PlayerFarming.Location != FollowerLocation.Church)
    {
      TimeManager.SurvivalTallies = 3;
      TimeManager.SurvivalDamagedTimer = 0.0f;
    }
    SeasonsManager.LocationChanged(location);
    TwitchManager.LocationChanged(location);
    LocationManager.UpdateLocationInstance();
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
    LocationManager.UpdateLocationInstance();
  }

  public static void UnloadLocation(FollowerLocation location)
  {
    LocationManager._locationStates[location] = LocationState.Unloaded;
    LocationManager.UpdateLocationInstance();
    if (!FollowerManager.Followers.ContainsKey(location))
      return;
    FollowerManager.Followers[location].Clear();
  }

  public static void UpdateLocationInstance()
  {
    foreach (KeyValuePair<FollowerLocation, LocationState> locationState in LocationManager._locationStates)
    {
      if (locationState.Value == LocationState.Active)
      {
        LocationManager.LocationManagers.TryGetValue(locationState.Key, out LocationManager._Instance);
        return;
      }
    }
    LocationManager._Instance = (LocationManager) null;
  }

  public static System.Random GetLocationManagersRandom(FollowerLocation location)
  {
    LocationManager locationManager;
    LocationManager.LocationManagers.TryGetValue(location, out locationManager);
    return (UnityEngine.Object) locationManager == (UnityEngine.Object) null ? (System.Random) null : locationManager.Random;
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
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.winter_random);
    else
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.StandardAmbience);
  }

  public void EnableNudism()
  {
    this.nuidismLutActive = true;
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.nudist_ritual);
  }

  public void DisableNudism()
  {
    this.nuidismLutActive = false;
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.winter_random);
    else
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.StandardAmbience);
  }

  public void EnablePurge()
  {
  }

  public void DisablePurge()
  {
  }

  public void ReEnableRitualEffects()
  {
    if (FollowerBrainStats.IsBloodMoon)
    {
      this.EnableBloodMoon();
    }
    else
    {
      if (!FollowerBrainStats.IsNudism)
        return;
      this.EnableNudism();
    }
  }

  public void PopOutDeadBodiesFromGraves(int amount)
  {
    List<Structures_Grave> structuresGraveList = new List<Structures_Grave>((IEnumerable<Structures_Grave>) StructureManager.GetAllStructuresOfType<Structures_Grave>());
    for (int index = structuresGraveList.Count - 1; index >= 0; --index)
    {
      if (structuresGraveList[index].Data.FollowerID == -1)
        structuresGraveList.RemoveAt(index);
    }
    for (int index = 0; index < amount; ++index)
    {
      if (structuresGraveList.Count > 0)
      {
        Structures_Grave structuresGrave = structuresGraveList[UnityEngine.Random.Range(0, structuresGraveList.Count)];
        structuresGraveList.Remove(structuresGrave);
        foreach (Grave grave in Grave.Graves)
        {
          if (grave.structureBrain.Data.ID == structuresGrave.Data.ID)
          {
            grave.SpawnDeadBody();
            break;
          }
        }
      }
    }
  }

  public void InstantlyFillAllToilets()
  {
    List<Structures_Outhouse> structuresOuthouseList = new List<Structures_Outhouse>((IEnumerable<Structures_Outhouse>) StructureManager.GetAllStructuresOfType<Structures_Outhouse>());
    for (int index = structuresOuthouseList.Count - 1; index >= 0; --index)
    {
      if (structuresOuthouseList[index].IsFull)
        structuresOuthouseList.RemoveAt(index);
    }
    foreach (Structures_Outhouse structuresOuthouse in structuresOuthouseList)
      structuresOuthouse.DepositItem(InventoryItem.ITEM_TYPE.POOP, Structures_Outhouse.Capacity(structuresOuthouse.Data.Type) - structuresOuthouse.GetPoopCount());
  }

  public void InstantlyClearKitchenQueues()
  {
    foreach (Structures_Kitchen structuresKitchen in new List<Structures_Kitchen>((IEnumerable<Structures_Kitchen>) StructureManager.GetAllStructuresOfType<Structures_Kitchen>()))
    {
      structuresKitchen.Data.QueuedMeals.Clear();
      structuresKitchen.Data.QueuedResources.Clear();
      structuresKitchen.FoodStorage.Data.Inventory.Clear();
      structuresKitchen.Data.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
    }
    foreach (Interaction_FollowerKitchen followerKitchen in Interaction_FollowerKitchen.FollowerKitchens)
    {
      followerKitchen.UpdateCurrentMeal();
      followerKitchen.DisplayUI();
      followerKitchen.foodStorage.UpdateFoodDisplayed();
    }
  }

  public void InstantlyFertilizeAllCrops()
  {
    foreach (Structures_FarmerPlot structuresFarmerPlot in new List<Structures_FarmerPlot>((IEnumerable<Structures_FarmerPlot>) StructureManager.GetAllStructuresOfType<Structures_FarmerPlot>()))
    {
      if (structuresFarmerPlot.HasPlantedSeed())
        structuresFarmerPlot.AddFertilizer(InventoryItem.ITEM_TYPE.POOP);
    }
    foreach (FarmPlot farmPlot in FarmPlot.FarmPlots)
      farmPlot.UpdateCropImage();
  }

  public void InstantlyRefineMaterials()
  {
    foreach (Structures_Refinery structuresRefinery in new List<Structures_Refinery>((IEnumerable<Structures_Refinery>) StructureManager.GetAllStructuresOfType<Structures_Refinery>()))
    {
      for (int index = structuresRefinery.Data.QueuedResources.Count - 1; index >= 0; --index)
        structuresRefinery.RefineryDeposit();
    }
  }

  [CompilerGenerated]
  public bool \u003CStart\u003Eb__51_0(DataManager.LocationSeedsData x)
  {
    return x.Location == this.Location;
  }

  [CompilerGenerated]
  public bool \u003CStart\u003Eb__51_1(DataManager.LocationSeedsData x)
  {
    return x.Location == this.Location;
  }
}
