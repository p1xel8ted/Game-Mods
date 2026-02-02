// Decompiled with JetBrains decompiler
// Type: MMRoomGeneration.GenerateRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using MMBiomeGeneration;
using Pathfinding;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

#nullable disable
namespace MMRoomGeneration;

public class GenerateRoom : BaseMonoBehaviour
{
  public static GenerateRoom Instance;
  public int Seed;
  public System.Random RandomSeed;
  public SoundConstants.RoomID roomMusicID = SoundConstants.RoomID.StandardAmbience;
  [EventRef]
  public string biomeAtmosOverridePath = string.Empty;
  public Sprite MapIcon;
  public List<AsyncOperationHandle<GameObject>> asyncOperationHandles = new List<AsyncOperationHandle<GameObject>>();
  public bool isDecorationsInitialized;
  public List<GeneraterDecorations> _decorationSetList = new List<GeneraterDecorations>();
  public List<AssetReferenceGameObject> Addr_DecorationSetList = new List<AssetReferenceGameObject>();
  [HideInInspector]
  public GeneraterDecorations DecorationList;
  [SerializeField]
  public bool customDecorations;
  public bool CreateOnlyOneEncounterRoom;
  public bool CreateRandomExtraPaths = true;
  [Range(0.0f, 1f)]
  public float EncounterWillBeEnemyOrResource = 0.5f;
  public CompositeCollider2D RoomTransform;
  public GameObject SceneryTransform;
  public GameObject CustomTransform;
  public int Margin = 5;
  [HideInInspector]
  public int Scale = 2;
  public bool LockingDoors = true;
  public bool LockEntranceBehindPlayer;
  public GenerateRoom.ConnectionTypes North;
  public GenerateRoom.ConnectionTypes East;
  public GenerateRoom.ConnectionTypes South;
  public GenerateRoom.ConnectionTypes West;
  public GameObject BloodSplatterPrefab;
  public IslandPiece _northIsland;
  public IslandPiece _eastIsland;
  public IslandPiece _southIsland;
  public IslandPiece _westIsland;
  public IslandPiece _northBossDoor;
  public IslandPiece _eastBossDoor;
  public IslandPiece _southBossDoor;
  public IslandPiece _westBossDoor;
  public IslandPiece _northEntranceDoor;
  public IslandPiece _eastEntranceDoor;
  public IslandPiece _southEntranceDoor;
  public IslandPiece _westEntranceDoor;
  public IslandPiece _northBossDoor_P2;
  public IslandPiece _eastBossDoor_P2;
  public IslandPiece _southBossDoor_P2;
  public IslandPiece _westBossDoor_P2;
  public AssetReferenceGameObject Addr_NorthIsland;
  public AssetReferenceGameObject Addr_EastIsland;
  public AssetReferenceGameObject Addr_SouthIsland;
  public AssetReferenceGameObject Addr_WestIsland;
  public AssetReferenceGameObject Addr_NorthBossDoor;
  public AssetReferenceGameObject Addr_EastBossDoor;
  public AssetReferenceGameObject Addr_SouthBossDoor;
  public AssetReferenceGameObject Addr_WestBossDoor;
  public AssetReferenceGameObject Addr_NorthEntranceDoor;
  public AssetReferenceGameObject Addr_EastEntranceDoor;
  public AssetReferenceGameObject Addr_SouthEntranceDoor;
  public AssetReferenceGameObject Addr_WestEntranceDoor;
  public AssetReferenceGameObject Addr_NorthBossDoor_P2;
  public AssetReferenceGameObject Addr_EastBossDoor_P2;
  public AssetReferenceGameObject Addr_SouthBossDoor_P2;
  public AssetReferenceGameObject Addr_WestBossDoor_P2;
  public bool isStartPiecesInitiliazed;
  public bool isIslandPiecesInitiliazed;
  public bool isResourcePiecesInitiliazed;
  public List<IslandPiece> _startPieces = new List<IslandPiece>();
  public List<IslandPiece> _islandPieces = new List<IslandPiece>();
  public List<IslandPiece> _resourcePieces = new List<IslandPiece>();
  public List<AssetReferenceGameObject> Addr_StartPieces = new List<AssetReferenceGameObject>();
  public List<AssetReferenceGameObject> Addr_IslandPieces = new List<AssetReferenceGameObject>();
  public List<AssetReferenceGameObject> Addr_ResourcePieces = new List<AssetReferenceGameObject>();
  public List<IslandPiece> NorthIslandPieces;
  public List<IslandPiece> EastIslandPieces;
  public List<IslandPiece> SouthIslandPieces;
  public List<IslandPiece> WestIslandPieces;
  public List<IslandPiece> NorthIslandEncounterPieces;
  public List<IslandPiece> EastIslandEncounterPieces;
  public List<IslandPiece> SouthIslandEncounterPieces;
  public List<IslandPiece> WestIslandEncounterPieces;
  public List<IslandPiece> NorthIslandResourcesPieces;
  public List<IslandPiece> EastIslandResourcesPieces;
  public List<IslandPiece> SouthIslandResourcesPieces;
  public List<IslandPiece> WestIslandResourcesPieces;
  public List<IslandPiece> Pieces = new List<IslandPiece>();
  [CompilerGenerated]
  public IslandPiece \u003CStartPiece\u003Ek__BackingField;
  public IslandPiece CurrentPiece;
  public IslandPiece PrevPiece;
  public List<int> PreviousSeeds = new List<int>();
  public AssetReferenceGameObject RoomHeavyAssets;
  public Transform HeavyAssetsTransform;
  public AsyncOperationHandle<GameObject> heavyAssetsHandle;
  [CompilerGenerated]
  public bool \u003Cgenerated\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CGeneratedDecorations\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CGeneratedPathing\u003Ek__BackingField;
  public UnityEvent OnGenerateComplete;
  public UnityEvent OnRoomComplete;
  public bool regenerateNavOnEnable;
  public bool Testing;
  public List<GenerateRoom.RoomPath> Paths;
  public float PrevTime;
  public Door revealingDoor;
  public bool revalingDoorComplete;
  public float LimitNorth = (float) int.MinValue;
  public float LimitEast = (float) int.MinValue;
  public float LimitSouth = (float) int.MaxValue;
  public float LimitWest = (float) int.MaxValue;
  public SpriteShapeController RoomSpriteShape;
  public List<List<int>> DecorationGrid;
  public int DecorationGridWidth;
  public int DecorationGridHeight;
  public GeneraterDecorations.DecorationAndProbability d;
  public GameObject PerlinNoiseDecoration;
  public Vector3 PerlinScale;
  public string NoiseName;
  public float Noise;
  public List<SpriteShapeController> _SpriteShapeControllers;
  public static SpriteShapeController CurrentSpriteShape;
  public static List<Vector3> Points;
  public Vector2 RoomPerlinOffset;
  public List<IslandConnector> Connectors;
  public IslandConnector CurrentConnector;
  public IslandConnector Connector;
  public IslandPiece RandomPiece;
  public List<Collider2D> Collisions;

  public List<GeneraterDecorations> DecorationSetList
  {
    get
    {
      if (!this.isDecorationsInitialized)
        this.InitializeDecorationSetList();
      return this._decorationSetList;
    }
    set => this._decorationSetList = value;
  }

  public IslandPiece NorthIsland
  {
    get
    {
      return !((UnityEngine.Object) this._northIsland == (UnityEngine.Object) null) ? this._northIsland : (this._northIsland = this.LoadAddresableAsset<IslandPiece>(this.Addr_NorthIsland));
    }
    set => this._northIsland = value;
  }

  public IslandPiece EastIsland
  {
    get
    {
      return !((UnityEngine.Object) this._eastIsland == (UnityEngine.Object) null) ? this._eastIsland : (this._eastIsland = this.LoadAddresableAsset<IslandPiece>(this.Addr_EastIsland));
    }
    set => this._eastIsland = value;
  }

  public IslandPiece SouthIsland
  {
    get
    {
      return !((UnityEngine.Object) this._southIsland == (UnityEngine.Object) null) ? this._southIsland : (this._southIsland = this.LoadAddresableAsset<IslandPiece>(this.Addr_SouthIsland));
    }
    set => this._southIsland = value;
  }

  public IslandPiece WestIsland
  {
    get
    {
      return !((UnityEngine.Object) this._westIsland == (UnityEngine.Object) null) ? this._westIsland : (this._westIsland = this.LoadAddresableAsset<IslandPiece>(this.Addr_WestIsland));
    }
    set => this._westIsland = value;
  }

  public IslandPiece NorthBossDoor
  {
    get
    {
      return !((UnityEngine.Object) this._northBossDoor == (UnityEngine.Object) null) ? this._northBossDoor : (this._northBossDoor = this.LoadAddresableAsset<IslandPiece>(this.Addr_NorthBossDoor));
    }
    set => this._northBossDoor = value;
  }

  public IslandPiece EastBossDoor
  {
    get
    {
      return !((UnityEngine.Object) this._eastBossDoor == (UnityEngine.Object) null) ? this._eastBossDoor : (this._eastBossDoor = this.LoadAddresableAsset<IslandPiece>(this.Addr_EastBossDoor));
    }
    set => this._eastBossDoor = value;
  }

  public IslandPiece SouthBossDoor
  {
    get
    {
      return !((UnityEngine.Object) this._southBossDoor == (UnityEngine.Object) null) ? this._southBossDoor : (this._southBossDoor = this.LoadAddresableAsset<IslandPiece>(this.Addr_SouthBossDoor));
    }
    set => this._southBossDoor = value;
  }

  public IslandPiece WestBossDoor
  {
    get
    {
      return !((UnityEngine.Object) this._westBossDoor == (UnityEngine.Object) null) ? this._westBossDoor : (this._westBossDoor = this.LoadAddresableAsset<IslandPiece>(this.Addr_WestBossDoor));
    }
    set => this._westBossDoor = value;
  }

  public IslandPiece NorthEntranceDoor
  {
    get
    {
      return !((UnityEngine.Object) this._northEntranceDoor == (UnityEngine.Object) null) ? this._northEntranceDoor : (this._northEntranceDoor = this.LoadAddresableAsset<IslandPiece>(this.Addr_NorthEntranceDoor));
    }
    set => this._northEntranceDoor = value;
  }

  public IslandPiece EastEntranceDoor
  {
    get
    {
      return !((UnityEngine.Object) this._eastEntranceDoor == (UnityEngine.Object) null) ? this._eastEntranceDoor : (this._eastEntranceDoor = this.LoadAddresableAsset<IslandPiece>(this.Addr_EastEntranceDoor));
    }
    set => this._eastEntranceDoor = value;
  }

  public IslandPiece SouthEntranceDoor
  {
    get
    {
      return !((UnityEngine.Object) this._southEntranceDoor == (UnityEngine.Object) null) ? this._southEntranceDoor : (this._southEntranceDoor = this.LoadAddresableAsset<IslandPiece>(this.Addr_SouthEntranceDoor));
    }
    set => this._southEntranceDoor = value;
  }

  public IslandPiece WestEntranceDoor
  {
    get
    {
      return !((UnityEngine.Object) this._westEntranceDoor == (UnityEngine.Object) null) ? this._westEntranceDoor : (this._westEntranceDoor = this.LoadAddresableAsset<IslandPiece>(this.Addr_WestEntranceDoor));
    }
    set => this._westEntranceDoor = value;
  }

  public IslandPiece NorthBossDoor_P2
  {
    get
    {
      return !((UnityEngine.Object) this._northBossDoor_P2 == (UnityEngine.Object) null) ? this._northBossDoor_P2 : (this._northBossDoor_P2 = this.LoadAddresableAsset<IslandPiece>(this.Addr_NorthBossDoor_P2));
    }
    set => this._northBossDoor_P2 = value;
  }

  public IslandPiece EastBossDoor_P2
  {
    get
    {
      return !((UnityEngine.Object) this._eastBossDoor_P2 == (UnityEngine.Object) null) ? this._eastBossDoor_P2 : (this._eastBossDoor_P2 = this.LoadAddresableAsset<IslandPiece>(this.Addr_EastBossDoor_P2));
    }
    set => this._eastBossDoor_P2 = value;
  }

