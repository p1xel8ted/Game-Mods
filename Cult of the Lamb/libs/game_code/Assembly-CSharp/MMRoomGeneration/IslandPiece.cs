// Decompiled with JetBrains decompiler
// Type: MMRoomGeneration.IslandPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Map;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

#nullable disable
namespace MMRoomGeneration;

public class IslandPiece : BaseMonoBehaviour
{
  public bool TestMe;
  public bool CanUseRandomDoors;
  public IslandConnector[] Connectors;
  public List<IslandConnector> NorthConnectors = new List<IslandConnector>();
  public List<IslandConnector> EastConnectors = new List<IslandConnector>();
  public List<IslandConnector> SouthConnectors = new List<IslandConnector>();
  public List<IslandConnector> WestConnectors = new List<IslandConnector>();
  public PolygonCollider2D _Collider;
  public bool CanSpawnEncounter = true;
  public bool IsDoor;
  public bool IsEntrance;
  [CompilerGenerated]
  public string \u003CCurrentEncounter\u003Ek__BackingField;
  public string CurrentFilePath;
  public MeshRenderer m;
  public bool checkedMesh;
  public GameObject SpriteShapeGO;
  public IslandPiece.ListOfGameObjectAndProbability SpriteShapes = new IslandPiece.ListOfGameObjectAndProbability();
  public IslandPiece.ListOfGameObjectAndProbability SpriteShapes2 = new IslandPiece.ListOfGameObjectAndProbability();
  public IslandPiece.ListOfGameObjectAndProbability Encounters = new IslandPiece.ListOfGameObjectAndProbability();
  public List<SpriteShapeController> EncounterSpriteShapes = new List<SpriteShapeController>();
  public List<IslandConnector> ReturnConnectors;
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
    set => this._Collider = value;
  }

  public string CurrentEncounter
  {
    get => this.\u003CCurrentEncounter\u003Ek__BackingField;
    set => this.\u003CCurrentEncounter\u003Ek__BackingField = value;
  }

  public void OnDrawGizmos()
  {
    this.CurrentFilePath = $"{this.NewPrefabPath}/Encounter{this.Encounters.ObjectList.Count.ToString()}.prefab";
  }

  public void CreateEnemyEncounterPrefab()
  {
  }

  public void GetConnectors()
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

  public void OnEnable()
  {
    if (this.checkedMesh)
      return;
    this.m = this.GetComponent<MeshRenderer>();
    if ((UnityEngine.Object) this.m != (UnityEngine.Object) null)
      this.m.enabled = false;
    else
      this.checkedMesh = true;
    if (!this.IsDoor || PlayerFarming.Location != FollowerLocation.Dungeon1_6 || this.IsEntrance)
      return;
    if (this.Connectors[0].MyDirection == IslandConnector.Direction.East || this.Connectors[0].MyDirection == IslandConnector.Direction.West)
      this.Collider.SetPath(0, new Vector2[4]
      {
        new Vector2((double) this.Collider.points[0].x > 0.0 ? 20f : -20f, this.Collider.points[0].y),
        new Vector2((double) this.Collider.points[1].x > 0.0 ? 20f : -20f, this.Collider.points[1].y),
        this.Collider.points[2],
        this.Collider.points[3]
      });
    else if (this.Connectors[0].MyDirection == IslandConnector.Direction.North)
    {
      this.Collider.SetPath(0, new Vector2[4]
      {
        new Vector2(this.Collider.points[0].x, (double) this.Collider.points[0].y > 0.0 ? 10f : -10f),
        new Vector2(this.Collider.points[1].x, (double) this.Collider.points[1].y > 0.0 ? 10f : -10f),
        this.Collider.points[2],
        this.Collider.points[3]
      });
    }
    else
    {
      GenerateRoom componentInParent = this.GetComponentInParent<GenerateRoom>(true);
      if (!((UnityEngine.Object) componentInParent == (UnityEngine.Object) null) && componentInParent.North == GenerateRoom.ConnectionTypes.NextLayer && (!((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null) || MapManager.Instance.CurrentNode == null || MapManager.Instance.CurrentNode.nodeType == Map.NodeType.DungeonFloor || MapManager.Instance.CurrentNode.nodeType == Map.NodeType.MiniBossFloor))
        return;
      this.Collider.SetPath(0, new Vector2[4]
      {
        new Vector2(this.Collider.points[0].x, (double) this.Collider.points[0].y > 0.0 ? 30f : -30f),
        new Vector2(this.Collider.points[1].x, (double) this.Collider.points[1].y > 0.0 ? 30f : -30f),
        this.Collider.points[2],
        this.Collider.points[3]
      });
    }
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
    Array.Reverse<Vector2>(points);
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
    IslandPiece.ListOfGameObjectAndProbability objectAndProbability1 = GameManager.Layer2 ? islandPiece.SpriteShapes2 : islandPiece.SpriteShapes;
    if (objectAndProbability1.ObjectList.Count > 0)
    {
      string gameObjectPath = objectAndProbability1.ObjectList[Seed.Next(0, objectAndProbability1.ObjectList.Count)].GameObjectPath;
      if (gameObjectPath != null)
      {
        Addressables_wrapper.InstantiateAsync((object) gameObjectPath, new Vector3(0.0f, 0.0f, -0.005f), Quaternion.identity, islandPiece.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
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
        }));
        while (waiting)
          yield return (object) null;
      }
    }
    List<IslandPiece.GameObjectAndProbability> objectAndProbabilityList = new List<IslandPiece.GameObjectAndProbability>();
    foreach (IslandPiece.GameObjectAndProbability objectAndProbability2 in islandPiece.Encounters.ObjectList)
    {
      if (objectAndProbability2.AvailableOnLayer() && ((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || !BiomeGenerator.EncounterAlreadyUsed(objectAndProbability2.GameObjectPath)))
        objectAndProbabilityList.Add(objectAndProbability2);
    }
    if (islandPiece.Encounters.ObjectList.Count > 0 && objectAndProbabilityList.Count <= 0)
    {
      foreach (IslandPiece.GameObjectAndProbability objectAndProbability3 in islandPiece.Encounters.ObjectList)
        BiomeGenerator.RemoveEncounterAsUsed(objectAndProbability3.GameObjectPath);
      foreach (IslandPiece.GameObjectAndProbability objectAndProbability4 in islandPiece.Encounters.ObjectList)
      {
        if (objectAndProbability4.AvailableOnLayer() && ((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || !BiomeGenerator.EncounterAlreadyUsed(objectAndProbability4.GameObjectPath)))
          objectAndProbabilityList.Add(objectAndProbability4);
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
        Addressables_wrapper.InstantiateAsync((object) gameObjectPath, islandPiece.transform, false, (Action<AsyncOperationHandle<GameObject>>) (obj => waiting = false));
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

  public void ShowSprites()
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

  public void RoundColliders()
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
    public bool NewGamePlus;
    public bool LayerOne;
    public bool LayerTwo;
    public bool LayerThree;
    public bool LayerFour;

    public bool AvailableOnLayer()
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
