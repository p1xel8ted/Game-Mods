// Decompiled with JetBrains decompiler
// Type: MMRoomGeneration.GenerateRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

#nullable disable
namespace MMRoomGeneration;

public class GenerateRoom : BaseMonoBehaviour
{
  public static GenerateRoom Instance;
  public int Seed;
  private System.Random RandomSeed;
  public SoundConstants.RoomID roomMusicID = SoundConstants.RoomID.StandardAmbience;
  [EventRef]
  public string biomeAtmosOverridePath = string.Empty;
  public Sprite MapIcon;
  public List<GeneraterDecorations> DecorationSetList = new List<GeneraterDecorations>();
  [HideInInspector]
  public GeneraterDecorations DecorationList;
  public bool CreateOnlyOneEncounterRoom;
  public bool CreateRandomExtraPaths = true;
  [Range(0.0f, 1f)]
  public float EncounterWillBeEnemyOrResource = 0.5f;
  public CompositeCollider2D RoomTransform;
  public GameObject SceneryTransform;
  public GameObject CustomTransform;
  [HideInInspector]
  public int Scale = 2;
  public bool LockingDoors = true;
  public bool LockEntranceBehindPlayer;
  public GenerateRoom.ConnectionTypes North;
  public GenerateRoom.ConnectionTypes East;
  public GenerateRoom.ConnectionTypes South;
  public GenerateRoom.ConnectionTypes West;
  public GameObject BloodSplatterPrefab;
  public IslandPiece NorthIsland;
  public IslandPiece EastIsland;
  public IslandPiece SouthIsland;
  public IslandPiece WestIsland;
  public IslandPiece NorthBossDoor;
  public IslandPiece EastBossDoor;
  public IslandPiece SouthBossDoor;
  public IslandPiece WestBossDoor;
  public IslandPiece NorthEntranceDoor;
  public IslandPiece EastEntranceDoor;
  public IslandPiece SouthEntranceDoor;
  public IslandPiece WestEntranceDoor;
  public List<IslandPiece> StartPieces = new List<IslandPiece>();
  public List<IslandPiece> IslandPieces = new List<IslandPiece>();
  public List<IslandPiece> ResourcePieces = new List<IslandPiece>();
  private List<IslandPiece> NorthIslandPieces;
  private List<IslandPiece> EastIslandPieces;
  private List<IslandPiece> SouthIslandPieces;
  private List<IslandPiece> WestIslandPieces;
  private List<IslandPiece> NorthIslandEncounterPieces;
  private List<IslandPiece> EastIslandEncounterPieces;
  private List<IslandPiece> SouthIslandEncounterPieces;
  private List<IslandPiece> WestIslandEncounterPieces;
  private List<IslandPiece> NorthIslandResourcesPieces;
  private List<IslandPiece> EastIslandResourcesPieces;
  private List<IslandPiece> SouthIslandResourcesPieces;
  private List<IslandPiece> WestIslandResourcesPieces;
  public List<IslandPiece> Pieces = new List<IslandPiece>();
  private IslandPiece CurrentPiece;
  private IslandPiece PrevPiece;
  private List<int> PreviousSeeds = new List<int>();
  private bool Testing;
  private List<GenerateRoom.RoomPath> Paths;
  private float PrevTime;
  private float LimitNorth = (float) int.MinValue;
  private float LimitEast = (float) int.MinValue;
  private float LimitSouth = (float) int.MaxValue;
  private float LimitWest = (float) int.MaxValue;
  private SpriteShapeController RoomSpriteShape;
  private List<List<int>> DecorationGrid;
  private int DecorationGridWidth;
  private int DecorationGridHeight;
  private GeneraterDecorations.DecorationAndProbability d;
  private GameObject PerlinNoiseDecoration;
  private Vector3 PerlinScale;
  private string NoiseName;
  private float Noise;
  private List<SpriteShapeController> _SpriteShapeControllers;
  private static SpriteShapeController CurrentSpriteShape;
  private static List<Vector3> Points;
  private Vector2 RoomPerlinOffset;
  private List<IslandConnector> Connectors;
  private IslandConnector CurrentConnector;
  private IslandConnector Connector;
  private IslandPiece RandomPiece;
  private List<Collider2D> Collisions;