  public IslandPiece SouthBossDoor_P2
  {
    get
    {
      return !((UnityEngine.Object) this._southBossDoor_P2 == (UnityEngine.Object) null) ? this._southBossDoor_P2 : (this._southBossDoor_P2 = this.LoadAddresableAsset<IslandPiece>(this.Addr_SouthBossDoor_P2));
    }
    set => this._southBossDoor_P2 = value;
  }

  public IslandPiece WestBossDoor_P2
  {
    get
    {
      return !((UnityEngine.Object) this._westBossDoor_P2 == (UnityEngine.Object) null) ? this._westBossDoor_P2 : (this._westBossDoor_P2 = this.LoadAddresableAsset<IslandPiece>(this.Addr_WestBossDoor_P2));
    }
    set => this._westBossDoor_P2 = value;
  }

  public IslandPiece NorthTarotDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Tarot North");
  }

  public IslandPiece EastTarotDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Tarot East");
  }

  public IslandPiece SouthTarotDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Tarot South");
  }

  public IslandPiece WestTarotDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Tarot West");
  }

  public IslandPiece NorthWeaponDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Weapon North");
  }

  public IslandPiece EastWeaponDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Weapon East");
  }

  public IslandPiece SouthWeaponDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Weapon South");
  }

  public IslandPiece WestWeaponDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Weapon West");
  }

  public IslandPiece NorthRelicDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Relic North");
  }

  public IslandPiece EastRelicDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Relic East");
  }

  public IslandPiece SouthRelicDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Relic South");
  }

  public IslandPiece WestRelicDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Relic West");
  }

  public IslandPiece NorthLoreStoneDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Lore Stone North");
  }

  public IslandPiece EastLoreStoneDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Lore Stone East");
  }

  public IslandPiece SouthLoreStoneDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Lore Stone South");
  }

  public IslandPiece WestLoreStoneDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Lore Stone West");
  }

  public List<IslandPiece> StartPieces
  {
    get
    {
      if (!this.isStartPiecesInitiliazed)
      {
        this.InitializeStartPrefabs();
        this.isStartPiecesInitiliazed = true;
      }
      return this._startPieces;
    }
    set => this._startPieces = value;
  }

  public List<IslandPiece> IslandPieces
  {
    get
    {
      if (!this.isIslandPiecesInitiliazed)
      {
        this.InitializeIslandPrefabs();
        this.isIslandPiecesInitiliazed = true;
      }
      return this._islandPieces;
    }
  }

  public List<IslandPiece> ResourcePieces
  {
    get
    {
      if (!this.isResourcePiecesInitiliazed)
      {
        this.InitializeResourcePrefabs();
        this.isResourcePiecesInitiliazed = true;
      }
      return this._resourcePieces;
    }
  }

  public IslandPiece StartPiece
  {
    get => this.\u003CStartPiece\u003Ek__BackingField;
    set => this.\u003CStartPiece\u003Ek__BackingField = value;
  }

  public bool generated
  {
    get => this.\u003Cgenerated\u003Ek__BackingField;
    set => this.\u003Cgenerated\u003Ek__BackingField = value;
  }

  public bool GeneratedDecorations
  {
    get => this.\u003CGeneratedDecorations\u003Ek__BackingField;
    set => this.\u003CGeneratedDecorations\u003Ek__BackingField = value;
  }

  public bool GeneratedPathing
  {
    get => this.\u003CGeneratedPathing\u003Ek__BackingField;
    set => this.\u003CGeneratedPathing\u003Ek__BackingField = value;
  }

  public event GenerateRoom.GenerateEvent OnGenerated;

  public event GenerateRoom.GenerateEvent OnRegenerated;

  public event GenerateRoom.GenerateEvent OnSetCollider;

  public IEnumerator MatchCoopToMain()
  {
    float time = Time.time + 1f;
    do
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        PlayerFarming player = PlayerFarming.players[index];
        if ((UnityEngine.Object) player != (UnityEngine.Object) PlayerFarming.Instance)
          player.transform.position = PlayerFarming.Instance.transform.position;
        player.gameObject.GetComponent<Collider2D>().enabled = true;
      }
      yield return (object) new WaitForEndOfFrame();
    }
    while ((double) Time.time <= (double) time);
  }

  public void OnEnable()
  {
    GenerateRoom.Instance = this;
    this.InitSpriteShapes();
    if (this.regenerateNavOnEnable)
      this.StartCoroutine((IEnumerator) this.SetAStar());
    if (!this.generated)
      return;
    this.StartCoroutine((IEnumerator) this.RegenerateDecorationsWithPool());
  }

  public IEnumerator RegenerateDecorationsWithPool()
  {
    GenerateRoom generateRoom = this;
    generateRoom.RoomTransform.geometryType = CompositeCollider2D.GeometryType.Polygons;
    generateRoom.RoomTransform.GenerateGeometry();
    Physics2D.SyncTransforms();
    yield return (object) generateRoom.StartCoroutine((IEnumerator) generateRoom.SpawnDecorations(true));
    generateRoom.SetCollider();
    Debug.Log((object) ("OBJECT POOL GenerateRoom.OnEnable PoolCount: " + ObjectPool.CountAllPooled().ToString()));
    foreach (Door componentsInChild in generateRoom.transform.GetComponentsInChildren<Door>())
      generateRoom.DisableDecorationsNearDoor(componentsInChild);
    GenerateRoom.GenerateEvent onRegenerated = generateRoom.OnRegenerated;
    if (onRegenerated != null)
      onRegenerated();
    generateRoom.OnGenerateComplete?.Invoke();
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) GenerateRoom.Instance == (UnityEngine.Object) this)
      GenerateRoom.Instance = (GenerateRoom) null;
    if (this.generated && (UnityEngine.Object) this.SceneryTransform != (UnityEngine.Object) null && !this.customDecorations)
    {
      for (int index = this.SceneryTransform.transform.childCount - 1; index >= 0; --index)
        ObjectPool.Recycle(this.SceneryTransform.transform.GetChild(index).gameObject);
    }
    Debug.Log((object) ("OBJECT POOL GenerateRoom.OnDisable PoolCount: " + ObjectPool.CountAllPooled().ToString()));
    if (!((UnityEngine.Object) this.revealingDoor != (UnityEngine.Object) null))
      return;
    CameraFollowTarget.Instance.ClearAllTargets();
    CameraManager.instance.Stopshake();
    if (this.revalingDoorComplete)
      return;
    this.SetRevealedDoorSpriteShape();
  }

  public void OnDestroy()
  {
    this.CleanUpAddressables();
    this.BloodSplatterPrefab = (GameObject) null;
  }

  public void CleanUpAddressables()
  {
    this._decorationSetList.Clear();
    this._startPieces.Clear();
    this._islandPieces.Clear();
    this._resourcePieces.Clear();
    this._northIsland = (IslandPiece) null;
    this._eastIsland = (IslandPiece) null;
    this._southIsland = (IslandPiece) null;
    this._westIsland = (IslandPiece) null;
    this._northBossDoor = (IslandPiece) null;
    this._eastBossDoor = (IslandPiece) null;
    this._southBossDoor = (IslandPiece) null;
    this._westBossDoor = (IslandPiece) null;
    this._northEntranceDoor = (IslandPiece) null;
    this._eastEntranceDoor = (IslandPiece) null;
    this._southEntranceDoor = (IslandPiece) null;
    this._westEntranceDoor = (IslandPiece) null;
    this._northBossDoor_P2 = (IslandPiece) null;
    this._eastBossDoor_P2 = (IslandPiece) null;
    this._southBossDoor_P2 = (IslandPiece) null;
    this._westBossDoor_P2 = (IslandPiece) null;
    foreach (AsyncOperationHandle<GameObject> asyncOperationHandle in this.asyncOperationHandles)
    {
      if (asyncOperationHandle.IsValid())
        Addressables.Release<GameObject>(asyncOperationHandle);
    }
    if ((UnityEngine.Object) this.HeavyAssetsTransform != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.HeavyAssetsTransform);
    if (!this.heavyAssetsHandle.IsValid())
      return;
    Addressables.Release<GameObject>(this.heavyAssetsHandle);
  }

  public void Start()
  {
    this.PreviousSeeds.Add(this.Seed);
    if (!((UnityEngine.Object) this.BloodSplatterPrefab == (UnityEngine.Object) null))
      return;
    this.BloodSplatterPrefab = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/BloodParticles_Prefab"), this.transform) as GameObject;
  }

  public void GeneratePreviousSeed()
  {
    if (this.PreviousSeeds.Count <= 1)
      return;
    this.PreviousSeeds.RemoveAt(this.PreviousSeeds.Count - 1);
    this.Seed = this.PreviousSeeds[this.PreviousSeeds.Count - 1];
    this.StartCoroutine((IEnumerator) this.Generate());
  }

  public void GenerateRandomSeedTest()
  {
    this.Testing = true;
    this.GenerateRandomSeed();
    this.Testing = false;
  }

  public void GenerateRandomSeed()
  {
    this.Seed = UnityEngine.Random.Range(0, int.MaxValue);
    this.PreviousSeeds.Add(this.Seed);
    this.StartCoroutine((IEnumerator) this.Generate());
  }

  public void Generate(
    int Seed,
    GenerateRoom.ConnectionTypes North,
    GenerateRoom.ConnectionTypes East,
    GenerateRoom.ConnectionTypes South,
    GenerateRoom.ConnectionTypes West)
  {
    this.Seed = Seed;
    this.North = North;
    this.East = East;
    this.South = South;
    this.West = West;
    this.StartCoroutine((IEnumerator) this.Generate());
  }

  public void GenerateRoomFunc() => this.StartCoroutine((IEnumerator) this.Generate());

  public IEnumerator Generate()
  {
    GenerateRoom generateRoom = this;
    generateRoom.ClearPrefabs();
    generateRoom.Pieces = new List<IslandPiece>();
    generateRoom.RandomSeed = new System.Random(generateRoom.Seed);
    generateRoom.CollateLists();
    generateRoom.CreateStartPiece();
    generateRoom.CreatePaths();
    generateRoom.PlaceDoors();
    generateRoom.CompositeColliders();
    generateRoom.RoomTransform.geometryType = CompositeCollider2D.GeometryType.Polygons;
    generateRoom.RoomTransform.GenerateGeometry();
    Physics2D.SyncTransforms();
    generateRoom.PlaceDecorations(false);
    generateRoom.CreateSpriteShape();
    yield return (object) generateRoom.StartCoroutine((IEnumerator) generateRoom.DisableIslands());
    generateRoom.SetCollider();
    yield return (object) generateRoom.StartCoroutine((IEnumerator) generateRoom.SpawnDecorations(true, true));
    generateRoom.SpawnSpecialContent();
    generateRoom.SetColliderAndUpdatePathfinding();
    generateRoom.CreateBackgroundSpriteShape();
    generateRoom.InitSpriteShapes();
    generateRoom.generated = true;
    GenerateRoom.GenerateEvent onGenerated = generateRoom.OnGenerated;
    if (onGenerated != null)
      onGenerated();
    generateRoom.OnGenerateComplete?.Invoke();
  }

  public void SpawnHeavyAssets()
  {
    if (!this.RoomHeavyAssets.RuntimeKeyIsValid() || this.heavyAssetsHandle.IsValid())
      return;
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.RoomHeavyAssets);
    this.heavyAssetsHandle = asyncOperationHandle;
    asyncOperationHandle.WaitForCompletion();
    UnityEngine.Object.Instantiate<GameObject>(asyncOperationHandle.Result, this.HeavyAssetsTransform);
  }

  public void InitSpriteShapes()
  {
    SpriteShapeRenderer[] objectsOfType = (SpriteShapeRenderer[]) UnityEngine.Object.FindObjectsOfType((System.Type) typeof (SpriteShapeRenderer));
    CommandBuffer buffer = new CommandBuffer();
    buffer.GetTemporaryRT(0, 256 /*0x0100*/, 256 /*0x0100*/, 0);
    buffer.SetRenderTarget((RenderTargetIdentifier) 0);
    foreach (SpriteShapeRenderer spriteShapeRenderer in objectsOfType)
    {
      SpriteShapeController component = spriteShapeRenderer.gameObject.GetComponent<SpriteShapeController>();
      if ((UnityEngine.Object) spriteShapeRenderer != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && !spriteShapeRenderer.isVisible)
      {
        component.BakeMesh();
        buffer.DrawRenderer((Renderer) spriteShapeRenderer, spriteShapeRenderer.sharedMaterial);
      }
    }
    buffer.ReleaseTemporaryRT(0);
    Graphics.ExecuteCommandBuffer(buffer);
  }

  public void RevealDoor(Door door, LoreTotem totem)
  {
    this.StartCoroutine((IEnumerator) this.RevealDoorIE(door, totem));
  }

  public IEnumerator RevealDoorIE(Door door, LoreTotem totem)
  {
    GenerateRoom coroutineSupport = this;
    while (!BiomeGenerator.Instance.CurrentRoom.Completed || Health.team2.Count > 0)
    {
      if (Health.team2.Count <= 0)
      {
        yield return (object) new WaitForSeconds(1f);
        if (Health.team2.Count <= 0)
          break;
      }
      else if (BiomeGenerator.Instance.CurrentRoom.Completed)
      {
        bool flag = true;
        foreach (UnityEngine.Object @object in Health.team2)
        {
          if (@object != (UnityEngine.Object) null)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          break;
      }
      yield return (object) null;
    }
    if (BiomeGenerator.Instance.CurrentRoom.N_Room.ConnectionType == GenerateRoom.ConnectionTypes.LoreStoneRoom)
      BiomeGenerator.Instance.CurrentRoom.N_Room.Room.Hidden = false;
    if (BiomeGenerator.Instance.CurrentRoom.S_Room.ConnectionType == GenerateRoom.ConnectionTypes.LoreStoneRoom)
      BiomeGenerator.Instance.CurrentRoom.S_Room.Room.Hidden = false;
    if (BiomeGenerator.Instance.CurrentRoom.E_Room.ConnectionType == GenerateRoom.ConnectionTypes.LoreStoneRoom)
      BiomeGenerator.Instance.CurrentRoom.E_Room.Room.Hidden = false;
    if (BiomeGenerator.Instance.CurrentRoom.W_Room.ConnectionType == GenerateRoom.ConnectionTypes.LoreStoneRoom)
      BiomeGenerator.Instance.CurrentRoom.W_Room.Room.Hidden = false;
    coroutineSupport.revealingDoor = door;
    coroutineSupport.revalingDoorComplete = false;
    AudioManager.Instance.PlayOneShot("event:/door/boss_door_piece");
    Vector3 doorDir = door.transform.position.normalized;
    CameraFollowTarget.Instance.AddTarget(door.gameObject, 0.5f);
    if ((bool) (UnityEngine.Object) PlayerFarming.Instance && (bool) (UnityEngine.Object) PlayerFarming.Instance.playerWeapon)
      PlayerFarming.Instance.playerWeapon.StopHeavyAttackRoutine();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/rituals/enlightenment_beam");
    LoreTotem target = door.GetComponentInChildren<LoreTotem>();
    totem.ActivateRay(target);
    target.Activate();
    yield return (object) new WaitForSeconds(0.5f);
    MMVibrate.Rumble(0.1f, 0.4f, 2f, (MonoBehaviour) coroutineSupport);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 5.5f);
    yield return (object) new WaitForSeconds(1.5f);
    totem.DeactivateRay();
    target.DeactivateRay();
    totem.transform.DOMoveZ(2f, 4.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      totem.totemLeaveVFX.Stop();
      totem.gameObject.SetActive(false);
    }));
    totem.transform.DOShakeScale(4f, 0.05f);
    totem.totemLeaveVFX.Play();
    target.transform.DOMoveZ(2f, 4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      target.totemLeaveVFX.Stop();
      target.gameObject.SetActive(false);
    }));
    target.transform.DOShakeScale(4f, 0.05f);
    target.totemLeaveVFX.Play();
    for (int index = coroutineSupport.SceneryTransform.transform.childCount - 1; index >= 0; --index)
    {
      if (!coroutineSupport.SceneryTransform.transform.GetChild(index).name.Contains("Perlin") && (double) Vector3.Distance(coroutineSupport.SceneryTransform.transform.GetChild(index).position, door.transform.position - doorDir) < 3.0)
        coroutineSupport.SceneryTransform.transform.GetChild(index).DOMove(coroutineSupport.SceneryTransform.transform.GetChild(index).position + doorDir * 3f, 4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    }
    coroutineSupport.SetRevealedDoorSpriteShape(2f);
    door.Init(GenerateRoom.ConnectionTypes.LoreStoneRoom);
    yield return (object) new WaitForSeconds(2f);
    PlayerFarming.SetStateForAllPlayers();
    CameraFollowTarget.Instance.RemoveTarget(door.gameObject);
    coroutineSupport.revealingDoor = (Door) null;
    MiniMap.Instance.OnChangeRoom();
  }

  public void SetRevealedDoorSpriteShape(float fadeDuration = -1f)
  {
    foreach (Door componentsInChild in this.transform.GetComponentsInChildren<Door>())
    {
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) this.revealingDoor)
        componentsInChild.transform.parent.GetComponentInChildren<PolygonCollider2D>().gameObject.SetActive(false);
    }
    this.CompositeColliders();
    this.RoomTransform.geometryType = CompositeCollider2D.GeometryType.Polygons;
    this.RoomTransform.GenerateGeometry();
    Physics2D.SyncTransforms();
    this.CreateSpriteShape(fadeDuration, this.revealingDoor.direction == Door.Direction.East || this.revealingDoor.direction == Door.Direction.West);
    this.InitSpriteShapes();
    foreach (Door componentsInChild in this.transform.GetComponentsInChildren<Door>())
    {
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) this.revealingDoor)
        componentsInChild.transform.parent.GetComponentInChildren<PolygonCollider2D>(true).gameObject.SetActive(true);
    }
    this.SetCollider();
    this.revalingDoorComplete = true;
  }

  public void DisableDecorationsNearDoor(Door door)
  {
    for (int index = this.SceneryTransform.transform.childCount - 1; index >= 0; --index)
    {
      if ((double) Vector3.Distance(this.SceneryTransform.transform.GetChild(index).position, door.transform.position - door.transform.position.normalized) < 3.0)
        this.SceneryTransform.transform.GetChild(index).gameObject.SetActive(false);
    }
  }

  public void SetCollider()
  {
    this.RoomTransform.geometryType = CompositeCollider2D.GeometryType.Outlines;
    this.RoomTransform.gameObject.layer = LayerMask.NameToLayer("Island");
    this.RoomTransform.GenerateGeometry();
    GenerateRoom.GenerateEvent onSetCollider = this.OnSetCollider;
    if (onSetCollider == null)
      return;
    onSetCollider();
  }

  public void SetColliderAndUpdatePathfinding()
  {
    this.SetCollider();
    if (this.Testing)
    {
      this.RoomTransform.enabled = false;
      Physics2D.SyncTransforms();
    }
    if (!Application.isPlaying || !(bool) (UnityEngine.Object) AstarPath.active)
      return;
    this.StartCoroutine((IEnumerator) this.SetAStar());
  }

  public void UpdateAstar() => this.StartCoroutine((IEnumerator) this.SetAStar());

  public IEnumerator SetAStar()
  {
    GenerateRoom generateRoom = this;
    generateRoom.GeneratedPathing = false;
    if (generateRoom.regenerateNavOnEnable)
      yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null || AstarPath.active.data.gridGraph == null)
      yield return (object) null;
    GridGraph gridGraph1 = AstarPath.active.data.gridGraph;
    Bounds bounds = generateRoom.RoomTransform.bounds;
    Vector3 center = bounds.center;
    gridGraph1.center = center;
    GridGraph gridGraph2 = AstarPath.active.data.gridGraph;
    bounds = generateRoom.RoomTransform.bounds;
    int width = (int) bounds.size.x * 2 + generateRoom.Margin;
    bounds = generateRoom.RoomTransform.bounds;
    int depth = (int) bounds.size.y * 2 + generateRoom.Margin;
    gridGraph2.SetDimensions(width, depth, 0.5f);
    GameManager.RecalculatePaths(true, callback: new System.Action(generateRoom.\u003CSetAStar\u003Eb__235_0));
  }

  public void CreatePaths()
  {
    this.Paths = new List<GenerateRoom.RoomPath>();
    if (this.North != GenerateRoom.ConnectionTypes.False)
      this.Paths.Add(new GenerateRoom.RoomPath(IslandConnector.Direction.North, true, this.North));
    if (this.East != GenerateRoom.ConnectionTypes.False)
      this.Paths.Add(new GenerateRoom.RoomPath(IslandConnector.Direction.East, true, this.East));
    if (this.South != GenerateRoom.ConnectionTypes.False)
      this.Paths.Add(new GenerateRoom.RoomPath(IslandConnector.Direction.South, true, this.South));
    if (this.West != GenerateRoom.ConnectionTypes.False)
      this.Paths.Add(new GenerateRoom.RoomPath(IslandConnector.Direction.West, true, this.West));
    int num = this.CreateOnlyOneEncounterRoom ? 0 : this.RandomSeed.Next(1, 3);
    while (--num > 0)
      ++this.Paths[this.RandomSeed.Next(0, this.Paths.Count)].Encounters;
    foreach (GenerateRoom.RoomPath path in this.Paths)
      this.GeneratePath(path);
  }

  public IslandConnector.Direction PathGetUnusedDirection()
  {
    List<IslandConnector.Direction> directionList = new List<IslandConnector.Direction>()
    {
      IslandConnector.Direction.North,
      IslandConnector.Direction.East,
      IslandConnector.Direction.South,
      IslandConnector.Direction.West
    };
    foreach (GenerateRoom.RoomPath path in this.Paths)
      directionList.Remove(path.Direction);
    return directionList[this.RandomSeed.Next(0, directionList.Count)];
  }

  public void CompositeColliders()
  {
    foreach (IslandPiece piece in this.Pieces)
      piece.Collider.usedByComposite = true;
  }

  public IEnumerator DisableIslands()
  {
    GenerateRoom generateRoom = this;
    int completed = 0;
    foreach (IslandPiece piece in generateRoom.Pieces)
      generateRoom.StartCoroutine((IEnumerator) piece.InitIsland(generateRoom.RandomSeed, generateRoom.DecorationList.SpriteShapeSecondary, (System.Action) (() => ++completed)));
    while (completed < generateRoom.Pieces.Count)
      yield return (object) null;
  }

  public void CustomLevel()
  {
    this.RandomSeed = new System.Random(UnityEngine.Random.Range(-2147483647 /*0x80000001*/, int.MaxValue));
    this.Pieces = new List<IslandPiece>();
    int index = -1;
    while (++index < this.RoomTransform.transform.childCount)
    {
      IslandPiece component = this.RoomTransform.transform.GetChild(index).GetComponent<IslandPiece>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        this.Pieces.Add(component);
    }
    this.CompositeColliders();
    this.RoomTransform.geometryType = CompositeCollider2D.GeometryType.Polygons;
    this.RoomTransform.GenerateGeometry();
    Physics2D.SyncTransforms();
    this.ClearPrefabs(false);
    IEnumerator enumerator = (IEnumerator) this.RoomTransform.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if (current.name.Contains("Sprite shape"))
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) current.gameObject);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.PlaceDecorations(true);
    this.CreateSpriteShape();
    this.StartCoroutine((IEnumerator) this.SpawnDecorations(true));
    this.CreateBackgroundSpriteShape();
    foreach (IslandPiece piece in this.Pieces)
      piece.HideSprites();
    this.SetColliderAndUpdatePathfinding();
  }

  public void CreateBackgroundSpriteShape()
  {
    if (!((UnityEngine.Object) this.DecorationList.SpriteShapeBack != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.RoomSpriteShape == (UnityEngine.Object) null)
      this.RoomSpriteShape = this.RoomTransform.GetComponentInChildren<SpriteShapeController>();
    GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Room Back Sprite"), new Vector3(0.0f, 0.0f, 0.01f), Quaternion.identity, this.RoomTransform.transform) as GameObject;
    gameObject.transform.localScale = new Vector3(this.LimitEast - this.LimitWest, this.LimitNorth - this.LimitSouth) * 2f;
    SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
    component.shadowCastingMode = ShadowCastingMode.Off;
    component.receiveShadows = false;
    component.sortingLayerName = "Island";
    if (!this.DecorationList.overrideBackSprite)
      return;
    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, this.DecorationList.backSpriteShapeZOffset);
    component.material = this.DecorationList.overrideBackSpriteMaterial;
    component.transform.localScale *= this.DecorationList.spriteBackScaleMultiplier;
    if (!this.DecorationList.createRoomMesh)
      return;
    Mesh compositeCollider = ColliderToMeshUtility.GenerateMeshFromCompositeCollider(this.RoomTransform, this.DecorationList.backSpriteShapeZOffset);
    GameObject target = new GameObject();
    target.transform.SetParent(this.RoomSpriteShape.transform);
    ColliderToMeshUtility.ApplyMeshToRenderer(target, compositeCollider, this.DecorationList.roomMeshMaterial);
  }

  public void CreateSpriteShape(float scaleDuration = -1f, bool horizontalDir = true)
  {
    int index1 = -1;
    while (++index1 < this.RoomTransform.pathCount)
    {
      GameObject gameObject = new GameObject();
      gameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0001f);
      gameObject.transform.parent = this.RoomTransform.transform;
      gameObject.name = "Sprite shape " + index1.ToString();
      this.RoomSpriteShape = gameObject.AddComponent<SpriteShapeController>();
      this.RoomSpriteShape.spriteShape = this.DecorationList.SpriteShape;
      if ((UnityEngine.Object) this.DecorationList.SpriteShapeMaterial != (UnityEngine.Object) null)
      {
        Material[] sharedMaterials = this.RoomSpriteShape.spriteShapeRenderer.sharedMaterials;
        for (int index2 = 0; index2 < sharedMaterials.Length; ++index2)
          sharedMaterials[index2] = this.DecorationList.SpriteShapeMaterial;
        this.RoomSpriteShape.spriteShapeRenderer.sharedMaterials = sharedMaterials;
      }
      this.RoomSpriteShape.spriteShapeRenderer.shadowCastingMode = ShadowCastingMode.Off;
      this.RoomSpriteShape.spriteShapeRenderer.receiveShadows = true;
      this.RoomSpriteShape.fillPixelsPerUnit = 200f;
      this.RoomSpriteShape.gameObject.layer = 17;
      this.RoomSpriteShape.spline.Clear();
      this.RoomSpriteShape.splineDetail = 4;
      this.RoomSpriteShape.spriteShapeRenderer.sortingLayerName = "Ground";
      Vector2[] points = new Vector2[this.RoomTransform.GetPathPointCount(index1)];
      this.RoomTransform.GetPath(index1, points);
      Array.Reverse<Vector2>(points);
      int index3 = 0;
      this.RoomSpriteShape.spline.InsertPointAt(0, (Vector3) points[0]);
      while (++index3 < points.Length)
      {
        if ((double) Vector2.Distance((Vector2) this.RoomTransform.transform.TransformPoint((Vector3) points[index3]), (Vector2) this.RoomTransform.transform.TransformPoint((Vector3) points[index3 - 1])) > 0.10000000149011612)
          this.RoomSpriteShape.spline.InsertPointAt(this.RoomSpriteShape.spline.GetPointCount() - 1, this.RoomTransform.transform.TransformPoint((Vector3) points[index3]));
      }
      if ((double) scaleDuration != -1.0)
      {
        this.RoomSpriteShape.spriteShapeRenderer.sortingOrder = -1;
        this.RoomSpriteShape.transform.localScale = new Vector3(horizontalDir ? 0.0f : 1f, horizontalDir ? 1f : 0.0f, 1f);
        this.RoomSpriteShape.transform.DOScale(Vector3.one, scaleDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      }
    }
  }

  public void CollateLists()
  {
    this.NorthIslandPieces = new List<IslandPiece>();
    this.EastIslandPieces = new List<IslandPiece>();
    this.SouthIslandPieces = new List<IslandPiece>();
    this.WestIslandPieces = new List<IslandPiece>();
    foreach (IslandPiece islandPiece in this.IslandPieces)
    {
      if (islandPiece.GetConnectorsDirection(IslandConnector.Direction.North, false).Count > 0)
        this.NorthIslandPieces.Add(islandPiece);
      if (islandPiece.GetConnectorsDirection(IslandConnector.Direction.East, false).Count > 0)
        this.EastIslandPieces.Add(islandPiece);
      if (islandPiece.GetConnectorsDirection(IslandConnector.Direction.South, false).Count > 0)
        this.SouthIslandPieces.Add(islandPiece);
      if (islandPiece.GetConnectorsDirection(IslandConnector.Direction.West, false).Count > 0)
        this.WestIslandPieces.Add(islandPiece);
    }
    this.NorthIslandEncounterPieces = new List<IslandPiece>();
    this.EastIslandEncounterPieces = new List<IslandPiece>();
    this.SouthIslandEncounterPieces = new List<IslandPiece>();
    this.WestIslandEncounterPieces = new List<IslandPiece>();
    foreach (IslandPiece startPiece in this.StartPieces)
    {
      if (startPiece.GetConnectorsDirection(IslandConnector.Direction.North, false).Count > 0)
        this.NorthIslandEncounterPieces.Add(startPiece);
      if (startPiece.GetConnectorsDirection(IslandConnector.Direction.East, false).Count > 0)
        this.EastIslandEncounterPieces.Add(startPiece);
      if (startPiece.GetConnectorsDirection(IslandConnector.Direction.South, false).Count > 0)
        this.SouthIslandEncounterPieces.Add(startPiece);
      if (startPiece.GetConnectorsDirection(IslandConnector.Direction.West, false).Count > 0)
        this.WestIslandEncounterPieces.Add(startPiece);
    }
    this.NorthIslandResourcesPieces = new List<IslandPiece>();
    this.EastIslandResourcesPieces = new List<IslandPiece>();
    this.SouthIslandResourcesPieces = new List<IslandPiece>();
    this.WestIslandResourcesPieces = new List<IslandPiece>();
    foreach (IslandPiece resourcePiece in this.ResourcePieces)
    {
      if (resourcePiece.GetConnectorsDirection(IslandConnector.Direction.North, false).Count > 0)
        this.NorthIslandResourcesPieces.Add(resourcePiece);
      if (resourcePiece.GetConnectorsDirection(IslandConnector.Direction.East, false).Count > 0)
        this.EastIslandResourcesPieces.Add(resourcePiece);
      if (resourcePiece.GetConnectorsDirection(IslandConnector.Direction.South, false).Count > 0)
        this.SouthIslandResourcesPieces.Add(resourcePiece);
      if (resourcePiece.GetConnectorsDirection(IslandConnector.Direction.West, false).Count > 0)
        this.WestIslandResourcesPieces.Add(resourcePiece);
    }
  }

  public IslandPiece GetIslandListByDirection(IslandConnector.Direction Direction)
  {
    switch (Direction)
    {
      case IslandConnector.Direction.North:
        return this.NorthIslandPieces[this.RandomSeed.Next(0, this.NorthIslandPieces.Count)];
      case IslandConnector.Direction.East:
        return this.EastIslandPieces[this.RandomSeed.Next(0, this.EastIslandPieces.Count)];
      case IslandConnector.Direction.South:
        return this.SouthIslandPieces[this.RandomSeed.Next(0, this.SouthIslandPieces.Count)];
      case IslandConnector.Direction.West:
        return this.WestIslandPieces[this.RandomSeed.Next(0, this.WestIslandPieces.Count)];
      default:
        return (IslandPiece) null;
    }
  }

  public IslandPiece GetIslandFromMultipleLists(
    IslandConnector.Direction Direction1,
    IslandConnector.Direction Direction2)
  {
    List<IslandPiece> islandPieceList = new List<IslandPiece>();
    switch (Direction1)
    {
      case IslandConnector.Direction.North:
        Debug.Log((object) "NORTH");
        switch (Direction2)
        {
          case IslandConnector.Direction.North:
            using (List<IslandPiece>.Enumerator enumerator = this.NorthIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.NorthIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.East:
            using (List<IslandPiece>.Enumerator enumerator = this.NorthIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.EastIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.South:
            using (List<IslandPiece>.Enumerator enumerator = this.NorthIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.SouthIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.West:
            using (List<IslandPiece>.Enumerator enumerator = this.NorthIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.WestIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
        }
        break;
      case IslandConnector.Direction.East:
        switch (Direction2)
        {
          case IslandConnector.Direction.North:
            using (List<IslandPiece>.Enumerator enumerator = this.EastIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.NorthIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.East:
            using (List<IslandPiece>.Enumerator enumerator = this.EastIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.EastIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.South:
            using (List<IslandPiece>.Enumerator enumerator = this.EastIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.SouthIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.West:
            using (List<IslandPiece>.Enumerator enumerator = this.EastIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.WestIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
        }
        break;
      case IslandConnector.Direction.South:
        Debug.Log((object) "SOUTH");
        switch (Direction2)
        {
          case IslandConnector.Direction.North:
            using (List<IslandPiece>.Enumerator enumerator = this.SouthIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.NorthIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.East:
            using (List<IslandPiece>.Enumerator enumerator = this.SouthIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.EastIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.South:
            using (List<IslandPiece>.Enumerator enumerator = this.SouthIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.SouthIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.West:
            using (List<IslandPiece>.Enumerator enumerator = this.SouthIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.WestIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
        }
        break;
      case IslandConnector.Direction.West:
        switch (Direction2)
        {
          case IslandConnector.Direction.North:
            using (List<IslandPiece>.Enumerator enumerator = this.WestIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.NorthIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.East:
            using (List<IslandPiece>.Enumerator enumerator = this.WestIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.EastIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.South:
            using (List<IslandPiece>.Enumerator enumerator = this.WestIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.SouthIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
          case IslandConnector.Direction.West:
            using (List<IslandPiece>.Enumerator enumerator = this.WestIslandPieces.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandPiece current = enumerator.Current;
                if (this.WestIslandPieces.Contains(current))
                  islandPieceList.Add(current);
              }
              break;
            }
        }
        break;
    }
    Debug.Log((object) ("AvailableConnectors.Count " + islandPieceList.Count.ToString()));
    return islandPieceList[this.RandomSeed.Next(0, islandPieceList.Count)];
  }

  public IslandPiece GetRandomEncounterIsland()
  {
    List<IslandPiece> islandPieceList = new List<IslandPiece>();
    foreach (IslandPiece startPiece in this.StartPieces)
    {
      int num1 = 0;
      foreach (IslandPiece.GameObjectAndProbability objectAndProbability in startPiece.Encounters.ObjectList)
      {
        if (objectAndProbability.AvailableOnLayer())
          ++num1;
      }
      int num2 = 0;
      foreach (IslandPiece.GameObjectAndProbability objectAndProbability in startPiece.Encounters.ObjectList)
      {
        if (BiomeGenerator.EncounterAlreadyUsed(objectAndProbability.GameObjectPath) && objectAndProbability.AvailableOnLayer())
          ++num2;
      }
      if (num2 < num1)
        islandPieceList.Add(startPiece);
    }
    if (islandPieceList.Count <= 0)
    {
      Debug.Log((object) "We've used all the island's encounters - RESET AND START AGAIN!");
      if (this.StartPieces.Count > 1)
        BiomeGenerator.UsedEncounters.Clear();
      return this.StartPieces[this.RandomSeed.Next(0, this.StartPieces.Count)];
    }
    Debug.Log((object) ("Remaing islands with encounters: " + islandPieceList.Count.ToString()));
    return islandPieceList[this.RandomSeed.Next(0, islandPieceList.Count)];
  }

  public IslandPiece GetEncounterIslandListByDirection(IslandConnector.Direction Direction)
  {
    if (this.ResourcePieces.Count > 0 && this.RandomSeed.NextDouble() >= (double) this.EncounterWillBeEnemyOrResource)
      return this.GetResourceIslandListByDirection(Direction);
    switch (Direction)
    {
      case IslandConnector.Direction.North:
        return this.NorthIslandEncounterPieces[this.RandomSeed.Next(0, this.NorthIslandEncounterPieces.Count)];
      case IslandConnector.Direction.East:
        return this.EastIslandEncounterPieces[this.RandomSeed.Next(0, this.EastIslandEncounterPieces.Count)];
      case IslandConnector.Direction.South:
        return this.SouthIslandEncounterPieces[this.RandomSeed.Next(0, this.SouthIslandEncounterPieces.Count)];
      case IslandConnector.Direction.West:
        return this.WestIslandEncounterPieces[this.RandomSeed.Next(0, this.WestIslandEncounterPieces.Count)];
      default:
        return (IslandPiece) null;
    }
  }

  public IslandPiece GetResourceIslandListByDirection(IslandConnector.Direction Direction)
  {
    switch (Direction)
    {
      case IslandConnector.Direction.North:
        return this.NorthIslandResourcesPieces[this.RandomSeed.Next(0, this.NorthIslandResourcesPieces.Count)];
      case IslandConnector.Direction.East:
        return this.EastIslandResourcesPieces[this.RandomSeed.Next(0, this.EastIslandResourcesPieces.Count)];
      case IslandConnector.Direction.South:
        return this.SouthIslandResourcesPieces[this.RandomSeed.Next(0, this.SouthIslandResourcesPieces.Count)];
      case IslandConnector.Direction.West:
        return this.WestIslandResourcesPieces[this.RandomSeed.Next(0, this.WestIslandResourcesPieces.Count)];
      default:
        return (IslandPiece) null;
    }
  }

  public void PlaceDecorations(bool CustomLevel)
  {
    if (CustomLevel)
      this.DecorationList = !((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null) || BiomeGenerator.Instance.BiomeDecorationSet.Count <= 0 ? this.DecorationSetList[this.RandomSeed.Next(0, this.DecorationSetList.Count)] : BiomeGenerator.Instance.BiomeDecorationSet[this.RandomSeed.Next(0, BiomeGenerator.Instance.BiomeDecorationSet.Count)];
    else if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
    {
      if (BiomeGenerator.Instance.BiomeDecorationSet != null && BiomeGenerator.Instance.BiomeDecorationSet.Count > 0)
        this.DecorationList = BiomeGenerator.Instance.BiomeDecorationSet[Mathf.Clamp(Mathf.Min(GameManager.CurrentDungeonLayer - 1, BiomeGenerator.Instance.BiomeDecorationSet.Count), 0, BiomeGenerator.Instance.BiomeDecorationSet.Count - 1)];
    }
    else if (Application.isEditor && !Application.isPlaying)
    {
      this.DecorationList = this.DecorationSetList[0];
    }
    else
    {
      Debug.Log((object) $"DecorationSetList {this.DecorationSetList?.ToString()}   GameManager.CurrentDungeonLayer - 1{(GameManager.CurrentDungeonLayer - 1).ToString()}");
      this.DecorationList = this.DecorationSetList[Mathf.Min(GameManager.CurrentDungeonLayer - 1, this.DecorationSetList.Count - 1)];
    }
    this.DecorationGrid = new List<List<int>>();
    this.DecorationGridWidth = (int) Mathf.Max(this.RoomTransform.bounds.size.x, this.RoomTransform.bounds.size.y);
    this.DecorationGridHeight = this.DecorationGridWidth;
    for (int index = -this.DecorationGridHeight; index < this.DecorationGridHeight; index += this.Scale)
    {
      List<int> intList = new List<int>();
      for (int x = -this.DecorationGridWidth; x < this.DecorationGridWidth; x += this.Scale)
      {
        if (this.RoomTransform.ClosestPoint(new Vector2((float) x, (float) index - (float) this.Scale * 0.5f)) != new Vector2((float) x, (float) index - (float) this.Scale * 0.5f) && this.RoomTransform.ClosestPoint(new Vector2((float) x, (float) index + (float) this.Scale * 0.5f)) != new Vector2((float) x, (float) index + (float) this.Scale * 0.5f))
          intList.Add(1);
        else
          intList.Add(0);
      }
      this.DecorationGrid.Add(intList);
    }
    for (int index1 = 0; index1 < this.DecorationGrid.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.DecorationGrid.Count; ++index2)
      {
        for (int index3 = -1; index3 <= 1; ++index3)
        {
          for (int index4 = -1; index4 <= 1; ++index4)
          {
            if (this.DecorationGrid[index2][index1] != 0 && (index3 != 0 || index4 != 0) && index2 + index3 >= 0 && index1 + index4 >= 0 && index2 + index3 < this.DecorationGrid.Count && index1 + index4 < this.DecorationGrid.Count && this.DecorationGrid[index2 + index3][index1 + index4] == 0)
              this.DecorationGrid[index2][index1] = 2;
          }
        }
      }
    }
    int num1 = 3;
    for (int index5 = 0; index5 < this.DecorationGrid.Count; ++index5)
    {
      for (int index6 = 0; index6 < this.DecorationGrid.Count; ++index6)
      {
        bool flag = true;
        for (int index7 = 0; index7 < num1; ++index7)
        {
          for (int index8 = 0; index8 < num1; ++index8)
          {
            if (this.DecorationGrid[index6][index5] != 1 || index6 + index7 >= this.DecorationGrid.Count || index5 + index8 >= this.DecorationGrid.Count || this.DecorationGrid[index6 + index7][index5 + index8] != 1)
              flag = false;
          }
        }
        if (flag)
        {
          this.DecorationGrid[index6][index5] = 4;
          for (int index9 = 0; index9 < num1; ++index9)
          {
            for (int index10 = 0; index10 < num1; ++index10)
            {
              if (index9 != 0 || index10 != 0)
                this.DecorationGrid[index6 + index9][index5 + index10] = 999;
            }
          }
        }
      }
    }
    int num2 = 2;
    for (int index11 = 0; index11 < this.DecorationGrid.Count; ++index11)
    {
      for (int index12 = 0; index12 < this.DecorationGrid.Count; ++index12)
      {
        bool flag = true;
        for (int index13 = 0; index13 < num2; ++index13)
        {
          for (int index14 = 0; index14 < num2; ++index14)
          {
            if (this.DecorationGrid[index12][index11] != 1 || index12 + index13 >= this.DecorationGrid.Count || index11 + index14 >= this.DecorationGrid.Count || this.DecorationGrid[index12 + index13][index11 + index14] != 1)
              flag = false;
          }
        }
        if (flag)
        {
          this.DecorationGrid[index12][index11] = 3;
          for (int index15 = 0; index15 < num2; ++index15)
          {
            for (int index16 = 0; index16 < num2; ++index16)
            {
              if (index15 != 0 || index16 != 0)
                this.DecorationGrid[index12 + index15][index11 + index16] = 999;
            }
          }
        }
        else if (this.DecorationGrid[index12][index11] == 1)
          this.DecorationGrid[index12][index11] = 2;
      }
    }
  }

  public void PlaceNoiseDecorations(float x, float y, Vector3 Position)
  {
    foreach (GeneraterDecorations.DecorationPerlinSpriteShape decorationAndProabily in this.DecorationList.DecorationPerlinSpriteShapePrimary.DecorationAndProabilies)
      this.PrimarySpriteShapeNoiseDecortations(decorationAndProabily, x, y, Position + new Vector3(UnityEngine.Random.Range((float) -this.Scale * 0.25f, (float) this.Scale * 0.25f), UnityEngine.Random.Range((float) -this.Scale * 0.25f, (float) this.Scale * 0.25f)));
    foreach (GeneraterDecorations.DecorationPerlinSpriteShape decorationAndProabily in this.DecorationList.DecorationPerlinSpriteShapeSecondary.DecorationAndProabilies)
      this.SecondarySpriteShapeNoiseDecortations(decorationAndProabily, x, y, Position + new Vector3(UnityEngine.Random.Range((float) -this.Scale * 0.25f, (float) this.Scale * 0.25f), UnityEngine.Random.Range((float) -this.Scale * 0.25f, (float) this.Scale * 0.25f)));
    this.Noise = Mathf.PerlinNoise((x / (float) this.DecorationGridWidth + this.RoomPerlinOffset.x) * this.DecorationList.NoiseScale, (y / (float) this.DecorationGridHeight + this.RoomPerlinOffset.y) * this.DecorationList.NoiseScale);
    if ((double) this.Noise < (double) this.DecorationList.NoiseThreshold)
      return;
    Position += new Vector3(UnityEngine.Random.Range((float) -this.Scale * 0.25f, (float) this.Scale * 0.25f), UnityEngine.Random.Range((float) -this.Scale * 0.25f, (float) this.Scale * 0.25f));
    if (this.RoomTransform.ClosestPoint((Vector2) Position) == (Vector2) Position)
    {
      this.d = this.DecorationList.DecorationPerlinNoiseOnPath.GetRandomGameObject(this.RandomSeed.NextDouble());
      this.NoiseName = "PerlinOnPath";
    }
    else
    {
      this.d = this.DecorationList.DecorationPerlinNoiseOffPath.GetRandomGameObject(this.RandomSeed.NextDouble());
      this.NoiseName = "PerlinOffPath";
    }
    if (this.d == null)
      return;
    ObjectPool.Spawn(this.d.ObjectPath, Position + this.d.GetRandomOffset(), Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj =>
    {
      this.PerlinNoiseDecoration = obj;
      this.PerlinNoiseDecoration.name = this.NoiseName;
      this.PerlinScale = this.PerlinNoiseDecoration.transform.localScale;
      this.PerlinScale.z *= this.Noise * 1.4f;
      this.PerlinNoiseDecoration.transform.localScale = this.PerlinScale;
    }));
  }

  public void PlacePerlinCritters(float x, float y, Vector3 Position)
  {
    this.Noise = Mathf.PerlinNoise(x / (float) this.DecorationGridWidth * this.DecorationList.NoiseScale, y / (float) this.DecorationGridHeight * this.DecorationList.NoiseScale);
    if (this.DecorationList.Critters.DecorationAndProabilies.Count <= 0 || (double) this.Noise > (double) this.DecorationList.CritterThreshold || (double) Vector3.Distance((Vector3) this.RoomTransform.ClosestPoint((Vector2) Position), (Vector3) (Vector2) Position) >= 1.0 || (double) UnityEngine.Random.value >= 0.6600000262260437)
      return;
    this.d = this.DecorationList.Critters.GetRandomGameObject(this.RandomSeed.NextDouble());
    if (this.d.ObjectPath == "Assets/Resources_Moved/Prefabs/Units/Wild Life/Snail.prefab")
    {
      if (DataManager.GetFollowerSkinUnlocked("Snail"))
        return;
      int itemQuantity = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SHELL);
      if (DataManager.Instance.ShellsGifted_0)
        ++itemQuantity;
      if (DataManager.Instance.ShellsGifted_1)
        ++itemQuantity;
      if (DataManager.Instance.ShellsGifted_2)
        ++itemQuantity;
      if (DataManager.Instance.ShellsGifted_3)
        ++itemQuantity;
      if (DataManager.Instance.ShellsGifted_4)
        ++itemQuantity;
      if (itemQuantity > 4 || (double) UnityEngine.Random.value < 0.64999997615814209)
        return;
    }
    ObjectPool.Spawn(this.d.ObjectPath, Position + this.d.GetRandomOffset(), Quaternion.identity, this.transform, (Action<GameObject>) (obj => obj.name = "Critter"));
  }

  public List<SpriteShapeController> SpriteShapeControllers
  {
    get
    {
      if (this._SpriteShapeControllers == null)
        this._SpriteShapeControllers = new List<SpriteShapeController>((IEnumerable<SpriteShapeController>) this.gameObject.GetComponentsInChildren<SpriteShapeController>());
      return this._SpriteShapeControllers;
    }
  }

  public static GroundType GetGroundTypeFromPosition(Vector3 Position)
  {
    GenerateRoom instance = GenerateRoom.Instance;
    if ((UnityEngine.Object) instance == (UnityEngine.Object) null)
      return GroundType.Hard;
    GenerateRoom.CurrentSpriteShape = (SpriteShapeController) null;
    foreach (SpriteShapeController spriteShapeController in instance.SpriteShapeControllers)
    {
      if ((UnityEngine.Object) spriteShapeController != (UnityEngine.Object) null && (UnityEngine.Object) spriteShapeController.transform.parent != (UnityEngine.Object) instance.RoomTransform.transform && (UnityEngine.Object) spriteShapeController.transform.parent != (UnityEngine.Object) instance.SceneryTransform.transform)
      {
        GenerateRoom.Points = new List<Vector3>(spriteShapeController.spline.GetPointCount());
        for (int index = 0; index < spriteShapeController.spline.GetPointCount(); ++index)
          GenerateRoom.Points.Add(spriteShapeController.transform.TransformPoint(spriteShapeController.spline.GetPosition(index)));
        if (Utils.PointWithinPolygon(Position, GenerateRoom.Points) && ((UnityEngine.Object) GenerateRoom.CurrentSpriteShape == (UnityEngine.Object) null || spriteShapeController.spriteShapeRenderer.sortingLayerID > GenerateRoom.CurrentSpriteShape.spriteShapeRenderer.sortingLayerID))
          GenerateRoom.CurrentSpriteShape = spriteShapeController;
      }
    }
    if ((UnityEngine.Object) GenerateRoom.CurrentSpriteShape != (UnityEngine.Object) null)
      return SetSpriteshapeMaterial.Instance.GetGroundType(GenerateRoom.CurrentSpriteShape.spriteShape);
    if ((UnityEngine.Object) instance.RoomSpriteShape == (UnityEngine.Object) null)
      instance.RoomSpriteShape = instance.RoomTransform.GetComponentInChildren<SpriteShapeController>();
    return (UnityEngine.Object) SetSpriteshapeMaterial.Instance == (UnityEngine.Object) null || (UnityEngine.Object) instance == (UnityEngine.Object) null || (UnityEngine.Object) instance.RoomSpriteShape == (UnityEngine.Object) null || (UnityEngine.Object) instance.RoomSpriteShape.spriteShape == (UnityEngine.Object) null ? GroundType.None : SetSpriteshapeMaterial.Instance.GetGroundType(instance.RoomSpriteShape.spriteShape);
  }

  public void PrimarySpriteShapeNoiseDecortations(
    GeneraterDecorations.DecorationPerlinSpriteShape d,
    float x,
    float y,
    Vector3 Position)
  {
    this.Noise = Mathf.PerlinNoise((x / (float) this.DecorationGridWidth + d.PerlinOffset) * d.PerlinScale, (y / (float) this.DecorationGridHeight + d.PerlinOffset) * d.PerlinScale);
    if ((double) this.Noise < (double) d.PerlinThreshold || (UnityEngine.Object) this.RoomSpriteShape == (UnityEngine.Object) null)
      return;
    GenerateRoom.Points = new List<Vector3>();
    for (int index = 0; index < this.RoomSpriteShape.spline.GetPointCount(); ++index)
      GenerateRoom.Points.Add(this.RoomSpriteShape.spline.GetPosition(index));
    if (!Utils.PointWithinPolygon(Position, GenerateRoom.Points))
      return;
    List<Collider2D> collider2DList = new List<Collider2D>();
    foreach (IslandPiece piece in this.Pieces)
    {
      foreach (SpriteShapeController encounterSpriteShape in piece.EncounterSpriteShapes)
      {
        if (!((UnityEngine.Object) encounterSpriteShape == (UnityEngine.Object) null) && encounterSpriteShape.spline != null)
        {
          collider2DList.Add((Collider2D) encounterSpriteShape.polygonCollider);
          GenerateRoom.Points = new List<Vector3>();
          for (int index = 0; index < encounterSpriteShape.spline.GetPointCount(); ++index)
            GenerateRoom.Points.Add(piece.transform.position + encounterSpriteShape.spline.GetPosition(index));
          if (Utils.PointWithinPolygon(Position, GenerateRoom.Points))
            return;
        }
      }
      collider2DList.Add((Collider2D) piece.Collider);
    }
    Collider2D[] componentsInChildren = this.GetComponentsInChildren<Collider2D>();
    bool flag = true;
    foreach (Collider2D collider2D in componentsInChildren)
    {
      if (collider2D.OverlapPoint((Vector2) Position) && !collider2DList.Contains(collider2D) && (collider2D.gameObject.layer == LayerMask.NameToLayer("Obstacles") || collider2D.gameObject.layer == LayerMask.NameToLayer("ObstaclesAndPlayer") || collider2D.gameObject.layer == LayerMask.NameToLayer("Obstacles Player Ignore")))
      {
        flag = false;
        break;
      }
    }
    if (!flag)
      return;
    ObjectPool.Spawn(d.ObjectPath, Position + Vector3.back * 0.01f, Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj => obj.name = "Perlin Decoration - Primary SpriteShape"));
  }

  public void SecondarySpriteShapeNoiseDecortations(
    GeneraterDecorations.DecorationPerlinSpriteShape d,
    float x,
    float y,
    Vector3 Position)
  {
    this.Noise = Mathf.PerlinNoise((x / (float) this.DecorationGridWidth + d.PerlinOffset) * d.PerlinScale, (y / (float) this.DecorationGridHeight + d.PerlinOffset) * d.PerlinScale);
    if ((double) this.Noise < (double) d.PerlinThreshold)
      return;
    Collider2D[] componentsInChildren = this.GetComponentsInChildren<Collider2D>();
    foreach (IslandPiece piece in this.Pieces)
    {
      foreach (SpriteShapeController encounterSpriteShape in piece.EncounterSpriteShapes)
      {
        GenerateRoom.Points = new List<Vector3>();
        for (int index = 0; index < encounterSpriteShape.spline.GetPointCount(); ++index)
          GenerateRoom.Points.Add(piece.transform.position + encounterSpriteShape.spline.GetPosition(index));
        if (Utils.PointWithinPolygon(Position, GenerateRoom.Points))
        {
          bool flag = true;
          foreach (Collider2D collider2D in componentsInChildren)
          {
            if (!collider2D.OverlapPoint((Vector2) Position) && (UnityEngine.Object) collider2D != (UnityEngine.Object) encounterSpriteShape.polygonCollider && (collider2D.gameObject.layer == LayerMask.NameToLayer("Obstacles") || collider2D.gameObject.layer == LayerMask.NameToLayer("ObstaclesAndPlayer") || collider2D.gameObject.layer == LayerMask.NameToLayer("Obstacles Player Ignore")))
            {
              flag = false;
              break;
            }
          }
          if (flag)
            ObjectPool.Spawn(d.ObjectPath, Position + Vector3.back * 0.01f, Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj => obj.name = "Perlin Decoration - Secondary SpriteShape"));
        }
      }
    }
  }

  public IEnumerator SpawnDecorations(bool spawnInsideShapeDecorations, bool spawnCritters = false)
  {
    if (this.RandomSeed != null)
    {
      this.RoomPerlinOffset = new Vector2((float) this.RandomSeed.NextDouble(), (float) this.RandomSeed.NextDouble());
      for (int y = 0; y < this.DecorationGrid.Count; ++y)
      {
        for (int x = 0; x < this.DecorationGrid.Count; ++x)
        {
          bool waiting = true;
          Position = new Vector3((float) (x * this.Scale - this.DecorationGridWidth), (float) (y * this.Scale - this.DecorationGridHeight));
          Vector3 ClosestPosition = (Vector3) this.RoomTransform.ClosestPoint((Vector2) Position);
          if ((double) Vector3.Distance(ClosestPosition, Position) < (double) (6 * this.Scale))
          {
            float num = (float) this.Scale * 0.25f;
            if (spawnInsideShapeDecorations)
            {
              this.PlaceNoiseDecorations((float) x - num, (float) y - num, Position + new Vector3(-num, -num));
              this.PlaceNoiseDecorations((float) x - num, (float) y + num, Position + new Vector3(-num, num));
              this.PlaceNoiseDecorations((float) x + num, (float) y - num, Position + new Vector3(num, -num));
              this.PlaceNoiseDecorations((float) x + num, (float) y + num, Position + new Vector3(num, num));
              if (spawnCritters)
                this.PlacePerlinCritters((float) x, (float) y, BiomeGenerator.GetRandomPositionInIsland());
            }
            bool shouldSpawn = true;
            if (((this.DecorationGrid[y][x] != 2 ? 0 : (this.DecorationList.DecorationPiece.DecorationAndProabilies.Count > 0 ? 1 : 0)) & (shouldSpawn ? 1 : 0)) != 0)
            {
              this.d = this.DecorationList.DecorationPiece.GetRandomGameObject(this.RandomSeed.NextDouble());
              if ((double) Vector3.Distance((Vector3) this.RoomTransform.ClosestPoint((Vector2) Position + new Vector2((float) this.Scale * 0.5f, 0.0f)), (Vector3) ((Vector2) Position + new Vector2((float) this.Scale * 0.5f, 0.0f))) < 1.0)
                Position += Vector3.left * ((float) this.Scale * 0.5f);
              else if ((double) Vector3.Distance((Vector3) this.RoomTransform.ClosestPoint((Vector2) Position + new Vector2((float) -this.Scale * 0.5f, 0.0f)), (Vector3) ((Vector2) Position + new Vector2((float) -this.Scale * 0.5f, 0.0f))) < 1.0)
                Position += Vector3.right * ((float) this.Scale * 0.5f);
              waiting = true;
              ObjectPool.Spawn(this.d.ObjectPath, Position + this.d.GetRandomOffset(), Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj =>
              {
                g = obj;
                g.name = "1x1";
                this.CheckLimit(Position);
                waiting = false;
              }));
              while (waiting && !string.IsNullOrEmpty(this.d.ObjectPath))
                yield return (object) null;
            }
            if (((this.DecorationGrid[y][x] != 3 ? 0 : (this.DecorationList.DecorationPiece2x2.DecorationAndProabilies.Count > 0 ? 1 : 0)) & (shouldSpawn ? 1 : 0)) != 0)
            {
              waiting = true;
              this.d = this.DecorationList.DecorationPiece2x2.GetRandomGameObject(this.RandomSeed.NextDouble());
              ObjectPool.Spawn(this.d.ObjectPath, Position + this.d.GetRandomOffset() + new Vector3(1f, 1f), Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj =>
              {
                g = obj;
                g.name = "2x2";
                this.CheckLimit(Position);
                waiting = false;
              }));
              while (waiting && !string.IsNullOrEmpty(this.d.ObjectPath))
                yield return (object) null;
            }
            if (this.DecorationGrid[y][x] == 4)
            {
              if ((((double) ClosestPosition.y <= (double) Position.y ? 0 : (this.DecorationList.DecorationPiece3x3.DecorationAndProabilies.Count > 0 ? 1 : 0)) & (shouldSpawn ? 1 : 0)) != 0)
              {
                waiting = true;
                this.d = this.DecorationList.DecorationPiece3x3.GetRandomGameObject(this.RandomSeed.NextDouble());
                ObjectPool.Spawn(this.d.ObjectPath, Position + this.d.GetRandomOffset() + new Vector3(2f, 2f), Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj =>
                {
                  g = obj;
                  g.name = "3x3";
                  this.CheckLimit(Position);
                  waiting = false;
                }));
                while (waiting && !string.IsNullOrEmpty(this.d.ObjectPath))
                  yield return (object) null;
              }
              else if (this.DecorationList.DecorationPiece3x3Tall.DecorationAndProabilies.Count > 0 & shouldSpawn)
              {
                waiting = true;
                this.d = this.DecorationList.DecorationPiece3x3Tall.GetRandomGameObject(this.RandomSeed.NextDouble());
                ObjectPool.Spawn(this.d.ObjectPath, Position + this.d.GetRandomOffset() + new Vector3(2f, 2f), Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj =>
                {
                  g = obj;
                  g.name = "3x3Tall";
                  this.CheckLimit(Position);
                  waiting = false;
                }));
                while (waiting && string.IsNullOrEmpty(this.d.ObjectPath))
                  yield return (object) null;
              }
            }
          }
        }
      }
      this.GeneratedDecorations = true;
    }
  }

  public void CheckLimit(Vector3 Position)
  {
    if ((double) Position.y > (double) this.LimitNorth)
      this.LimitNorth = Position.y;
    if ((double) Position.y < (double) this.LimitSouth)
      this.LimitSouth = Position.y;
    if ((double) Position.x > (double) this.LimitEast)
      this.LimitEast = Position.x;
    if ((double) Position.x >= (double) this.LimitWest)
      return;
    this.LimitWest = Position.x;
  }

  public void ReplaceDecorations()
  {
    this.DecorationList = this.DecorationSetList[UnityEngine.Random.Range(0, this.DecorationSetList.Count)];
    List<GameObject> gameObjectList = new List<GameObject>();
    int childCount = this.SceneryTransform.transform.childCount;
    while (--childCount > 0)
      gameObjectList.Add(this.SceneryTransform.transform.GetChild(childCount).gameObject);
    this.d = (GeneraterDecorations.DecorationAndProbability) null;
    foreach (GameObject gameObject in gameObjectList)
    {
      GameObject g = gameObject;
      switch (g.name)
      {
        case "1x1":
          this.d = this.DecorationList.DecorationPiece.GetRandomGameObject((double) UnityEngine.Random.value);
          break;
        case "2x2":
          this.d = this.DecorationList.DecorationPiece2x2.GetRandomGameObject((double) UnityEngine.Random.value);
          break;
        case "3x3":
          this.d = this.DecorationList.DecorationPiece3x3.GetRandomGameObject((double) UnityEngine.Random.value);
          break;
        case "3x3Tall":
          this.d = this.DecorationList.DecorationPiece3x3Tall.GetRandomGameObject((double) UnityEngine.Random.value);
          break;
        case "Critter":
          this.d = this.DecorationList.Critters.GetRandomGameObject((double) UnityEngine.Random.value);
          break;
        case "PerlinOffPath":
          this.d = this.DecorationList.DecorationPerlinNoiseOffPath.GetRandomGameObject((double) UnityEngine.Random.value);
          break;
        case "PerlinOnPath":
          this.d = this.DecorationList.DecorationPerlinNoiseOnPath.GetRandomGameObject((double) UnityEngine.Random.value);
          break;
      }
      if (this.d != null)
        ObjectPool.Spawn(this.d.ObjectPath, g.transform.position + this.d.GetRandomOffset() + new Vector3(0.0f, 0.0f, this.DecorationList.backSpriteShapeZOffset), Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj =>
        {
          obj.name = g.name;
          this.CheckLimit(g.transform.position + this.d.GetRandomOffset());
        }));
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) g);
    }
    this.CreateBackgroundSpriteShape();
  }

  public void CreateStartPiece()
  {
    this.StartPiece = UnityEngine.Object.Instantiate<IslandPiece>(this.GetRandomEncounterIsland(), Vector3.zero, Quaternion.identity, this.RoomTransform.transform);
    this.Pieces.Add(this.StartPiece);
  }

  public void PlaceDoors()
  {
    foreach (GenerateRoom.RoomPath path in this.Paths)
    {
      if (path.Door)
      {
        List<IslandConnector> islandConnectorList = new List<IslandConnector>();
        foreach (IslandPiece piece in this.Pieces)
        {
          foreach (IslandConnector islandConnector in piece.GetConnectorsDirection(path.Direction, false))
            islandConnectorList.Add(islandConnector);
        }
        IslandConnector islandConnector1 = (IslandConnector) null;
        float num1 = float.MaxValue;
        switch (path.Direction)
        {
          case IslandConnector.Direction.North:
            float num2 = float.MinValue;
            using (List<IslandConnector>.Enumerator enumerator = islandConnectorList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandConnector current = enumerator.Current;
                if ((double) current.transform.position.y > (double) num2)
                {
                  num2 = current.transform.position.y;
                  islandConnector1 = current;
                }
              }
              break;
            }
          case IslandConnector.Direction.East:
            float num3 = float.MinValue;
            using (List<IslandConnector>.Enumerator enumerator = islandConnectorList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandConnector current = enumerator.Current;
                if ((double) current.transform.position.x > (double) num3)
                {
                  num3 = current.transform.position.x;
                  islandConnector1 = current;
                }
              }
              break;
            }
          case IslandConnector.Direction.South:
            using (List<IslandConnector>.Enumerator enumerator = islandConnectorList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandConnector current = enumerator.Current;
                if ((double) current.transform.position.y < (double) num1)
                {
                  num1 = current.transform.position.y;
                  islandConnector1 = current;
                }
              }
              break;
            }
          case IslandConnector.Direction.West:
            using (List<IslandConnector>.Enumerator enumerator = islandConnectorList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IslandConnector current = enumerator.Current;
                if ((double) current.transform.position.x < (double) num1)
                {
                  num1 = current.transform.position.x;
                  islandConnector1 = current;
                }
              }
              break;
            }
        }
        if ((UnityEngine.Object) islandConnector1 != (UnityEngine.Object) null && islandConnector1.ParentIsland.CanUseRandomDoors)
          islandConnector1 = islandConnectorList[this.RandomSeed.Next(islandConnectorList.Count)];
        if ((UnityEngine.Object) islandConnector1 != (UnityEngine.Object) null && (UnityEngine.Object) this.GetDirectionDoor(path.Direction, path.ConnectionType) != (UnityEngine.Object) null)
        {
          islandConnector1.ParentIsland.CanSpawnEncounter = false;
          this.Connector = islandConnector1;
          this.CurrentPiece = UnityEngine.Object.Instantiate<IslandPiece>(this.GetDirectionDoor(path.Direction, path.ConnectionType), Vector3.zero, Quaternion.identity, this.RoomTransform.transform);
          this.PositionIsland();
          this.Pieces.Add(this.CurrentPiece);
        }
        else
          Debug.Log((object) "NO PLACE TO PUT DOOR!");
      }
    }
  }

  public void GeneratePath(GenerateRoom.RoomPath Path)
  {
    this.CurrentPiece = this.StartPiece;
    List<IslandConnector> islandConnectorList = new List<IslandConnector>();
    foreach (IslandPiece piece in this.Pieces)
    {
      foreach (IslandConnector islandConnector in piece.GetConnectorsDirection(Path.Direction, false))
        islandConnectorList.Add(islandConnector);
    }
    float num1 = float.MaxValue;
    IslandConnector islandConnector1 = (IslandConnector) null;
    switch (Path.Direction)
    {
      case IslandConnector.Direction.North:
        float num2 = float.MinValue;
        using (List<IslandConnector>.Enumerator enumerator = islandConnectorList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            IslandConnector current = enumerator.Current;
            if ((double) current.transform.position.y > (double) num2)
            {
              num2 = current.transform.position.y;
              islandConnector1 = current;
            }
          }
          break;
        }
      case IslandConnector.Direction.East:
        float num3 = float.MinValue;
        using (List<IslandConnector>.Enumerator enumerator = islandConnectorList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            IslandConnector current = enumerator.Current;
            if ((double) current.transform.position.x > (double) num3)
            {
              num3 = current.transform.position.x;
              islandConnector1 = current;
            }
          }
          break;
        }
      case IslandConnector.Direction.South:
        using (List<IslandConnector>.Enumerator enumerator = islandConnectorList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            IslandConnector current = enumerator.Current;
            if ((double) current.transform.position.y < (double) num1)
            {
              num1 = current.transform.position.y;
              islandConnector1 = current;
            }
          }
          break;
        }
      case IslandConnector.Direction.West:
        using (List<IslandConnector>.Enumerator enumerator = islandConnectorList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            IslandConnector current = enumerator.Current;
            if ((double) current.transform.position.x < (double) num1)
            {
              num1 = current.transform.position.x;
              islandConnector1 = current;
            }
          }
          break;
        }
    }
    if ((UnityEngine.Object) islandConnector1 != (UnityEngine.Object) null)
      this.CurrentPiece = islandConnector1.GetComponentInParent<IslandPiece>();
    int num4 = 0;
    bool flag1 = true;
    while (--num4 >= 0)
    {
      this.Connectors = this.CurrentPiece.GetConnectorsDirection(Path.Direction, true);
      if (this.Connectors == null || this.Connectors.Count <= 0)
        break;
      this.Connector = this.Connectors[this.RandomSeed.Next(0, this.Connectors.Count)];
      this.PrevPiece = this.CurrentPiece;
      if (num4 == 0)
      {
        if (this.Connector.MyDirection == Path.Direction)
        {
          if (!Path.Door)
          {
            this.RandomPiece = this.GetEncounterIslandListByDirection(this.GetOppositeDirection(this.Connector.MyDirection));
            this.CurrentPiece = UnityEngine.Object.Instantiate<IslandPiece>(this.RandomPiece, Vector3.zero, Quaternion.identity, this.RoomTransform.transform);
            this.PositionIsland();
            this.Pieces.Add(this.CurrentPiece);
          }
        }
        else
        {
          this.RandomPiece = this.GetIslandFromMultipleLists(this.GetOppositeDirection(this.Connector.MyDirection), Path.Direction);
          this.CurrentPiece = UnityEngine.Object.Instantiate<IslandPiece>(this.RandomPiece, Vector3.zero, Quaternion.identity, this.RoomTransform.transform);
          this.PositionIsland();
          this.Pieces.Add(this.CurrentPiece);
          Debug.Log((object) "EXTENDING LEVEL");
        }
      }
      bool flag2 = Path.Encounters > 0 && !flag1;
      flag1 = flag2;
      if (num4 > 0)
      {
        this.RandomPiece = flag2 ? this.GetEncounterIslandListByDirection(this.GetOppositeDirection(this.Connector.MyDirection)) : this.GetIslandListByDirection(this.GetOppositeDirection(this.Connector.MyDirection));
        this.CurrentPiece = UnityEngine.Object.Instantiate<IslandPiece>(this.RandomPiece, Vector3.zero, Quaternion.identity, this.RoomTransform.transform);
        this.PositionIsland();
        this.Pieces.Add(this.CurrentPiece);
      }
    }
  }

  public void TurnEnemyIntoCritter(UnitObject enemy)
  {
    Vector3 position = enemy.transform.position with
    {
      z = 0.0f
    };
    Transform parent = enemy.transform.parent;
    if ((bool) (UnityEngine.Object) enemy.GetComponent<SpawnDeadBodyOnDeath>())
      UnityEngine.Object.Destroy((UnityEngine.Object) enemy.GetComponent<SpawnDeadBodyOnDeath>());
    enemy.GetComponent<IAttackResilient>()?.StopResilience();
    enemy.health.ImpactOnHit = false;
    enemy.health.ImpactSoundToPlay = Health.IMPACT_SFX.NONE;
    enemy.health.ImpactOnHitScale = 0.0f;
    enemy.health.DeathSoundToPlay = Health.DEATH_SFX.NONE;
    enemy.health.InanimateObject = true;
    enemy.health.InanimateObjectEffect = false;
    enemy.health.invincible = false;
    enemy.health.untouchable = false;
    enemy.health.DealDamage(enemy.health.HP, PlayerFarming.Instance.gameObject, position, AttackType: Health.AttackTypes.NoKnockBack, dealDamageImmediately: true, AttackFlags: Health.AttackFlags.ForceKill);
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/RelicCritter.prefab", position, Quaternion.identity, parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      SkeletonAnimation componentInChildren = obj.Result.GetComponentInChildren<SkeletonAnimation>();
      componentInChildren.AnimationState.SetAnimation(0, "appear", false);
      componentInChildren.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/goat/bleat", this.gameObject);
    }));
    BiomeConstants.Instance.EmitSmokeExplosionVFX(position);
  }

  public void PositionIsland()
  {
    this.Connectors = this.CurrentPiece.GetConnectorsDirection(this.GetOppositeDirection(this.Connector.MyDirection), false);
    this.CurrentConnector = this.Connectors[this.RandomSeed.Next(0, this.Connectors.Count)];
    this.CurrentPiece.transform.position = this.Connector.transform.position - this.CurrentConnector.transform.position;
    this.Connector.SetActive();
    this.CurrentConnector.SetActive();
  }

  public IslandPiece GetDirectionDoor(
    IslandConnector.Direction Direction,
    GenerateRoom.ConnectionTypes ConnectionType)
  {
    switch (Direction)
    {
      case IslandConnector.Direction.North:
        switch (ConnectionType)
        {
          case GenerateRoom.ConnectionTypes.Entrance:
            return this.NorthEntranceDoor;
          case GenerateRoom.ConnectionTypes.Boss:
            return !GameManager.Layer2 ? this.NorthBossDoor : BiomeGenerator.Instance.GeneratorRoomPrefab.NorthBossDoor_P2;
          case GenerateRoom.ConnectionTypes.Tarot:
            return this.NorthTarotDoor;
          case GenerateRoom.ConnectionTypes.WeaponShop:
            return this.NorthWeaponDoor;
          case GenerateRoom.ConnectionTypes.RelicShop:
            return this.NorthRelicDoor;
          case GenerateRoom.ConnectionTypes.LoreStoneRoom:
            return this.NorthLoreStoneDoor;
          default:
            return this.NorthIsland;
        }
      case IslandConnector.Direction.East:
        switch (ConnectionType)
        {
          case GenerateRoom.ConnectionTypes.Entrance:
            return this.EastEntranceDoor;
          case GenerateRoom.ConnectionTypes.Boss:
            return !GameManager.Layer2 ? this.EastBossDoor : BiomeGenerator.Instance.GeneratorRoomPrefab.EastBossDoor_P2;
          case GenerateRoom.ConnectionTypes.Tarot:
            return this.EastTarotDoor;
          case GenerateRoom.ConnectionTypes.WeaponShop:
            return this.EastWeaponDoor;
          case GenerateRoom.ConnectionTypes.RelicShop:
            return this.EastRelicDoor;
          case GenerateRoom.ConnectionTypes.LoreStoneRoom:
            return this.EastLoreStoneDoor;
          default:
            return this.EastIsland;
        }
      case IslandConnector.Direction.South:
        switch (ConnectionType)
        {
          case GenerateRoom.ConnectionTypes.Entrance:
            return this.SouthEntranceDoor;
          case GenerateRoom.ConnectionTypes.Boss:
            return !GameManager.Layer2 ? this.SouthBossDoor : BiomeGenerator.Instance.GeneratorRoomPrefab.SouthBossDoor_P2;
          case GenerateRoom.ConnectionTypes.Tarot:
            return this.SouthTarotDoor;
          case GenerateRoom.ConnectionTypes.WeaponShop:
            return this.SouthWeaponDoor;
          case GenerateRoom.ConnectionTypes.RelicShop:
            return this.SouthRelicDoor;
          case GenerateRoom.ConnectionTypes.LoreStoneRoom:
            return this.SouthLoreStoneDoor;
          default:
            return this.SouthIsland;
        }
      case IslandConnector.Direction.West:
        switch (ConnectionType)
        {
          case GenerateRoom.ConnectionTypes.Entrance:
            return this.WestEntranceDoor;
          case GenerateRoom.ConnectionTypes.Boss:
            return !GameManager.Layer2 ? this.WestBossDoor : BiomeGenerator.Instance.GeneratorRoomPrefab.WestBossDoor_P2;
          case GenerateRoom.ConnectionTypes.Tarot:
            return this.WestTarotDoor;
          case GenerateRoom.ConnectionTypes.WeaponShop:
            return this.WestWeaponDoor;
          case GenerateRoom.ConnectionTypes.RelicShop:
            return this.WestRelicDoor;
          case GenerateRoom.ConnectionTypes.LoreStoneRoom:
            return this.WestLoreStoneDoor;
          default:
            return this.WestIsland;
        }
      default:
        return (IslandPiece) null;
    }
  }

  public IslandConnector.Direction GetOppositeDirection(IslandConnector.Direction Direction)
  {
    switch (Direction)
    {
      case IslandConnector.Direction.North:
        return IslandConnector.Direction.South;
      case IslandConnector.Direction.East:
        return IslandConnector.Direction.West;
      case IslandConnector.Direction.South:
        return IslandConnector.Direction.North;
      case IslandConnector.Direction.West:
        return IslandConnector.Direction.East;
      default:
        return IslandConnector.Direction.North;
    }
  }

  public void BtnClearPrefabs() => this.ClearPrefabs();

  public void ClearPrefabs(bool ClearRoomTransform = true, bool ClearSceneryTransform = true)
  {
    int childCount = this.transform.childCount;
    while (--childCount >= 0)
    {
      if (this.transform.GetChild(childCount).name == "SceneryTransform")
        this.SceneryTransform = this.transform.GetChild(childCount).gameObject;
      if (this.transform.GetChild(childCount).name == "CustomTransform")
        this.CustomTransform = this.transform.GetChild(childCount).gameObject;
    }
    if ((UnityEngine.Object) this.SceneryTransform == (UnityEngine.Object) null)
      this.SceneryTransform = new GameObject("SceneryTransform");
    if ((UnityEngine.Object) this.CustomTransform == (UnityEngine.Object) null)
      this.CustomTransform = new GameObject("CustomTransform");
    this.SceneryTransform.transform.parent = this.transform;
    this.CustomTransform.transform.parent = this.transform;
    if (Application.isEditor && !Application.isPlaying)
    {
      if (ClearRoomTransform && (UnityEngine.Object) this.RoomTransform != (UnityEngine.Object) null)
      {
        while (this.RoomTransform.transform.childCount > 0)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.RoomTransform.transform.GetChild(0).gameObject);
      }
      if (ClearSceneryTransform && (UnityEngine.Object) this.SceneryTransform != (UnityEngine.Object) null)
      {
        while (this.SceneryTransform.transform.childCount > 0)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.SceneryTransform.transform.GetChild(0).gameObject);
      }
    }
    else
    {
      if (ClearRoomTransform)
      {
        for (int index = this.RoomTransform.transform.childCount - 1; index >= 0; --index)
          ObjectPool.Recycle(this.RoomTransform.transform.GetChild(index).gameObject);
      }
      if (ClearSceneryTransform)
      {
        for (int index = this.SceneryTransform.transform.childCount - 1; index >= 0; --index)
          ObjectPool.Recycle(this.SceneryTransform.transform.GetChild(index).gameObject);
      }
    }
    Physics2D.SyncTransforms();
    this.RoomTransform.GenerateGeometry();
  }

  public void UnlockDoors() => RoomLockController.RoomCompleted();

  public void SpawnSpecialContent()
  {
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.LoreTotemRooms != null && BiomeGenerator.Instance.LoreTotemRooms.Contains(BiomeGenerator.Instance.CurrentRoom))
      this.SpawnLoreTotem();
    if (!((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null) || DungeonSandboxManager.Active || (double) UnityEngine.Random.value >= 0.039999999105930328 || Health.team2.Count <= 0 || (double) TimeManager.TotalElapsedGameTime <= (double) DataManager.Instance.TimeSinceLastMissionaryFollowerEncounter || DataManager.Instance.Followers_OnMissionary_IDs.Count <= 0 || BiomeGenerator.Instance.OverrideRandomWalk || !((UnityEngine.Object) DungeonLeaderMechanics.Instance == (UnityEngine.Object) null))
      return;
    this.SpawnMissionaryFollower();
  }

  public void SpawnLoreTotem()
  {
    List<Health> healthList = new List<Health>();
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.CompareTag("BreakableDecoration"))
        healthList.Add(allUnit);
    }
    Vector3 position = (Vector3) (UnityEngine.Random.insideUnitCircle * 3f);
    if (healthList.Count > 0)
      position = healthList[UnityEngine.Random.Range(0, healthList.Count)].transform.position;
    Addressables.InstantiateAsync((object) "Assets/Prefabs/Dungeon/InteractableObjects/Lore Totem.prefab", position, Quaternion.identity, this.CustomTransform.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj => { });
  }

  public void SpawnMissionaryFollower()
  {
    List<Health> healthList = new List<Health>();
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.CompareTag("BreakableDecoration"))
        healthList.Add(allUnit);
    }
    Vector3 position = (Vector3) (UnityEngine.Random.insideUnitCircle * 3f);
    if (healthList.Count > 0)
      position = healthList[UnityEngine.Random.Range(0, healthList.Count)].transform.position + Vector3.up * 0.05f;
    DataManager.Instance.TimeSinceLastMissionaryFollowerEncounter = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(3600, 7200);
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.CombatFollowerPrefab, FollowerInfo.GetInfoByID(DataManager.Instance.Followers_OnMissionary_IDs[UnityEngine.Random.Range(0, DataManager.Instance.Followers_OnMissionary_IDs.Count)]), position, this.CustomTransform.transform, BiomeGenerator.Instance.DungeonLocation);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) spawnedFollower.Follower.SetBodyAnimation("Existential Dread/dread-trauma", true);
    spawnedFollower.Follower.GetComponent<UnitObject>().CanHaveModifier = false;
    spawnedFollower.Follower.GetComponent<UnitObject>().RemoveModifier();
    Health.team2.Remove(spawnedFollower.Follower.Health);
    spawnedFollower.Follower.Health.team = Health.Team.PlayerTeam;
    spawnedFollower.Follower.Health.enabled = false;
    spawnedFollower.Follower.gameObject.AddComponent<DungeonMissionaryFollower>().Type = DungeonMissionaryFollower.HiddenType.HiddenInBush;
    spawnedFollower.Follower.OverridingEmotions = true;
    spawnedFollower.Follower.Brain.Info.Outfit = FollowerOutfitType.Sherpa;
    spawnedFollower.Follower.SetOutfit(FollowerOutfitType.Sherpa, false);
    FollowerBrain.RemoveBrain(spawnedFollower.FollowerFakeBrain.Info.ID);
  }

  public void InitializeDecorationSetList()
  {
    this._decorationSetList.Clear();
    foreach (AssetReferenceGameObject addrDecorationSet in this.Addr_DecorationSetList)
      this._decorationSetList.Add(this.LoadAddresableAsset<GeneraterDecorations>(addrDecorationSet));
    this.isDecorationsInitialized = true;
  }

  public void InitializeStartPrefabs()
  {
    this._startPieces.Clear();
    foreach (AssetReferenceGameObject addrStartPiece in this.Addr_StartPieces)
      this._startPieces.Add(this.LoadAddresableAsset<IslandPiece>(addrStartPiece));
    this.isStartPiecesInitiliazed = true;
  }

  public void InitializeIslandPrefabs()
  {
    this._islandPieces.Clear();
    foreach (AssetReferenceGameObject addrIslandPiece in this.Addr_IslandPieces)
      this._islandPieces.Add(this.LoadAddresableAsset<IslandPiece>(addrIslandPiece));
    this.isIslandPiecesInitiliazed = true;
  }

  public void InitializeResourcePrefabs()
  {
    this._resourcePieces.Clear();
    foreach (AssetReferenceGameObject addrResourcePiece in this.Addr_ResourcePieces)
      this._resourcePieces.Add(this.LoadAddresableAsset<IslandPiece>(addrResourcePiece));
    this.isResourcePiecesInitiliazed = true;
  }

  public T LoadAddresableAsset<T>(AssetReferenceGameObject item) where T : MonoBehaviour
  {
    T obj = default (T);
    if (!item.RuntimeKeyIsValid())
      return obj;
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) item);
    asyncOperationHandle.WaitForCompletion();
    this.asyncOperationHandles.Add(asyncOperationHandle);
    if ((UnityEngine.Object) asyncOperationHandle.Result != (UnityEngine.Object) null)
    {
      obj = asyncOperationHandle.Result.GetComponent<T>();
      if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
        obj = asyncOperationHandle.Result.GetComponentInChildren<T>();
    }
    return obj;
  }

  [CompilerGenerated]
  public void \u003CSetAStar\u003Eb__235_0() => this.GeneratedPathing = true;

  [CompilerGenerated]
  public void \u003CPlaceNoiseDecorations\u003Eb__263_0(GameObject obj)
  {
    this.PerlinNoiseDecoration = obj;
    this.PerlinNoiseDecoration.name = this.NoiseName;
    this.PerlinScale = this.PerlinNoiseDecoration.transform.localScale;
    this.PerlinScale.z *= this.Noise * 1.4f;
    this.PerlinNoiseDecoration.transform.localScale = this.PerlinScale;
  }

  [CompilerGenerated]
  public void \u003CTurnEnemyIntoCritter\u003Eb__285_0(AsyncOperationHandle<GameObject> obj)
  {
    SkeletonAnimation componentInChildren = obj.Result.GetComponentInChildren<SkeletonAnimation>();
    componentInChildren.AnimationState.SetAnimation(0, "appear", false);
    componentInChildren.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/goat/bleat", this.gameObject);
  }

  public delegate void GenerateEvent();

  public class RoomPath
  {
    public IslandConnector.Direction Direction;
    public int Encounters;
    public bool Door;
    public GenerateRoom.ConnectionTypes ConnectionType;

    public RoomPath(
      IslandConnector.Direction Direction,
      bool Door,
      GenerateRoom.ConnectionTypes ConnectionType)
    {
      this.Direction = Direction;
      this.Door = Door;
      this.ConnectionType = ConnectionType;
    }
  }

  public enum ConnectionTypes
  {
    False,
    True,
    Entrance,
    Exit,
    Boss,
    DoorRoom,
    NextLayer,
    DungeonFirstRoom,
    LeaderBoss,
    Tarot,
    WeaponShop,
    RelicShop,
    LoreStoneRoom,
  }
}
