// Decompiled with JetBrains decompiler
// Type: PlacementObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

#nullable disable
public class PlacementObject : BaseMonoBehaviour
{
  public static PlacementObject Instance;
  public Transform ChildObject;
  public Vector2Int Bounds = new Vector2Int(1, 1);
  public string ToBuildAsset;
  public Vector3 Scale = Vector3.one;
  public Vector3 ScaleSpeed = Vector3.zero;
  public Vector2 ScaleEasing = new Vector2(0.3f, 0.5f);
  public float Shake;
  public float ShakeSpeed;
  public Vector2 ShakeEasing = new Vector2(0.3f, 0.5f);
  public float ShakeIntensity = 0.2f;
  [CompilerGenerated]
  public StructureBrain.TYPES \u003CStructureType\u003Ek__BackingField;
  public Vector3 originalPosition = new Vector3(0.0f, 0.0f, -0.01f);
  public int Direction;
  public Transform RotatedObject;

  public StructureBrain.TYPES StructureType
  {
    get => this.\u003CStructureType\u003Ek__BackingField;
    set => this.\u003CStructureType\u003Ek__BackingField = value;
  }

  public virtual void Start()
  {
    if (string.IsNullOrEmpty(this.ToBuildAsset))
      return;
    this.ToBuildAsset = $"Assets/{this.ToBuildAsset}.prefab";
    Addressables_wrapper.InstantiateAsync((object) this.ToBuildAsset, Vector3.zero, Quaternion.identity, this.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.ChildObject = obj.Result?.transform;
      foreach (MonoBehaviour componentsInChild in this.ChildObject.GetComponentsInChildren<MonoBehaviour>())
      {
        switch (componentsInChild)
        {
          case SkeletonRenderer _:
          case SpriteShapeController _:
          case Interaction_RacingGate _:
          case Interaction_FarmCropGrower _:
            continue;
          default:
            componentsInChild.enabled = false;
            continue;
        }
      }
      if (this.StructureType != StructureBrain.TYPES.FARM_PLOT)
        return;
      this.ChildObject.GetComponentInChildren<FarmPlot>().ShowSoil();
    }));
  }

  public void OnEnable() => PlacementObject.Instance = this;

  public void OnDisable()
  {
    if (!((UnityEngine.Object) PlacementObject.Instance == (UnityEngine.Object) this))
      return;
    PlacementObject.Instance = (PlacementObject) null;
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.ChildObject == (UnityEngine.Object) null)
      return;
    this.ScaleSpeed.x += (1f - this.Scale.x) * this.ScaleEasing.x / Time.unscaledDeltaTime;
    this.Scale.x += (this.ScaleSpeed.x *= this.ScaleEasing.y) * Time.unscaledDeltaTime;
    this.ScaleSpeed.y += (1f - this.Scale.y) * this.ScaleEasing.x / Time.unscaledDeltaTime;
    this.Scale.y += (this.ScaleSpeed.y *= this.ScaleEasing.y) * Time.unscaledDeltaTime;
    this.ScaleSpeed.z += (1f - this.Scale.z) * this.ScaleEasing.x / Time.unscaledDeltaTime;
    this.Scale.z += (this.ScaleSpeed.z *= this.ScaleEasing.y) * Time.unscaledDeltaTime;
    this.ChildObject.localScale = this.Scale;
    this.ShakeSpeed += (0.0f - this.Shake) * this.ShakeEasing.x / Time.unscaledDeltaTime;
    this.Shake += (this.ShakeSpeed *= this.ShakeEasing.y) * Time.unscaledDeltaTime;
    this.ChildObject.localPosition = this.originalPosition + new Vector3(this.Shake, 0.0f, 0.0f);
  }

  public void SetScale(Vector3 Scale) => this.Scale = Scale;

  public void DoShake()
  {
    this.ShakeSpeed = this.ShakeIntensity * (++this.Direction % 2 == 0 ? -1f : 1f);
  }

  public void OnDrawGizmos()
  {
    int x = -1;
    while (++x < this.Bounds.x)
    {
      float y = -1f;
      while ((double) ++y < (double) this.Bounds.y)
      {
        Gizmos.matrix = this.RotatedObject.localToWorldMatrix;
        Gizmos.DrawWireCube(new Vector3((float) x, y), new Vector3(0.7f, 0.7f, 0.0f));
      }
    }
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__16_0(AsyncOperationHandle<GameObject> obj)
  {
    this.ChildObject = obj.Result?.transform;
    foreach (MonoBehaviour componentsInChild in this.ChildObject.GetComponentsInChildren<MonoBehaviour>())
    {
      switch (componentsInChild)
      {
        case SkeletonRenderer _:
        case SpriteShapeController _:
        case Interaction_RacingGate _:
        case Interaction_FarmCropGrower _:
          continue;
        default:
          componentsInChild.enabled = false;
          continue;
      }
    }
    if (this.StructureType != StructureBrain.TYPES.FARM_PLOT)
      return;
    this.ChildObject.GetComponentInChildren<FarmPlot>().ShowSoil();
  }
}
