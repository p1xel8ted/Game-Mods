// Decompiled with JetBrains decompiler
// Type: PlacementRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class PlacementRegion : BaseMonoBehaviour
{
  public static PlacementRegion Instance;
  public bool PlaceWeeds = true;
  public bool PlaceRubble = true;
  public List<PlacementRegion.ResourcesAndCount> ResourcesToPlace = new List<PlacementRegion.ResourcesAndCount>();
  public static PlacementRegion.NewBuilding OnNewBuilding;
  public static List<PlacementRegion> PlacementRegions = new List<PlacementRegion>();
  public GameObject PlacementGameObject;
  public GameObject PlacementSquare;
  public StructureBrain.TYPES StructureType;
  public PlacementObjectUI PlacementObjectUI;
  private PlacementObject placementObject;
  private bool isEditingBuildings;
  private Vector3 previousEditingPosition = Vector3.zero;
  private Structures_PlacementRegion _StructureBrain;
  public Dictionary<Vector2Int, PlacementRegion.TileGridTile> GridTileLookup = new Dictionary<Vector2Int, PlacementRegion.TileGridTile>();
  private float InputDelay;
  private PlacementObjectUI placementUI;
  private bool canPlaceObjectOnBuildings;
  private bool isPath;
  private List<Vector3> placingPathsPositions = new List<Vector3>();
  private int direction = 1;
  private Tween moveTween;
  private Tween shakeTween;
  private Vector3 cachedDirection;
  private Plane plane = new Plane(Vector3.forward, Vector3.zero);
  private Vector3 _PlacementPosition;
  public Structure structure;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  private List<PlacementTile> Tiles;
  private List<PlacementRegion.TileGridTile> PreviewTilesList = new List<PlacementRegion.TileGridTile>();
  public PolygonCollider2D polygonCollider2D;
  private int Count;
  public int MaxTileCount = 50;
  private static Vector3Int Left = new Vector3Int(-1, 1, 0);
  private static Vector3Int Right = new Vector3Int(1, -1, 0);
  private static Vector3Int Up = new Vector3Int(1, 1, 0);
  private static Vector3Int Down = new Vector3Int(-1, -1, 0);
  private static Vector3Int UpLeft = new Vector3Int(0, 1, 0);
  private static Vector3Int DownLeft = new Vector3Int(-1, 0, 0);
  private static Vector3Int UpRight = new Vector3Int(1, 0, 0);
  private static Vector3Int DownRight = new Vector3Int(0, -1, 0);
  private float Lerp;
  private float LerpSpeed = 7f;
  private PlacementTile CurrentTile;
  private PlacementTile PreviousTile;
  public PlacementRegion.Mode CurrentMode;
  private Structure PreviousStructure;
  private Structure CurrentStructureToUpgrade;
  private Structure CurrentStructureToMove;
  private float TopSpeed = 10f;
  private float LerpToTileSpeed = 10f;
  public GameObject BuildSitePrefab;
  public GameObject BuildSiteBuildingProjectPrefab;

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

  private void Awake()
  {
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    PlacementRegion.Instance = this;
  }

  private void OnDestroy()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    PlacementRegion.Instance = (PlacementRegion) null;
  }

  private void OnBrainAssigned()
  {
    if (this.Grid.Count > 0)
      return;
    this.CreateFloodFill();
  }

  private void OnEnable()
  {
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    PlacementRegion.PlacementRegions.Add(this);
  }

  private void OnDisable()
  {
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    PlacementRegion.PlacementRegions.Remove(this);
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.PlayRoutine());

  public void PlayMove(Structure structure)
  {
    this.CurrentMode = PlacementRegion.Mode.Moving;
    this.CurrentStructureToMove = structure;
    this.CurrentStructureToMove.gameObject.SetActive(false);
    this.StructureType = structure.Brain.Data.Type;
    this.direction = 1;
    this.PlacementGameObject = TypeAndPlacementObjects.GetByType(this.StructureType).PlacementObject;
    if (this.CurrentMode == PlacementRegion.Mode.Moving)
    {
      PlacementRegion.BuildingEvent buildingBeganMoving = PlacementRegion.OnBuildingBeganMoving;
      if (buildingBeganMoving != null)
        buildingBeganMoving(structure.Structure_Info.ID);
    }
    this.StartCoroutine((IEnumerator) this.PlayRoutine());
  }

  private void OnStructuresPlaced()
  {
    if ((UnityEngine.Object) this.structure == (UnityEngine.Object) null || this.structure.Brain == null)
      return;
    this.structureBrain = this.structure.Brain as Structures_PlacementRegion;
    if (this.structureBrain == null)
      return;
    this.structureBrain.ResourcesToPlace = new List<PlacementRegion.ResourcesAndCount>((IEnumerable<PlacementRegion.ResourcesAndCount>) this.ResourcesToPlace);
    this.structureBrain.PlaceWeeds = this.PlaceWeeds;
    this.structureBrain.PlaceRubble = this.PlaceRubble;
  }

  private IEnumerator PlayRoutine()
  {
    PlacementRegion placementRegion = this;
    HUD_Manager.Instance.ShowEditMode(true);
    int x = 1;
    if ((UnityEngine.Object) placementRegion.CurrentStructureToMove != (UnityEngine.Object) null)
      x = placementRegion.CurrentStructureToMove.Structure_Info.Direction;
    else if ((UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null)
      x = placementRegion.CurrentStructureToMove.Structure_Info.Direction;
    placementRegion.direction = x;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    placementRegion.placementObject = UnityEngine.Object.Instantiate<GameObject>(placementRegion.PlacementGameObject, placementRegion.transform.parent).GetComponent<PlacementObject>();
    placementRegion.placementObject.StructureType = placementRegion.StructureType;
    placementRegion.placementObject.transform.position = new Vector3(-7.6f, -5.61f, 0.0f);
    placementRegion.placementObject.transform.localScale = new Vector3((float) x, placementRegion.placementObject.transform.localScale.y, placementRegion.placementObject.transform.localScale.z);
    placementRegion.PreviousTile = placementRegion.CurrentTile;
    placementRegion.CurrentTile = placementRegion.GetClosestTileAtWorldPosition(placementRegion.placementObject.transform.position, 1.5f);
    if (placementRegion.StructureType == StructureBrain.TYPES.EDIT_BUILDINGS)
    {
      placementRegion.CurrentMode = PlacementRegion.Mode.Demolishing;
      placementRegion.isEditingBuildings = true;
      LightingManager.Instance.inOverride = true;
      placementRegion.LightingSettings.overrideLightingProperties = placementRegion.overrideLightingProperties;
      LightingManager.Instance.overrideSettings = placementRegion.LightingSettings;
      LightingManager.Instance.transitionDurationMultiplier = 0.0f;
      LightingManager.Instance.UpdateLighting(true);
    }
    if (placementRegion.CurrentMode != PlacementRegion.Mode.Demolishing && placementRegion.CurrentMode != PlacementRegion.Mode.Moving && (UnityEngine.Object) placementRegion.placementUI == (UnityEngine.Object) null && StructuresData.GetCost(placementRegion.StructureType).Count > 0)
      placementRegion.placementUI = UnityEngine.Object.Instantiate<PlacementObjectUI>(placementRegion.PlacementObjectUI, GameObject.FindWithTag("Canvas").transform);
    placementRegion.isPath = StructureBrain.IsPath(placementRegion.StructureType);
    placementRegion.canPlaceObjectOnBuildings = placementRegion.isPath;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(placementRegion.placementObject.gameObject);
    Time.timeScale = 0.0f;
    Debug.Log((object) ("TIME SCALE! " + (object) Time.timeScale));
    GameManager.overridePlayerPosition = true;
    placementRegion.PlaceTiles();
    WeedManager.HideAll();
    if (placementRegion.isPath)
      PathTileManager.Instance.ShowPathsBeingBuilt();
    yield return (object) placementRegion.StartCoroutine((IEnumerator) placementRegion.PlaceObject());
    HUD_Manager.Instance.Show();
    Time.timeScale = 1f;
    placementRegion._PlacementPosition = Vector3.zero;
    placementRegion.previousEditingPosition = Vector3.zero;
    placementRegion.ClearPrefabs();
    HUD_Manager.Instance.ShowEditMode(false);
    GameManager.GetInstance().CamFollowTarget.enabled = true;
    GameManager.GetInstance().RemoveFromCamera(placementRegion.placementObject.gameObject);
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.transitionDurationMultiplier = 0.2f;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.UpdateLighting(true);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraResetTargetZoom();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  private bool IsNaturalObstruction(StructureBrain.TYPES type)
  {
    return type == StructureBrain.TYPES.TREE || type == StructureBrain.TYPES.BERRY_BUSH || type == StructureBrain.TYPES.RUBBLE || type == StructureBrain.TYPES.RUBBLE_BIG || type == StructureBrain.TYPES.WATER_SMALL || type == StructureBrain.TYPES.WATER_MEDIUM || type == StructureBrain.TYPES.WATER_BIG;
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
    if (this.GridTileLookup != null && this.GridTileLookup.Count < this.Grid.Count)
      this.CreateDictionaryLookup();
    try
    {
      return this.GridTileLookup[Position];
    }
    catch
    {
      foreach (PlacementRegion.TileGridTile tileGridTile in this.Grid)
      {
        if (tileGridTile != null && tileGridTile.Position.x == Position.x && tileGridTile.Position.y == Position.y)
          return tileGridTile;
      }
      return (PlacementRegion.TileGridTile) null;
    }
  }

  public PlacementRegion.TileGridTile GetTileGridTile(float x, float y)
  {
    if (this.GridTileLookup.Count < this.Grid.Count)
      this.CreateDictionaryLookup();
    Vector2Int key = new Vector2Int((int) x, (int) y);
    PlacementRegion.TileGridTile tileGridTile1;
    if (this.GridTileLookup.ContainsKey(key))
    {
      tileGridTile1 = this.GridTileLookup[key];
    }
    else
    {
      foreach (PlacementRegion.TileGridTile tileGridTile2 in this.Grid)
      {
        if ((double) tileGridTile2.Position.x == (double) x && (double) tileGridTile2.Position.y == (double) y)
          return tileGridTile2;
      }
      tileGridTile1 = (PlacementRegion.TileGridTile) null;
    }
    return tileGridTile1;
  }

  private void CreateFloodFill()
  {
    this.Count = 0;
    this.FloodFillCreateTiles(0, 0);
  }

  private void PlaceTiles()
  {
    this.Tiles = new List<PlacementTile>();
    foreach (PlacementRegion.TileGridTile tileGridTile in this.Grid)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PlacementSquare, this.transform, false);
      gameObject.transform.localPosition = new Vector3((float) tileGridTile.Position.x, (float) tileGridTile.Position.y);
      PlacementTile component = gameObject.GetComponent<PlacementTile>();
      component.Position = new Vector3((float) tileGridTile.Position.x, (float) tileGridTile.Position.y);
      component.GridPosition = new Vector2Int(tileGridTile.Position.x, tileGridTile.Position.y);
      this.Tiles.Add(component);
    }
  }

  private void PreviewTiles()
  {
    this.PreviewTilesList = new List<PlacementRegion.TileGridTile>();
    this.CreateFloodFill();
  }

  private void FloodFillCreateTiles(int x, int y)
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

  private void ClearTiles()
  {
    foreach (Component tile in this.Tiles)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) tile.gameObject);
    this.Tiles.Clear();
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
      float num2 = Vector3.Distance(Position, tile.Position);
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

  private bool IsValidPlacement(Vector3 Position, bool CheckOccupied, bool AllowObstructions)
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
    PlacementRegion.TileGridTile tileAtWorldPosition = this.Grid[0];
    foreach (PlacementRegion.TileGridTile tileGridTile in this.Grid)
    {
      if ((double) Vector3.Distance(tileGridTile.WorldPosition, Position) < (double) Vector3.Distance(tileAtWorldPosition.WorldPosition, Position))
        tileAtWorldPosition = tileGridTile;
    }
    return tileAtWorldPosition;
  }

  private IEnumerator PlaceObject()
  {
    PlacementRegion placementRegion = this;
    HUD_Manager.Instance.Hide(false);
    LightingManager.Instance.inOverride = true;
    placementRegion.LightingSettings.overrideLightingProperties = placementRegion.overrideLightingProperties;
    LightingManager.Instance.overrideSettings = placementRegion.LightingSettings;
    LightingManager.Instance.transitionDurationMultiplier = 0.0f;
    LightingManager.Instance.UpdateLighting(true);
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
      placementRegion.ClearTiles();
      float num1 = float.MaxValue;
      Structure structure1 = (Structure) null;
      foreach (Structure structure2 in Structure.Structures)
      {
        if (structure2.Type == structuresData.UpgradeFromType)
        {
          float num2 = Vector3.Distance(new Vector3((float) placementRegion.Grid[0].Position.x, (float) placementRegion.Grid[0].Position.y), structure2.transform.position);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            structure1 = structure2;
          }
        }
      }
      Debug.Log((object) ("CurrentStructureToUpgrade " + (object) placementRegion.CurrentStructureToUpgrade));
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
        yield return (object) null;
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
          Vector2 inputDir;
          inputDir.x = InputManager.Gameplay.GetHorizontalAxis();
          inputDir.y = InputManager.Gameplay.GetVerticalAxis();
          placementRegion.InputDelay -= Time.unscaledDeltaTime;
          if (placementRegion.CurrentMode == PlacementRegion.Mode.Upgrading)
          {
            if ((double) placementRegion.Lerp >= 0.5 && (double) placementRegion.InputDelay < 0.0 && ((double) Mathf.Abs(inputDir.x) >= 0.30000001192092896 || (double) Mathf.Abs(inputDir.y) >= 0.30000001192092896 || InputManager.General.MouseInputActive))
            {
              placementRegion.InputDelay = 0.5f;
              Structure structure3 = (Structure) null;
              foreach (Structure structure4 in Structure.Structures)
              {
                if ((placementRegion.CurrentMode == PlacementRegion.Mode.Demolishing && structure4.Structure_Info.GridTilePosition != StructuresData.NullPosition || structure4.Type == structuresData.UpgradeFromType) && (UnityEngine.Object) structure4 != (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade && (UnityEngine.Object) structure4 != (UnityEngine.Object) null)
                {
                  Vector3 normalized1 = (Vector3) inputDir.normalized;
                  Vector3 vector3 = structure4.transform.position - placementRegion.placementObject.transform.position;
                  Vector3 normalized2 = vector3.normalized;
                  double num3 = (double) Vector3.Dot(normalized1, normalized2);
                  double num4;
                  if (!((UnityEngine.Object) structure3 == (UnityEngine.Object) null))
                  {
                    Vector3 normalized3 = (Vector3) inputDir.normalized;
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
            if ((InputManager.Gameplay.GetPlaceMoveUpgradeButtonDown() || placementRegion.isPath && InputManager.Gameplay.GetPlaceMoveUpgradeButtonHeld()) && (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null)
            {
              AudioManager.Instance.PlayOneShot("event:/building/place_building_spot", placementRegion.placementObject.transform.position);
              Loop = false;
            }
          }
          else
          {
            if ((double) inputDir.magnitude <= 0.0 && InputManager.General.MouseInputActive && GameManager.GetInstance().CamFollowTarget.enabled)
            {
              GameManager.GetInstance().CamFollowTarget.enabled = false;
              GameManager.GetInstance().RemoveFromCamera(placementRegion.placementObject.gameObject);
            }
            else if (!InputManager.General.MouseInputActive && !GameManager.GetInstance().CamFollowTarget.enabled)
            {
              GameManager.GetInstance().CamFollowTarget.CurrentPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
              GameManager.GetInstance().CamFollowTarget.enabled = true;
              GameManager.GetInstance().AddToCamera(placementRegion.placementObject.gameObject);
            }
            if ((double) Mathf.Abs(inputDir.x) >= 0.30000001192092896 || (double) Mathf.Abs(inputDir.y) >= 0.30000001192092896 || InputManager.General.MouseInputActive)
            {
              Vector3 direction = GetDirection((Vector3) inputDir);
              bool flag1 = false;
              if ((double) inputDir.magnitude <= 0.0 && InputManager.General.MouseInputActive)
              {
                Vector3 mousePositionWorld = placementRegion.GetMousePositionWorld();
                PlacementTile tileAtWorldPosition = placementRegion.GetClosestTileAtWorldPosition(placementRegion.placementObject.transform.position);
                if ((double) Vector3.Distance(placementRegion.transform.TransformPoint(tileAtWorldPosition.Position), mousePositionWorld) > (double) Mathf.Max((float) placementRegion.placementObject.Bounds.x / 2f, 1.5f))
                {
                  direction = GetDirection((mousePositionWorld - placementRegion.placementObject.transform.position).normalized);
                }
                else
                {
                  direction = (Vector3) Vector3Int.zero;
                  flag1 = true;
                }
              }
              bool flag2 = false;
              if (InputManager.General.MouseInputActive)
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
              if (((UnityEngine.Object) placementRegion.CurrentTile == (UnityEngine.Object) null || !placementRegion.IsValidPlacement(placementRegion.CurrentTile.Position, false, true)) && (UnityEngine.Object) placementRegion.PreviousTile != (UnityEngine.Object) null && InputManager.General.MouseInputActive)
                placementRegion.CurrentTile = placementRegion.PreviousTile;
              if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && !(direction == (Vector3) Vector3Int.zero) && !SetTile(placementRegion.CurrentTile.Position, direction) && !SetTile(placementRegion.CurrentTile.Position, direction + Vector3.left) && !SetTile(placementRegion.CurrentTile.Position, direction + Vector3.right) && !SetTile(placementRegion.CurrentTile.Position, direction + Vector3.up) && !SetTile(placementRegion.CurrentTile.Position, direction + Vector3.down) && ((double) inputDir.y <= 0.0 || !SetTile(placementRegion.CurrentTile.Position, (Vector3) PlacementRegion.Up)) && (double) inputDir.y < 0.0)
                SetTile(placementRegion.CurrentTile.Position, (Vector3) PlacementRegion.Down);
              if (placementRegion.shakeTween != null)
              {
                placementRegion.shakeTween.Kill();
                placementRegion.shakeTween = (Tween) null;
              }
              if ((double) inputDir.magnitude <= 0.0 && InputManager.General.MouseInputActive)
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
                Vector3 viewportPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().ScreenToViewportPoint((Vector3) InputManager.General.GetMousePosition());
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
              if ((UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && !flag2 && !InputManager.General.MouseInputActive)
              {
                if (placementRegion.moveTween == null || !placementRegion.moveTween.active)
                  placementRegion.moveTween = (Tween) placementRegion.placementObject.transform.DOMove(placementRegion.CurrentTile.transform.position, placementRegion.TopSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late, true);
              }
              else
                placementRegion.placementObject.transform.position += placementRegion.TopSpeed * Time.unscaledDeltaTime * Vector3.Normalize((Vector3) inputDir);
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
            if (InputManager.Gameplay.GetPlaceMoveUpgradeButtonDown() || placementRegion.isPath && InputManager.Gameplay.GetPlaceMoveUpgradeButtonHeld())
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
                if (flag && (UnityEngine.Object) placementRegion.CurrentTile != (UnityEngine.Object) null && placementRegion.IsValidPlacement(placementRegion.CurrentTile.Position, !placementRegion.isPath, AllowObstructions))
                {
                  AudioManager.Instance.PlayOneShot("event:/building/place_building_spot", placementRegion.placementObject.transform.position);
                  Loop = false;
                }
                else if (!placementRegion.isPath)
                {
                  Debug.Log((object) "Cant build here");
                  placementRegion.placementObject.gameObject.transform.DOKill();
                  MonoSingleton<Indicator>.Instance.gameObject.transform.DOKill();
                  placementRegion.shakeTween = (Tween) placementRegion.placementObject.gameObject.transform.DOShakePosition(0.3f, new Vector3(0.5f, 0.0f), 20).SetUpdate<Tweener>(true);
                  MonoSingleton<Indicator>.Instance.gameObject.transform.DOShakePosition(0.3f, new Vector3(0.5f, 0.0f), 20).SetUpdate<Tweener>(true);
                  AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", placementRegion.placementObject.transform.position);
                }
              }
            }
          }
          if ((double) InputManager.Gameplay.GetHorizontalAxis() > -0.30000001192092896 && (double) InputManager.Gameplay.GetHorizontalAxis() < 0.30000001192092896 && (double) InputManager.Gameplay.GetVerticalAxis() > -0.30000001192092896 && (double) InputManager.Gameplay.GetVerticalAxis() < 0.30000001192092896)
            placementRegion.InputDelay = 0.0f;
          if (placementRegion.isPath || placementRegion.GetPathAtPosition() != StructureBrain.TYPES.NONE && ((UnityEngine.Object) placementRegion.placementObject == (UnityEngine.Object) null || placementRegion.isEditingBuildings || placementRegion.isPath) && (UnityEngine.Object) placementRegion.CurrentStructureToUpgrade == (UnityEngine.Object) null && (UnityEngine.Object) placementRegion.CurrentStructureToMove == (UnityEngine.Object) null)
          {
            if (InputManager.Gameplay.GetInteract3ButtonHeld())
              PathTileManager.Instance.DeleteTile(placementRegion.PlacementPosition);
          }
          else if (InputManager.Gameplay.GetRemoveFlipButtonDown())
          {
            if ((bool) (UnityEngine.Object) placementRegion.placementObject && placementRegion.StructureType != StructureBrain.TYPES.EDIT_BUILDINGS && StructuresData.CanBeFlipped(placementRegion.StructureType))
            {
              placementRegion.placementObject.transform.localScale = new Vector3(placementRegion.placementObject.transform.localScale.x * -1f, placementRegion.placementObject.transform.localScale.y, placementRegion.placementObject.transform.localScale.z);
              placementRegion.direction = (int) placementRegion.placementObject.transform.localScale.x;
            }
            else if ((UnityEngine.Object) placementRegion.CurrentStructureToMove != (UnityEngine.Object) null && StructuresData.CanBeFlipped(placementRegion.CurrentStructureToMove.Type))
            {
              placementRegion.CurrentStructureToMove.Structure_Info.Direction *= -1;
              placementRegion.CurrentStructureToMove.transform.localScale = new Vector3((float) placementRegion.CurrentStructureToMove.Structure_Info.Direction, placementRegion.CurrentStructureToMove.transform.localScale.y, placementRegion.CurrentStructureToMove.transform.localScale.z);
              placementRegion.direction = placementRegion.CurrentStructureToMove.Structure_Info.Direction;
            }
            else if ((UnityEngine.Object) placementRegion.CurrentStructureToUpgrade != (UnityEngine.Object) null && StructuresData.CanBeFlipped(placementRegion.CurrentStructureToUpgrade.Type))
            {
              placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction *= -1;
              placementRegion.CurrentStructureToUpgrade.transform.localScale = new Vector3((float) placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction, placementRegion.CurrentStructureToUpgrade.transform.localScale.y, placementRegion.CurrentStructureToUpgrade.transform.localScale.z);
              placementRegion.direction = placementRegion.CurrentStructureToUpgrade.Structure_Info.Direction;
            }
          }
          if (InputManager.UI.GetCancelButtonDown())
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
                placementRegion.Play();
                yield break;
              }
            }
            else
              HUD_Manager.Instance.Show();
            Time.timeScale = 1f;
            placementRegion._PlacementPosition = Vector3.zero;
            placementRegion.previousEditingPosition = Vector3.zero;
            placementRegion.ClearPrefabs();
            placementRegion.isEditingBuildings = false;
            placementRegion._PlacementPosition = Vector3.zero;
            placementRegion.previousEditingPosition = Vector3.zero;
            MonoSingleton<Indicator>.Instance.HasSecondaryInteraction = false;
            MonoSingleton<Indicator>.Instance.SecondaryText.text = "";
            MonoSingleton<Indicator>.Instance.HideTopInfo();
            GameManager.GetInstance().CamFollowTarget.enabled = true;
            GameManager.GetInstance().RemoveFromCamera(placementRegion.placementObject.gameObject);
            yield break;
          }
          if (InputManager.Gameplay.GetRemoveFlipButtonDown() || placementRegion.isPath && InputManager.Gameplay.GetRemoveFlipButtonHeld())
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
            placementRegion.ClearPrefabs();
            placementRegion.StopAllCoroutines();
            placementRegion.Play();
            break;
        }
        if (placementRegion.CurrentMode != PlacementRegion.Mode.MultiBuild && placementRegion.CurrentMode != PlacementRegion.Mode.Demolishing && (placementRegion.CurrentMode != PlacementRegion.Mode.Upgrading || StructuresData.GetBuildOnlyOne(placementRegion.StructureType)) || !StructuresData.CanAfford(placementRegion.StructureType))
          goto label_142;
      }
      while (placementRegion.CurrentMode != PlacementRegion.Mode.Upgrading);
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().AddToCamera(placementRegion.placementObject.gameObject);
    }
