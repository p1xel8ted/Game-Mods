// Decompiled with JetBrains decompiler
// Type: MMRoomGeneration.IslandPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

#nullable disable
namespace MMRoomGeneration;

public class IslandPiece : BaseMonoBehaviour
{
  public IslandConnector[] Connectors;
  public List<IslandConnector> NorthConnectors = new List<IslandConnector>();
  public List<IslandConnector> EastConnectors = new List<IslandConnector>();
  public List<IslandConnector> SouthConnectors = new List<IslandConnector>();
  public List<IslandConnector> WestConnectors = new List<IslandConnector>();
  public PolygonCollider2D _Collider;
  public bool CanSpawnEncounter = true;
  public bool IsDoor;
  private string CurrentFilePath;
  private MeshRenderer m;
  private bool checkedMesh;
  private GameObject SpriteShapeGO;
  public IslandPiece.ListOfGameObjectAndProbability SpriteShapes = new IslandPiece.ListOfGameObjectAndProbability();
  public IslandPiece.ListOfGameObjectAndProbability Encounters = new IslandPiece.ListOfGameObjectAndProbability();
  public List<SpriteShapeController> EncounterSpriteShapes = new List<SpriteShapeController>();
  private List<IslandConnector> ReturnConnectors;
  public string NewPrefabPath = "Assets";

  public PolygonCollider2D Collider
  {
    get
    {
      if ((UnityEngine.Object) this._Collider == (UnityEngine.Object) null)
        this._Collider = this.GetComponentInChildren<PolygonCollider2D>();
      if ((UnityEngine.Object) this._Collider == (UnityEngine.Object) null)
        this._Collider = this.GetComponent<PolygonCollider2D>();
      return this._Collider;
    }
  }

  public string CurrentEncounter { get; private set; }

  private void OnDrawGizmos()
  {
    this.CurrentFilePath = $"{this.NewPrefabPath}/Encounter{(object) this.Encounters.ObjectList.Count}.prefab";
  }

  private void CreateEnemyEncounterPrefab()
  {
  }

  private void GetConnectors()
  {
    this.Connectors = this.GetComponentsInChildren<IslandConnector>();
    this.NorthConnectors.Clear();
    this.EastConnectors.Clear();
    this.SouthConnectors.Clear();
    this.WestConnectors.Clear();
    foreach (IslandConnector connector in this.Connectors)
    {
      switch (connector.MyDirection)
      {
        case IslandConnector.Direction.North:
          this.NorthConnectors.Add(connector);
          break;
        case IslandConnector.Direction.East:
          this.EastConnectors.Add(connector);
          break;
        case IslandConnector.Direction.South:
          this.SouthConnectors.Add(connector);
          break;
        case IslandConnector.Direction.West:
          this.WestConnectors.Add(connector);
          break;
      }
    }
  }

  private void OnEnable()
  {
    if (this.checkedMesh)
      return;
    this.m = this.GetComponent<MeshRenderer>();
    if ((UnityEngine.Object) this.m != (UnityEngine.Object) null)
      this.m.enabled = false;
    else
      this.checkedMesh = true;
  }

