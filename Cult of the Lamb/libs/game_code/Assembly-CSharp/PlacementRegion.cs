// Decompiled with JetBrains decompiler
// Type: PlacementRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
public class PlacementRegion : BaseMonoBehaviour
{
  public static PlacementRegion Instance;
  public bool SetAsInstance = true;
  public bool PlaceWeeds = true;
  public bool PlaceRubble = true;
  public List<PlacementRegion.ResourcesAndCount> ResourcesToPlace = new List<PlacementRegion.ResourcesAndCount>();
  public static PlacementRegion.NewBuilding OnNewBuilding;
  public static List<PlacementRegion> PlacementRegions = new List<PlacementRegion>();
  public GameObject PlacementGameObject;
  public GameObject PlacementSquare;
  public StructureBrain.TYPES StructureType;
  public PlacementObjectUI PlacementObjectUI;
  public PlacementObject placementObject;
  public bool isEditingBuildings;
  public Vector3 previousEditingPosition = Vector3.zero;
  public List<Vector2Int> ranchingGridPositions = new List<Vector2Int>();
  public List<Vector2Int> fenceableGridPositions = new List<Vector2Int>();
  public List<Vector2Int> touchingRanchGridPositions = new List<Vector2Int>();
  public Structures_PlacementRegion _StructureBrain;
  public Dictionary<Vector2Int, PlacementRegion.TileGridTile> GridTileLookup = new Dictionary<Vector2Int, PlacementRegion.TileGridTile>();
  public float InputDelay;
  public PlacementObjectUI placementUI;
  public bool canPlaceObjectOnBuildings;
  public bool isPath;
  public List<Vector3> placingPathsPositions = new List<Vector3>();
  public int direction = 1;
  [CompilerGenerated]
  public int \u003CRotation\u003Ek__BackingField;
  public Tween moveTween;
  public Tween prisonerMoveTween;
  public Tween shakeTween;
  public Vector3 cachedDirection;
  public Plane plane = new Plane(Vector3.forward, Vector3.zero);
  public Vector2[] cachedColliderPoints;
  public bool tilesInitiated;
  public string flipSFX = "event:/building/building_flip_editmode";
  public Vector3 _PlacementPosition;
  public Structure structure;
  public Vector3 positionOffset;
  public bool structureIsOnlyPlaceableInRanch;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  public List<PlacementTile> Tiles;
  public List<PlacementRegion.TileGridTile> PreviewTilesList = new List<PlacementRegion.TileGridTile>();
  public PolygonCollider2D polygonCollider2D;
  public int Count;
  public int MaxTileCount = 50;
  public bool isClearingTiles;
  public static Vector3Int Left = new Vector3Int(-1, 1, 0);
  public static Vector3Int Right = new Vector3Int(1, -1, 0);
  public static Vector3Int Up = new Vector3Int(1, 1, 0);
  public static Vector3Int Down = new Vector3Int(-1, -1, 0);
  public static Vector3Int UpLeft = new Vector3Int(0, 1, 0);
  public static Vector3Int DownLeft = new Vector3Int(-1, 0, 0);
  public static Vector3Int UpRight = new Vector3Int(1, 0, 0);
  public static Vector3Int DownRight = new Vector3Int(0, -1, 0);
  public float Lerp;
  public float LerpSpeed = 7f;
  public PlacementTile CurrentTile;
  public PlacementTile PreviousTile;
  public PlacementRegion.TileGridTile closestTile;
  public float tileToPositionDistance = float.PositiveInfinity;
  public PlacementRegion.Mode CurrentMode;
  public Structure PreviousStructure;
  public Structure CurrentStructureToUpgrade;
  public Structure CurrentStructureToMove;
  public float TopSpeed = 10f;
  public float LerpToTileSpeed = 10f;
  public GameManager C_GameManagerInstance;
  public Camera camera;
  public GameObject BuildSitePrefab;
  public GameObject BuildSiteBuildingProjectPrefab;
  public static int counter = 0;
  public static int SignBitMask = int.MaxValue;
  public Queue<GameObject> prefabPool = new Queue<GameObject>();

  public static Vector2 X_Constraints => new Vector2(-25f, 25f);

  public static Vector2 Y_Constraints
  {
    get => DataManager.Instance.LandPurchased >= 0 ? new Vector2(-60f, 1f) : new Vector2(-30f, 1f);
  }

  public Structure CurrentStructure
  {
    get
    {
      return !((UnityEngine.Object) this.CurrentStructureToMove != (UnityEngine.Object) null) ? this.CurrentStructureToUpgrade : this.CurrentStructureToMove;
    }
  }

  public static event PlacementRegion.BuildingEvent OnBuildingBeganMoving;

  public static event PlacementRegion.BuildingEvent OnBuildingPlaced;

  public Structures_PlacementRegion structureBrain
  {
    get
    {
      if (this._StructureBrain == null)
        this._StructureBrain = this.structure.Brain as Structures_PlacementRegion;
      return this._StructureBrain;
    }
    set => this._StructureBrain = value;
  }

  public StructuresData StructureInfo
  {
    get
    {
      return !((UnityEngine.Object) this.structure != (UnityEngine.Object) null) ? (StructuresData) null : this.structure.Structure_Info;
    }
  }

  public List<PlacementRegion.TileGridTile> Grid
  {
    get
    {
      return !((UnityEngine.Object) this.structure != (UnityEngine.Object) null) || this.StructureInfo == null ? new List<PlacementRegion.TileGridTile>() : this.StructureInfo.Grid;
    }
  }

  public int Rotation
  {
    get => this.\u003CRotation\u003Ek__BackingField;
    set => this.\u003CRotation\u003Ek__BackingField = value;
  }

  public Vector3 PlacementPosition
  {
    get => this._PlacementPosition;
    set
    {
      if ((UnityEngine.Object) this.placementObject != (UnityEngine.Object) null && value != this._PlacementPosition)
        AudioManager.Instance.PlayOneShot("event:/building/move_building_placement", this.placementObject.transform.position);
      this.InputDelay = 0.2f;
      Shader.SetGlobalVector("_PlayerPosition", (Vector4) value);
      this._PlacementPosition = value;
      this.CurrentTile = this.GetClosestTileAtWorldPosition(this._PlacementPosition, 1.5f);
      this.Lerp = 0.0f;
      if (!((UnityEngine.Object) this.placementObject != (UnityEngine.Object) null))
        return;
      this.UpdateTileAvailability();
    }
  }

  public bool mouseActive
  {
    get => InputManager.General.MouseInputActive && InputManager.General.MouseInputEnabled;
  }

  public bool OnlyPlaceableInRanch
  {
    get => StructureManager.OnlyPlaceableInRanch.Contains(this.StructureType);
  }

