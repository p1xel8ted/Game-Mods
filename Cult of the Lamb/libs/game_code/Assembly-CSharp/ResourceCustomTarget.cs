// Decompiled with JetBrains decompiler
// Type: ResourceCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMRoomGeneration;
using System;
using UnityEngine;

#nullable disable
public class ResourceCustomTarget : BaseMonoBehaviour, IPoolListener
{
  public const float MinimumCollectDistance = 1f;
  public const float MinimumCollectDuration = 0.33f;
  public const float Acceleration = 12f;
  public const float MaxSpeed = 15f;
  public InventoryItemDisplay inventoryItemDisplay;
  public GameObject Target;
  public Transform ResourceTransform;
  public float Delay = 0.5f;
  public Vector3 TargetPosition;
  public float Speed = -7f;
  public float ResourceTargetDistance;
  public float Angle;
  public float AngleTimer;
  public float TargetAngle;
  public TrailRenderer Trail;
  public float ImageZ;
  public float ImageZSpeed;
  public float TurnSpeed = 7f;
  public Vector3 ZPosition;
  public System.Action Callback;
  public static GameObject resourceCustomTargetPrefab;
  public bool UseDeltaTime = true;
  public float lifetime;
  public string SfxToPlay;
  public bool init;
  public GameObject createdObject;

  public void Init(GameObject Target, InventoryItem.ITEM_TYPE Item, System.Action Callback)
  {
    this.Target = Target;
    this.Angle = (float) UnityEngine.Random.Range(0, 360);
    this.inventoryItemDisplay.SetImage(Item);
    this.Callback = Callback;
    this.transform.localScale = Vector3.one * 1.5f;
    this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.ResourceTransform = this.transform;
  }

  public void Init(GameObject Target, Sprite sprite, System.Action Callback)
  {
    this.Target = Target;
    this.Angle = (float) UnityEngine.Random.Range(0, 360);
    this.inventoryItemDisplay.SetImage(sprite);
    this.Callback = Callback;
    this.transform.localScale = Vector3.one * 1.5f;
    this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.ResourceTransform = this.transform;
  }

  public void OnEnable() => this.lifetime = 0.0f;

  public static void CreatePool(int count)
  {
    if ((UnityEngine.Object) ResourceCustomTarget.resourceCustomTargetPrefab == (UnityEngine.Object) null)
      ResourceCustomTarget.resourceCustomTargetPrefab = Resources.Load("Prefabs/Resources/ResourceCustomTarget") as GameObject;
    ObjectPool.CreatePool(ResourceCustomTarget.resourceCustomTargetPrefab, count);
  }

  public static void Create(
    GameObject Target,
    Vector3 position,
    InventoryItem.ITEM_TYPE Item,
    System.Action Callback,
    Transform parent,
    bool UseDeltaTime = true)
  {
    if ((UnityEngine.Object) ResourceCustomTarget.resourceCustomTargetPrefab == (UnityEngine.Object) null)
      ResourceCustomTarget.resourceCustomTargetPrefab = Resources.Load("Prefabs/Resources/ResourceCustomTarget") as GameObject;
    GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(ResourceCustomTarget.resourceCustomTargetPrefab, parent, true);
    gameObject1.transform.position = position;
    gameObject1.transform.eulerAngles = Vector3.zero;
    ResourceCustomTarget component = gameObject1.GetComponent<ResourceCustomTarget>();
    component.createdObject = gameObject1;
    component.Init(Target, Item, Callback);
    component.UseDeltaTime = UseDeltaTime;
    GameObject gameObject2 = BiomeConstants.Instance.HitFX_Blocked.Spawn();
    gameObject2.transform.parent = parent;
    gameObject2.transform.rotation = Quaternion.identity;
    gameObject2.transform.position = position;
    component.init = true;
  }