label_142:
    yield return (object) new WaitForSecondsRealtime(0.15f);

    static Vector3 GetDirection(Vector3 inputDir)
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

    bool SetTile(Vector3 position, Vector3 direction)
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
  }

  private Vector3 GetMousePositionWorld()
  {
    Ray ray = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().ScreenPointToRay((Vector3) InputManager.General.GetMousePosition());
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

  private void DestroyBuilding()
  {
    for (int index = 0; index < this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs.Count; ++index)
    {
      DataManager.Instance.Followers_Imprisoned_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
      DataManager.Instance.Followers_OnMissionary_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
      DataManager.Instance.Followers_Elderly_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
      DataManager.Instance.Followers_Transitioning_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
    }
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (this.CurrentStructureToUpgrade.Brain.Data.FollowerID == allBrain.Info.ID || this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs.Contains(allBrain.Info.ID))
        allBrain.CurrentTask?.Abort();
    }
    this.structureBrain.ClearStructureFromGrid(this.CurrentStructureToUpgrade.Brain);
    CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
    this.CurrentStructureToUpgrade.GetComponent<Structure>().RemoveStructure();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentStructureToUpgrade.gameObject);
    GameManager.RecalculatePaths();
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

  private void Upgrade() => this.DoUpgradeRoutine();

  private void MoveBuilding(Vector3 Position, Vector2Int GridPosition)
  {
    this.CurrentStructureToMove.Brain.Data.Direction = this.direction;
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
      if (locationFollower.Brain.DesiredLocation == PlayerFarming.Location && this.CurrentStructureToMove.Brain.Data.FollowerID == locationFollower.Brain.Info.ID && !(locationFollower.Brain.CurrentTask is FollowerTask_ChangeLocation) || locationFollower.Brain.CurrentTask != null && locationFollower.Brain.CurrentTask is FollowerTask_ChangeLocation && ((FollowerTask_ChangeLocation) locationFollower.Brain.CurrentTask).TargetLocation == PlayerFarming.Location)
        locationFollower.Brain.CurrentTask?.Abort();
    }
    StructureManager.StructureChanged onStructureMoved = StructureManager.OnStructureMoved;
    if (onStructureMoved != null)
      onStructureMoved(this.CurrentStructureToMove.Brain.Data);
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

  private void DoUpgradeRoutine()
  {
    StructureBrain.TYPES type = this.CurrentStructureToUpgrade.Type;
    for (int index = 0; index < this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs.Count; ++index)
    {
      DataManager.Instance.Followers_Imprisoned_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
      DataManager.Instance.Followers_OnMissionary_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
      DataManager.Instance.Followers_Elderly_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
      DataManager.Instance.Followers_Transitioning_IDs.Remove(this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs[index]);
    }
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (this.CurrentStructureToUpgrade.Brain.Data.FollowerID == allBrain.Info.ID || this.CurrentStructureToUpgrade.Brain.Data.MultipleFollowerIDs.Contains(allBrain.Info.ID))
        allBrain.CurrentTask?.Abort();
    }
    FollowerLocation location = this.CurrentStructureToUpgrade.Structure_Info.Location;
    LocationManager locationManager = LocationManager.LocationManagers[location];
    int index1 = -1;
    while (++index1 < StructuresData.GetCost(this.StructureType).Count)
    {
      Debug.Log((object) $"{(object) StructuresData.GetCost(this.StructureType)[index1].CostItem}  {(object) StructuresData.GetCost(this.StructureType)[index1].CostValue}");
      Inventory.ChangeItemQuantity((int) StructuresData.GetCost(this.StructureType)[index1].CostItem, -StructuresData.GetCost(this.StructureType)[index1].CostValue);
    }
    StructuresData infoByType = StructuresData.GetInfoByType(this.StructureType, 0);
    infoByType.FollowerID = this.CurrentStructureToUpgrade.Structure_Info.FollowerID;
    infoByType.Inventory = this.CurrentStructureToUpgrade.Structure_Info.Inventory;
    infoByType.QueuedResources = this.CurrentStructureToUpgrade.Structure_Info.QueuedResources;
    GameObject gameObject;
    Structure component1;
    if (infoByType.IgnoreGrid)
    {
      gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BuildSiteBuildingProjectPrefab, this.CurrentStructureToUpgrade.transform.position, Quaternion.identity, locationManager.StructureLayer);
      component1 = gameObject.GetComponent<Structure>();
      component1.CreateStructure(this.StructureInfo.Location, gameObject.transform.position, this.placementObject.Bounds, this.StructureType);
      BuildSitePlotProject component2 = gameObject.GetComponent<BuildSitePlotProject>();
      component2.StructureInfo.Direction = this.direction;
      component2.StructureInfo.Inventory = this.CurrentStructureToUpgrade.Inventory;
      component2.StructureInfo.QueuedResources = this.CurrentStructureToUpgrade.Structure_Info.QueuedResources;
      component2.StructureInfo.ToBuildType = this.StructureType;
      component2.StructureInfo.FollowerID = infoByType.FollowerID;
      component2.StructureInfo.Bounds = this.placementObject.Bounds;
      component2.StructureInfo.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
      component2.Bounds = this.placementObject.Bounds;
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
      component3.StructureInfo.Inventory = this.CurrentStructureToUpgrade.Inventory;
      component3.StructureInfo.QueuedResources = this.CurrentStructureToUpgrade.Structure_Info.QueuedResources;
      component3.StructureInfo.ToBuildType = this.StructureType;
      component3.StructureInfo.Bounds = this.placementObject.Bounds;
      component3.StructureInfo.FollowerID = infoByType.FollowerID;
      component3.StructureInfo.GridTilePosition = this.CurrentStructureToUpgrade.Brain.Data.GridTilePosition;
      component3.StructureInfo.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
      component3.Bounds = this.placementObject.Bounds;
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

  private void Build(
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
    Structure s;
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
      if (!StructuresData.GetInfoByType(StructureType, 0).IsBuildingProject)
      {
        gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BuildSitePrefab, Position, Quaternion.identity, locationManager.StructureLayer);
        s = gameObject.GetComponent<Structure>();
        s.CreateStructure(this.StructureInfo.Location, gameObject.transform.position, Bounds, StructureType);
        BuildSitePlot component = gameObject.GetComponent<BuildSitePlot>();
        component.StructureInfo.Direction = this.direction;
        component.StructureInfo.ToBuildType = StructureType;
        component.StructureInfo.Bounds = Bounds;
        component.StructureInfo.GridTilePosition = GridPosition;
        component.StructureInfo.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
        component.Bounds = Bounds;
        this.MarkObstructionsForClearing(component.StructureInfo.GridTilePosition, component.StructureInfo.Bounds, component.StructureBrain.Data);
        if (this.isPath)
          component.StructureBrain.Build();
      }
      else
      {
        gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BuildSiteBuildingProjectPrefab, Position, Quaternion.identity, locationManager.StructureLayer);
        s = gameObject.GetComponent<Structure>();
        s.CreateStructure(this.StructureInfo.Location, gameObject.transform.position, Bounds, StructureType);
        BuildSitePlotProject component = gameObject.GetComponent<BuildSitePlotProject>();
        component.StructureInfo.Direction = this.direction;
        component.StructureInfo.ToBuildType = StructureType;
        component.StructureInfo.Bounds = Bounds;
        component.StructureInfo.GridTilePosition = GridPosition;
        component.StructureInfo.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
        component.Bounds = Bounds;
        component.StructureBrain.MarkObstructionsForClearing(component.StructureInfo.GridTilePosition, component.StructureInfo.Bounds, true);
      }
      this.structureBrain.AddStructureToGrid(s.Structure_Info);
      if (ManageCamera)
      {
        GameManager.GetInstance().RemoveAllFromCamera();
        GameManager.GetInstance().AddToCamera(gameObject);
      }
      GameManager.RecalculatePaths();
      PlacementRegion.NewBuilding onNewBuilding = PlacementRegion.OnNewBuilding;
      if (onNewBuilding == null)
        return;
      onNewBuilding(StructureType);
    }
    else
    {
      StructuresData infoByType = StructuresData.GetInfoByType(StructureType, 0);
      this.structureBrain.AddStructureToGrid(infoByType);
      infoByType.PrefabPath = $"Assets/{infoByType.PrefabPath}.prefab";
      Addressables.InstantiateAsync((object) infoByType.PrefabPath, locationManager.StructureLayer).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        obj.Result.transform.position = Position;
        WorkPlace component = obj.Result.GetComponent<WorkPlace>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.SetID($"{(object) obj.Result.transform.position.x}_{(object) obj.Result.transform.position.y}");
        s = obj.Result.GetComponent<Structure>();
        if ((UnityEngine.Object) s != (UnityEngine.Object) null)
          s.CreateStructure(this.StructureInfo.Location, obj.Result.transform.position, Bounds);
        s.Structure_Info.Direction = this.direction;
        s.Structure_Info.GridTilePosition = GridPosition;
        s.Structure_Info.PlacementRegionPosition = new Vector3Int((int) this.transform.position.x, (int) this.transform.position.y, 0);
        if (ManageCamera)
        {
          GameManager.GetInstance().RemoveAllFromCamera();
          GameManager.GetInstance().AddToCamera(obj.Result);
        }
        GameManager.RecalculatePaths();
        PlacementRegion.NewBuilding onNewBuilding = PlacementRegion.OnNewBuilding;
        if (onNewBuilding == null)
          return;
        onNewBuilding(StructureType);
      });
    }
  }

  private void ClearPrefabs()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.placementObject.gameObject);
    if ((UnityEngine.Object) this.CurrentStructureToMove != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.placementObject.gameObject);
    if ((bool) (UnityEngine.Object) this.placementUI)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.placementUI.gameObject);
    foreach (Component tile in this.Tiles)
      UnityEngine.Object.Destroy((UnityEngine.Object) tile.gameObject);
    this.Tiles.Clear();
    WeedManager.ShowAll();
    GameManager.overridePlayerPosition = false;
    Interactor.CurrentInteraction = (Interaction) null;
    Interactor.PreviousInteraction = (Interaction) null;
    MonoSingleton<Indicator>.Instance.text.text = "";
    MonoSingleton<Indicator>.Instance.SecondaryText.text = "";
    MonoSingleton<Indicator>.Instance.Thirdtext.text = "";
    MonoSingleton<Indicator>.Instance.Fourthtext.text = "";
    MonoSingleton<Indicator>.Instance.HideTopInfo();
    MonoSingleton<Indicator>.Instance.Reset();
    if (this.isPath)
      this.placingPathsPositions.Clear();
    foreach (Structure structure in Structure.Structures)
    {
      SpriteRenderer[] componentsInChildren = structure.gameObject.GetComponentsInChildren<SpriteRenderer>(true);
      if (componentsInChildren.Length != 0)
      {
        foreach (SpriteRenderer spriteRenderer in componentsInChildren)
        {
          if (!spriteRenderer.gameObject.CompareTag("BuildingEffectRadius"))
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

  private void UpdateTileAvailability()
  {
    if ((UnityEngine.Object) this.CurrentTile == (UnityEngine.Object) null)
      return;
    foreach (PlacementTile tile in this.Tiles)
    {
      PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile((float) tile.GridPosition.x, (float) tile.GridPosition.y);
      bool flag = tileGridTile.CanPlaceStructure || this.canPlaceObjectOnBuildings && !this.IsNaturalObstruction(tileGridTile.ObjectOnTile) && tileGridTile.ObjectOnTile != StructureBrain.TYPES.BUILD_SITE;
      tile.SetColor(flag || this.isPath ? Color.white : Color.red, this.placementObject.transform.position);
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
          else
            tile.SetColor(!flag1 || this.isPath ? Color.green : Color.red, this.placementObject.transform.position);
        }
      }
    }
  }

  private void OnDrawGizmos()
  {
    if (!Application.isEditor || Application.isPlaying)
      return;
    foreach (PlacementRegion.TileGridTile previewTiles in this.PreviewTilesList)
    {
      Gizmos.matrix = this.transform.localToWorldMatrix;
      Gizmos.DrawWireCube(new Vector3((float) previewTiles.Position.x, (float) previewTiles.Position.y, 0.0f), new Vector3(0.8f, 0.8f, 0.0f));
    }
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