  private IslandPiece NorthTarotDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Tarot North");
  }

  private IslandPiece EastTarotDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Tarot East");
  }

  private IslandPiece SouthTarotDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Tarot South");
  }

  private IslandPiece WestTarotDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Tarot West");
  }

  private IslandPiece NorthWeaponDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Weapon North");
  }

  private IslandPiece EastWeaponDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Weapon East");
  }

  private IslandPiece SouthWeaponDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Weapon South");
  }

  private IslandPiece WestWeaponDoor
  {
    get => Resources.Load<IslandPiece>("Prefabs/Custom Entrances/Door Weapon West");
  }

  public IslandPiece StartPiece { get; private set; }

  public bool generated { get; private set; }

  public bool GeneratedDecorations { get; set; }

  private void OnEnable()
  {
    GenerateRoom.Instance = this;
    this.InitSpriteShapes();
    if (!this.generated)
      return;
    this.StartCoroutine((IEnumerator) this.RegenerateDecorationsWithPool());
  }

  private IEnumerator RegenerateDecorationsWithPool()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    GenerateRoom generateRoom = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      generateRoom.SetCollider();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    generateRoom.RoomTransform.geometryType = CompositeCollider2D.GeometryType.Polygons;
    generateRoom.RoomTransform.GenerateGeometry();
    Physics2D.SyncTransforms();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) generateRoom.StartCoroutine((IEnumerator) generateRoom.SpawnDecorations(false));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnDisable()
  {
    if ((UnityEngine.Object) GenerateRoom.Instance == (UnityEngine.Object) this)
      GenerateRoom.Instance = (GenerateRoom) null;
    if (!this.generated)
      return;
    int index = -1;
    if (!((UnityEngine.Object) this.SceneryTransform != (UnityEngine.Object) null))
      return;
    while (++index < this.SceneryTransform.transform.childCount)
      ObjectPool.Recycle(this.SceneryTransform.transform.GetChild(index).gameObject);
  }

  private void OnDestroy() => this.BloodSplatterPrefab = (GameObject) null;

  private void Start()
  {
    this.PreviousSeeds.Add(this.Seed);
    if (!((UnityEngine.Object) this.BloodSplatterPrefab == (UnityEngine.Object) null))
      return;
    this.BloodSplatterPrefab = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/BloodParticles_Prefab"), this.transform) as GameObject;
  }

  private void GeneratePreviousSeed()
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

  private void GenerateRandomSeed()
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

  private void GenerateRoomFunc() => this.StartCoroutine((IEnumerator) this.Generate());

  private IEnumerator Generate()
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
    yield return (object) generateRoom.StartCoroutine((IEnumerator) generateRoom.SpawnDecorations(true));
    generateRoom.CreateBackgroundSpriteShape();
    generateRoom.SetColliderAndUpdatePathfinding();
    generateRoom.InitSpriteShapes();
    generateRoom.generated = true;
  }

  private void InitSpriteShapes()
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

  public void SetCollider()
  {
    this.RoomTransform.geometryType = CompositeCollider2D.GeometryType.Outlines;
    this.RoomTransform.gameObject.layer = LayerMask.NameToLayer("Island");
    this.RoomTransform.GenerateGeometry();
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

  private IEnumerator SetAStar()
  {
    while ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      yield return (object) null;
    int num = 5;
    AstarPath.active.data.gridGraph.center = this.RoomTransform.bounds.center;
    GridGraph gridGraph = AstarPath.active.data.gridGraph;
    Bounds bounds = this.RoomTransform.bounds;
    int width = (int) bounds.size.x * 2 + num;
    bounds = this.RoomTransform.bounds;
    int depth = (int) bounds.size.y * 2 + num;
    gridGraph.SetDimensions(width, depth, 0.5f);
    GameManager.RecalculatePaths(true);
  }

  private void CreatePaths()
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

  private IslandConnector.Direction PathGetUnusedDirection()
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

  private void CompositeColliders()
  {
    foreach (IslandPiece piece in this.Pieces)
      piece.Collider.usedByComposite = true;
  }

  private IEnumerator DisableIslands()
  {
    GenerateRoom generateRoom = this;
    int completed = 0;
    foreach (IslandPiece piece in generateRoom.Pieces)
      generateRoom.StartCoroutine((IEnumerator) piece.InitIsland(generateRoom.RandomSeed, generateRoom.DecorationList.SpriteShapeSecondary, (System.Action) (() => ++completed)));
    while (completed < generateRoom.Pieces.Count)
      yield return (object) null;
  }

  private void CustomLevel()
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
    this.SpawnDecorations(true);
    this.CreateBackgroundSpriteShape();
    foreach (IslandPiece piece in this.Pieces)
      piece.HideSprites();
    this.SetColliderAndUpdatePathfinding();
  }

  private void CreateBackgroundSpriteShape()
  {
    if (!((UnityEngine.Object) this.DecorationList.SpriteShapeBack != (UnityEngine.Object) null))
      return;
    GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Room Back Sprite"), new Vector3(0.0f, 0.0f, 0.01f), Quaternion.identity, this.RoomTransform.transform) as GameObject;
    gameObject.transform.localScale = new Vector3(this.LimitEast - this.LimitWest, this.LimitNorth - this.LimitSouth);
    SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
    component.shadowCastingMode = ShadowCastingMode.Off;
    component.receiveShadows = false;
    component.sortingLayerName = "Island";
  }

  private void CreateSpriteShape()
  {
    int index1 = -1;
    while (++index1 < this.RoomTransform.pathCount)
    {
      GameObject gameObject = new GameObject();
      gameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0001f);
      gameObject.transform.parent = this.RoomTransform.transform;
      gameObject.name = "Sprite shape " + (object) index1;
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
      this.RoomSpriteShape.splineDetail = 3;
      this.RoomSpriteShape.spriteShapeRenderer.sortingLayerName = "Ground";
      Vector2[] points = new Vector2[this.RoomTransform.GetPathPointCount(index1)];
      this.RoomTransform.GetPath(index1, points);
      Array.Reverse((Array) points);
      int index3 = 0;
      this.RoomSpriteShape.spline.InsertPointAt(0, (Vector3) points[0]);
      while (++index3 < points.Length)
      {
        if ((double) Vector2.Distance((Vector2) this.RoomTransform.transform.TransformPoint((Vector3) points[index3]), (Vector2) this.RoomTransform.transform.TransformPoint((Vector3) points[index3 - 1])) > 0.10000000149011612)
          this.RoomSpriteShape.spline.InsertPointAt(this.RoomSpriteShape.spline.GetPointCount() - 1, this.RoomTransform.transform.TransformPoint((Vector3) points[index3]));
      }
    }
  }

  private void CollateLists()
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

  private IslandPiece GetIslandListByDirection(IslandConnector.Direction Direction)
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

  private IslandPiece GetIslandFromMultipleLists(
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
    Debug.Log((object) ("AvailableConnectors.Count " + (object) islandPieceList.Count));
    return islandPieceList[this.RandomSeed.Next(0, islandPieceList.Count)];
  }

  private IslandPiece GetRandomEncounterIsland()
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
    Debug.Log((object) ("Remaing islands with encounters: " + (object) islandPieceList.Count));
    return islandPieceList[this.RandomSeed.Next(0, islandPieceList.Count)];
  }

  private IslandPiece GetEncounterIslandListByDirection(IslandConnector.Direction Direction)
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

  private IslandPiece GetResourceIslandListByDirection(IslandConnector.Direction Direction)
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

  private void PlaceDecorations(bool CustomLevel)
  {
    if (CustomLevel)
      this.DecorationList = !((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null) ? this.DecorationSetList[this.RandomSeed.Next(0, this.DecorationSetList.Count)] : BiomeGenerator.Instance.BiomeDecorationSet[this.RandomSeed.Next(0, BiomeGenerator.Instance.BiomeDecorationSet.Count)];
    else if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
    {
      if (BiomeGenerator.Instance.BiomeDecorationSet != null && BiomeGenerator.Instance.BiomeDecorationSet.Count > 0)
        this.DecorationList = BiomeGenerator.Instance.BiomeDecorationSet[Mathf.Min(GameManager.CurrentDungeonLayer - 1, BiomeGenerator.Instance.BiomeDecorationSet.Count)];
    }
    else if (Application.isEditor && !Application.isPlaying)
    {
      this.DecorationList = this.DecorationSetList[0];
    }
    else
    {
      Debug.Log((object) $"DecorationSetList {(object) this.DecorationSetList}   GameManager.CurrentDungeonLayer - 1{(object) (GameManager.CurrentDungeonLayer - 1)}");
      this.DecorationList = this.DecorationSetList[Mathf.Min(GameManager.CurrentDungeonLayer - 1, this.DecorationSetList.Count - 1)];
    }
    this.DecorationGrid = new List<List<int>>();
    Bounds bounds = this.RoomTransform.bounds;
    double x1 = (double) bounds.size.x;
    bounds = this.RoomTransform.bounds;
    double y = (double) bounds.size.y;
    this.DecorationGridWidth = (int) Mathf.Max((float) x1, (float) y);
    this.DecorationGridHeight = this.DecorationGridWidth;
    for (int index = -this.DecorationGridHeight; index < this.DecorationGridHeight; index += this.Scale)
    {
      List<int> intList = new List<int>();
      for (int x2 = -this.DecorationGridWidth; x2 < this.DecorationGridWidth; x2 += this.Scale)
      {
        if (this.RoomTransform.ClosestPoint(new Vector2((float) x2, (float) index - (float) this.Scale * 0.5f)) != new Vector2((float) x2, (float) index - (float) this.Scale * 0.5f) && this.RoomTransform.ClosestPoint(new Vector2((float) x2, (float) index + (float) this.Scale * 0.5f)) != new Vector2((float) x2, (float) index + (float) this.Scale * 0.5f))
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

  private void PlaceNoiseDecorations(float x, float y, Vector3 Position)
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

  private void PlacePerlinCritters(float x, float y, Vector3 Position)
  {
    this.Noise = Mathf.PerlinNoise(x / (float) this.DecorationGridWidth * this.DecorationList.NoiseScale, y / (float) this.DecorationGridHeight * this.DecorationList.NoiseScale);
    if (this.DecorationList.Critters.DecorationAndProabilies.Count <= 0 || (double) this.Noise > (double) this.DecorationList.CritterThreshold || !(this.RoomTransform.ClosestPoint((Vector2) Position) == (Vector2) Position) || (double) this.DecorationList.MaxRadiusFromMiddle != -1.0 && (double) Vector3.Distance(Vector3.zero, Position) >= (double) this.DecorationList.MaxRadiusFromMiddle)
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
    ObjectPool.Spawn(this.d.ObjectPath, Position + this.d.GetRandomOffset(), Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj => obj.name = "Critter"));
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

  private void PrimarySpriteShapeNoiseDecortations(
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
    ObjectPool.Spawn(d.ObjectPath, Position + Vector3.back * 0.01f, Quaternion.identity, this.RoomTransform.transform, (Action<GameObject>) (obj => obj.name = "Perlin Decoration - Primary SpriteShape"));
  }

  private void SecondarySpriteShapeNoiseDecortations(
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
            ObjectPool.Spawn(d.ObjectPath, Position + Vector3.back * 0.01f, Quaternion.identity, this.RoomTransform.transform, (Action<GameObject>) (obj => obj.name = "Perlin Decoration - Secondary SpriteShape"));
        }
      }
    }
  }

  private IEnumerator SpawnDecorations(bool spawnInsideShapeDecorations)
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
            this.PlacePerlinCritters((float) x, (float) y, Position);
          }
          if (this.DecorationGrid[y][x] == 2 && this.DecorationList.DecorationPiece.DecorationAndProabilies.Count > 0)
          {
            this.d = this.DecorationList.DecorationPiece.GetRandomGameObject(this.RandomSeed.NextDouble());
            if (this.RoomTransform.ClosestPoint((Vector2) Position + new Vector2((float) this.Scale * 0.5f, 0.0f)) == (Vector2) Position + new Vector2((float) this.Scale * 0.5f, 0.0f))
              Position += Vector3.left * ((float) this.Scale * 0.5f);
            else if (this.RoomTransform.ClosestPoint((Vector2) Position + new Vector2((float) -this.Scale * 0.5f, 0.0f)) == (Vector2) Position + new Vector2((float) -this.Scale * 0.5f, 0.0f))
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
          if (this.DecorationGrid[y][x] == 3 && this.DecorationList.DecorationPiece2x2.DecorationAndProabilies.Count > 0)
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
            if ((double) ClosestPosition.y > (double) Position.y && this.DecorationList.DecorationPiece3x3.DecorationAndProabilies.Count > 0)
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
            else if (this.DecorationList.DecorationPiece3x3Tall.DecorationAndProabilies.Count > 0)
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

  private void CheckLimit(Vector3 Position)
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

  private void ReplaceDecorations()
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
        ObjectPool.Spawn(this.d.ObjectPath, g.transform.position + this.d.GetRandomOffset(), Quaternion.identity, this.SceneryTransform.transform, (Action<GameObject>) (obj =>
        {
          obj.name = g.name;
          this.CheckLimit(g.transform.position + this.d.GetRandomOffset());
        }));
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) g);
    }
    this.CreateBackgroundSpriteShape();
  }

  private void CreateStartPiece()
  {
    this.StartPiece = UnityEngine.Object.Instantiate<IslandPiece>(this.GetRandomEncounterIsland(), Vector3.zero, Quaternion.identity, this.RoomTransform.transform);
    this.Pieces.Add(this.StartPiece);
  }

  private void PlaceDoors()
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
        float num1 = float.MaxValue;
        IslandConnector islandConnector1 = (IslandConnector) null;
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
        if ((UnityEngine.Object) islandConnector1 != (UnityEngine.Object) null)
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

  private void GeneratePath(GenerateRoom.RoomPath Path)
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

  private void PositionIsland()
  {
    this.Connectors = this.CurrentPiece.GetConnectorsDirection(this.GetOppositeDirection(this.Connector.MyDirection), false);
    this.CurrentConnector = this.Connectors[this.RandomSeed.Next(0, this.Connectors.Count)];
    this.CurrentPiece.transform.position = this.Connector.transform.position - this.CurrentConnector.transform.position;
    this.Connector.SetActive();
    this.CurrentConnector.SetActive();
  }

  private IslandPiece GetDirectionDoor(
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
            return this.NorthBossDoor;
          case GenerateRoom.ConnectionTypes.Tarot:
            return this.NorthTarotDoor;
          case GenerateRoom.ConnectionTypes.WeaponShop:
            return this.NorthWeaponDoor;
          default:
            return this.NorthIsland;
        }
      case IslandConnector.Direction.East:
        switch (ConnectionType)
        {
          case GenerateRoom.ConnectionTypes.Entrance:
            return this.EastEntranceDoor;
          case GenerateRoom.ConnectionTypes.Boss:
            return this.EastBossDoor;
          case GenerateRoom.ConnectionTypes.Tarot:
            return this.EastTarotDoor;
          case GenerateRoom.ConnectionTypes.WeaponShop:
            return this.EastWeaponDoor;
          default:
            return this.EastIsland;
        }
      case IslandConnector.Direction.South:
        switch (ConnectionType)
        {
          case GenerateRoom.ConnectionTypes.Entrance:
            return this.SouthEntranceDoor;
          case GenerateRoom.ConnectionTypes.Boss:
            return this.SouthBossDoor;
          case GenerateRoom.ConnectionTypes.Tarot:
            return this.SouthTarotDoor;
          case GenerateRoom.ConnectionTypes.WeaponShop:
            return this.SouthWeaponDoor;
          default:
            return this.SouthIsland;
        }
      case IslandConnector.Direction.West:
        switch (ConnectionType)
        {
          case GenerateRoom.ConnectionTypes.Entrance:
            return this.WestEntranceDoor;
          case GenerateRoom.ConnectionTypes.Boss:
            return this.WestBossDoor;
          case GenerateRoom.ConnectionTypes.Tarot:
            return this.WestTarotDoor;
          case GenerateRoom.ConnectionTypes.WeaponShop:
            return this.WestWeaponDoor;
          default:
            return this.WestIsland;
        }
      default:
        return (IslandPiece) null;
    }
  }

  private IslandConnector.Direction GetOppositeDirection(IslandConnector.Direction Direction)
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

  private void BtnClearPrefabs() => this.ClearPrefabs();

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
      int index1 = -1;
      if (ClearRoomTransform)
      {
        while (++index1 < this.RoomTransform.transform.childCount)
          ObjectPool.Recycle(this.RoomTransform.transform.GetChild(index1).gameObject);
      }
      int index2 = -1;
      if (ClearSceneryTransform)
      {
        while (++index2 < this.SceneryTransform.transform.childCount)
          ObjectPool.Recycle(this.SceneryTransform.transform.GetChild(index2).gameObject);
      }
    }
    Physics2D.SyncTransforms();
    this.RoomTransform.GenerateGeometry();
  }

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
  }
}