  public void Awake()
  {
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.SetAsInstance)
      PlacementRegion.Instance = this;
    this.cachedColliderPoints = this.polygonCollider2D.points;
  }

  public void OnDestroy()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    PlacementRegion.Instance = (PlacementRegion) null;
  }

  public void OnBrainAssigned()
  {
    if (this.Grid.Count > 0)
      return;
    this.CreateFloodFill();
  }

  public void OnEnable()
  {
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.OnStructuresMoved);
    PlacementRegion.PlacementRegions.Add(this);
  }

  public void OnDisable()
  {
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.OnStructuresMoved);
    PlacementRegion.PlacementRegions.Remove(this);
  }

  public void Play(Vector3 pos = default (Vector3))
  {
    this.positionOffset = pos;
    this.StartCoroutine((IEnumerator) this.PlayRoutine(pos));
  }

  public void Play(int structureID)
  {
    this.StartCoroutine((IEnumerator) this.PlayRoutine(StructureManager.GetStructureByID<StructureBrain>(structureID).Data.Position - Vector3.forward));
  }

  public void PlayMove(Structure structure)
  {
    AudioManager.Instance.PlayOneShot("event:/building/building_pickup_editmode", structure.transform.position);
    this.CurrentMode = PlacementRegion.Mode.Moving;
    this.CurrentStructureToMove = structure;
    this.CurrentStructureToMove.gameObject.SetActive(false);
    this.StructureType = structure.Brain.Data.Type;
    this.structureIsOnlyPlaceableInRanch = this.OnlyPlaceableInRanch;
    this.direction = 1;
    this.Rotation = 0;
    this.CurrentStructureToMove.gameObject.SetActive(false);
    this.PlacementGameObject = TypeAndPlacementObjects.GetByType(this.StructureType).PlacementObject;
    if (this.CurrentMode == PlacementRegion.Mode.Moving)
    {
      PlacementRegion.BuildingEvent buildingBeganMoving = PlacementRegion.OnBuildingBeganMoving;
      if (buildingBeganMoving != null)
        buildingBeganMoving(structure.Structure_Info.ID);
    }
    this.StartCoroutine((IEnumerator) this.PlayRoutine(structure.transform.position));
  }

  public void OnStructuresMoved(StructuresData structure) => this.OnStructuresPlaced();

  public void OnStructuresPlaced()
  {
    if ((UnityEngine.Object) this.structure == (UnityEngine.Object) null || this.structure.Brain == null)
      return;
    this.structureBrain = this.structure.Brain as Structures_PlacementRegion;
    if (this.structureBrain != null)
    {
      this.structureBrain.ResourcesToPlace = new List<PlacementRegion.ResourcesAndCount>((IEnumerable<PlacementRegion.ResourcesAndCount>) this.ResourcesToPlace);
      this.structureBrain.PlaceWeeds = this.PlaceWeeds;
      this.structureBrain.PlaceRubble = this.PlaceRubble;
    }
    this.ranchingGridPositions.Clear();
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      foreach (Structures_Ranch structuresRanch in StructureManager.GetAllStructuresOfType<Structures_Ranch>())
      {
        if (structuresRanch.HasValidEnclosure())
        {
          List<PlacementRegion.TileGridTile> ranchingTiles = structuresRanch.RanchingTiles;
          for (int index = 0; index < ranchingTiles.Count; ++index)
          {
            if (!this.ranchingGridPositions.Contains(ranchingTiles[index].Position))
              this.ranchingGridPositions.Add(ranchingTiles[index].Position);
          }
        }
      }
    }
    this.UpdateFenceablePositions();
  }

  public void UpdateFenceablePositions()
  {
    if (this.StructureType != StructureBrain.TYPES.RANCH_FENCE)
      return;
    List<Structures_Ranch> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Ranch>();
    this.fenceableGridPositions.Clear();
    foreach (Structures_Ranch structuresRanch in structuresOfType)
    {
      List<PlacementRegion.TileGridTile> validFenceTiles = structuresRanch.GetValidFenceTiles();
      for (int index = 0; index < validFenceTiles.Count; ++index)
      {
        if (!this.fenceableGridPositions.Contains(validFenceTiles[index].Position))
          this.fenceableGridPositions.Add(validFenceTiles[index].Position);
      }
    }
    this.touchingRanchGridPositions.Clear();
    foreach (Structures_Ranch structuresRanch in structuresOfType)
    {
      List<PlacementRegion.TileGridTile> surroundingRanchTiles = structuresRanch.GetSurroundingRanchTiles();
      for (int index = 0; index < surroundingRanchTiles.Count; ++index)
      {
        if (!this.touchingRanchGridPositions.Contains(surroundingRanchTiles[index].Position))
          this.touchingRanchGridPositions.Add(surroundingRanchTiles[index].Position);
      }
    }
  }

  public IEnumerator PlayRoutine(Vector3 pos = default (Vector3))
  {
    PlacementRegion placementRegion = this;
    DLCLandController.Instance.HideBridge();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer)
      {
        player.interactor.CurrentInteraction = (Interaction) null;
        player.indicator.gameObject.SetActive(false);
      }
    }
    HUD_Manager.Instance.ShowEditMode(true);
    placementRegion.Rotation = 0;
    int x = 1;
    if ((UnityEngine.Object) placementRegion.CurrentStructureToMove != (UnityEngine.Object) null)
    {
      x = placementRegion.CurrentStructureToMove.Structure_Info.Direction;
      placementRegion.Rotation = placementRegion.CurrentStructureToMove.Structure_Info.Rotation;
    }
    else if ((UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null)
    {
      x = placementRegion.CurrentStructureToMove.Structure_Info.Direction;
      placementRegion.Rotation = placementRegion.CurrentStructureToMove.Structure_Info.Rotation;
    }
    placementRegion.structureIsOnlyPlaceableInRanch = placementRegion.OnlyPlaceableInRanch;
    if (placementRegion.structureIsOnlyPlaceableInRanch)
    {
      bool flag = false;
      foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
      {
        if (ranch.IsValid && ranch.Brain.RanchingTiles.Count > 0)
        {
          foreach (PlacementRegion.TileGridTile ranchingTile in ranch.Brain.RanchingTiles)
          {
            if (ranchingTile.CanPlaceStructure)
            {
              pos = ranchingTile.WorldPosition;
              flag = true;
              break;
            }
          }
          if (flag)
            break;
        }
      }
    }
    placementRegion.direction = x;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    placementRegion.placementObject = UnityEngine.Object.Instantiate<GameObject>(placementRegion.PlacementGameObject, placementRegion.transform.parent).GetComponent<PlacementObject>();
    placementRegion.placementObject.StructureType = placementRegion.StructureType;
    if (placementRegion.StructureType == StructureBrain.TYPES.EDIT_BUILDINGS)
    {
      Vector3 targetWorldPosition = GameManager.GetInstance().CamFollowTarget.GetCameraTargetWorldPosition();
      placementRegion.placementObject.transform.position = new Vector3(targetWorldPosition.x, targetWorldPosition.y, 0.0f);
    }
    else
      placementRegion.placementObject.transform.position = pos == new Vector3() ? new Vector3(-7.6f, -5.61f, 0.0f) : pos;
    placementRegion.placementObject.transform.localScale = new Vector3((float) x, placementRegion.placementObject.transform.localScale.y, placementRegion.placementObject.transform.localScale.z);
    placementRegion.PreviousTile = placementRegion.CurrentTile;
    placementRegion.CurrentTile = placementRegion.GetClosestTileAtWorldPosition(placementRegion.placementObject.transform.position, 1.5f);
    if (placementRegion.StructureType == StructureBrain.TYPES.EDIT_BUILDINGS)
    {
      placementRegion.CurrentMode = PlacementRegion.Mode.Demolishing;
      placementRegion.isEditingBuildings = true;
    }
    if (placementRegion.CurrentMode != PlacementRegion.Mode.Demolishing && placementRegion.CurrentMode != PlacementRegion.Mode.Moving && (UnityEngine.Object) placementRegion.placementUI == (UnityEngine.Object) null && StructuresData.GetCost(placementRegion.StructureType).Count > 0)
      placementRegion.placementUI = UnityEngine.Object.Instantiate<PlacementObjectUI>(placementRegion.PlacementObjectUI, GameObject.FindWithTag("Canvas").transform);
    placementRegion.isPath = StructureBrain.IsPath(placementRegion.StructureType);
    placementRegion.canPlaceObjectOnBuildings = placementRegion.isPath;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(placementRegion.placementObject.gameObject);
    Time.timeScale = 0.0f;
    Debug.Log((object) ("TIME SCALE! " + Time.timeScale.ToString()));
    GameManager.overridePlayerPosition = true;
    yield return (object) placementRegion.PlaceTiles();
    placementRegion.UpdateFenceablePositions();
    WeedManager.HideAll();
    if (placementRegion.isPath)
      PathTileManager.Instance.ShowPathsBeingBuilt();
    yield return (object) placementRegion.StartCoroutine((IEnumerator) placementRegion.PlaceObject());
    DLCLandController.Instance.ShowBridge();
    HUD_Manager.Instance.Show();
    WeatherSystemController.Instance.ShowWeather(false);
    Time.timeScale = 1f;
    placementRegion._PlacementPosition = Vector3.zero;
    placementRegion.previousEditingPosition = Vector3.zero;
    placementRegion.ClearPrefabs();
    HUD_Manager.Instance.ShowEditMode(false);
    GameManager.GetInstance().CamFollowTarget.enabled = true;
    if ((UnityEngine.Object) placementRegion.placementObject != (UnityEngine.Object) null)
      GameManager.GetInstance().RemoveFromCamera(placementRegion.placementObject.gameObject);
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.transitionDurationMultiplier = 0.2f;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.UpdateLighting(true);
    LightingManager.Instance.PrepareLightingSettings();
    placementRegion.tilesInitiated = false;
    yield return (object) placementRegion.ClearTilesIE();
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraResetTargetZoom();
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      if (PlayerFarming.players[index].state.CURRENT_STATE == StateMachine.State.InActive)
        PlayerFarming.players[index].state.CURRENT_STATE = StateMachine.State.Idle;
    }
    AudioManager.Instance.SetBuildSnapshotEnabled(false);
  }

  public bool IsNaturalObstruction(StructureBrain.TYPES type)
  {
    return type == StructureBrain.TYPES.TREE || type == StructureBrain.TYPES.BERRY_BUSH || type == StructureBrain.TYPES.RUBBLE || type == StructureBrain.TYPES.ICE_BLOCK || type == StructureBrain.TYPES.SNOW_DRIFT || type == StructureBrain.TYPES.RUBBLE_BIG || type == StructureBrain.TYPES.WATER_SMALL || type == StructureBrain.TYPES.WATER_MEDIUM || type == StructureBrain.TYPES.WATER_BIG;
  }

  public static PlacementRegion FindPlacementRegion(int ID)
  {
    PlacementRegion placementRegion1 = (PlacementRegion) null;
    foreach (PlacementRegion placementRegion2 in PlacementRegion.PlacementRegions)
    {
      if (placementRegion2.structureBrain.Data.ID == ID)
      {
        placementRegion1 = placementRegion2;
        break;
      }
    }
    return placementRegion1;
  }

  public void CreateDictionaryLookup()
  {
    foreach (PlacementRegion.TileGridTile tileGridTile in this.Grid)
    {
      if (!this.GridTileLookup.ContainsKey(tileGridTile.Position))
        this.GridTileLookup.Add(tileGridTile.Position, tileGridTile);
    }
  }

  public PlacementRegion.TileGridTile GetTileGridTile(Vector2Int Position)
  {
    if (this.GridTileLookup.Count < this.Grid.Count)
      this.CreateDictionaryLookup();
    PlacementRegion.TileGridTile tileGridTile;
    return !this.GridTileLookup.TryGetValue(Position, out tileGridTile) ? (PlacementRegion.TileGridTile) null : tileGridTile;
  }

  public PlacementRegion.TileGridTile GetTileGridTile(float x, float y)
  {
    if (this.GridTileLookup.Count < this.Grid.Count)
      this.CreateDictionaryLookup();
    PlacementRegion.TileGridTile tileGridTile;
    return !this.GridTileLookup.TryGetValue(new Vector2Int((int) x, (int) y), out tileGridTile) ? (PlacementRegion.TileGridTile) null : tileGridTile;
  }

  public void CreateFloodFill()
  {
    if (DataManager.Instance.MAJOR_DLC && (UnityEngine.Object) DLCLandController.Instance != (UnityEngine.Object) null)
      this.polygonCollider2D = DLCLandController.Instance.CombineColliders(this.cachedColliderPoints, this.polygonCollider2D);
    this.Grid.Clear();
    this.GridTileLookup.Clear();
    this.Count = 0;
    this.FloodFillCreateTiles(0, 0);
    if (!DataManager.Instance.MAJOR_DLC || !((UnityEngine.Object) DLCLandController.Instance != (UnityEngine.Object) null))
      return;
    this.Count = 0;
    this.FloodFillCreateTiles(-15, -15);
    Interaction_PurchaseLand.OccupyBridgeArea();
  }

  public IEnumerator PlaceTiles()
  {
    PlacementRegion placementRegion = this;
    if (!placementRegion.tilesInitiated)
    {
      placementRegion.tilesInitiated = true;
      placementRegion.Tiles = new List<PlacementTile>();
      int SplitBetweenFrames = 3;
      int indexCounter = 0;
      foreach (PlacementRegion.TileGridTile tileGridTile in placementRegion.Grid)
      {
        GameObject fromPool = placementRegion.GetFromPool(placementRegion.PlacementSquare);
        fromPool.transform.SetParent(placementRegion.transform, false);
        fromPool.transform.localPosition = new Vector3((float) tileGridTile.Position.x, (float) tileGridTile.Position.y);
        PlacementTile component = fromPool.GetComponent<PlacementTile>();
        component.Position = new Vector3((float) tileGridTile.Position.x, (float) tileGridTile.Position.y);
        component.GridPosition = new Vector2Int(tileGridTile.Position.x, tileGridTile.Position.y);
        placementRegion.Tiles.Add(component);
        ++indexCounter;
        if (indexCounter >= placementRegion.Grid.Count / SplitBetweenFrames)
        {
          indexCounter = 0;
          yield return (object) null;
        }
      }
      placementRegion.OnStructuresPlaced();
    }
  }

  public void PreviewTiles()
  {
    this.PreviewTilesList = new List<PlacementRegion.TileGridTile>();
    this.CreateFloodFill();
  }

  public void FloodFillCreateTiles(int x, int y)
  {
    if (this.GetTileGridTile((float) x, (float) y) != null || this.polygonCollider2D.ClosestPoint((Vector2) this.transform.TransformPoint((Vector3) new Vector2((float) x, (float) y))) != (Vector2) this.transform.TransformPoint((Vector3) new Vector2((float) x, (float) y)) || ++this.Count > this.MaxTileCount)
      return;
    if (Application.isEditor && !Application.isPlaying)
      this.PreviewTilesList.Add(PlacementRegion.TileGridTile.Create(this, new Vector2Int(x, y), false, false));
    else
      this.Grid.Add(PlacementRegion.TileGridTile.Create(this, new Vector2Int(x, y), false, false));
    this.FloodFillCreateTiles(x + 1, y);
    this.FloodFillCreateTiles(x - 1, y);
    this.FloodFillCreateTiles(x, y + 1);
    this.FloodFillCreateTiles(x, y - 1);
  }

  public void ClearTiles()
  {
    if (this.isClearingTiles)
      return;
    this.StartCoroutine((IEnumerator) this.ClearTilesIE());
  }

  public IEnumerator ClearTilesIE()
  {
    this.isClearingTiles = true;
    int SplitBetweenFrames = 3;
    int indexCounter = 0;
    foreach (Component tile in this.Tiles)
    {
      this.ReturnToPool(tile.gameObject);
      ++indexCounter;
      if (indexCounter >= this.Tiles.Count / SplitBetweenFrames)
      {
        indexCounter = 0;
        yield return (object) null;
      }
    }
    this.Tiles.Clear();
    this.isClearingTiles = false;
  }

  public PlacementTile GetTile(Vector3 Position)
  {
    foreach (PlacementTile tile in this.Tiles)
    {
      if (tile.Position == Position)
        return tile;
    }
    return (PlacementTile) null;
  }

  public PlacementTile GetClosestTileAtWorldPosition(Vector3 Position, float maxDistance = -1f)
  {
    if (this.Tiles == null)
      return (PlacementTile) null;
    float num1 = float.MaxValue;
    PlacementTile tileAtWorldPosition = (PlacementTile) null;
    Position = this.transform.InverseTransformPoint(Position);
    foreach (PlacementTile tile in this.Tiles)
    {
      float num2 = this.Distance(Position, tile.Position);
      if ((double) num2 < (double) num1 && ((double) maxDistance == -1.0 || (double) num2 < (double) maxDistance))
      {
        num1 = num2;
        tileAtWorldPosition = tile;
      }
    }
    return tileAtWorldPosition;
  }

  public static Vector2Int GetVector3FromDirection(PlacementRegion.Direction Direction)
  {
    switch (Direction)
    {
      case PlacementRegion.Direction.Right:
        return new Vector2Int(PlacementRegion.Right.x, PlacementRegion.Right.y);
      case PlacementRegion.Direction.Up:
        return new Vector2Int(PlacementRegion.Up.x, PlacementRegion.Up.y);
      case PlacementRegion.Direction.Down:
        return new Vector2Int(PlacementRegion.Down.x, PlacementRegion.Down.y);
      case PlacementRegion.Direction.UpLeft:
        return new Vector2Int(PlacementRegion.UpLeft.x, PlacementRegion.UpLeft.y);
      case PlacementRegion.Direction.DownLeft:
        return new Vector2Int(PlacementRegion.DownLeft.x, PlacementRegion.DownLeft.y);
      case PlacementRegion.Direction.UpRight:
        return new Vector2Int(PlacementRegion.UpRight.x, PlacementRegion.UpRight.y);
      case PlacementRegion.Direction.DownRight:
        return new Vector2Int(PlacementRegion.DownRight.x, PlacementRegion.DownRight.y);
      default:
        return new Vector2Int(PlacementRegion.Left.x, PlacementRegion.Left.y);
    }
  }

  public bool IsValidPlacement(Vector3 Position, bool CheckOccupied, bool AllowObstructions)
  {
    bool flag = true;
    int num1 = -1;
    while (++num1 < this.placementObject.Bounds.x)
    {
      int num2 = -1;
      while (++num2 < this.placementObject.Bounds.y)
      {
        Vector2Int Position1 = new Vector2Int((int) Position.x + num1, (int) Position.y + num2);
        PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(Position1);
        if (tileGridTile == null)
          flag = false;
        else if (!this.canPlaceObjectOnBuildings || this.IsNaturalObstruction(tileGridTile.ObjectOnTile))
        {
          if (CheckOccupied && !tileGridTile.CanPlaceStructure)
          {
            this._StructureBrain.GetOccupationAtPosition(Position1);
            flag = false;
          }
          if (!tileGridTile.CanPlaceObstruction)
          {
            this.GetObstructionAtPosition(Position1, this._StructureBrain.Data);
            if (!AllowObstructions)
              flag = false;
          }
        }
      }
    }
    return flag;
  }

  public void PlaceStructureAtGridPosition(
    StructureBrain.TYPES StructureType,
    Vector2Int Position,
    Vector2Int Bounds)
  {
    this.placementObject = (PlacementObject) null;
    PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile((float) Position.x, (float) Position.y);
    if (tileGridTile == null || !tileGridTile.CanPlaceStructure)
      return;
    this.Build(StructureType, this.transform.TransformPoint(new Vector3((float) tileGridTile.Position.x, (float) tileGridTile.Position.y)), tileGridTile.Position, Bounds, true, false);
  }

  public void PlaceStructureAtWorldPosition(
    StructureBrain.TYPES StructureType,
    Vector3 Position,
    Vector2Int Bounds)
  {
    this.placementObject = (PlacementObject) null;
    Position = this.transform.InverseTransformPoint(Position);
    PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(Position.x, Position.y);
    if (tileGridTile == null || !tileGridTile.CanPlaceStructure)
      return;
    this.Build(StructureType, this.transform.TransformPoint(new Vector3((float) tileGridTile.Position.x, (float) tileGridTile.Position.y)), tileGridTile.Position, Bounds, true, false);
  }

  public bool TryPlaceExistingStructureAtWorldPosition(Structure structure)
  {
    Vector3 vector3 = this.transform.InverseTransformPoint(structure.transform.position);
    vector3.x = Mathf.Round(vector3.x);
    vector3.y = Mathf.Round(vector3.y);
    PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(vector3.x, vector3.y);
    if (tileGridTile == null)
      return false;
    if (!tileGridTile.CanPlaceStructure)
      Debug.LogWarning((object) "Existing structure being placed where structure cannot be placed!");
    structure.Structure_Info.Bounds = StructuresData.GetInfoByType(structure.Type, structure.VariantIndex).Bounds;
    structure.Structure_Info.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
    structure.Structure_Info.GridTilePosition = tileGridTile.Position;
    structure.Brain.AddToGrid();
    return true;
  }

  public bool CanPlaceStructureAtWorldPosition(Vector3 pos)
  {
    PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(pos.x, pos.y);
    return tileGridTile != null && tileGridTile.CanPlaceStructure;
  }

  public PlacementRegion.TileGridTile GetTileGridTileAtWorldPosition(Vector3 Position)
  {
    Position = this.transform.InverseTransformPoint(Position);
    foreach (PlacementRegion.TileGridTile tileAtWorldPosition in this.Grid)
    {
      if ((double) tileAtWorldPosition.Position.x == (double) Position.x && (double) tileAtWorldPosition.Position.y == (double) Position.y)
        return tileAtWorldPosition;
    }
    return (PlacementRegion.TileGridTile) null;
  }

  public PlacementRegion.TileGridTile GetClosestTileGridTileAtWorldPosition(Vector3 Position)
  {
    this.closestTile = this.Grid.Count > 0 ? this.Grid[0] : (PlacementRegion.TileGridTile) null;
    this.tileToPositionDistance = this.Grid.Count > 0 ? this.Distance(this.closestTile.WorldPosition, Position) : 0.0f;
    foreach (PlacementRegion.TileGridTile tileGridTile in this.Grid)
    {
      if ((double) this.Distance(tileGridTile.WorldPosition, Position) < (double) this.tileToPositionDistance)
      {
        this.closestTile = tileGridTile;
        this.tileToPositionDistance = this.Distance(this.closestTile.WorldPosition, Position);
      }
    }
    this.tileToPositionDistance = float.PositiveInfinity;
    return this.closestTile;
  }

  public PlacementRegion.TileGridTile GetFreeClosestTileGridTileAtWorldPosition(Vector3 Position)
  {
    PlacementRegion.TileGridTile tileAtWorldPosition = this.GetClosestTileGridTileAtWorldPosition(Position);
    return tileAtWorldPosition != null && !tileAtWorldPosition.Occupied && !tileAtWorldPosition.Obstructed && !tileAtWorldPosition.ReservedForWaste ? tileAtWorldPosition : (PlacementRegion.TileGridTile) null;
  }

  public IEnumerator PlaceObject()
  {
    PlacementRegion placementRegion = this;
    Structure Closest = (Structure) null;
    if ((UnityEngine.Object) placementRegion.C_GameManagerInstance == (UnityEngine.Object) null)
      placementRegion.C_GameManagerInstance = GameManager.GetInstance();
    if ((UnityEngine.Object) placementRegion.camera == (UnityEngine.Object) null)
      placementRegion.camera = placementRegion.C_GameManagerInstance.CamFollowTarget.GetComponent<Camera>();
    Follower prisonFollower = placementRegion.GetPrisonerRef();
    yield return (object) null;
    Prison prisonPlacementObject = placementRegion.GetPrisonPlacementObjectRef();
    HUD_Manager.Instance.Hide(false);
    if (SettingsManager.Settings.Accessibility.ShowBuildModeFilter)
    {
      LightingManager.Instance.inOverride = true;
      LightingManager.Instance.lerpActive = false;
      placementRegion.LightingSettings.overrideLightingProperties = placementRegion.overrideLightingProperties;
      LightingManager.Instance.overrideSettings = placementRegion.LightingSettings;
      LightingManager.Instance.transitionDurationMultiplier = 0.0f;
      LightingManager.Instance.UpdateLighting(true, true);
    }
    StructuresData structuresData = StructuresData.GetInfoByType(placementRegion.StructureType, 0);
    if (placementRegion.CurrentMode == PlacementRegion.Mode.Moving)
    {
      Debug.Log((object) "Moving!");
      placementRegion.LerpSpeed = 7f;
      placementRegion.PlacementPosition = placementRegion.CurrentStructureToMove.transform.position;
      placementRegion.placementObject.transform.position = placementRegion.CurrentTile.transform.position;
      placementRegion.structureBrain.ClearStructureFromGrid(placementRegion.CurrentStructureToMove.Brain);
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      GameManager.RecalculatePaths();
    }
    else if (structuresData.IsUpgrade && StructureManager.GetAllStructuresOfType(structuresData.UpgradeFromType).Count > 0)
    {
      Debug.Log((object) "Upgrading!");
      placementRegion.CurrentMode = PlacementRegion.Mode.Upgrading;
      placementRegion.LerpSpeed = 2f;
      placementRegion.CurrentStructureToUpgrade = (Structure) null;
      yield return (object) placementRegion.ClearTilesIE();
      float num1 = float.MaxValue;
      for (int index = 0; index < Structure.Structures.Count; ++index)
      {
        Structure structure = Structure.Structures[index];
        if (structure.Type == structuresData.UpgradeFromType && (!structure.Structure_Info.ClaimedByPlayer || structure.Type != StructureBrain.TYPES.BED && structure.Type != StructureBrain.TYPES.BED_2 && structure.Type != StructureBrain.TYPES.BED_3))
        {
          float num2 = placementRegion.Distance(new Vector3((float) placementRegion.Grid[0].Position.x, (float) placementRegion.Grid[0].Position.y), structure.transform.position);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            Closest = structure;
          }
        }
      }
      Debug.Log((object) ("CurrentStructureToUpgrade " + ((object) placementRegion.CurrentStructureToUpgrade)?.ToString()));
      placementRegion.CurrentStructureToUpgrade = Closest;
      if ((bool) (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade && (bool) (UnityEngine.Object) placementRegion.placementObject)
        placementRegion.placementObject.transform.position = placementRegion.CurrentStructureToUpgrade.transform.position;
    }
    else
    {
      placementRegion.LerpSpeed = 7f;
      if (placementRegion.CurrentMode != PlacementRegion.Mode.Demolishing)
        placementRegion.CurrentMode = StructuresData.GetBuildOnlyOne(placementRegion.StructureType) ? PlacementRegion.Mode.Building : PlacementRegion.Mode.MultiBuild;
      placementRegion.PlacementPosition = !placementRegion.isEditingBuildings || !(placementRegion.previousEditingPosition != Vector3.zero) ? placementRegion.placementObject.transform.position : placementRegion.previousEditingPosition;
      if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null)
        placementRegion.placementObject.transform.position = placementRegion.CurrentTile.transform.position;
    }
    if ((bool) (UnityEngine.Object) placementRegion.placementUI && (placementRegion.CurrentMode == PlacementRegion.Mode.Building || placementRegion.CurrentMode == PlacementRegion.Mode.MultiBuild || placementRegion.CurrentMode == PlacementRegion.Mode.Upgrading))
    {
      float t = 0.0f;
      while ((UnityEngine.Object) placementRegion.placementObject.GetComponentInChildren<Structure>(true) == (UnityEngine.Object) null || (double) (t += Time.deltaTime) > 2.5)
        yield return (object) new WaitForEndOfFrame();
      placementRegion.placementUI.Play(placementRegion.placementObject, placementRegion.placementObject.GetComponentInChildren<Structure>(true));
    }
    if (placementRegion.IsPrisonWithPrisoner())
      placementRegion.MovePrisonerWithPrison(prisonFollower, prisonPlacementObject);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    while (true)
    {
      do
      {
        float Speed = 0.0f;
        placementRegion.Lerp = 1f;
        bool Loop = true;
        bool Moving = false;
        while (Loop)
        {
          InputManager.General.MouseInputEnabled = !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
          Vector2 vector2;
          vector2.x = InputManager.Gameplay.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
          vector2.y = InputManager.Gameplay.GetVerticalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
          Vector3 normalized = (Vector3) vector2.normalized;
          placementRegion.InputDelay -= Time.unscaledDeltaTime;
          if (placementRegion.CurrentMode == PlacementRegion.Mode.Upgrading)
          {
            if ((double) placementRegion.Lerp >= 0.5 && (double) placementRegion.InputDelay < 0.0 && ((double) placementRegion.UnsafeAbs(vector2.x) >= 0.30000001192092896 || (double) placementRegion.UnsafeAbs(vector2.y) >= 0.30000001192092896 || placementRegion.mouseActive))
            {
              placementRegion.InputDelay = 0.5f;
              Closest = (Structure) null;
              foreach (Structure structure in Structure.Structures)
              {
                if ((structure.Type != StructureBrain.TYPES.BED && structure.Type != StructureBrain.TYPES.BED_2 && structure.Type != StructureBrain.TYPES.BED_3 || !structure.Structure_Info.ClaimedByPlayer) && (placementRegion.CurrentMode == PlacementRegion.Mode.Demolishing && structure.Structure_Info.GridTilePosition != StructuresData.NullPosition || structure.Type == structuresData.UpgradeFromType) && (UnityEngine.Object) structure != (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade && (UnityEngine.Object) structure != (UnityEngine.Object) null)
                {
                  double num3 = (double) Vector3.Dot(normalized, (structure.transform.position - placementRegion.placementObject.transform.position).normalized);
                  float num4 = (UnityEngine.Object) Closest == (UnityEngine.Object) null ? 0.25f : Vector3.Dot(normalized, (Closest.transform.position - placementRegion.placementObject.transform.position).normalized);
                  float num5 = placementRegion.Distance(placementRegion.placementObject.transform.position, structure.transform.position);
                  float num6 = (UnityEngine.Object) Closest == (UnityEngine.Object) null ? num5 : placementRegion.Distance(placementRegion.placementObject.transform.position, Closest.transform.position);
                  double num7 = (double) num4;
                  if (num3 > num7 && ((double) num5 < (double) num6 || (double) placementRegion.UnsafeAbs(num5 - num6) < 1.0))
                    Closest = structure;
                }
              }
              if ((UnityEngine.Object) Closest != (UnityEngine.Object) null)
              {
                placementRegion.PreviousStructure = placementRegion.CurrentStructureToUpgrade;
                if ((UnityEngine.Object) placementRegion.PreviousStructure != (UnityEngine.Object) null)
                  placementRegion.PreviousStructure.gameObject.SetActive(true);
                placementRegion.CurrentStructureToUpgrade = Closest;
                placementRegion.Lerp = 0.0f;
              }
            }
            if ((UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null)
            {
              placementRegion.Lerp += Time.unscaledDeltaTime * placementRegion.LerpSpeed;
              placementRegion.placementObject.transform.position = Vector3.Lerp(placementRegion.placementObject.transform.position, placementRegion.CurrentStructureToUpgrade.transform.position, Mathf.SmoothStep(0.0f, 1f, placementRegion.Lerp));
              if ((double) placementRegion.Distance(placementRegion.placementObject.transform.position, placementRegion.CurrentStructureToUpgrade.transform.position) < 1.0)
                placementRegion.CurrentStructureToUpgrade.gameObject.SetActive(false);
            }
            if ((InputManager.Gameplay.GetPlaceMoveUpgradeButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || placementRegion.isPath && InputManager.Gameplay.GetPlaceMoveUpgradeButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer)) && (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null)
            {
              AudioManager.Instance.PlayOneShot("event:/building/place_building_spot", placementRegion.placementObject.transform.position);
              Loop = false;
            }
          }
          else
          {
            if ((double) vector2.magnitude <= 0.0 && placementRegion.mouseActive && placementRegion.C_GameManagerInstance.CamFollowTarget.enabled)
            {
              placementRegion.C_GameManagerInstance.CamFollowTarget.enabled = false;
              placementRegion.C_GameManagerInstance.RemoveFromCamera(placementRegion.placementObject.gameObject);
            }
            else if (!placementRegion.mouseActive && !placementRegion.C_GameManagerInstance.CamFollowTarget.enabled)
            {
              placementRegion.C_GameManagerInstance.CamFollowTarget.CurrentPosition = placementRegion.C_GameManagerInstance.CamFollowTarget.transform.position;
              placementRegion.C_GameManagerInstance.CamFollowTarget.enabled = true;
              placementRegion.C_GameManagerInstance.AddToCamera(placementRegion.placementObject.gameObject);
            }
            Vector2 localPosition = (Vector2) placementRegion.placementObject.gameObject.transform.localPosition;
            if ((double) placementRegion.UnsafeAbs(vector2.x) >= 0.30000001192092896 || (double) placementRegion.UnsafeAbs(vector2.y) >= 0.30000001192092896 || InputManager.General.MouseInputActive)
            {
              Vector3 direction = PlacementRegion.\u003CPlaceObject\u003Eg__GetDirection\u007C137_0(normalized);
              bool flag1 = false;
              if ((double) vector2.magnitude <= 0.0 && placementRegion.mouseActive)
              {
                Vector3 mousePositionWorld = placementRegion.GetMousePositionWorld();
                PlacementTile tileAtWorldPosition = placementRegion.GetClosestTileAtWorldPosition(placementRegion.placementObject.transform.position);
                if ((double) placementRegion.Distance(placementRegion.transform.TransformPoint(tileAtWorldPosition.Position), mousePositionWorld) > (double) Mathf.Max((float) placementRegion.placementObject.Bounds.x / 2f, 1.5f))
                {
                  direction = PlacementRegion.\u003CPlaceObject\u003Eg__GetDirection\u007C137_0((mousePositionWorld - placementRegion.placementObject.transform.position).normalized);
                }
                else
                {
                  direction = (Vector3) Vector3Int.zero;
                  flag1 = true;
                }
              }
              bool flag2 = false;
              if (placementRegion.mouseActive)
              {
                Vector3 mousePositionWorld = placementRegion.GetMousePositionWorld();
                float num8 = placementRegion.UnsafeAbs(TownCentre.Instance.transform.position.x - mousePositionWorld.x);
                float num9 = placementRegion.UnsafeAbs(TownCentre.Instance.transform.position.y - (mousePositionWorld.y + (float) placementRegion.placementObject.Bounds.y / 2f));
                flag2 = (double) num8 < 14.0 && (double) num9 < 8.1000003814697266;
                if (!flag2 && DataManager.Instance.LandPurchased >= 0)
                  flag2 = (double) placementRegion.UnsafeAbs(-35f - mousePositionWorld.y) < 8.0 && (double) num8 < 15.0;
              }
              else if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null)
              {
                float num10 = placementRegion.UnsafeAbs(TownCentre.Instance.transform.position.x - placementRegion.CurrentTile.transform.position.x);
                float num11 = placementRegion.UnsafeAbs(TownCentre.Instance.transform.position.y - (placementRegion.CurrentTile.transform.position.y + (float) placementRegion.placementObject.Bounds.y / 2f));
                flag2 = (double) num10 < 14.0 && (double) num11 < 8.1000003814697266;
                if (!flag2 && DataManager.Instance.LandPurchased >= 0)
                {
                  float x = placementRegion.CurrentTile.transform.position.x;
                  flag2 = (double) placementRegion.UnsafeAbs(-35f - placementRegion.CurrentTile.transform.position.y) < 8.0 && (double) num10 < 15.0;
                  if ((double) num10 >= 15.0 && (double) direction.x > 0.0 && (double) x < 0.0 || (double) direction.x < 0.0 && (double) x > 0.0)
                    flag2 = true;
                }
              }
              if (((UnityEngine.Object) placementRegion.CurrentTile == (UnityEngine.Object) null || !placementRegion.IsValidPlacement(placementRegion.CurrentTile.Position, false, true)) && (UnityEngine.Object) placementRegion.PreviousTile != (UnityEngine.Object) null && placementRegion.mouseActive)
                placementRegion.CurrentTile = placementRegion.PreviousTile;
              if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && !(direction == (Vector3) Vector3Int.zero) && !placementRegion.\u003CPlaceObject\u003Eg__SetTile\u007C137_1(placementRegion.CurrentTile.Position, direction) && !placementRegion.\u003CPlaceObject\u003Eg__SetTile\u007C137_1(placementRegion.CurrentTile.Position, direction + Vector3.left) && !placementRegion.\u003CPlaceObject\u003Eg__SetTile\u007C137_1(placementRegion.CurrentTile.Position, direction + Vector3.right) && !placementRegion.\u003CPlaceObject\u003Eg__SetTile\u007C137_1(placementRegion.CurrentTile.Position, direction + Vector3.up) && !placementRegion.\u003CPlaceObject\u003Eg__SetTile\u007C137_1(placementRegion.CurrentTile.Position, direction + Vector3.down) && ((double) vector2.y <= 0.0 || !placementRegion.\u003CPlaceObject\u003Eg__SetTile\u007C137_1(placementRegion.CurrentTile.Position, (Vector3) PlacementRegion.Up)) && (double) vector2.y < 0.0)
                placementRegion.\u003CPlaceObject\u003Eg__SetTile\u007C137_1(placementRegion.CurrentTile.Position, (Vector3) PlacementRegion.Down);
              if (placementRegion.shakeTween != null)
              {
                placementRegion.shakeTween.Kill();
                placementRegion.shakeTween = (Tween) null;
              }
              if ((double) vector2.magnitude <= 0.0 && placementRegion.mouseActive)
              {
                Vector3 mousePositionWorld = placementRegion.GetMousePositionWorld();
                Vector3 vector3 = placementRegion.transform.InverseTransformPoint(mousePositionWorld);
                bool flag3 = placementRegion.IsValidPlacement(new Vector3((float) Mathf.RoundToInt(vector3.x), (float) Mathf.RoundToInt(vector3.y), 0.0f), false, true);
                if (flag3 | flag2 | flag1)
                {
                  if (placementRegion.moveTween != null)
                  {
                    placementRegion.moveTween.Kill();
                    placementRegion.moveTween = (Tween) null;
                  }
                  if (placementRegion.prisonerMoveTween != null)
                  {
                    placementRegion.prisonerMoveTween.Kill();
                    placementRegion.prisonerMoveTween = (Tween) null;
                  }
                  if ((double) mousePositionWorld.x > (double) PlacementRegion.X_Constraints.x && (double) mousePositionWorld.x < (double) PlacementRegion.X_Constraints.y && (double) mousePositionWorld.y > (double) PlacementRegion.Y_Constraints.x && (double) mousePositionWorld.y < (double) PlacementRegion.Y_Constraints.y)
                    placementRegion.placementObject.transform.position = mousePositionWorld;
                  if (flag3)
                    placementRegion.PreviousTile = placementRegion.CurrentTile;
                }
                else if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && (placementRegion.moveTween == null || !placementRegion.moveTween.active))
                {
                  placementRegion.moveTween = (Tween) placementRegion.placementObject.transform.DOMove(placementRegion.CurrentTile.transform.position, placementRegion.TopSpeed * 2f).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late, true);
                  if (placementRegion.IsPrisonWithPrisoner())
                    placementRegion.PrisonerMoveTween(prisonFollower, prisonPlacementObject);
                }
                placementRegion.CurrentTile = placementRegion.GetClosestTileAtWorldPosition(mousePositionWorld);
                Vector3 viewportPoint = placementRegion.camera.ScreenToViewportPoint((Vector3) InputManager.General.GetMousePosition(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
                viewportPoint.x -= 0.5f;
                viewportPoint.y -= 0.5f;
                bool flag4 = (double) placementRegion.Distance(placementRegion.CurrentTile.transform.position, placementRegion.GetMousePositionWorld()) < 5.0;
                if ((double) viewportPoint.y > 0.30000001192092896 && (double) placementRegion.C_GameManagerInstance.CamFollowTarget.transform.position.y < -5.0)
                  flag4 = true;
                if (flag4 | flag2 && ((double) placementRegion.UnsafeAbs(viewportPoint.x) > 0.30000001192092896 || (double) placementRegion.UnsafeAbs(viewportPoint.y) > 0.30000001192092896))
                {
                  placementRegion.cachedDirection = viewportPoint;
                  Speed = Mathf.Clamp(Speed + 1f, 0.0f, 20f);
                }
                else
                  Speed = Mathf.Clamp(Speed - 2f, 0.0f, 20f);
                if ((double) Speed != 0.0)
                  placementRegion.C_GameManagerInstance.CamFollowTarget.transform.position += Time.unscaledDeltaTime * placementRegion.cachedDirection * Speed;
              }
              if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && !flag2 && !placementRegion.mouseActive)
              {
                if (placementRegion.moveTween == null || !placementRegion.moveTween.active)
                {
                  placementRegion.moveTween = (Tween) placementRegion.placementObject.transform.DOMove(placementRegion.CurrentTile.transform.position, placementRegion.TopSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late, true);
                  if (placementRegion.IsPrisonWithPrisoner())
                    placementRegion.PrisonerMoveTween(prisonFollower, prisonPlacementObject);
                }
              }
              else
              {
                Vector3 vector3 = placementRegion.placementObject.transform.position + placementRegion.TopSpeed * Time.unscaledDeltaTime * normalized;
                if ((double) localPosition.x > (double) PlacementRegion.X_Constraints.x && (double) localPosition.x < (double) PlacementRegion.X_Constraints.y && (double) localPosition.y > (double) PlacementRegion.Y_Constraints.x && (double) localPosition.y < (double) PlacementRegion.Y_Constraints.y)
                  placementRegion.placementObject.transform.position = vector3;
                else if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null)
                  placementRegion.placementObject.transform.position = placementRegion.CurrentTile.transform.position;
                else
                  placementRegion.placementObject.transform.position = Vector3.zero;
              }
              placementRegion.PlacementPosition = placementRegion.placementObject.transform.position;
              if (placementRegion.IsPrisonWithPrisoner())
                placementRegion.MovePrisonerWithPrison(prisonFollower, prisonPlacementObject);
              if (!Moving)
              {
                placementRegion.placementObject.SetScale(new Vector3(1.1f, 0.9f, 0.9f));
                Moving = true;
              }
            }
            else
            {
              if (Moving)
              {
                placementRegion.placementObject.SetScale(new Vector3(1.1f, 0.9f, 0.9f));
                Moving = false;
              }
              if ((UnityEngine.Object) placementRegion.placementObject != (UnityEngine.Object) null && (UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null)
              {
                placementRegion.moveTween.Kill();
                placementRegion.prisonerMoveTween.Kill();
                placementRegion.placementObject.transform.position = Vector3.Lerp(placementRegion.placementObject.transform.position, placementRegion.CurrentTile.transform.position, Time.unscaledDeltaTime * placementRegion.LerpToTileSpeed);
              }
              if (placementRegion.IsPrisonWithPrisoner())
                placementRegion.MovePrisonerWithPrison(prisonFollower, prisonPlacementObject);
            }
            if (InputManager.Gameplay.GetPlaceMoveUpgradeButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || placementRegion.isPath && InputManager.Gameplay.GetPlaceMoveUpgradeButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
            {
              if (placementRegion.CurrentMode == PlacementRegion.Mode.Demolishing)
              {
                Structure hoveredStructure = placementRegion.GetHoveredStructure();
                if ((bool) (UnityEngine.Object) hoveredStructure && hoveredStructure.Structure_Info.CanBeMoved)
                {
                  placementRegion.ClearPrefabs();
                  placementRegion.StopAllCoroutines();
                  placementRegion.PlayMove(hoveredStructure);
                  yield break;
                }
              }
              else
              {
                bool AllowObstructions = placementRegion.CurrentMode == PlacementRegion.Mode.Building || placementRegion.CurrentMode == PlacementRegion.Mode.MultiBuild || placementRegion.CurrentMode == PlacementRegion.Mode.Moving;
                bool flag = true;
                if (placementRegion.isPath && (UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.GetTileGridTile(placementRegion.CurrentTile.GridPosition) != null)
                {
                  StructureBrain.TYPES tileTypeAtPosition = PathTileManager.Instance.GetTileTypeAtPosition(placementRegion.PlacementPosition);
                  flag = tileTypeAtPosition == StructureBrain.TYPES.NONE || tileTypeAtPosition != placementRegion.StructureType;
                }
                if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.structureIsOnlyPlaceableInRanch && !placementRegion.ranchingGridPositions.Contains(placementRegion.CurrentTile.GridPosition))
                  flag = false;
                if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.StructureType == StructureBrain.TYPES.RANCH_FENCE && !placementRegion.fenceableGridPositions.Contains(placementRegion.CurrentTile.GridPosition))
                  flag = false;
                if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && !placementRegion.structureIsOnlyPlaceableInRanch && placementRegion.StructureType != StructureBrain.TYPES.RANCH_FENCE && placementRegion.ranchingGridPositions.Contains(placementRegion.CurrentTile.GridPosition) && !placementRegion.isPath)
                  flag = false;
                if (flag && (UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.IsValidPlacement(placementRegion.CurrentTile.Position, !placementRegion.isPath, AllowObstructions))
                {
                  AudioManager.Instance.PlayOneShot("event:/building/place_building_spot", placementRegion.placementObject.transform.position);
                  Loop = false;
                }
                else if (!placementRegion.isPath)
                {
                  Debug.Log((object) "Cant build here");
                  placementRegion.placementObject.gameObject.transform.DOKill();
                  PlayerFarming.Instance.indicator.gameObject.transform.DOKill();
                  placementRegion.shakeTween = (Tween) placementRegion.placementObject.gameObject.transform.DOShakePosition(0.3f, new Vector3(0.5f, 0.0f), 20).SetUpdate<Tweener>(true);
                  PlayerFarming.Instance.indicator.gameObject.transform.DOShakePosition(0.3f, new Vector3(0.5f, 0.0f), 20).SetUpdate<Tweener>(true);
                  AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", placementRegion.placementObject.transform.position);
                }
              }
            }
          }
          if ((double) vector2.x > -0.30000001192092896 && (double) vector2.x < 0.30000001192092896 && (double) vector2.y > -0.30000001192092896 && (double) vector2.y < 0.30000001192092896)
            placementRegion.InputDelay = 0.0f;
          if (placementRegion.isPath || placementRegion.GetPathAtPosition() != StructureBrain.TYPES.NONE && ((UnityEngine.Object) placementRegion.placementObject == (UnityEngine.Object) null || placementRegion.isEditingBuildings || placementRegion.isPath) && (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade == (UnityEngine.Object) null && (UnityEngine.Object) placementRegion.CurrentStructureToMove == (UnityEngine.Object) null)
          {
            if (InputManager.Gameplay.GetInteract3ButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
              PathTileManager.Instance.DeleteTile(placementRegion.PlacementPosition);
          }
          else if (InputManager.Gameplay.GetRemoveFlipButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          {
            if ((UnityEngine.Object) placementRegion.placementObject != (UnityEngine.Object) null && placementRegion.StructureType == StructureBrain.TYPES.RANCH_FENCE)
            {
              Structure hoveredStructure = placementRegion.GetHoveredStructure();
              if ((UnityEngine.Object) hoveredStructure != (UnityEngine.Object) null && hoveredStructure.Type == StructureBrain.TYPES.RANCH_FENCE)
              {
                placementRegion.DestroyBuilding(hoveredStructure);
                for (int index = 0; index < StructuresData.GetCost(placementRegion.StructureType).Count; ++index)
                  Inventory.AddItem(StructuresData.GetCost(placementRegion.StructureType)[index].CostItem, StructuresData.GetCost(placementRegion.StructureType)[index].CostValue);
                if ((UnityEngine.Object) placementRegion.placementUI != (UnityEngine.Object) null)
                  placementRegion.placementUI.UpdateText(placementRegion.StructureType);
              }
            }
            if ((bool) (UnityEngine.Object) placementRegion.placementObject && placementRegion.StructureType != StructureBrain.TYPES.EDIT_BUILDINGS && StructuresData.CanBeFlipped(placementRegion.StructureType))
            {
              if (placementRegion.StructureType != StructureBrain.TYPES.FARM_CROP_GROWER)
                placementRegion.placementObject.transform.localScale = new Vector3(placementRegion.placementObject.transform.localScale.x * -1f, placementRegion.placementObject.transform.localScale.y, placementRegion.placementObject.transform.localScale.z);
              placementRegion.Rotation = (int) Mathf.Repeat((float) (placementRegion.Rotation + 1), 4f);
              placementRegion.direction = (int) placementRegion.placementObject.transform.localScale.x;
              AudioManager.Instance.PlayOneShot(placementRegion.flipSFX);
            }
            else if ((UnityEngine.Object) placementRegion.CurrentStructureToMove != (UnityEngine.Object) null && StructuresData.CanBeFlipped(placementRegion.CurrentStructureToMove.Type))
            {
              placementRegion.CurrentStructureToMove.Structure_Info.Direction *= -1;
              placementRegion.CurrentStructureToMove.transform.localScale = new Vector3((float) placementRegion.CurrentStructureToMove.Structure_Info.Direction, placementRegion.CurrentStructureToMove.transform.localScale.y, placementRegion.CurrentStructureToMove.transform.localScale.z);
              placementRegion.CurrentStructureToMove.Structure_Info.Rotation = (int) Mathf.Repeat((float) (placementRegion.CurrentStructureToMove.Structure_Info.Rotation + 1), 4f);
              placementRegion.direction = placementRegion.CurrentStructureToMove.Structure_Info.Direction;
              AudioManager.Instance.PlayOneShot(placementRegion.flipSFX);
            }
            else if ((UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null && StructuresData.CanBeFlipped(placementRegion.CurrentStructureToUpgrade.Type))
            {
              placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction *= -1;
              placementRegion.CurrentStructureToUpgrade.transform.localScale = new Vector3((float) placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction, placementRegion.CurrentStructureToUpgrade.transform.localScale.y, placementRegion.CurrentStructureToUpgrade.transform.localScale.z);
              placementRegion.CurrentStructureToMove.Structure_Info.Rotation = (int) Mathf.Repeat((float) (placementRegion.CurrentStructureToMove.Structure_Info.Rotation + 1), 4f);
              placementRegion.direction = placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction;
              AudioManager.Instance.PlayOneShot(placementRegion.flipSFX);
            }
          }
          if (InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          {
            if (placementRegion.CurrentMode == PlacementRegion.Mode.Moving)
            {
              placementRegion.CurrentStructureToMove.Brain.AddToGrid();
              PlacementRegion.BuildingEvent onBuildingPlaced = PlacementRegion.OnBuildingPlaced;
              if (onBuildingPlaced != null)
                onBuildingPlaced(placementRegion.CurrentStructureToMove.Structure_Info.ID);
              foreach (SpriteRenderer componentsInChild in placementRegion.CurrentStructureToMove.gameObject.GetComponentsInChildren<SpriteRenderer>())
              {
                if (componentsInChild.gameObject.activeSelf && !componentsInChild.CompareTag("IgnoreBuildRendering"))
                  componentsInChild.color = new Color(1f, 1f, 1f, 1f);
              }
              if (placementRegion.IsPrisonWithPrisoner())
                placementRegion.ResetPrisonerOnCancelReposition(prisonFollower);
              if (placementRegion.isEditingBuildings)
              {
                placementRegion.previousEditingPosition = placementRegion.CurrentStructureToMove.transform.position;
                placementRegion.ClearPrefabs();
                placementRegion.StopAllCoroutines();
                placementRegion.CurrentMode = PlacementRegion.Mode.Demolishing;
                placementRegion.PlacementGameObject = TypeAndPlacementObjects.GetByType(StructureBrain.TYPES.EDIT_BUILDINGS).PlacementObject;
                placementRegion.StructureType = StructureBrain.TYPES.EDIT_BUILDINGS;
                placementRegion.structureIsOnlyPlaceableInRanch = placementRegion.OnlyPlaceableInRanch;
                placementRegion.Play(placementRegion.previousEditingPosition);
                yield break;
              }
            }
            else
              HUD_Manager.Instance.Show();
            DLCLandController.Instance.ShowBridge();
            Time.timeScale = 1f;
            placementRegion._PlacementPosition = Vector3.zero;
            placementRegion.previousEditingPosition = Vector3.zero;
            placementRegion.ClearPrefabs();
            placementRegion.isEditingBuildings = false;
            placementRegion._PlacementPosition = Vector3.zero;
            placementRegion.previousEditingPosition = Vector3.zero;
            PlayerFarming.Instance.indicator.HasSecondaryInteraction = false;
            PlayerFarming.Instance.indicator.SecondaryText.text = "";
            PlayerFarming.Instance.indicator.HideTopInfo();
            placementRegion.C_GameManagerInstance.CamFollowTarget.enabled = true;
            placementRegion.C_GameManagerInstance.RemoveFromCamera(placementRegion.placementObject.gameObject);
            placementRegion.tilesInitiated = false;
            yield return (object) placementRegion.ClearTilesIE();
            yield break;
          }
          if (InputManager.Gameplay.GetRemoveFlipButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || placementRegion.isPath && InputManager.Gameplay.GetRemoveFlipButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          {
            if (placementRegion.isPath)
            {
              PathTileManager.Instance.DeleteTile(placementRegion.PlacementPosition);
              if ((bool) (UnityEngine.Object) placementRegion.placementUI && placementRegion.CurrentMode == PlacementRegion.Mode.MultiBuild)
                placementRegion.placementUI.UpdateText(placementRegion.StructureType);
            }
            else if (placementRegion.CurrentMode == PlacementRegion.Mode.Demolishing && (UnityEngine.Object) placementRegion.GetHoveredStructure() != (UnityEngine.Object) null && placementRegion.GetHoveredStructure().Brain.Data.IsDeletable)
            {
              Loop = false;
              Structure hoveredStructure = placementRegion.GetHoveredStructure();
              if ((UnityEngine.Object) hoveredStructure != (UnityEngine.Object) null)
                placementRegion.DestroyBuilding(hoveredStructure);
            }
            else if (placementRegion.GetPathAtPosition() != StructureBrain.TYPES.NONE && placementRegion.CurrentMode != PlacementRegion.Mode.Building && placementRegion.CurrentMode != PlacementRegion.Mode.MultiBuild && placementRegion.CurrentMode != PlacementRegion.Mode.Moving)
              PathTileManager.Instance.DeleteTile(placementRegion.PlacementPosition);
          }
          yield return (object) null;
        }
        switch (placementRegion.CurrentMode)
        {
          case PlacementRegion.Mode.Building:
          case PlacementRegion.Mode.MultiBuild:
            bool ManageCamera = placementRegion.CurrentMode == PlacementRegion.Mode.Building;
            placementRegion.Build(placementRegion.StructureType, placementRegion.CurrentTile.transform.position, placementRegion.CurrentTile.GridPosition, placementRegion.placementObject.Bounds, false, ManageCamera);
            break;
          case PlacementRegion.Mode.Upgrading:
            placementRegion.Upgrade();
            break;
          case PlacementRegion.Mode.Moving:
            placementRegion.previousEditingPosition = placementRegion.CurrentTile.transform.position;
            PlacementRegion.BuildingEvent onBuildingPlaced1 = PlacementRegion.OnBuildingPlaced;
            if (onBuildingPlaced1 != null)
              onBuildingPlaced1(placementRegion.CurrentStructureToMove.Structure_Info.ID);
            placementRegion.MoveBuilding(placementRegion.CurrentTile.transform.position, placementRegion.CurrentTile.GridPosition);
            placementRegion.CurrentMode = PlacementRegion.Mode.Demolishing;
            placementRegion.PlacementGameObject = TypeAndPlacementObjects.GetByType(StructureBrain.TYPES.EDIT_BUILDINGS).PlacementObject;
            placementRegion.StructureType = StructureBrain.TYPES.EDIT_BUILDINGS;
            placementRegion.structureIsOnlyPlaceableInRanch = placementRegion.OnlyPlaceableInRanch;
            placementRegion.ClearPrefabs();
            placementRegion.StopAllCoroutines();
            placementRegion.Play(placementRegion.previousEditingPosition);
            yield break;
        }
        if (placementRegion.CurrentMode != PlacementRegion.Mode.MultiBuild && placementRegion.CurrentMode != PlacementRegion.Mode.Demolishing && (placementRegion.CurrentMode != PlacementRegion.Mode.Upgrading || StructuresData.GetBuildOnlyOne(placementRegion.StructureType)) || !StructuresData.CanAfford(placementRegion.StructureType))
          goto label_181;
      }
      while (placementRegion.CurrentMode != PlacementRegion.Mode.Upgrading);
      placementRegion.C_GameManagerInstance.RemoveAllFromCamera();
      placementRegion.C_GameManagerInstance.AddToCamera(placementRegion.placementObject.gameObject);
    }
label_181:
    yield return (object) new WaitForSecondsRealtime(0.15f);
  }

  public static List<PlacementRegion.TileGridTile> GetSurroundingTiles(
    PlacementRegion.TileGridTile tile)
  {
    List<PlacementRegion.TileGridTile> surroundingTiles = new List<PlacementRegion.TileGridTile>();
    int num1 = -2;
    while (num1++ < 1)
    {
      int num2 = -2;
      while (num2++ < 1)
      {
        if (Mathf.Abs(num1) + Mathf.Abs(num2) == 1)
        {
          PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(tile.Position.x + num1, tile.Position.y + num2));
          if (tileGridTile != null && !surroundingTiles.Contains(tileGridTile))
            surroundingTiles.Add(tileGridTile);
        }
      }
    }
    return surroundingTiles;
  }

  public IEnumerator PlaceObject_o()
  {
    PlacementRegion placementRegion = this;
    HUD_Manager.Instance.Hide(false);
    if (SettingsManager.Settings.Accessibility.ShowBuildModeFilter)
    {
      LightingManager.Instance.inOverride = true;
      placementRegion.LightingSettings.overrideLightingProperties = placementRegion.overrideLightingProperties;
      LightingManager.Instance.overrideSettings = placementRegion.LightingSettings;
      LightingManager.Instance.transitionDurationMultiplier = 0.0f;
      LightingManager.Instance.UpdateLighting(true, true);
    }
    StructuresData structuresData = StructuresData.GetInfoByType(placementRegion.StructureType, 0);
    if (placementRegion.CurrentMode == PlacementRegion.Mode.Moving)
    {
      Debug.Log((object) "Moving!");
      placementRegion.LerpSpeed = 7f;
      placementRegion.PlacementPosition = placementRegion.CurrentStructureToMove.transform.position;
      placementRegion.placementObject.transform.position = placementRegion.CurrentTile.transform.position;
      placementRegion.structureBrain.ClearStructureFromGrid(placementRegion.CurrentStructureToMove.Brain);
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      GameManager.RecalculatePaths();
    }
    else if (structuresData.IsUpgrade && StructureManager.GetAllStructuresOfType(structuresData.UpgradeFromType).Count > 0)
    {
      Debug.Log((object) "Upgrading!");
      placementRegion.CurrentMode = PlacementRegion.Mode.Upgrading;
      placementRegion.LerpSpeed = 2f;
      placementRegion.CurrentStructureToUpgrade = (Structure) null;
      yield return (object) placementRegion.ClearTilesIE();
      float num1 = float.MaxValue;
      Structure structure1 = (Structure) null;
      foreach (Structure structure2 in Structure.Structures)
      {
        if (structure2.Type == structuresData.UpgradeFromType && (structure2.Type != StructureBrain.TYPES.BED && structure2.Type != StructureBrain.TYPES.BED_2 && structure2.Type != StructureBrain.TYPES.BED_3 || !structure2.Structure_Info.ClaimedByPlayer))
        {
          float num2 = Vector3.Distance(new Vector3((float) placementRegion.Grid[0].Position.x, (float) placementRegion.Grid[0].Position.y), structure2.transform.position);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            structure1 = structure2;
          }
        }
      }
      Debug.Log((object) ("CurrentStructureToUpgrade " + ((object) placementRegion.CurrentStructureToUpgrade)?.ToString()));
      placementRegion.CurrentStructureToUpgrade = structure1;
      if ((bool) (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade && (bool) (UnityEngine.Object) placementRegion.placementObject)
        placementRegion.placementObject.transform.position = placementRegion.CurrentStructureToUpgrade.transform.position;
    }
    else
    {
      placementRegion.LerpSpeed = 7f;
      if (placementRegion.CurrentMode != PlacementRegion.Mode.Demolishing)
        placementRegion.CurrentMode = StructuresData.GetBuildOnlyOne(placementRegion.StructureType) ? PlacementRegion.Mode.Building : PlacementRegion.Mode.MultiBuild;
      placementRegion.PlacementPosition = !placementRegion.isEditingBuildings || !(placementRegion.previousEditingPosition != Vector3.zero) ? placementRegion.placementObject.transform.position : placementRegion.previousEditingPosition;
      placementRegion.placementObject.transform.position = placementRegion.CurrentTile.transform.position;
    }
    if ((bool) (UnityEngine.Object) placementRegion.placementUI && (placementRegion.CurrentMode == PlacementRegion.Mode.Building || placementRegion.CurrentMode == PlacementRegion.Mode.MultiBuild || placementRegion.CurrentMode == PlacementRegion.Mode.Upgrading))
    {
      float t = 0.0f;
      while ((UnityEngine.Object) placementRegion.placementObject.GetComponentInChildren<Structure>(true) == (UnityEngine.Object) null || (double) (t += Time.deltaTime) > 2.5)
        yield return (object) new WaitForEndOfFrame();
      placementRegion.placementUI.Play(placementRegion.placementObject, placementRegion.placementObject.GetComponentInChildren<Structure>(true));
    }
    yield return (object) new WaitForSecondsRealtime(0.1f);
    while (true)
    {
      do
      {
        float Speed = 0.0f;
        placementRegion.Lerp = 1f;
        bool Loop = true;
        bool Moving = false;
        while (Loop)
        {
          Vector2 vector2;
          vector2.x = InputManager.Gameplay.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
          vector2.y = InputManager.Gameplay.GetVerticalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
          placementRegion.InputDelay -= Time.unscaledDeltaTime;
          if (placementRegion.CurrentMode == PlacementRegion.Mode.Upgrading)
          {
            if ((double) placementRegion.Lerp >= 0.5 && (double) placementRegion.InputDelay < 0.0 && ((double) Mathf.Abs(vector2.x) >= 0.30000001192092896 || (double) Mathf.Abs(vector2.y) >= 0.30000001192092896 || placementRegion.mouseActive))
            {
              placementRegion.InputDelay = 0.5f;
              Structure structure3 = (Structure) null;
              foreach (Structure structure4 in Structure.Structures)
              {
                if ((structure4.Type != StructureBrain.TYPES.BED && structure4.Type != StructureBrain.TYPES.BED_2 && structure4.Type != StructureBrain.TYPES.BED_3 || !structure4.Structure_Info.ClaimedByPlayer) && (placementRegion.CurrentMode == PlacementRegion.Mode.Demolishing && structure4.Structure_Info.GridTilePosition != StructuresData.NullPosition || structure4.Type == structuresData.UpgradeFromType) && (UnityEngine.Object) structure4 != (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade && (UnityEngine.Object) structure4 != (UnityEngine.Object) null)
                {
                  Vector3 normalized1 = (Vector3) vector2.normalized;
                  Vector3 vector3 = structure4.transform.position - placementRegion.placementObject.transform.position;
                  Vector3 normalized2 = vector3.normalized;
                  double num3 = (double) Vector3.Dot(normalized1, normalized2);
                  double num4;
                  if (!((UnityEngine.Object) structure3 == (UnityEngine.Object) null))
                  {
                    Vector3 normalized3 = (Vector3) vector2.normalized;
                    vector3 = structure3.transform.position - placementRegion.placementObject.transform.position;
                    Vector3 normalized4 = vector3.normalized;
                    num4 = (double) Vector3.Dot(normalized3, normalized4);
                  }
                  else
                    num4 = 0.25;
                  float num5 = (float) num4;
                  float num6 = Vector3.Distance(placementRegion.placementObject.transform.position, structure4.transform.position);
                  float num7 = (UnityEngine.Object) structure3 == (UnityEngine.Object) null ? num6 : Vector3.Distance(placementRegion.placementObject.transform.position, structure3.transform.position);
                  double num8 = (double) num5;
                  if (num3 > num8 && ((double) num6 < (double) num7 || (double) Mathf.Abs(num6 - num7) < 1.0))
                    structure3 = structure4;
                }
              }
              if ((UnityEngine.Object) structure3 != (UnityEngine.Object) null)
              {
                placementRegion.PreviousStructure = placementRegion.CurrentStructureToUpgrade;
                if ((UnityEngine.Object) placementRegion.PreviousStructure != (UnityEngine.Object) null)
                  placementRegion.PreviousStructure.gameObject.SetActive(true);
                placementRegion.CurrentStructureToUpgrade = structure3;
                placementRegion.Lerp = 0.0f;
              }
            }
            if ((UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null)
            {
              placementRegion.Lerp += Time.unscaledDeltaTime * placementRegion.LerpSpeed;
              placementRegion.placementObject.transform.position = Vector3.Lerp(placementRegion.placementObject.transform.position, placementRegion.CurrentStructureToUpgrade.transform.position, Mathf.SmoothStep(0.0f, 1f, placementRegion.Lerp));
              if ((double) Vector3.Distance(placementRegion.placementObject.transform.position, placementRegion.CurrentStructureToUpgrade.transform.position) < 1.0)
                placementRegion.CurrentStructureToUpgrade.gameObject.SetActive(false);
            }
            if ((InputManager.Gameplay.GetPlaceMoveUpgradeButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || placementRegion.isPath && InputManager.Gameplay.GetPlaceMoveUpgradeButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer)) && (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null)
            {
              AudioManager.Instance.PlayOneShot("event:/building/place_building_spot", placementRegion.placementObject.transform.position);
              Loop = false;
            }
          }
          else
          {
            if ((double) vector2.magnitude <= 0.0 && placementRegion.mouseActive && GameManager.GetInstance().CamFollowTarget.enabled)
            {
              GameManager.GetInstance().CamFollowTarget.enabled = false;
              GameManager.GetInstance().RemoveFromCamera(placementRegion.placementObject.gameObject);
            }
            else if (!placementRegion.mouseActive && !GameManager.GetInstance().CamFollowTarget.enabled)
            {
              GameManager.GetInstance().CamFollowTarget.CurrentPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
              GameManager.GetInstance().CamFollowTarget.enabled = true;
              GameManager.GetInstance().AddToCamera(placementRegion.placementObject.gameObject);
            }
            if ((double) Mathf.Abs(vector2.x) >= 0.30000001192092896 || (double) Mathf.Abs(vector2.y) >= 0.30000001192092896 || placementRegion.mouseActive)
            {
              Vector3 direction = PlacementRegion.\u003CPlaceObject_o\u003Eg__GetDirection\u007C139_0((Vector3) vector2.normalized);
              bool flag1 = false;
              if ((double) vector2.magnitude <= 0.0 && placementRegion.mouseActive)
              {
                Vector3 mousePositionWorld = placementRegion.GetMousePositionWorld();
                PlacementTile tileAtWorldPosition = placementRegion.GetClosestTileAtWorldPosition(placementRegion.placementObject.transform.position);
                if ((double) Vector3.Distance(placementRegion.transform.TransformPoint(tileAtWorldPosition.Position), mousePositionWorld) > (double) Mathf.Max((float) placementRegion.placementObject.Bounds.x / 2f, 1.5f))
                {
                  direction = PlacementRegion.\u003CPlaceObject_o\u003Eg__GetDirection\u007C139_0((mousePositionWorld - placementRegion.placementObject.transform.position).normalized);
                }
                else
                {
                  direction = (Vector3) Vector3Int.zero;
                  flag1 = true;
                }
              }
              bool flag2 = false;
              if (placementRegion.mouseActive)
              {
                Vector3 mousePositionWorld = placementRegion.GetMousePositionWorld();
                double num9 = (double) Mathf.Abs(TownCentre.Instance.transform.position.x - mousePositionWorld.x);
                float num10 = Mathf.Abs(TownCentre.Instance.transform.position.y - (mousePositionWorld.y + (float) placementRegion.placementObject.Bounds.y / 2f));
                flag2 = num9 < 14.0 && (double) num10 < 8.1000003814697266;
              }
              else if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null)
              {
                double num11 = (double) Mathf.Abs(TownCentre.Instance.transform.position.x - placementRegion.CurrentTile.transform.position.x);
                float num12 = Mathf.Abs(TownCentre.Instance.transform.position.y - (placementRegion.CurrentTile.transform.position.y + (float) placementRegion.placementObject.Bounds.y / 2f));
                flag2 = num11 < 14.0 && (double) num12 < 8.1000003814697266;
              }
              if (((UnityEngine.Object) placementRegion.CurrentTile == (UnityEngine.Object) null || !placementRegion.IsValidPlacement(placementRegion.CurrentTile.Position, false, true)) && (UnityEngine.Object) placementRegion.PreviousTile != (UnityEngine.Object) null && placementRegion.mouseActive)
                placementRegion.CurrentTile = placementRegion.PreviousTile;
              if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && !(direction == (Vector3) Vector3Int.zero) && !placementRegion.\u003CPlaceObject_o\u003Eg__SetTile\u007C139_1(placementRegion.CurrentTile.Position, direction) && !placementRegion.\u003CPlaceObject_o\u003Eg__SetTile\u007C139_1(placementRegion.CurrentTile.Position, direction + Vector3.left) && !placementRegion.\u003CPlaceObject_o\u003Eg__SetTile\u007C139_1(placementRegion.CurrentTile.Position, direction + Vector3.right) && !placementRegion.\u003CPlaceObject_o\u003Eg__SetTile\u007C139_1(placementRegion.CurrentTile.Position, direction + Vector3.up) && !placementRegion.\u003CPlaceObject_o\u003Eg__SetTile\u007C139_1(placementRegion.CurrentTile.Position, direction + Vector3.down) && ((double) vector2.y <= 0.0 || !placementRegion.\u003CPlaceObject_o\u003Eg__SetTile\u007C139_1(placementRegion.CurrentTile.Position, (Vector3) PlacementRegion.Up)) && (double) vector2.y < 0.0)
                placementRegion.\u003CPlaceObject_o\u003Eg__SetTile\u007C139_1(placementRegion.CurrentTile.Position, (Vector3) PlacementRegion.Down);
              if (placementRegion.shakeTween != null)
              {
                placementRegion.shakeTween.Kill();
                placementRegion.shakeTween = (Tween) null;
              }
              if ((double) vector2.magnitude <= 0.0 && placementRegion.mouseActive)
              {
                Vector3 mousePositionWorld = placementRegion.GetMousePositionWorld();
                Vector3 vector3 = placementRegion.transform.InverseTransformPoint(mousePositionWorld);
                bool flag3 = placementRegion.IsValidPlacement(new Vector3((float) Mathf.RoundToInt(vector3.x), (float) Mathf.RoundToInt(vector3.y), 0.0f), false, true);
                if (flag3 | flag2 | flag1)
                {
                  if (placementRegion.moveTween != null)
                  {
                    placementRegion.moveTween.Kill();
                    placementRegion.moveTween = (Tween) null;
                  }
                  placementRegion.placementObject.transform.position = mousePositionWorld;
                  if (flag3)
                    placementRegion.PreviousTile = placementRegion.CurrentTile;
                }
                else if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && (placementRegion.moveTween == null || !placementRegion.moveTween.active))
                  placementRegion.moveTween = (Tween) placementRegion.placementObject.transform.DOMove(placementRegion.CurrentTile.transform.position, placementRegion.TopSpeed * 2f).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late, true);
                placementRegion.CurrentTile = placementRegion.GetClosestTileAtWorldPosition(mousePositionWorld);
                Vector3 viewportPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().ScreenToViewportPoint((Vector3) InputManager.General.GetMousePosition(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
                viewportPoint.x -= 0.5f;
                viewportPoint.y -= 0.5f;
                bool flag4 = (double) Vector3.Distance(placementRegion.CurrentTile.transform.position, placementRegion.GetMousePositionWorld()) < 5.0;
                if ((double) viewportPoint.y > 0.30000001192092896 && (double) GameManager.GetInstance().CamFollowTarget.transform.position.y < -5.0)
                  flag4 = true;
                if (flag4 | flag2 && ((double) Mathf.Abs(viewportPoint.x) > 0.30000001192092896 || (double) Mathf.Abs(viewportPoint.y) > 0.30000001192092896))
                {
                  placementRegion.cachedDirection = viewportPoint;
                  Speed = Mathf.Clamp(Speed + 1f, 0.0f, 20f);
                }
                else
                  Speed = Mathf.Clamp(Speed - 2f, 0.0f, 20f);
                if ((double) Speed != 0.0)
                  GameManager.GetInstance().CamFollowTarget.transform.position += Time.unscaledDeltaTime * placementRegion.cachedDirection * Speed;
              }
              if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && !flag2 && !placementRegion.mouseActive)
              {
                if (placementRegion.moveTween == null || !placementRegion.moveTween.active)
                  placementRegion.moveTween = (Tween) placementRegion.placementObject.transform.DOMove(placementRegion.CurrentTile.transform.position, placementRegion.TopSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late, true);
              }
              else
                placementRegion.placementObject.transform.position += placementRegion.TopSpeed * Time.unscaledDeltaTime * Vector3.Normalize((Vector3) vector2);
              placementRegion.PlacementPosition = placementRegion.placementObject.transform.position;
              if (!Moving)
              {
                placementRegion.placementObject.SetScale(new Vector3(1.1f, 0.9f, 0.9f));
                Moving = true;
              }
            }
            else
            {
              if (Moving)
              {
                placementRegion.placementObject.SetScale(new Vector3(1.1f, 0.9f, 0.9f));
                Moving = false;
              }
              if ((UnityEngine.Object) placementRegion.placementObject != (UnityEngine.Object) null && (UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null)
              {
                placementRegion.moveTween.Kill();
                placementRegion.placementObject.transform.position = Vector3.Lerp(placementRegion.placementObject.transform.position, placementRegion.CurrentTile.transform.position, Time.unscaledDeltaTime * placementRegion.LerpToTileSpeed);
              }
            }
            if (InputManager.Gameplay.GetPlaceMoveUpgradeButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || placementRegion.isPath && InputManager.Gameplay.GetPlaceMoveUpgradeButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
            {
              if (placementRegion.CurrentMode == PlacementRegion.Mode.Demolishing)
              {
                Structure hoveredStructure = placementRegion.GetHoveredStructure();
                if ((bool) (UnityEngine.Object) hoveredStructure && hoveredStructure.Structure_Info.CanBeMoved)
                {
                  placementRegion.ClearPrefabs();
                  placementRegion.StopAllCoroutines();
                  placementRegion.PlayMove(hoveredStructure);
                  yield break;
                }
              }
              else
              {
                bool AllowObstructions = placementRegion.CurrentMode == PlacementRegion.Mode.Building || placementRegion.CurrentMode == PlacementRegion.Mode.MultiBuild || placementRegion.CurrentMode == PlacementRegion.Mode.Moving;
                bool flag = true;
                if (placementRegion.isPath && (UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.GetTileGridTile(placementRegion.CurrentTile.GridPosition) != null)
                {
                  StructureBrain.TYPES tileTypeAtPosition = PathTileManager.Instance.GetTileTypeAtPosition(placementRegion.PlacementPosition);
                  flag = tileTypeAtPosition == StructureBrain.TYPES.NONE || tileTypeAtPosition != placementRegion.StructureType;
                }
                if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.structureIsOnlyPlaceableInRanch && !placementRegion.ranchingGridPositions.Contains(placementRegion.CurrentTile.GridPosition))
                  flag = false;
                if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.StructureType == StructureBrain.TYPES.RANCH_FENCE && !placementRegion.fenceableGridPositions.Contains(placementRegion.CurrentTile.GridPosition))
                  flag = false;
                if (!placementRegion.structureIsOnlyPlaceableInRanch && placementRegion.ranchingGridPositions.Contains(placementRegion.CurrentTile.GridPosition))
                  flag = false;
                if (flag && (UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.IsValidPlacement(placementRegion.CurrentTile.Position, !placementRegion.isPath, AllowObstructions))
                {
                  AudioManager.Instance.PlayOneShot("event:/building/place_building_spot", placementRegion.placementObject.transform.position);
                  Loop = false;
                }
                else if (!placementRegion.isPath)
                {
                  Debug.Log((object) "Cant build here");
                  placementRegion.placementObject.gameObject.transform.DOKill();
                  PlayerFarming.Instance.indicator.gameObject.transform.DOKill();
                  placementRegion.shakeTween = (Tween) placementRegion.placementObject.gameObject.transform.DOShakePosition(0.3f, new Vector3(0.5f, 0.0f), 20).SetUpdate<Tweener>(true);
                  PlayerFarming.Instance.indicator.gameObject.transform.DOShakePosition(0.3f, new Vector3(0.5f, 0.0f), 20).SetUpdate<Tweener>(true);
                  AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", placementRegion.placementObject.transform.position);
                }
              }
            }
          }
          if ((double) InputManager.Gameplay.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) > -0.30000001192092896 && (double) InputManager.Gameplay.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) < 0.30000001192092896 && (double) InputManager.Gameplay.GetVerticalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) > -0.30000001192092896 && (double) InputManager.Gameplay.GetVerticalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) < 0.30000001192092896)
            placementRegion.InputDelay = 0.0f;
          if (placementRegion.isPath || placementRegion.GetPathAtPosition() != StructureBrain.TYPES.NONE && ((UnityEngine.Object) placementRegion.placementObject == (UnityEngine.Object) null || placementRegion.isEditingBuildings || placementRegion.isPath) && (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade == (UnityEngine.Object) null && (UnityEngine.Object) placementRegion.CurrentStructureToMove == (UnityEngine.Object) null)
          {
            if (InputManager.Gameplay.GetInteract3ButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
              PathTileManager.Instance.DeleteTile(placementRegion.PlacementPosition);
          }
          else if (InputManager.Gameplay.GetRemoveFlipButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          {
            if ((bool) (UnityEngine.Object) placementRegion.placementObject && placementRegion.StructureType != StructureBrain.TYPES.EDIT_BUILDINGS && StructuresData.CanBeFlipped(placementRegion.StructureType))
            {
              if (placementRegion.StructureType != StructureBrain.TYPES.FARM_CROP_GROWER)
                placementRegion.placementObject.transform.localScale = new Vector3(placementRegion.placementObject.transform.localScale.x * -1f, placementRegion.placementObject.transform.localScale.y, placementRegion.placementObject.transform.localScale.z);
              placementRegion.Rotation = (int) Mathf.Repeat((float) (placementRegion.Rotation + 1), 4f);
              placementRegion.direction = (int) placementRegion.placementObject.transform.localScale.x;
              AudioManager.Instance.PlayOneShot(placementRegion.flipSFX);
            }
            else if ((UnityEngine.Object) placementRegion.CurrentStructureToMove != (UnityEngine.Object) null && StructuresData.CanBeFlipped(placementRegion.CurrentStructureToMove.Type))
            {
              placementRegion.CurrentStructureToMove.Structure_Info.Direction *= -1;
              placementRegion.CurrentStructureToMove.transform.localScale = new Vector3((float) placementRegion.CurrentStructureToMove.Structure_Info.Direction, placementRegion.CurrentStructureToMove.transform.localScale.y, placementRegion.CurrentStructureToMove.transform.localScale.z);
              placementRegion.CurrentStructureToMove.Structure_Info.Rotation = (int) Mathf.Repeat((float) (placementRegion.CurrentStructureToMove.Structure_Info.Rotation + 1), 4f);
              placementRegion.direction = placementRegion.CurrentStructureToMove.Structure_Info.Direction;
              AudioManager.Instance.PlayOneShot(placementRegion.flipSFX);
            }
            else if ((UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null && StructuresData.CanBeFlipped(placementRegion.CurrentStructureToUpgrade.Type))
            {
              placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction *= -1;
              placementRegion.CurrentStructureToUpgrade.transform.localScale = new Vector3((float) placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction, placementRegion.CurrentStructureToUpgrade.transform.localScale.y, placementRegion.CurrentStructureToUpgrade.transform.localScale.z);
              placementRegion.CurrentStructureToMove.Structure_Info.Rotation = (int) Mathf.Repeat((float) (placementRegion.CurrentStructureToMove.Structure_Info.Rotation + 1), 4f);
              placementRegion.direction = placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction;
              AudioManager.Instance.PlayOneShot(placementRegion.flipSFX);
            }
          }
          if (InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          {
            if (placementRegion.CurrentMode == PlacementRegion.Mode.Moving)
            {
              placementRegion.CurrentStructureToMove.Brain.AddToGrid();
              PlacementRegion.BuildingEvent onBuildingPlaced = PlacementRegion.OnBuildingPlaced;
              if (onBuildingPlaced != null)
                onBuildingPlaced(placementRegion.CurrentStructureToMove.Structure_Info.ID);
              foreach (SpriteRenderer componentsInChild in placementRegion.CurrentStructureToMove.gameObject.GetComponentsInChildren<SpriteRenderer>())
              {
                if (componentsInChild.gameObject.activeSelf && !componentsInChild.CompareTag("IgnoreBuildRendering"))
                  componentsInChild.color = new Color(1f, 1f, 1f, 1f);
              }
              if (placementRegion.isEditingBuildings)
              {
                placementRegion.previousEditingPosition = placementRegion.CurrentStructureToMove.transform.position;
                placementRegion.ClearPrefabs();
                placementRegion.StopAllCoroutines();
                placementRegion.CurrentMode = PlacementRegion.Mode.Demolishing;
                placementRegion.PlacementGameObject = TypeAndPlacementObjects.GetByType(StructureBrain.TYPES.EDIT_BUILDINGS).PlacementObject;
                placementRegion.StructureType = StructureBrain.TYPES.EDIT_BUILDINGS;
                placementRegion.structureIsOnlyPlaceableInRanch = placementRegion.OnlyPlaceableInRanch;
                placementRegion.Play(placementRegion.previousEditingPosition);
                yield break;
              }
            }
            else
              HUD_Manager.Instance.Show();
            DLCLandController.Instance.ShowBridge();
            Time.timeScale = 1f;
            placementRegion._PlacementPosition = Vector3.zero;
            placementRegion.previousEditingPosition = Vector3.zero;
            placementRegion.ClearPrefabs();
            placementRegion.isEditingBuildings = false;
            placementRegion._PlacementPosition = Vector3.zero;
            placementRegion.previousEditingPosition = Vector3.zero;
            PlayerFarming.Instance.indicator.HasSecondaryInteraction = false;
            PlayerFarming.Instance.indicator.SecondaryText.text = "";
            PlayerFarming.Instance.indicator.HideTopInfo();
            GameManager.GetInstance().CamFollowTarget.enabled = true;
            GameManager.GetInstance().RemoveFromCamera(placementRegion.placementObject.gameObject);
            placementRegion.tilesInitiated = false;
            yield return (object) placementRegion.ClearTilesIE();
            yield break;
          }
          if (InputManager.Gameplay.GetRemoveFlipButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || placementRegion.isPath && InputManager.Gameplay.GetRemoveFlipButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          {
            if (placementRegion.isPath)
            {
              PathTileManager.Instance.DeleteTile(placementRegion.PlacementPosition);
              if ((bool) (UnityEngine.Object) placementRegion.placementUI && placementRegion.CurrentMode == PlacementRegion.Mode.MultiBuild)
                placementRegion.placementUI.UpdateText(placementRegion.StructureType);
            }
            else if (placementRegion.CurrentMode == PlacementRegion.Mode.Demolishing && (UnityEngine.Object) placementRegion.GetHoveredStructure() != (UnityEngine.Object) null && placementRegion.GetHoveredStructure().Brain.Data.IsDeletable)
            {
              Loop = false;
              Structure hoveredStructure = placementRegion.GetHoveredStructure();
              if ((UnityEngine.Object) hoveredStructure != (UnityEngine.Object) null)
                placementRegion.DestroyBuilding(hoveredStructure);
            }
            else if (placementRegion.GetPathAtPosition() != StructureBrain.TYPES.NONE && placementRegion.CurrentMode != PlacementRegion.Mode.Building && placementRegion.CurrentMode != PlacementRegion.Mode.MultiBuild && placementRegion.CurrentMode != PlacementRegion.Mode.Moving)
              PathTileManager.Instance.DeleteTile(placementRegion.PlacementPosition);
          }
          yield return (object) null;
        }
        switch (placementRegion.CurrentMode)
        {
          case PlacementRegion.Mode.Building:
          case PlacementRegion.Mode.MultiBuild:
            bool ManageCamera = placementRegion.CurrentMode == PlacementRegion.Mode.Building;
            placementRegion.Build(placementRegion.StructureType, placementRegion.CurrentTile.transform.position, placementRegion.CurrentTile.GridPosition, placementRegion.placementObject.Bounds, false, ManageCamera);
            break;
          case PlacementRegion.Mode.Upgrading:
            placementRegion.Upgrade();
            break;
          case PlacementRegion.Mode.Moving:
            placementRegion.previousEditingPosition = placementRegion.CurrentTile.transform.position;
            PlacementRegion.BuildingEvent onBuildingPlaced1 = PlacementRegion.OnBuildingPlaced;
            if (onBuildingPlaced1 != null)
              onBuildingPlaced1(placementRegion.CurrentStructureToMove.Structure_Info.ID);
            placementRegion.MoveBuilding(placementRegion.CurrentTile.transform.position, placementRegion.CurrentTile.GridPosition);
            placementRegion.CurrentMode = PlacementRegion.Mode.Demolishing;
            placementRegion.PlacementGameObject = TypeAndPlacementObjects.GetByType(StructureBrain.TYPES.EDIT_BUILDINGS).PlacementObject;
            placementRegion.StructureType = StructureBrain.TYPES.EDIT_BUILDINGS;
            placementRegion.structureIsOnlyPlaceableInRanch = placementRegion.OnlyPlaceableInRanch;
            placementRegion.ClearPrefabs();
            placementRegion.StopAllCoroutines();
            placementRegion.Play(placementRegion.previousEditingPosition);
            break;
        }
        if (placementRegion.CurrentMode != PlacementRegion.Mode.MultiBuild && placementRegion.CurrentMode != PlacementRegion.Mode.Demolishing && (placementRegion.CurrentMode != PlacementRegion.Mode.Upgrading || StructuresData.GetBuildOnlyOne(placementRegion.StructureType)) || !StructuresData.CanAfford(placementRegion.StructureType))
          goto label_152;
      }
      while (placementRegion.CurrentMode != PlacementRegion.Mode.Upgrading);
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().AddToCamera(placementRegion.placementObject.gameObject);
    }
label_152:
    yield return (object) new WaitForSecondsRealtime(0.15f);
  }

  public Vector3 GetMousePositionWorld()
  {
    Ray ray = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().ScreenPointToRay((Vector3) InputManager.General.GetMousePosition(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
    float enter;
    this.plane.Raycast(ray, out enter);
    return ray.GetPoint(enter);
  }

  public void DestroyBuilding(Structure structure)
  {
    this.CurrentStructureToUpgrade = structure;
    this.DestroyBuilding();
    this.CurrentStructureToUpgrade = (Structure) null;
    BiomeConstants.Instance.EmitDustCloudParticles(structure.Brain.Data.Position, 5, ignoreTimescale: true);
  }

  public void DestroyBuilding()
  {
    for (int index = 0; index < this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs.Count; ++index)
      DataManager.Instance.Followers_Transitioning_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if ((this.CurrentStructureToUpgrade.Brain.Data.FollowerID == allBrain.Info.ID || this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs.Contains(allBrain.Info.ID)) && this.AbortTaskOnDestroyBuilding(this.CurrentStructureToUpgrade, allBrain.Info.ID))
        allBrain.CurrentTask?.Abort();
    }
    this.structureBrain.ClearStructureFromGrid(this.CurrentStructureToUpgrade.Brain);
    CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
    Structure component = this.CurrentStructureToUpgrade.GetComponent<Structure>();
    AudioManager.Instance.PlayOneShot("event:/building/finished_stone", this.CurrentStructureToUpgrade.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    component.RemoveStructure();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentStructureToUpgrade.gameObject);
    GameManager.RecalculatePaths();
  }

  public bool AbortTaskOnDestroyBuilding(Structure structureToDestroy, int followerID)
  {
    return (structureToDestroy.Type == StructureBrain.TYPES.PRISON || !DataManager.Instance.Followers_Imprisoned_IDs.Contains(followerID)) && (structureToDestroy.Type == StructureBrain.TYPES.MISSIONARY || structureToDestroy.Type == StructureBrain.TYPES.MISSIONARY_II || structureToDestroy.Type == StructureBrain.TYPES.MISSIONARY_III || !DataManager.Instance.Followers_OnMissionary_IDs.Contains(followerID));
  }

  public Structure GetHoveredStructure()
  {
    PlacementTile tileAtWorldPosition = this.GetClosestTileAtWorldPosition(this.PlacementPosition);
    if ((UnityEngine.Object) tileAtWorldPosition == (UnityEngine.Object) null)
      return (Structure) null;
    PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile((float) tileAtWorldPosition.GridPosition.x, (float) tileAtWorldPosition.GridPosition.y);
    Structure hoveredStructure = (Structure) null;
    foreach (Structure structure in Structure.Structures)
    {
      if (structure.Type != StructureBrain.TYPES.EDIT_BUILDINGS && !this.IsNaturalObstruction(structure.Type) && structure.Type != StructureBrain.TYPES.BUILD_SITE && structure.Type != StructureBrain.TYPES.BUILD_PLOT && structure.Type == tileGridTile.ObjectOnTile)
      {
        if (structure.Brain != null && tileGridTile != null && structure.Brain.Data != null && structure.Brain.Data.ID == tileGridTile.ObjectID)
        {
          hoveredStructure = structure;
          break;
        }
        if ((UnityEngine.Object) hoveredStructure == (UnityEngine.Object) null || (UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && structure.Brain.Data != null && tileGridTile != null && (double) Vector3.Distance(structure.Brain.Data.Position, tileGridTile.WorldPosition) < (double) Vector3.Distance(hoveredStructure.Brain.Data.Position, tileGridTile.WorldPosition))
          hoveredStructure = structure;
      }
    }
    return hoveredStructure;
  }

  public StructureBrain.TYPES GetPathAtPosition()
  {
    return PathTileManager.Instance.GetTileTypeAtPosition(this.PlacementPosition);
  }

  public Transform GetWeedAtPosition(Vector3 worldPosition)
  {
    Transform transform = (Transform) null;
    foreach (WeedManager weedManager in WeedManager.WeedManagers)
    {
      if ((UnityEngine.Object) transform == (UnityEngine.Object) null || (double) Vector3.Distance(weedManager.transform.position, worldPosition) < (double) Vector3.Distance(transform.position, worldPosition))
        transform = weedManager.transform;
    }
    return (UnityEngine.Object) transform != (UnityEngine.Object) null && (double) Vector3.Distance(transform.position, worldPosition) < 1.0 ? transform : (Transform) null;
  }

  public void Upgrade() => this.DoUpgradeRoutine();

  public void MoveBuilding(Vector3 Position, Vector2Int GridPosition)
  {
    this.CurrentStructureToMove.gameObject.SetActive(true);
    this.CurrentStructureToMove.Brain.Data.Direction = this.direction;
    this.CurrentStructureToMove.Brain.Data.Rotation = this.Rotation;
    this.CurrentStructureToMove.transform.localScale = new Vector3((float) this.direction, this.CurrentStructureToMove.transform.localScale.y, this.CurrentStructureToMove.transform.localScale.z);
    this.CurrentStructureToMove.gameObject.transform.position = Position;
    this.CurrentStructureToMove.Brain.Data.Position = Position;
    this.CurrentStructureToMove.Brain.Data.GridTilePosition = GridPosition;
    this.CurrentStructureToMove.gameObject.SetActive(true);
    this.CurrentStructureToMove.Brain.AddToGrid();
    this.MarkObstructionsForClearing(this.CurrentStructureToMove.Brain.Data.GridTilePosition, this.CurrentStructureToMove.Brain.Data.Bounds, this.CurrentStructureToMove.Brain.Data);
    foreach (SpriteRenderer componentsInChild in this.CurrentStructureToMove.gameObject.GetComponentsInChildren<SpriteRenderer>())
    {
      if (componentsInChild.gameObject.activeSelf && !componentsInChild.CompareTag("IgnoreBuildRendering"))
        componentsInChild.color = new Color(1f, 1f, 1f, 1f);
    }
    foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
    {
      if (locationFollower.Brain.DesiredLocation == PlayerFarming.Location && this.CurrentStructureToMove.Brain.Data.FollowerID == locationFollower.Brain.Info.ID && !(locationFollower.Brain.CurrentTask is FollowerTask_ChangeLocation) && !(locationFollower.Brain.CurrentTask is FollowerTask_Imprisoned) || locationFollower.Brain.CurrentTask != null && locationFollower.Brain.CurrentTask is FollowerTask_ChangeLocation && ((FollowerTask_ChangeLocation) locationFollower.Brain.CurrentTask).TargetLocation == PlayerFarming.Location)
        locationFollower.Brain.CurrentTask?.Abort();
    }
    if (this.IsPrisonWithPrisoner())
      this.DoPrisonPlacementActions();
    else if (this.CurrentStructureToMove.Brain.Data.Type == StructureBrain.TYPES.DAYCARE)
    {
      List<Structures_Daycare> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Daycare>();
      foreach (int multipleFollowerId in this.CurrentStructureToMove.Brain.Data.MultipleFollowerIDs)
      {
        foreach (Structures_Daycare structuresDaycare in structuresOfType)
        {
          if (structuresDaycare.Data.MultipleFollowerIDs.Contains(multipleFollowerId))
          {
            foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
            {
              if (structuresDaycare != null && daycare.Structure.Brain != null && daycare.Structure.Brain.Data.ID == structuresDaycare.Data.ID)
              {
                Follower followerById = FollowerManager.FindFollowerByID(multipleFollowerId);
                followerById.transform.position = followerById.Brain.LastPosition = daycare.MiddlePosition + (Vector3) UnityEngine.Random.insideUnitCircle * ((Structures_Daycare) daycare.Structure.Brain).BoundariesRadius;
              }
            }
          }
        }
      }
    }
    StructureManager.StructureChanged onStructureMoved = StructureManager.OnStructureMoved;
    if (onStructureMoved != null)
      onStructureMoved(this.CurrentStructureToMove.Brain.Data);
    this.CurrentStructureToMove.Brain.Data.Rotation = this.Rotation;
    this.CurrentStructureToMove = (Structure) null;
  }

  public void MarkObstructionsForClearing(
    Vector2Int GridPosition,
    Vector2Int Bounds,
    StructuresData data)
  {
    int num1 = -2;
    while (++num1 < Bounds.x + 1)
    {
      int num2 = -2;
      while (++num2 < Bounds.y + 1)
      {
        PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(new Vector2Int(GridPosition.x + num1, GridPosition.y + num2));
        if (tileGridTile != null && (UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
        {
          Transform weedAtPosition = PlacementRegion.Instance.GetWeedAtPosition(tileGridTile.WorldPosition);
          if ((UnityEngine.Object) weedAtPosition != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) weedAtPosition.gameObject);
        }
      }
    }
  }

  public StructuresData GetObstructionAtPosition(Vector2Int Position, StructuresData data)
  {
    StructuresData obstructionAtPosition = (StructuresData) null;
    foreach (StructuresData structuresData in StructureManager.StructuresDataAtLocation(data.Location))
    {
      if (Position.x >= structuresData.GridTilePosition.x && Position.x < structuresData.GridTilePosition.x + structuresData.Bounds.x && Position.y >= structuresData.GridTilePosition.y && Position.y < structuresData.GridTilePosition.y + structuresData.Bounds.y && structuresData.IsObstruction)
        obstructionAtPosition = structuresData;
    }
    return obstructionAtPosition;
  }

  public void DoUpgradeRoutine()
  {
    StructureBrain.TYPES type = this.CurrentStructureToUpgrade.Type;
    for (int index = 0; index < this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs.Count; ++index)
      DataManager.Instance.Followers_Transitioning_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if ((this.CurrentStructureToUpgrade.Brain.Data.FollowerID == allBrain.Info.ID || this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs.Contains(allBrain.Info.ID)) && this.AbortTaskOnDestroyBuilding(this.CurrentStructureToUpgrade, allBrain.Info.ID))
        allBrain.CurrentTask?.Abort();
    }
    FollowerLocation location = this.CurrentStructureToUpgrade.Structure_Info.Location;
    LocationManager locationManager = LocationManager.LocationManagers[location];
    int index1 = -1;
    while (++index1 < StructuresData.GetCost(this.StructureType).Count)
    {
      Debug.Log((object) $"{StructuresData.GetCost(this.StructureType)[index1].CostItem.ToString()}  {StructuresData.GetCost(this.StructureType)[index1].CostValue.ToString()}");
      Inventory.ChangeItemQuantity((int) StructuresData.GetCost(this.StructureType)[index1].CostItem, -StructuresData.GetCost(this.StructureType)[index1].CostValue);
    }
    StructuresData infoByType = StructuresData.GetInfoByType(this.StructureType, 0);
    GameObject gameObject;
    Structure component1;
    if (infoByType.IgnoreGrid)
    {
      gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BuildSiteBuildingProjectPrefab, this.CurrentStructureToUpgrade.transform.position, Quaternion.identity, locationManager.StructureLayer);
      component1 = gameObject.GetComponent<Structure>();
      component1.CreateStructure(this.StructureInfo.Location, gameObject.transform.position, this.placementObject.Bounds, this.StructureType);
      BuildSitePlotProject component2 = gameObject.GetComponent<BuildSitePlotProject>();
      component2.StructureInfo.Direction = this.direction;
      component2.StructureInfo.Rotation = this.Rotation;
      component2.StructureInfo.Inventory = this.CurrentStructureToUpgrade.Inventory;
      component2.StructureInfo.QueuedResources = this.CurrentStructureToUpgrade.Structure_Info.QueuedResources;
      component2.StructureInfo.QueuedRefineryVariants = this.CurrentStructureToUpgrade.Structure_Info.QueuedRefineryVariants;
      component2.StructureInfo.ToBuildType = this.StructureType;
      component2.StructureInfo.FollowerID = infoByType.FollowerID;
      component2.StructureInfo.Bounds = this.placementObject.Bounds;
      component2.StructureInfo.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
      component2.Bounds = this.placementObject.Bounds;
      component2.StructureInfo.FollowerID = this.CurrentStructureToUpgrade.Structure_Info.FollowerID;
      component2.StructureInfo.MultipleFollowerIDs = this.CurrentStructureToUpgrade.Structure_Info.MultipleFollowerIDs;
      component2.StructureInfo.ClaimedByPlayer = this.CurrentStructureToUpgrade.Structure_Info.ClaimedByPlayer;
      component2.StructureInfo.Inventory = this.CurrentStructureToUpgrade.Structure_Info.Inventory;
      if (infoByType.IsUpgradeDestroyPrevious)
      {
        Debug.Log((object) "REMOVE!");
        this.CurrentStructureToUpgrade.RemoveStructure();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentStructureToUpgrade.gameObject);
      }
    }
    else
    {
      gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BuildSitePrefab, this.CurrentStructureToUpgrade.transform.position, Quaternion.identity, locationManager.StructureLayer);
      component1 = gameObject.GetComponent<Structure>();
      component1.CreateStructure(this.StructureInfo.Location, gameObject.transform.position, this.placementObject.Bounds, this.StructureType);
      BuildSitePlot component3 = gameObject.GetComponent<BuildSitePlot>();
      component3.StructureInfo.Direction = this.direction;
      component3.StructureInfo.Rotation = this.Rotation;
      component3.StructureInfo.Inventory = this.CurrentStructureToUpgrade.Inventory;
      component3.StructureInfo.QueuedResources = this.CurrentStructureToUpgrade.Structure_Info.QueuedResources;
      component3.StructureInfo.QueuedRefineryVariants = this.CurrentStructureToUpgrade.Structure_Info.QueuedRefineryVariants;
      component3.StructureInfo.ToBuildType = this.StructureType;
      component3.StructureInfo.Bounds = this.placementObject.Bounds;
      component3.StructureInfo.FollowerID = infoByType.FollowerID;
      component3.StructureInfo.GridTilePosition = this.CurrentStructureToUpgrade.Brain.Data.GridTilePosition;
      component3.StructureInfo.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
      component3.Bounds = this.placementObject.Bounds;
      component3.StructureInfo.FollowerID = this.CurrentStructureToUpgrade.Structure_Info.FollowerID;
      component3.StructureInfo.MultipleFollowerIDs = this.CurrentStructureToUpgrade.Structure_Info.MultipleFollowerIDs;
      component3.StructureInfo.ClaimedByPlayer = this.CurrentStructureToUpgrade.Structure_Info.ClaimedByPlayer;
      component3.StructureInfo.Inventory = this.CurrentStructureToUpgrade.Structure_Info.Inventory;
      if (this.CurrentStructureToUpgrade.Structure_Info.IsUpgradeDestroyPrevious)
      {
        this.CurrentStructureToUpgrade.RemoveStructure();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentStructureToUpgrade.gameObject);
      }
      this.MarkObstructionsForClearing(component3.StructureInfo.GridTilePosition, component3.StructureInfo.Bounds, component3.StructureBrain.Data);
      this.structureBrain.AddStructureToGrid(component3.StructureInfo, true);
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().AddToCamera(gameObject);
      GameManager.RecalculatePaths();
      StructureManager.StructureChanged structureUpgraded = StructureManager.OnStructureUpgraded;
      if (structureUpgraded != null)
        structureUpgraded(component1.Brain.Data);
    }
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(gameObject);
    GameManager.RecalculatePaths();
    if ((bool) (UnityEngine.Object) this.placementUI)
      this.placementUI.UpdateText(this.StructureType);
    StructureManager.StructureChanged structureUpgraded1 = StructureManager.OnStructureUpgraded;
    if (structureUpgraded1 != null)
      structureUpgraded1(component1.Brain.Data);
    if (!DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Materialistic))
      return;
    if (type == StructureBrain.TYPES.BED)
    {
      CultFaithManager.AddThought(Thought.Cult_Materialistic_Trait_Hut);
    }
    else
    {
      if (type != StructureBrain.TYPES.BED_2)
        return;
      CultFaithManager.AddThought(Thought.Cult_Materialistic_Trait_House);
    }
  }

  public void Build(
    StructureBrain.TYPES StructureType,
    Vector3 Position,
    Vector2Int GridPosition,
    Vector2Int Bounds,
    bool Free,
    bool ManageCamera)
  {
    if (!Free && !StructuresData.CanAfford(StructureType))
      return;
    if (!Free)
    {
      int index = -1;
      while (++index < StructuresData.GetCost(StructureType).Count)
        Inventory.ChangeItemQuantity((int) StructuresData.GetCost(StructureType)[index].CostItem, -StructuresData.GetCost(StructureType)[index].CostValue);
    }
    if ((bool) (UnityEngine.Object) this.placementUI && this.CurrentMode == PlacementRegion.Mode.MultiBuild)
      this.placementUI.UpdateText(StructureType);
    LocationManager locationManager = LocationManager.LocationManagers[this.StructureInfo.Location];
    if (this.isPath)
    {
      PathTileManager.Instance.DeleteTile(Position);
      if ((bool) (UnityEngine.Object) this.placementUI && this.CurrentMode == PlacementRegion.Mode.MultiBuild)
        this.placementUI.UpdateText(StructureType);
      PathTileManager.Instance.DisplayTile(StructureType, Position);
      if (!this.placingPathsPositions.Contains(Position))
        this.placingPathsPositions.Add(Position);
      if ((UnityEngine.Object) this.CurrentTile != (UnityEngine.Object) null)
        this.MarkObstructionsForClearing(this.CurrentTile.GridPosition, Vector2Int.one, (StructuresData) null);
      PathTileManager.Instance.SetTile(StructureType, Position);
    }
    else if (StructuresData.CreateBuildSite(StructureType))
    {
      this._StructureBrain.MarkObstructionsForClearing(GridPosition, Bounds);
      GameObject gameObject;
      Structure component1;
      if (!StructuresData.GetInfoByType(StructureType, 0).IsBuildingProject)
      {
        gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BuildSitePrefab, Position, Quaternion.identity, locationManager.StructureLayer);
        component1 = gameObject.GetComponent<Structure>();
        component1.CreateStructure(this.StructureInfo.Location, gameObject.transform.position, Bounds, StructureType);
        BuildSitePlot component2 = gameObject.GetComponent<BuildSitePlot>();
        component2.StructureInfo.Direction = this.direction;
        component2.StructureInfo.Rotation = this.Rotation;
        component2.StructureInfo.ToBuildType = StructureType;
        component2.StructureInfo.Bounds = Bounds;
        component2.StructureInfo.GridTilePosition = GridPosition;
        component2.StructureInfo.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
        component2.Bounds = Bounds;
        this.MarkObstructionsForClearing(component2.StructureInfo.GridTilePosition, component2.StructureInfo.Bounds, component2.StructureBrain.Data);
        if (this.isPath)
          component2.StructureBrain.Build();
      }
      else
      {
        gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BuildSiteBuildingProjectPrefab, Position, Quaternion.identity, locationManager.StructureLayer);
        component1 = gameObject.GetComponent<Structure>();
        component1.CreateStructure(this.StructureInfo.Location, gameObject.transform.position, Bounds, StructureType);
        BuildSitePlotProject component3 = gameObject.GetComponent<BuildSitePlotProject>();
        component3.StructureInfo.Direction = this.direction;
        component3.StructureInfo.Rotation = this.Rotation;
        component3.StructureInfo.ToBuildType = StructureType;
        component3.StructureInfo.Bounds = Bounds;
        component3.StructureInfo.GridTilePosition = GridPosition;
        component3.StructureInfo.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
        component3.Bounds = Bounds;
        component3.StructureBrain.MarkObstructionsForClearing(component3.StructureInfo.GridTilePosition, component3.StructureInfo.Bounds, true);
      }
      this.structureBrain.AddStructureToGrid(component1.Structure_Info);
      if (ManageCamera)
      {
        GameManager.GetInstance().RemoveAllFromCamera();
        GameManager.GetInstance().AddToCamera(gameObject);
      }
      GameManager.RecalculatePaths();
      PlacementRegion.NewBuilding onNewBuilding = PlacementRegion.OnNewBuilding;
      if (onNewBuilding != null)
        onNewBuilding(StructureType);
      this.UpdateFenceablePositions();
    }
    else
    {
      StructuresData infoByType = StructuresData.GetInfoByType(StructureType, 0);
      infoByType.Direction = this.direction;
      infoByType.Rotation = this.Rotation;
      infoByType.GridTilePosition = GridPosition;
      infoByType.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
      StructureManager.BuildStructure(FollowerLocation.Base, infoByType, Position, Bounds, callback: (Action<GameObject>) (r => this.UpdateFenceablePositions()));
      GameManager.RecalculatePaths();
      PlacementRegion.NewBuilding onNewBuilding = PlacementRegion.OnNewBuilding;
      if (onNewBuilding == null)
        return;
      onNewBuilding(StructureType);
    }
  }

  public void ClearPrefabs()
  {
    if ((UnityEngine.Object) this.placementObject != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.placementObject.gameObject);
    if ((UnityEngine.Object) this.CurrentStructureToMove != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.placementObject.gameObject);
    if ((bool) (UnityEngine.Object) this.placementUI)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.placementUI.gameObject);
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      if (this.isEditingBuildings)
        return;
      WeedManager.ShowAll();
    }));
    GameManager.overridePlayerPosition = false;
    PlayerFarming.Instance.interactor.CurrentInteraction = (Interaction) null;
    PlayerFarming.Instance.interactor.PreviousInteraction = (Interaction) null;
    PlayerFarming.Instance.indicator.text.text = "";
    PlayerFarming.Instance.indicator.SecondaryText.text = "";
    PlayerFarming.Instance.indicator.Thirdtext.text = "";
    PlayerFarming.Instance.indicator.Fourthtext.text = "";
    PlayerFarming.Instance.indicator.HideTopInfo();
    PlayerFarming.Instance.indicator.Reset();
    if (this.isPath)
      this.placingPathsPositions.Clear();
    foreach (Structure structure in Structure.Structures)
    {
      SpriteRenderer[] componentsInChildren = structure.gameObject.GetComponentsInChildren<SpriteRenderer>(true);
      if (componentsInChildren.Length != 0)
      {
        foreach (SpriteRenderer spriteRenderer in componentsInChildren)
        {
          if (!spriteRenderer.gameObject.CompareTag("BuildingEffectRadius") && !spriteRenderer.gameObject.CompareTag("IgnoreBuildRendering"))
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
      }
      foreach (MeshRenderer componentsInChild in structure.gameObject.GetComponentsInChildren<MeshRenderer>(true))
      {
        if (componentsInChild.material.HasProperty("_Color"))
          componentsInChild.material.color = new Color(componentsInChild.material.color.r, componentsInChild.material.color.g, componentsInChild.material.color.b, 1f);
      }
    }
    this.CurrentMode = PlacementRegion.Mode.None;
    if ((UnityEngine.Object) this.CurrentStructureToMove != (UnityEngine.Object) null)
      this.CurrentStructureToMove.gameObject.SetActive(true);
    this.CurrentStructureToMove = (Structure) null;
    if ((UnityEngine.Object) this.CurrentStructureToUpgrade != (UnityEngine.Object) null)
      this.CurrentStructureToUpgrade.gameObject.SetActive(true);
    this.CurrentStructureToUpgrade = (Structure) null;
  }

  public void UpdateTileAvailability()
  {
    if ((UnityEngine.Object) this.CurrentTile == (UnityEngine.Object) null)
      return;
    foreach (PlacementTile tile in this.Tiles)
    {
      PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile((float) tile.GridPosition.x, (float) tile.GridPosition.y);
      bool flag = tileGridTile.CanPlaceStructure || this.canPlaceObjectOnBuildings && !this.IsNaturalObstruction(tileGridTile.ObjectOnTile) && tileGridTile.ObjectOnTile != StructureBrain.TYPES.BUILD_SITE;
      tile.SetColor(flag || this.isPath ? Color.white : Color.red, this.placementObject.transform.position);
      if (this.structureIsOnlyPlaceableInRanch)
      {
        if (flag && this.ranchingGridPositions.Contains(tile.GridPosition))
        {
          tile.gameObject.SetActive(true);
          tile.SetColor(Color.white, this.placementObject.transform.position);
        }
        else
          tile.gameObject.SetActive(false);
      }
      else if (this.StructureType == StructureBrain.TYPES.RANCH_FENCE)
      {
        if (this.touchingRanchGridPositions.Contains(tile.GridPosition))
        {
          tile.gameObject.SetActive(true);
          tile.SetColor(flag ? StaticColors.OrangeColor : Color.red, this.placementObject.transform.position);
        }
        else if (this.fenceableGridPositions.Contains(tile.GridPosition))
        {
          tile.gameObject.SetActive(true);
          tile.SetColor(flag ? Color.white : Color.red, this.placementObject.transform.position);
        }
        else
          tile.gameObject.SetActive(false);
      }
      else if (!this.structureIsOnlyPlaceableInRanch & flag && this.ranchingGridPositions.Contains(tile.GridPosition))
        tile.SetColor(Color.red, this.placementObject.transform.position);
    }
    bool flag1 = false;
    int x1 = -1;
    while (++x1 < this.placementObject.Bounds.x)
    {
      int y = -1;
      while (++y < this.placementObject.Bounds.y)
      {
        PlacementTile tile = this.GetTile(this.CurrentTile.Position + new Vector3((float) x1, (float) y));
        if ((UnityEngine.Object) tile != (UnityEngine.Object) null)
        {
          PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(tile.GridPosition);
          if (tileGridTile == null || !tileGridTile.CanPlaceStructure)
          {
            flag1 = true;
            break;
          }
        }
        else
          flag1 = true;
      }
      if (flag1)
        break;
    }
    int x2 = -1;
    while (++x2 < this.placementObject.Bounds.x)
    {
      int y1 = -1;
      while (++y1 < this.placementObject.Bounds.y)
      {
        PlacementTile tile = this.GetTile(this.CurrentTile.Position + new Vector3((float) x2, (float) y1));
        if ((UnityEngine.Object) tile != (UnityEngine.Object) null && this.GetTileGridTile(tile.GridPosition) != null)
        {
          if (this.isEditingBuildings && (UnityEngine.Object) this.CurrentStructureToMove == (UnityEngine.Object) null)
          {
            tile.SetColor(StaticColors.OrangeColor, tile.transform.position);
            if ((UnityEngine.Object) this.GetHoveredStructure() != (UnityEngine.Object) null)
            {
              Vector2Int bounds = this.GetHoveredStructure().Brain.Data.Bounds;
              for (int x3 = 0; x3 < bounds.x; ++x3)
              {
                for (int y2 = 0; y2 < bounds.y; ++y2)
                {
                  PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(this.GetHoveredStructure().Brain.Data.GridTilePosition + new Vector2Int(x3, y2));
                  if (tileGridTile != null)
                  {
                    PlacementTile tileAtWorldPosition = this.GetClosestTileAtWorldPosition(tileGridTile.WorldPosition);
                    if ((UnityEngine.Object) tileAtWorldPosition != (UnityEngine.Object) null)
                      tileAtWorldPosition.SetColor(StaticColors.OrangeColor, tileAtWorldPosition.transform.position);
                  }
                }
              }
            }
          }
          else if (this.structureIsOnlyPlaceableInRanch)
          {
            bool flag2 = this.ranchingGridPositions.Contains(tile.GridPosition);
            tile.SetColor(flag1 && !this.isPath || !flag2 ? Color.red : Color.green, this.placementObject.transform.position);
          }
          else
            tile.SetColor(!flag1 || this.isPath ? Color.green : Color.red, this.placementObject.transform.position);
        }
      }
    }
  }

  public void OnDrawGizmos()
  {
    if (!Application.isEditor || Application.isPlaying)
      return;
    foreach (PlacementRegion.TileGridTile previewTiles in this.PreviewTilesList)
    {
      Gizmos.matrix = this.transform.localToWorldMatrix;
      Gizmos.DrawWireCube(new Vector3((float) previewTiles.Position.x, (float) previewTiles.Position.y, 0.0f), new Vector3(0.8f, 0.8f, 0.0f));
    }
  }

  public float UnsafeAbs(float n)
  {
    int num = MemoryMarshal.Cast<float, int>(MemoryMarshal.CreateSpan<float>(ref n, 1))[0] & int.MaxValue;
    return MemoryMarshal.Cast<int, float>(MemoryMarshal.CreateSpan<int>(ref num, 1))[0];
  }

  public float Distance(Vector3 from, Vector3 to) => (from - to).sqrMagnitude;

  public Follower GetPrisonerRef()
  {
    return (UnityEngine.Object) this.CurrentStructureToMove != (UnityEngine.Object) null && this.CurrentStructureToMove.Type == StructureBrain.TYPES.PRISON && this.CurrentStructureToMove.Brain.Data.FollowerID != -1 ? FollowerManager.FindFollowerByID(this.CurrentStructureToMove.Brain.Data.FollowerID) : (Follower) null;
  }

  public Prison GetPrisonPlacementObjectRef()
  {
    return (UnityEngine.Object) this.CurrentStructureToMove != (UnityEngine.Object) null && this.CurrentStructureToMove.Type == StructureBrain.TYPES.PRISON && (UnityEngine.Object) this.placementObject != (UnityEngine.Object) null ? this.placementObject.GetComponentInChildren<Prison>(true) : (Prison) null;
  }

  public void MovePrisonerWithPrison(Follower prisonFollower, Prison prisonPlacementObject)
  {
    if (!((UnityEngine.Object) prisonFollower != (UnityEngine.Object) null) || !((UnityEngine.Object) prisonPlacementObject != (UnityEngine.Object) null))
      return;
    Vector3 position = prisonPlacementObject.PrisonerLocation.position;
    prisonFollower.transform.position = position;
    prisonFollower.Brain.LastPosition = position;
  }

  public void PrisonerMoveTween(Follower prisonFollower, Prison prisonPlacementObject)
  {
    if (!((UnityEngine.Object) prisonFollower != (UnityEngine.Object) null) || !((UnityEngine.Object) prisonPlacementObject != (UnityEngine.Object) null) || this.prisonerMoveTween != null && this.prisonerMoveTween.active)
      return;
    Vector3 endValue = this.CurrentTile.transform.position + prisonPlacementObject.PrisonerLocation.localPosition;
    this.prisonerMoveTween = (Tween) prisonFollower.transform.DOMove(endValue, this.TopSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late, true);
  }

  public void ResetPrisonerOnCancelReposition(Follower prisonFollower)
  {
    if (!((UnityEngine.Object) prisonFollower != (UnityEngine.Object) null))
      return;
    Prison componentInChildren = this.CurrentStructureToMove.GetComponentInChildren<Prison>(true);
    prisonFollower.transform.position = componentInChildren.PrisonerLocation.position;
    prisonFollower.Brain.LastPosition = componentInChildren.PrisonerLocation.position;
  }

  public void DoPrisonPlacementActions()
  {
    Follower prisonerRef = this.GetPrisonerRef();
    if (!((UnityEngine.Object) prisonerRef != (UnityEngine.Object) null))
      return;
    Prison componentInChildren = this.CurrentStructureToMove.GetComponentInChildren<Prison>(true);
    componentInChildren.StructureInfo.FollowerID = prisonerRef.Brain.Info.ID;
    prisonerRef.transform.position = componentInChildren.PrisonerLocation.position;
    prisonerRef.Brain.LastPosition = componentInChildren.PrisonerLocation.position;
  }

  public bool IsPrisonWithPrisoner()
  {
    return (UnityEngine.Object) this.CurrentStructureToMove != (UnityEngine.Object) null && this.CurrentStructureToMove.Type == StructureBrain.TYPES.PRISON && this.CurrentStructureToMove.Brain.Data.FollowerID != -1;
  }

  public GameObject GetFromPool(GameObject prefab)
  {
    if (this.prefabPool.Count <= 0)
      return UnityEngine.Object.Instantiate<GameObject>(prefab);
    GameObject fromPool = this.prefabPool.Dequeue();
    fromPool.SetActive(true);
    return fromPool;
  }

  public void ReturnToPool(GameObject obj)
  {
    obj.SetActive(false);
    obj.transform.SetParent((Transform) null, false);
    this.prefabPool.Enqueue(obj);
  }

  [CompilerGenerated]
  public static Vector3 \u003CPlaceObject\u003Eg__GetDirection\u007C137_0(Vector3 inputDir)
  {
    if ((double) inputDir.x > 0.0 && (double) inputDir.y < 0.5 && (double) inputDir.y > -0.5)
      return (Vector3) PlacementRegion.Right;
    if ((double) inputDir.x > 0.5 && (double) inputDir.y > 0.5)
      return (Vector3) PlacementRegion.UpRight;
    if ((double) inputDir.x < 0.0 && (double) inputDir.y < 0.5 && (double) inputDir.y > -0.5)
      return (Vector3) PlacementRegion.Left;
    if ((double) inputDir.x < -0.5 && (double) inputDir.y > 0.5)
      return (Vector3) PlacementRegion.UpLeft;
    if ((double) inputDir.y < 0.0 && (double) inputDir.x < 0.5 && (double) inputDir.x > -0.5)
      return (Vector3) PlacementRegion.Down;
    if ((double) inputDir.y < 0.0 && (double) inputDir.x > 0.5)
      return (Vector3) PlacementRegion.DownRight;
    if ((double) inputDir.y < 0.0 && (double) inputDir.x < -0.5)
      return (Vector3) PlacementRegion.DownLeft;
    if ((double) inputDir.y <= 0.0 || (double) inputDir.x >= 0.5)
      return (Vector3) PlacementRegion.Up;
    double x = (double) inputDir.x;
    return (Vector3) PlacementRegion.Up;
  }

  [CompilerGenerated]
  public bool \u003CPlaceObject\u003Eg__SetTile\u007C137_1(Vector3 position, Vector3 direction)
  {
    if (this.IsValidPlacement(position + direction, false, true))
    {
      PlacementTile tile = this.GetTile(position + direction);
      if ((UnityEngine.Object) tile != (UnityEngine.Object) this.CurrentTile)
      {
        this.PreviousTile = this.CurrentTile;
        this.CurrentTile = tile;
        return true;
      }
    }
    return false;
  }

  [CompilerGenerated]
  public static Vector3 \u003CPlaceObject_o\u003Eg__GetDirection\u007C139_0(Vector3 inputDir)
  {
    if ((double) inputDir.x > 0.0 && (double) inputDir.y < 0.5 && (double) inputDir.y > -0.5)
      return (Vector3) PlacementRegion.Right;
    if ((double) inputDir.x > 0.5 && (double) inputDir.y > 0.5)
      return (Vector3) PlacementRegion.UpRight;
    if ((double) inputDir.x < 0.0 && (double) inputDir.y < 0.5 && (double) inputDir.y > -0.5)
      return (Vector3) PlacementRegion.Left;
    if ((double) inputDir.x < -0.5 && (double) inputDir.y > 0.5)
      return (Vector3) PlacementRegion.UpLeft;
    if ((double) inputDir.y < 0.0 && (double) inputDir.x < 0.5 && (double) inputDir.x > -0.5)
      return (Vector3) PlacementRegion.Down;
    if ((double) inputDir.y < 0.0 && (double) inputDir.x > 0.5)
      return (Vector3) PlacementRegion.DownRight;
    if ((double) inputDir.y < 0.0 && (double) inputDir.x < -0.5)
      return (Vector3) PlacementRegion.DownLeft;
    if ((double) inputDir.y <= 0.0 || (double) inputDir.x >= 0.5)
      return (Vector3) PlacementRegion.Up;
    double x = (double) inputDir.x;
    return (Vector3) PlacementRegion.Up;
  }

  [CompilerGenerated]
  public bool \u003CPlaceObject_o\u003Eg__SetTile\u007C139_1(Vector3 position, Vector3 direction)
  {
    if (this.IsValidPlacement(position + direction, false, true))
    {
      PlacementTile tile = this.GetTile(position + direction);
      if ((UnityEngine.Object) tile != (UnityEngine.Object) this.CurrentTile)
      {
        this.PreviousTile = this.CurrentTile;
        this.CurrentTile = tile;
        return true;
      }
    }
    return false;
  }

  [CompilerGenerated]
  public void \u003CBuild\u003Eb__154_0(GameObject r) => this.UpdateFenceablePositions();

  [CompilerGenerated]
  public void \u003CClearPrefabs\u003Eb__156_0()
  {
    if (this.isEditingBuildings)
      return;
    WeedManager.ShowAll();
  }

  public enum Direction
  {
    Left,
    Right,
    Up,
    Down,
    UpLeft,
    DownLeft,
    UpRight,
    DownRight,
  }

  [Serializable]
  public class ResourcesAndCount
  {
    public StructureBrain.TYPES Resource;
    public int Count;
    public int Variant;
    public Vector2 MinMaxDistanceFromCenter = Vector2.zero;
    public float MinDistanceBetweenSameStructure;
    public List<PlacementRegion.Direction> BlockNeighbouringTiles = new List<PlacementRegion.Direction>();
    public Vector2Int RandomVariation = Vector2Int.zero;
  }

  public delegate void NewBuilding(StructureBrain.TYPES Type);

  public delegate void BuildingEvent(int structureID);

  [Serializable]
  public class TileGridTile
  {
    public Vector2Int Position;
    public Vector3 WorldPosition;
    public bool Occupied;
    public bool Obstructed;
    public bool ReservedForWaste;
    public int BlockNeighbouringTiles;
    public bool IsUpgrade;
    public int PathID = -1;
    public StructureBrain.TYPES ObjectOnTile;
    public int ObjectID = -1;
    public int OldObjectID = -1;
    public bool Collapsed;

    public bool CanPlaceStructure => !this.Occupied;

    public bool CanPlaceObstruction
    {
      get
      {
        return !this.Occupied && !this.Obstructed && !this.ReservedForWaste && this.PathID == -1 && this.BlockNeighbouringTiles <= 0;
      }
    }

    public static PlacementRegion.TileGridTile Create(
      PlacementRegion p,
      Vector2Int Position,
      bool Occupied,
      bool Obstructed)
    {
      return new PlacementRegion.TileGridTile()
      {
        Position = Position,
        WorldPosition = Utils.RotatePointAroundPivot(p.transform.position + new Vector3((float) Position.x, (float) Position.y), p.transform.position, new Vector3(0.0f, 0.0f, 45f)),
        Occupied = Occupied,
        Obstructed = Obstructed,
        BlockNeighbouringTiles = 0
      };
    }
  }

  public enum Mode
  {
    None,
    Building,
    Upgrading,
    Demolishing,
    Moving,
    MultiBuild,
  }
}
