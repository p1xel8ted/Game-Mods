// Decompiled with JetBrains decompiler
// Type: SoulCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SoulCustomTarget : BaseMonoBehaviour
{
  public AnimationCurve ZCurve;
  public GameObject Target;
  public float Delay = 1.25f;
  public float Speed = -15f;
  public float Angle;
  public float TargetAngle;
  public SpriteRenderer Image;
  public TrailRenderer Trail;
  public float ImageZ;
  public float ImageZSpeed;
  public float TurnSpeed = 7f;
  public Vector3 ZPosition;
  public System.Action Callback;
  public float MaxSpeed = 20f;
  public bool init;
  public bool AddZOffset = true;
  public Vector3 targetPosition = Vector3.zero;
  public bool UseDeltaTime;
  public bool isCollected;
  public static GameObject projectilePrefab;
  public static string collectMeSfxPath;

  public void Init(
    GameObject Target,
    Color color,
    System.Action Callback,
    Sprite sprite = null,
    float Scale = 0.4f,
    float MaxSpeed = 20f,
    bool AddZOffset = true,
    bool UseDeltaTime = true)
  {
    this.Speed = -15f;
    this.Delay = 1.25f;
    this.Target = Target;
    this.AddZOffset = AddZOffset;
    this.Angle = (float) UnityEngine.Random.Range(0, 360);
    this.Image.color = color;
    this.Image.transform.localScale = Vector3.one * Scale;
    this.Trail.startWidth = Scale;
    this.Trail.startColor = color;
    this.Trail.endColor = new Color(color.r, color.g, color.b, 0.0f);
    this.Trail.Clear();
    this.Callback = Callback;
    this.MaxSpeed = MaxSpeed;
    this.UseDeltaTime = UseDeltaTime;
    this.isCollected = false;
    this.init = true;
    if (!((UnityEngine.Object) sprite != (UnityEngine.Object) null))
      return;
    this.Image.sprite = sprite;
  }

  public void Init(
    Vector3 targetPosition,
    Color color,
    System.Action Callback,
    Sprite sprite = null,
    float Scale = 0.4f,
    float MaxSpeed = 20f,
    bool AddZOffset = true,
    bool UseDeltaTime = true)
  {
    this.targetPosition = targetPosition;
    this.Init(this.Target, color, Callback, sprite, Scale, MaxSpeed, AddZOffset, UseDeltaTime);
  }

  public static void CreatePool(int count)
  {
    if ((UnityEngine.Object) SoulCustomTarget.projectilePrefab == (UnityEngine.Object) null)
      SoulCustomTarget.projectilePrefab = Resources.Load("Prefabs/Resources/SoulCustomTarget") as GameObject;
    ObjectPool.CreatePool(SoulCustomTarget.projectilePrefab, count);
  }

  public static GameObject Create(
    GameObject Target,
    Vector3 position,
    Color color,
    System.Action Callback,
    float Scale = 0.4f,
    float MaxSpeed = 20f,
    bool AddZOffset = true,
    bool UseDeltaTime = true,
    bool fromPool = true,
    string sfxPath = null,
    string collectSfxPath = null,
    bool playDefaultSFX = true)
  {
    if (sfxPath == null & playDefaultSFX)
      AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", position);
    else
      AudioManager.Instance.PlayOneShot(sfxPath, position);
    SoulCustomTarget.collectMeSfxPath = collectSfxPath;
    if ((UnityEngine.Object) SoulCustomTarget.projectilePrefab == (UnityEngine.Object) null)
      SoulCustomTarget.projectilePrefab = Resources.Load("Prefabs/Resources/SoulCustomTarget") as GameObject;
    Transform parent;
    if ((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab != (UnityEngine.Object) null)
    {
      parent = RoomManager.Instance.CurrentRoomPrefab.transform;
    }
    else
    {
      GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Unit Layer");
      parent = !((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null) ? Target.transform.parent : gameObjectWithTag.transform;
    }
    GameObject gameObject = !fromPool ? UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Resources/SoulCustomTarget") as GameObject, parent, true) : ObjectPool.Spawn(SoulCustomTarget.projectilePrefab, parent);
    gameObject.transform.position = position;
    gameObject.transform.eulerAngles = Vector3.zero;
    gameObject.GetComponent<SoulCustomTarget>().Init(Target, color, Callback, Scale: Scale, MaxSpeed: MaxSpeed, AddZOffset: AddZOffset, UseDeltaTime: UseDeltaTime);
    return gameObject;
  }

  public static GameObject Create(
    Vector3 targetPosition,
    Vector3 position,
    Color color,
    System.Action Callback,
    float Scale = 0.4f,
    float MaxSpeed = 20f,
    bool AddZOffset = true,
    bool UseDeltaTime = true,
    bool fromPool = true,
    string sfxPath = null,
    string collectSfxPath = null,
    bool playDefaultSFX = true)
  {
    if (sfxPath == null & playDefaultSFX)
      AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", position);
    else
      AudioManager.Instance.PlayOneShot(sfxPath, position);
    SoulCustomTarget.collectMeSfxPath = collectSfxPath;
    if ((UnityEngine.Object) SoulCustomTarget.projectilePrefab == (UnityEngine.Object) null)
      SoulCustomTarget.projectilePrefab = Resources.Load("Prefabs/Resources/SoulCustomTarget") as GameObject;
    Transform parent;
    if ((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab != (UnityEngine.Object) null)
    {
      parent = RoomManager.Instance.CurrentRoomPrefab.transform;
    }
    else
    {
      GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Unit Layer");
      parent = !((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null) ? PlayerFarming.Instance.transform.parent : gameObjectWithTag.transform;
    }
    GameObject gameObject = !fromPool ? UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Resources/SoulCustomTarget") as GameObject, parent, true) : ObjectPool.Spawn(SoulCustomTarget.projectilePrefab, parent);
    gameObject.transform.position = position;
    gameObject.transform.eulerAngles = Vector3.zero;
    SoulCustomTarget component = gameObject.GetComponent<SoulCustomTarget>();
    component.Target = (GameObject) null;
    component.Init(targetPosition, color, Callback, Scale: Scale, MaxSpeed: MaxSpeed, AddZOffset: AddZOffset, UseDeltaTime: UseDeltaTime);
    return gameObject;
  }

  public float DeltaTime => !this.UseDeltaTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;

  public void FixedUpdate()
  {
    if (!this.init || PlayerRelic.TimeFrozen)
      return;
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null && this.targetPosition == Vector3.zero)
    {
      this.Trail.Clear();
      this.gameObject.Recycle();
    }
    else
    {
      if (10.0 * (double) this.DeltaTime < 0.1)
        this.Trail.Clear();
      if ((double) this.Speed > 0.0)
      {
        this.ZPosition = this.Image.gameObject.transform.position;
        this.ZPosition.z = !(bool) (UnityEngine.Object) this.Target ? this.Lerp(this.ZPosition.z, this.targetPosition.z - (this.AddZOffset ? 0.5f : 0.0f), 10f * this.DeltaTime) : this.Lerp(this.ZPosition.z, this.Target.transform.position.z - (this.AddZOffset ? 0.5f : 0.0f), 10f * this.DeltaTime);
        this.Image.gameObject.transform.position = this.ZPosition;
      }
      this.Delay -= this.DeltaTime;
      if ((double) this.Delay <= 0.0)
        this.TurnSpeed = this.Lerp(this.TurnSpeed, 1f, 10f * this.DeltaTime);
      this.TargetAngle = !(bool) (UnityEngine.Object) this.Target ? Utils.GetAngle(this.transform.position, this.targetPosition) : Utils.GetAngle(this.transform.position, this.Target.transform.position);
      this.Angle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.Angle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.Angle) * (Math.PI / 180.0)))) * 57.295780181884766 / (double) this.TurnSpeed * (double) this.DeltaTime * 60.0);
      if ((double) this.Speed < (double) this.MaxSpeed)
        this.Speed += (float) (1.0 * (double) this.DeltaTime * 60.0);
      this.transform.position = this.transform.position + new Vector3(this.Speed * Mathf.Cos(this.Angle * ((float) Math.PI / 180f)) * this.DeltaTime, this.Speed * Mathf.Sin(this.Angle * ((float) Math.PI / 180f)) * this.DeltaTime);
      if ((bool) (UnityEngine.Object) this.Target && (double) SoulCustomTarget.Distance((Vector2) this.transform.position, (Vector2) this.Target.transform.position) < (double) this.Speed * (double) this.DeltaTime)
      {
        this.CollectMe();
      }
      else
      {
        if (!(this.targetPosition != Vector3.zero) || (double) SoulCustomTarget.Distance((Vector2) this.transform.position, (Vector2) this.targetPosition) >= (double) this.Speed * (double) this.DeltaTime)
          return;
        this.CollectMe();
      }
    }
  }

  public void CollectMe()
  {
    if (this.isCollected)
      return;
    BiomeConstants.Instance.EmitHitVFXSoul(this.Image.gameObject.transform.position, Quaternion.identity);
    if ((bool) (UnityEngine.Object) this.Target)
    {
      if (SoulCustomTarget.collectMeSfxPath == null)
        AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.Target);
      else
        AudioManager.Instance.PlayOneShot(SoulCustomTarget.collectMeSfxPath, this.Target);
    }
    else if (SoulCustomTarget.collectMeSfxPath == null)
      AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.targetPosition);
    else
      AudioManager.Instance.PlayOneShot(SoulCustomTarget.collectMeSfxPath, this.targetPosition);
    if ((double) Time.timeScale == 1.0 && !LetterBox.IsPlaying)
      CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.2f);
    if (this.Callback != null)
      this.Callback();
    this.Trail.Clear();
    this.gameObject.SetActive(false);
    this.gameObject.Recycle();
    this.isCollected = true;
  }

  public static float Distance(Vector2 a, Vector2 b) => (a - b).sqrMagnitude;

  public float Lerp(float firstFloat, float secondFloat, float by)
  {
    return firstFloat + (secondFloat - firstFloat) * by;
  }

  public Vector2 Lerp(Vector2 firstVector, Vector2 secondVector, float by)
  {
    return new Vector2(this.Lerp(firstVector.x, secondVector.x, by), this.Lerp(firstVector.y, secondVector.y, by));
  }
}