  public void SetSpriteShape()
  {
    GenerateRoom objectOfType = UnityEngine.Object.FindObjectOfType<GenerateRoom>();
    if ((UnityEngine.Object) this.SpriteShapeGO == (UnityEngine.Object) null)
    {
      this.SpriteShapeGO = new GameObject();
      this.SpriteShapeGO.transform.parent = this.transform;
      this.SpriteShapeGO.transform.localPosition = Vector3.zero;
      this.SpriteShapeGO.name = "Sprite Shape";
    }
    SpriteShapeController spriteShapeController = this.SpriteShapeGO.AddComponent<SpriteShapeController>();
    if ((UnityEngine.Object) objectOfType.DecorationList.SpriteShapeMaterial != (UnityEngine.Object) null)
    {
      Material[] sharedMaterials = spriteShapeController.spriteShapeRenderer.sharedMaterials;
      for (int index = 0; index < sharedMaterials.Length; ++index)
        sharedMaterials[index] = objectOfType.DecorationList.SpriteShapeMaterial;
      spriteShapeController.spriteShapeRenderer.sharedMaterials = sharedMaterials;
    }
    spriteShapeController.spriteShape = objectOfType.DecorationList.SpriteShape;
    spriteShapeController.spline.Clear();
    int index1 = -1;
    Vector2[] points = this.Collider.points;
    Array.Reverse((Array) points);
    while (++index1 < points.Length)
      spriteShapeController.spline.InsertPointAt(index1, (Vector3) points[index1]);
    foreach (Renderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.enabled = false;
  }

  public static IslandPiece.GameObjectAndProbability GetRandomWeightedIndex(
    List<IslandPiece.GameObjectAndProbability> weights,
    double Random)
  {
    if (weights == null || weights.Count == 0)
      return (IslandPiece.GameObjectAndProbability) null;
    int num1 = 0;
    for (int index = 0; index < weights.Count; ++index)
    {
      if (weights[index].Probability >= 0 && weights[index].AvailableOnLayer())
        num1 += weights[index].Probability;
    }
    float num2 = 0.0f;
    for (int index = 0; index < weights.Count; ++index)
    {
      if (weights[index].AvailableOnLayer() && (double) weights[index].Probability > 0.0)
      {
        num2 += (float) weights[index].Probability / (float) num1;
        if ((double) num2 >= Random)
          return weights[index];
      }
    }
    return (IslandPiece.GameObjectAndProbability) null;
  }

  public IEnumerator InitIsland(
    System.Random Seed,
    SpriteShape SecondarySpriteShape,
    System.Action completeCallback = null)
  {
    IslandPiece islandPiece = this;
    bool waiting = true;
    islandPiece.HideSprites();
    if (islandPiece.SpriteShapes.ObjectList.Count > 0)
    {
      string gameObjectPath = islandPiece.SpriteShapes.ObjectList[Seed.Next(0, islandPiece.SpriteShapes.ObjectList.Count)].GameObjectPath;
      if (gameObjectPath != null)
      {
        Addressables.InstantiateAsync((object) gameObjectPath, new Vector3(0.0f, 0.0f, -0.005f), Quaternion.identity, islandPiece.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          this.EncounterSpriteShapes = new List<SpriteShapeController>((IEnumerable<SpriteShapeController>) obj.Result.GetComponentsInChildren<SpriteShapeController>());
          foreach (SpriteShapeController encounterSpriteShape in this.EncounterSpriteShapes)
          {
            encounterSpriteShape.spriteShape = SecondarySpriteShape;
            if ((UnityEngine.Object) GenerateRoom.Instance.DecorationList.SpriteShapeMaterial != (UnityEngine.Object) null)
            {
              Material[] sharedMaterials = encounterSpriteShape.spriteShapeRenderer.sharedMaterials;
              for (int index = 0; index < sharedMaterials.Length; ++index)
                sharedMaterials[index] = GenerateRoom.Instance.DecorationList.SpriteShapeMaterial;
              encounterSpriteShape.spriteShapeRenderer.sharedMaterials = sharedMaterials;
            }
            encounterSpriteShape.spriteShapeRenderer.sortingLayerName = "Ground - Secondary Layer";
            encounterSpriteShape.spriteShapeRenderer.shadowCastingMode = ShadowCastingMode.Off;
            encounterSpriteShape.spriteShapeRenderer.receiveShadows = false;
          }
          waiting = false;
        });
        while (waiting)
          yield return (object) null;
      }
    }
    List<IslandPiece.GameObjectAndProbability> objectAndProbabilityList = new List<IslandPiece.GameObjectAndProbability>();
    foreach (IslandPiece.GameObjectAndProbability objectAndProbability in islandPiece.Encounters.ObjectList)
    {
      if (objectAndProbability.AvailableOnLayer() && ((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || !BiomeGenerator.EncounterAlreadyUsed(objectAndProbability.GameObjectPath)))
        objectAndProbabilityList.Add(objectAndProbability);
    }
    if (islandPiece.Encounters.ObjectList.Count > 0 && objectAndProbabilityList.Count <= 0)
    {
      foreach (IslandPiece.GameObjectAndProbability objectAndProbability in islandPiece.Encounters.ObjectList)
        BiomeGenerator.RemoveEncounterAsUsed(objectAndProbability.GameObjectPath);
      foreach (IslandPiece.GameObjectAndProbability objectAndProbability in islandPiece.Encounters.ObjectList)
      {
        if (objectAndProbability.AvailableOnLayer() && ((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || !BiomeGenerator.EncounterAlreadyUsed(objectAndProbability.GameObjectPath)))
          objectAndProbabilityList.Add(objectAndProbability);
      }
    }
    if (islandPiece.Encounters.ObjectList.Count > 0 && objectAndProbabilityList.Count > 0)
    {
      string gameObjectPath = objectAndProbabilityList[Seed.Next(0, objectAndProbabilityList.Count)].GameObjectPath;
      if (gameObjectPath != null)
      {
        BiomeGenerator.SetEncounterAsUsed(gameObjectPath);
        islandPiece.CurrentEncounter = gameObjectPath;
        waiting = true;
        Addressables.InstantiateAsync((object) gameObjectPath, islandPiece.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj => waiting = false);
        while (waiting)
          yield return (object) null;
      }
      else
        Debug.Log((object) "WARNING: Null encounter");
    }
    System.Action action = completeCallback;
    if (action != null)
      action();
  }

  public void HideSprites()
  {
    foreach (Renderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.enabled = false;
  }

  private void ShowSprites()
  {
    if ((UnityEngine.Object) this.SpriteShapeGO != (UnityEngine.Object) null)
    {
      if (Application.isEditor)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.SpriteShapeGO);
      else
        UnityEngine.Object.Destroy((UnityEngine.Object) this.SpriteShapeGO);
    }
    this.SpriteShapeGO = (GameObject) null;
    foreach (Renderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.enabled = true;
  }

  private void RoundColliders()
  {
    List<Vector2> points = new List<Vector2>();
    int index = -1;
    while (++index < this.Collider.points.Length)
    {
      float x = Mathf.Round(this.Collider.points[index].x * 2f) / 2f;
      float y = Mathf.Round(this.Collider.points[index].y * 2f) / 2f;
      points.Add(new Vector2(x, y));
    }
    this.Collider.SetPath(0, (List<Vector2>) points);
  }

  public List<IslandConnector> GetConnectorsDirection(
    IslandConnector.Direction Direction,
    bool AcceptOthers)
  {
    switch (Direction)
    {
      case IslandConnector.Direction.North:
        return AcceptOthers && this.NorthConnectors.Count <= 0 ? this.GetRandomConnector() : this.NorthConnectors;
      case IslandConnector.Direction.East:
        return AcceptOthers && this.EastConnectors.Count <= 0 ? this.GetRandomConnector() : this.EastConnectors;
      case IslandConnector.Direction.South:
        return AcceptOthers && this.SouthConnectors.Count <= 0 ? this.GetRandomConnector() : this.SouthConnectors;
      case IslandConnector.Direction.West:
        return AcceptOthers && this.WestConnectors.Count <= 0 ? this.GetRandomConnector() : this.WestConnectors;
      default:
        return (List<IslandConnector>) null;
    }
  }

  public List<IslandConnector> GetRandomConnector()
  {
    this.ReturnConnectors = new List<IslandConnector>();
    foreach (IslandConnector connector in this.Connectors)
    {
      if (!connector.Active)
        this.ReturnConnectors.Add(connector);
    }
    return this.ReturnConnectors;
  }

  [Serializable]
  public class GameObjectAndProbability
  {
    public string GameObjectPath;
    [Range(0.0f, 100f)]
    public int Probability = 50;
    public bool LayerOne;
    public bool LayerTwo;
    public bool LayerThree;
    public bool LayerFour;

    internal bool AvailableOnLayer()
    {
      if (GameManager.DungeonUseAllLayers)
        return true;
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
        default:
          return false;
      }
    }
  }

  [Serializable]
  public class ListOfGameObjectAndProbability
  {
    public List<IslandPiece.GameObjectAndProbability> ObjectList = new List<IslandPiece.GameObjectAndProbability>();

    public string GetRandomGameObject(double RandomSeed)
    {
      return IslandPiece.GetRandomWeightedIndex(this.ObjectList, RandomSeed).GameObjectPath;
    }
  }
}