  public static void Create(
    GameObject Target,
    Vector3 position,
    InventoryItem.ITEM_TYPE Item,
    System.Action Callback,
    bool UseDeltaTime = true)
  {
    Transform parent = (Transform) null;
    if ((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null)
      parent = RoomManager.Instance.CurrentRoomPrefab.transform;
    else if ((UnityEngine.Object) GameObject.FindGameObjectWithTag("Unit Layer") != (UnityEngine.Object) null)
      parent = GameObject.FindGameObjectWithTag("Unit Layer").transform;
    else if ((UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null)
      parent = GenerateRoom.Instance.transform;
    if ((UnityEngine.Object) ResourceCustomTarget.resourceCustomTargetPrefab == (UnityEngine.Object) null)
      ResourceCustomTarget.resourceCustomTargetPrefab = Resources.Load("Prefabs/Resources/ResourceCustomTarget") as GameObject;
    GameObject gameObject1 = ObjectPool.Spawn(ResourceCustomTarget.resourceCustomTargetPrefab, parent);
    gameObject1.transform.localPosition = parent.position;
    gameObject1.transform.position = position;
    gameObject1.transform.eulerAngles = Vector3.zero;
    ResourceCustomTarget component = gameObject1.GetComponent<ResourceCustomTarget>();
    component.createdObject = gameObject1;
    component.Init(Target, Item, Callback);
    component.UseDeltaTime = UseDeltaTime;
    GameObject gameObject2 = BiomeConstants.Instance.HitFX_Blocked.Spawn();
    gameObject2.transform.parent = parent;
    gameObject2.transform.rotation = Quaternion.identity;
    gameObject2.transform.position = position;
    component.init = true;
  }

  public static void Create(GameObject Target, Vector3 position, Sprite sprite, System.Action Callback)
  {
    Transform parent = (Transform) null;
    if ((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null)
      parent = RoomManager.Instance.CurrentRoomPrefab.transform;
    else if ((UnityEngine.Object) GameObject.FindGameObjectWithTag("Unit Layer") != (UnityEngine.Object) null)
      parent = GameObject.FindGameObjectWithTag("Unit Layer").transform;
    else if ((UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null)
      parent = GenerateRoom.Instance.transform;
    if ((UnityEngine.Object) ResourceCustomTarget.resourceCustomTargetPrefab == (UnityEngine.Object) null)
      ResourceCustomTarget.resourceCustomTargetPrefab = Resources.Load("Prefabs/Resources/ResourceCustomTarget") as GameObject;
    GameObject gameObject1 = ObjectPool.Spawn(ResourceCustomTarget.resourceCustomTargetPrefab, parent);
    gameObject1.transform.localPosition = parent.position;
    gameObject1.transform.position = position;
    gameObject1.transform.eulerAngles = Vector3.zero;
    ResourceCustomTarget component = gameObject1.GetComponent<ResourceCustomTarget>();
    component.createdObject = gameObject1;
    component.Init(Target, sprite, Callback);
    GameObject gameObject2 = BiomeConstants.Instance.HitFX_Blocked.Spawn();
    gameObject2.transform.parent = parent;
    gameObject2.transform.rotation = Quaternion.identity;
    gameObject2.transform.position = position;
    component.init = true;
  }

  public float DeltaTime => !this.UseDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;

  public void Update()
  {
    if (!this.init)
      return;
    this.lifetime += this.DeltaTime;
    if ((UnityEngine.Object) this.Target != (UnityEngine.Object) null)
    {
      this.TargetPosition = this.Target.transform.position;
      this.ResourceTargetDistance = Vector2.Distance((Vector2) this.ResourceTransform.position, (Vector2) this.Target.transform.position);
    }
    else
      this.ResourceTargetDistance = Vector2.Distance((Vector2) this.ResourceTransform.position, (Vector2) this.TargetPosition);
    this.MoveToTarget(this.TargetPosition);
    if ((double) this.ResourceTargetDistance >= 1.0 || (double) this.lifetime <= 0.33000001311302185)
      return;
    this.CollectMe();
  }

  public void MoveToTarget(Vector3 targetPosition)
  {
    if ((double) this.Speed > 0.0)
    {
      this.ZPosition = this.inventoryItemDisplay.gameObject.transform.position;
      this.ZPosition.z = Mathf.Lerp(this.ZPosition.z, targetPosition.z - 0.5f, 10f * this.DeltaTime);
      this.inventoryItemDisplay.gameObject.transform.position = this.ZPosition;
    }
    if ((double) (this.AngleTimer += this.DeltaTime) > 0.5)
      this.TurnSpeed = Mathf.Lerp(this.TurnSpeed, 1f, 20f * this.DeltaTime);
    this.TargetAngle = Utils.GetAngle(this.ResourceTransform.position, targetPosition);
    this.Angle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.Angle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.Angle) * (Math.PI / 180.0)))) * 57.295780181884766 / (double) this.TurnSpeed * (double) this.DeltaTime * 60.0);
    if ((double) this.Speed < 15.0)
      this.Speed += 12f;
    this.Speed *= this.DeltaTime;
    this.ResourceTransform.position += new Vector3(this.Speed * Mathf.Cos(this.Angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(this.Angle * ((float) Math.PI / 180f)));
  }

  public void CollectMe()
  {
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.gameObject.transform.position);
    GameObject gameObject = BiomeConstants.Instance.HitFX_Blocked.Spawn();
    gameObject.transform.position = this.inventoryItemDisplay.gameObject.transform.position;
    gameObject.transform.rotation = Quaternion.identity;
    CameraManager.shakeCamera(0.2f, this.Angle);
    System.Action callback = this.Callback;
    if (callback != null)
      callback();
    ObjectPool.Recycle(this.createdObject);
  }

  public void OnRecycled()
  {
    this.init = false;
    this.Speed = -7f;
    this.AngleTimer = 0.0f;
    this.TurnSpeed = 7f;
    this.Angle = 0.0f;
    this.TargetAngle = 0.0f;
    this.Target = (GameObject) null;
  }
}
