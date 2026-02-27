// Decompiled with JetBrains decompiler
// Type: SoulCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SoulCustomTarget : BaseMonoBehaviour
{
  public AnimationCurve ZCurve;
  private GameObject Target;
  private float Delay = 1.25f;
  private float Speed = -15f;
  private float Angle;
  private float TargetAngle;
  public SpriteRenderer Image;
  public TrailRenderer Trail;
  private float ImageZ;
  private float ImageZSpeed;
  private float TurnSpeed = 7f;
  private Vector3 ZPosition;
  private System.Action Callback;
  private float MaxSpeed = 20f;
  private bool init;
  private bool AddZOffset = true;
  private Vector3 targetPosition = Vector3.zero;
  private bool UseDeltaTime;
  private static GameObject projectilePrefab;

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

  public static GameObject Create(
    GameObject Target,
    Vector3 position,
    Color color,
    System.Action Callback,
    float Scale = 0.4f,
    float MaxSpeed = 20f,
    bool AddZOffset = true,
    bool UseDeltaTime = true)
  {
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", position);
    if ((UnityEngine.Object) SoulCustomTarget.projectilePrefab == (UnityEngine.Object) null)
      SoulCustomTarget.projectilePrefab = Resources.Load("Prefabs/Resources/SoulCustomTarget") as GameObject;
    GameObject gameObject = ObjectPool.Spawn(SoulCustomTarget.projectilePrefab, !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform);
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
    bool UseDeltaTime = true)
  {
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", position);
    if ((UnityEngine.Object) SoulCustomTarget.projectilePrefab == (UnityEngine.Object) null)
      SoulCustomTarget.projectilePrefab = Resources.Load("Prefabs/Resources/SoulCustomTarget") as GameObject;
    GameObject gameObject = ObjectPool.Spawn(SoulCustomTarget.projectilePrefab, !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform);
    gameObject.transform.position = position;
    gameObject.transform.eulerAngles = Vector3.zero;
    gameObject.GetComponent<SoulCustomTarget>().Init(targetPosition, color, Callback, Scale: Scale, MaxSpeed: MaxSpeed, AddZOffset: AddZOffset, UseDeltaTime: UseDeltaTime);
    return gameObject;
  }

  private float DeltaTime => !this.UseDeltaTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;

  private void FixedUpdate()
  {
    if (!this.init)
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

  private void CollectMe()
  {
    BiomeConstants.Instance.EmitHitVFXSoul(this.Image.gameObject.transform.position, Quaternion.identity);
    if ((bool) (UnityEngine.Object) this.Target)
      AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.Target);
    else
      AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.targetPosition);
    if ((double) Time.timeScale == 1.0)
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
    if (this.Callback != null)
      this.Callback();
    this.Trail.Clear();
    this.gameObject.SetActive(false);
    this.gameObject.Recycle();
  }

  public static float Distance(Vector2 a, Vector2 b) => (a - b).sqrMagnitude;

  private float Lerp(float firstFloat, float secondFloat, float by)
  {
    return firstFloat + (secondFloat - firstFloat) * by;
  }

  private Vector2 Lerp(Vector2 firstVector, Vector2 secondVector, float by)
  {
    return new Vector2(this.Lerp(firstVector.x, secondVector.x, by), this.Lerp(firstVector.y, secondVector.y, by));
  }
}
