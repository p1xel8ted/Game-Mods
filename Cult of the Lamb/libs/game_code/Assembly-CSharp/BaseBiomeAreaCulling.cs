// Decompiled with JetBrains decompiler
// Type: BaseBiomeAreaCulling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class BaseBiomeAreaCulling : MonoBehaviour
{
  public const string TeleportAreaName = "Teleport Area";
  public Camera _camera;
  public Transform _cameraTransform;
  public GameObject _teleportAreaReference;
  public static float2 up = (float2) Vector2.up;
  public static float2 right = (float2) Vector2.right;
  [SerializeField]
  public BaseBiomeAreaCulling.AABB _cameraBounds;
  public float2 _cameraPosition;
  public BaseBiomeAreaCulling.CullableArea _area;
  public float _halfWidth;
  public float _halfHeight;
  public bool _shouldLoad;
  public bool disabled;
  public static bool isPaused = false;
  [SerializeField]
  public List<BaseBiomeAreaCulling.CullableArea> _areaList;
  public BaseBiomeAreaCulling.AABB _lodadingBounds;
  public float2 minBoundsBuffer = new float2(5f, 2f);
  public float2 maxBoundsBuffer = new float2(5f, 15f);

  public static void SetPauseState(bool state) => BaseBiomeAreaCulling.isPaused = state;

  public async void Start()
  {
    this.disabled = true;
    await System.Threading.Tasks.Task.Delay(1000);
    this.CacheCameraReferences();
    this._areaList = new List<BaseBiomeAreaCulling.CullableArea>();
    this.GetTeleportAreaReference();
    await System.Threading.Tasks.Task.Yield();
    this.CreateSceneryBuckets();
    await System.Threading.Tasks.Task.Yield();
    this.RemoveUnusedBuckets();
    this.disabled = false;
  }

  public void RemoveUnusedBuckets()
  {
    List<BaseBiomeAreaCulling.CullableArea> cullableAreaList = new List<BaseBiomeAreaCulling.CullableArea>();
    for (int index = 0; index < this._areaList.Count; ++index)
    {
      BaseBiomeAreaCulling.CullableArea area = this._areaList[index];
      if ((UnityEngine.Object) area.reference == (UnityEngine.Object) null || area.reference.transform.childCount < 1)
        cullableAreaList.Add(area);
    }
    foreach (BaseBiomeAreaCulling.CullableArea cullableArea in cullableAreaList)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) cullableArea.reference);
      this._areaList.Remove(cullableArea);
    }
  }

  public void CreateSceneryBuckets()
  {
    GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
    GameObject gameObject1 = (GameObject) null;
    GameObject gameObject2 = (GameObject) null;
    foreach (GameObject gameObject3 in rootGameObjects)
    {
      if (gameObject3.name == "Room")
        gameObject1 = gameObject3;
    }
    IEnumerator enumerator = (IEnumerator) gameObject1.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if (current.gameObject.name == "SceneryTransform")
          gameObject2 = current.gameObject;
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    Queue<Transform> transformQueue = new Queue<Transform>((IEnumerable<Transform>) gameObject2.GetComponentsInChildren<Transform>(true));
    this.AddCullableAreasToList();
    this.CreateBuckets(gameObject2.transform);
    while (transformQueue.Count > 0)
    {
      Transform transform = transformQueue.Dequeue();
      if (!(transform.parent.name != gameObject2.name) && !(transform.name == "DecalMesh"))
      {
        foreach (BaseBiomeAreaCulling.CullableArea area in this._areaList)
        {
          if (!((UnityEngine.Object) area.reference == (UnityEngine.Object) null) && !(area.name == "Teleport Area") && area.bounds.OverlapPoint(transform.position.x, transform.position.y))
            transform.parent = area.reference.transform;
        }
      }
    }
  }

  public void CreateBuckets(Transform parent)
  {
    for (int index = 0; index < this._areaList.Count; ++index)
    {
      BaseBiomeAreaCulling.CullableArea area = this._areaList[index];
      if (!(area.name == "Teleport Area"))
      {
        GameObject gameObject = new GameObject(area.name);
        gameObject.transform.SetParent(parent);
        area.reference = gameObject;
        this._areaList[index] = area;
      }
    }
  }

  public void AddCullableAreasToList()
  {
    int num1 = 25;
    int num2 = 7;
    int x1 = -14 - num1;
    int x2 = 18 + num1;
    int num3 = -16 - num1;
    int y1 = 24;
    int num4 = x2 - x1;
    int num5 = y1 - num3;
    int num6 = num2;
    int num7 = num4 / num6;
    int num8 = num5 / num2;
    this._areaList.Add(new BaseBiomeAreaCulling.CullableArea()
    {
      name = "Bucket : Top Strip",
      bounds = new BaseBiomeAreaCulling.AABB()
      {
        min = new float2((float) x1, (float) y1),
        max = new float2((float) x2, (float) (y1 + num1))
      },
      isLoaded = true,
      reference = (GameObject) null
    });
    for (int index1 = 0; index1 < num2; ++index1)
    {
      int x3 = x1 + num7 * index1;
      for (int index2 = 0; index2 < num2; ++index2)
      {
        int y2 = num3 + num8 * index2;
        this._areaList.Add(new BaseBiomeAreaCulling.CullableArea()
        {
          name = $"Bucket : {index1},{index2}",
          bounds = new BaseBiomeAreaCulling.AABB()
          {
            min = new float2((float) x3, (float) y2),
            max = new float2((float) (x3 + num7), (float) (y2 + num8))
          },
          isLoaded = true,
          reference = (GameObject) null
        });
      }
    }
  }

  public void GetTeleportAreaReference()
  {
    this._teleportAreaReference = GameObject.Find("Teleport Area");
    if ((UnityEngine.Object) this._teleportAreaReference == (UnityEngine.Object) null)
      Debug.LogError((object) "Unable to Get a Reference to the Teleport Area");
    this._areaList.Add(new BaseBiomeAreaCulling.CullableArea()
    {
      name = "Teleport Area",
      bounds = new BaseBiomeAreaCulling.AABB()
      {
        min = new float2(-16f, -4f),
        max = new float2(16f, 16f)
      },
      isLoaded = true,
      reference = this._teleportAreaReference
    });
  }

  public void CacheCameraReferences()
  {
    this._camera = Camera.main;
    if ((UnityEngine.Object) this._camera == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Unable to cache main camera");
      this.enabled = false;
    }
    else
      this._cameraTransform = this._camera.transform;
  }

  public void Update()
  {
    if (this.disabled || BaseBiomeAreaCulling.isPaused)
      return;
    this._cameraPosition = new float2(this._cameraTransform.position.x, this._cameraTransform.position.y);
    this._cameraBounds = this.GetViewportBounds(this._cameraPosition, this._camera.orthographicSize, this._camera.aspect);
    this._lodadingBounds.min = this._cameraBounds.min - this.minBoundsBuffer;
    this._lodadingBounds.max = this._cameraBounds.max + this.maxBoundsBuffer;
    for (int index = 0; index < this._areaList.Count; ++index)
    {
      this._area = this._areaList[index];
      this._shouldLoad = this._lodadingBounds.Overlap(this._area.bounds);
      if (this._shouldLoad != this._area.isLoaded)
      {
        this._area.isLoaded = this._shouldLoad;
        this._areaList[index] = this._area;
        if (!((UnityEngine.Object) this._area.reference == (UnityEngine.Object) null))
          this._area.reference.SetActive(this._shouldLoad);
      }
    }
  }

  [BurstCompile]
  public BaseBiomeAreaCulling.AABB GetViewportBounds(float2 position, float orthSize, float aspect)
  {
    this._halfWidth = orthSize * aspect;
    this._halfHeight = orthSize;
    this._cameraBounds = new BaseBiomeAreaCulling.AABB(position);
    float2 point1 = position + BaseBiomeAreaCulling.up * this._halfHeight + BaseBiomeAreaCulling.right * this._halfWidth;
    float2 point2 = position - BaseBiomeAreaCulling.up * this._halfHeight - BaseBiomeAreaCulling.right * this._halfWidth;
    this._cameraBounds.Encapsulate(point1);
    this._cameraBounds.Encapsulate(point2);
    return this._cameraBounds;
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Vector3[] vector3Array = new Vector3[5]
    {
      new Vector3(this._cameraBounds.min.x, this._cameraBounds.min.y, 0.0f),
      new Vector3(this._cameraBounds.min.x, this._cameraBounds.max.y, 0.0f),
      new Vector3(this._cameraBounds.max.x, this._cameraBounds.max.y, 0.0f),
      new Vector3(this._cameraBounds.max.x, this._cameraBounds.min.y, 0.0f),
      new Vector3(this._cameraBounds.min.x, this._cameraBounds.min.y, 0.0f)
    };
    for (int index = 0; index < 4; ++index)
      Gizmos.DrawLine(vector3Array[index], vector3Array[index + 1]);
    Gizmos.color = Color.blue;
    vector3Array[0] = new Vector3(this._lodadingBounds.min.x, this._lodadingBounds.min.y, 0.0f);
    vector3Array[1] = new Vector3(this._lodadingBounds.min.x, this._lodadingBounds.max.y, 0.0f);
    vector3Array[2] = new Vector3(this._lodadingBounds.max.x, this._lodadingBounds.max.y, 0.0f);
    vector3Array[3] = new Vector3(this._lodadingBounds.max.x, this._lodadingBounds.min.y, 0.0f);
    vector3Array[4] = new Vector3(this._lodadingBounds.min.x, this._lodadingBounds.min.y, 0.0f);
    for (int index = 0; index < 4; ++index)
      Gizmos.DrawLine(vector3Array[index], vector3Array[index + 1]);
    if (this._areaList == null)
      return;
    foreach (BaseBiomeAreaCulling.CullableArea area in this._areaList)
    {
      Gizmos.color = !area.isLoaded ? Color.red : Color.green;
      BaseBiomeAreaCulling.AABB bounds = area.bounds;
      vector3Array[0] = new Vector3(bounds.min.x, bounds.min.y, 0.0f);
      vector3Array[1] = new Vector3(bounds.min.x, bounds.max.y, 0.0f);
      vector3Array[2] = new Vector3(bounds.max.x, bounds.max.y, 0.0f);
      vector3Array[3] = new Vector3(bounds.max.x, bounds.min.y, 0.0f);
      vector3Array[4] = new Vector3(bounds.min.x, bounds.min.y, 0.0f);
      for (int index = 0; index < 4; ++index)
        Gizmos.DrawLine(vector3Array[index], vector3Array[index + 1]);
    }
  }

  [BurstCompile]
  [Serializable]
  public struct AABB(float2 start)
  {
    public float2 min = start;
    public float2 max = start;

    [BurstCompile]
    public void Encapsulate(float point_x, float point_y)
    {
      float2 y = new float2(point_x, point_y);
      this.min = math.min(this.min, y);
      this.max = math.max(this.max, y);
    }

    [BurstCompile]
    public void Encapsulate(float2 point)
    {
      this.min = math.min(this.min, point);
      this.max = math.max(this.max, point);
    }

    [BurstCompile]
    public void Encapsulate(BaseBiomeAreaCulling.AABB bounds)
    {
      this.min = math.min(this.min, bounds.min);
      this.max = math.max(this.max, bounds.max);
    }

    [BurstCompile]
    public bool Overlap(BaseBiomeAreaCulling.AABB other)
    {
      return (double) this.min.x <= (double) other.max.x && (double) this.max.x >= (double) other.min.x && (double) this.min.y <= (double) other.max.y && (double) this.max.y >= (double) other.min.y;
    }

    [BurstCompile]
    public bool OverlapPoint(float x, float y)
    {
      return (double) this.min.x <= (double) x && (double) this.max.x >= (double) x && (double) this.min.y <= (double) y && (double) this.max.y >= (double) y;
    }
  }

  [Serializable]
  public struct CullableArea
  {
    public string name;
    public BaseBiomeAreaCulling.AABB bounds;
    public bool isLoaded;
    public GameObject reference;
  }
}
